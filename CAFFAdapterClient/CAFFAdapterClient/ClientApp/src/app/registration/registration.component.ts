import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

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
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.registrationForm = this.formBuilder.group({
      email: [null, [Validators.required, Validators.email]],
      password: [null, Validators.required],
      cPassword: [null, [Validators.required, this.doesPasswordsMatch()]],
    });

    this.registrationForm.get('password').valueChanges.subscribe((x) => {
      this.password = x;
    });

    this.registrationForm.get('cPassword').valueChanges.subscribe((x) => {
      this.passwordConfirm = x;
    });
  }

  doesPasswordsMatch(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null =>
      control.value === this.password
        ? null
        : { "Passwords do not match!": control.value };
  }
}
