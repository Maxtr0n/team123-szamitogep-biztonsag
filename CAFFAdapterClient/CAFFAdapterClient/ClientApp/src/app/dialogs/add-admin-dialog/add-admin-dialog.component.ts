import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { RegisterUserDto } from 'src/app/entities/user/RegisterUserDto';

@Component({
    selector: 'app-add-admin-dialog',
    templateUrl: './add-admin-dialog.component.html',
    styleUrls: ['./add-admin-dialog.component.css']
})
export class AddAdminDialogComponent implements OnInit {

    formGroup: FormGroup;

    constructor(public dialogRef: MatDialogRef<AddAdminDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: RegisterUserDto,
        private formBuilder: FormBuilder) { }

    ngOnInit() {
        this.formGroup = this.formBuilder.group({
            firstName: [null, [Validators.required]],
            lastName: [null, [Validators.required]],
            email: [null, [Validators.required]],
            password: [null, [Validators.required]],
        });
        this.formGroup.patchValue({
            firstName: this.data.firstName,
            lastName: this.data.lastName,
            email: this.data.email,
            password: this.data.password
        });

    }

    save() {
        this.setCloseData();
        this.dialogRef.close(this.data);
    }

    setCloseData() {
        this.data.firstName = this.formGroup.get('firstName').value;
        this.data.lastName = this.formGroup.get('lastName').value;
        this.data.email = this.formGroup.get('email').value;
        this.data.password = this.formGroup.get('password').value;
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

}
