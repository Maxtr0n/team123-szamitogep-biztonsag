import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DeleteCaffDialogData } from 'src/app/entities/DeleteCaffDialogData';

@Component({
  selector: 'app-delete-caff-dialog',
  templateUrl: './delete-caff-dialog.component.html',
  styleUrls: ['./delete-caff-dialog.component.css']
})
export class DeleteCaffDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<DeleteCaffDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DeleteCaffDialogData) { }

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
