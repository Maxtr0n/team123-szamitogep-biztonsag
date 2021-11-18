import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ReqisterResponseErrorData } from '../entities/RegisterResponseErrorData';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  registrationForm: FormGroup;
  password: string = '';
  passwordConfirm: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthenticationService,
    private toast: ToastrService,
    private router: Router) { }

  ngOnInit() {
    this.registrationForm = this.formBuilder.group({
      firstName: [null, Validators.required],
      lastName: [null, Validators.required],
      email: [null, [Validators.required, Validators.email]],
      password: [ null, Validators.compose([
        // 1. Password Field is Required
        Validators.required,
        // 2. check whether the entered password has a number
        this.patternValidator(/\d/, { hasNumber: true }),
        // 3. check whether the entered password has upper case letter
        this.patternValidator(/[A-Z]/, { hasCapitalCase: true }),
        // 4. check whether the entered password has a lower-case letter
        this.patternValidator(/[a-z]/, { hasSmallCase: true }),
        // 5. check whether the entered password has a special character
        this.patternValidator(/[$&+,:;=?@#|'<>.^*()%!-]/, { hasSpecialCharacters: true }),
        // 6. Has a minimum length of 6 characters
        Validators.minLength(6)])
     ],
      cPassword: [null, [Validators.required, this.doesPasswordsMatch()]],
    });

    this.registrationForm.get('password').valueChanges.subscribe((x) => {
      this.password = x;
    });

    this.registrationForm.get('cPassword').valueChanges.subscribe((x) => {
      this.passwordConfirm = x;
    });
  }

  getErrorMessage() {
    return this.registrationForm.controls['password'].hasError('required') ? 'Password is <strong>required</strong>' :
        this.registrationForm.controls['password'].hasError('hasNumber') ? 'Must contain at least <strong>1 number</strong>' :        
        this.registrationForm.controls['password'].hasError('hasCapitalCase') ? 'Must contain at least <strong>1 uppercase character</strong>' :
        this.registrationForm.controls['password'].hasError('hasSmallCase') ? 'Must contain at least <strong>1 lowercase character</strong>' :
        this.registrationForm.controls['password'].hasError('hasSpecialCharacters') ? 'Must contain at least <strong>1 special character</strong>' :
        this.registrationForm.controls['password'].hasError('minlength') ? 'Must contain at least <strong>6 characters</strong>' :
           '';
  }

  patternValidator(regex: RegExp, error: ValidationErrors): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
      if (!control.value) {
        // if control is empty return no error
        return null;
      }
  
      // test the value of the control against the regexp supplied
      const valid = regex.test(control.value);
  
      // if true, return no error (no error), else return error passed in the second parameter
      return valid ? null : error;
    };
  }

  doesPasswordsMatch(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null =>
      control.value === this.password
        ? null
        : { "Passwords do not match!": control.value };
  }

  register() {
    var firstname = this.registrationForm.get('firstName').value;
    var lastname = this.registrationForm.get('lastName').value;
    var email = this.registrationForm.get('email').value;
    var password = this.registrationForm.get('password').value;
    this.authService.registerUser(firstname, lastname, email, password).subscribe(
      res => {
        this.handleSuccessRegistration();
      },
      err => {
        this.handleErrorRegistration(err);
      }
    )
  }

  handleSuccessRegistration() {
    this.showSuccess();
    this.router.navigate(['/signin']);
  }

  handleErrorRegistration(err) {
    var error = err.error as ReqisterResponseErrorData;
    this.showErrorToast(error.error);
  }

  showErrorToast(message) {
    this.toast.error(message, 'Register failed');
  }

  showSuccess() {
    this.toast.success('You have successfully registered.', 'Success.');
  }
}
