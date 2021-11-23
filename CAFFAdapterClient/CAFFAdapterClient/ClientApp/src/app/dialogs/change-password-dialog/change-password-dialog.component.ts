import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ChangePasswordDialogData } from 'src/app/entities/ChangePasswordDialogData';

@Component({
  selector: 'app-change-password-dialog',
  templateUrl: './change-password-dialog.component.html',
  styleUrls: ['./change-password-dialog.component.css']
})
export class ChangePasswordDialogComponent implements OnInit {

  changePasswordForm: FormGroup; 

  constructor(public dialogRef: MatDialogRef<ChangePasswordDialogComponent>,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: ChangePasswordDialogData) { }

  ngOnInit() {
    this.changePasswordForm = this.formBuilder.group({
      oldPassword: [null, [Validators.required]],    
      newPassword: [ null, Validators.compose([
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
      newPasswordConfirm: [null, [Validators.required, this.doesPasswordsMatch()]],      
    });

    this.changePasswordForm.get('newPassword').valueChanges.subscribe((x) => {
      this.data.newPassword = x;
    });

    this.changePasswordForm.get('oldPassword').valueChanges.subscribe((x) => {
      this.data.oldPassword = x;
    });
  }

  getErrorMessage() {
    return this.changePasswordForm.controls['newPassword'].hasError('required') ? 'Password is <strong>required</strong>' :
        this.changePasswordForm.controls['newPassword'].hasError('hasNumber') ? 'Must contain at least <strong>1 number</strong>' :        
        this.changePasswordForm.controls['newPassword'].hasError('hasCapitalCase') ? 'Must contain at least <strong>1 uppercase character</strong>' :
        this.changePasswordForm.controls['newPassword'].hasError('hasSmallCase') ? 'Must contain at least <strong>1 lowercase character</strong>' :
        this.changePasswordForm.controls['newPassword'].hasError('hasSpecialCharacters') ? 'Must contain at least <strong>1 special character</strong>' :
        this.changePasswordForm.controls['newPassword'].hasError('minlength') ? 'Must contain at least <strong>6 characters</strong>' :
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
        control.value === this.data.newPassword ? null : {'Passwords do not match!' : control.value};
  }

  save() {
    this.setCloseData();
    this.dialogRef.close(this.data);
  }

  setCloseData() {
    this.data.oldPassword = this.changePasswordForm.get('oldPassword').value;
    this.data.newPassword = this.changePasswordForm.get('newPassword').value;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
