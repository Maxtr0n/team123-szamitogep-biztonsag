import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DeleteProfileDialogData } from 'src/app/entities/DeleteProfileDialogData';

@Component({
    selector: 'app-delete-user-dialog',
    templateUrl: './delete-user-dialog.component.html',
    styleUrls: ['./delete-user-dialog.component.css']
})
export class DeleteUserDialogComponent implements OnInit {

    constructor(public dialogRef: MatDialogRef<DeleteUserDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DeleteProfileDialogData) { }

    ngOnInit() {
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    deleteEntity() {
        this.dialogRef.close("Success.");
    }


}
