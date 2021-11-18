import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EditProfileDialogData } from 'src/app/entities/EditProfileDialogData';

@Component({
  selector: 'app-edit-profile-dialog',
  templateUrl: './edit-profile-dialog.component.html',
  styleUrls: ['./edit-profile-dialog.component.css']
})
export class EditProfileDialogComponent implements OnInit {

  editProfileFormGroup: FormGroup; 

  constructor(public dialogRef: MatDialogRef<EditProfileDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditProfileDialogData,
    private formBuilder: FormBuilder,) { }

  ngOnInit() {
    this.editProfileFormGroup = this.formBuilder.group({
      firstName: [null, [Validators.required]],    
      lastName: [null, [Validators.required]]          
    });
    this.editProfileFormGroup.patchValue({
      firstName: this.data.firstName,
      lastName: this.data.lastName
    });
  }

  save() {
    this.setCloseData();
    this.dialogRef.close(this.data);
  }

  setCloseData() {
    this.data.firstName = this.editProfileFormGroup.get('firstName').value;
    this.data.lastName = this.editProfileFormGroup.get('lastName').value;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
