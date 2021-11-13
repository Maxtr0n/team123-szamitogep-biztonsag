import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
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
      newPassword: [null, [Validators.required]],    
      newPasswordConfirm: [null, [Validators.required, this.doesPasswordsMatch()]],      
    });

    this.changePasswordForm.get('newPassword').valueChanges.subscribe((x) => {
      this.data.newPassword = x;
    });

    this.changePasswordForm.get('oldPassword').valueChanges.subscribe((x) => {
      this.data.oldPassword = x;
    });
  }

  doesPasswordsMatch(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null =>  
        control.value === this.data.newPassword ? null : {'Passwords do not match!' : control.value};
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
