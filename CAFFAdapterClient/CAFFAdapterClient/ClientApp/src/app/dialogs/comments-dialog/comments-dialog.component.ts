import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommentsDialogData } from 'src/app/entities/CommentsDialogData';

@Component({
  selector: 'app-comments-dialog',
  templateUrl: './comments-dialog.component.html',
  styleUrls: ['./comments-dialog.component.css']
})
export class CommentsDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CommentsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CommentsDialogData) { }

  ngOnInit() {
  }

  sendComment() {
    console.log(this.data.newComment);
    this.data.newComment = '';    
  }

  sendCommentByEnter() {
    if (this.data.newComment.length != 0) {
      this.sendComment();
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
