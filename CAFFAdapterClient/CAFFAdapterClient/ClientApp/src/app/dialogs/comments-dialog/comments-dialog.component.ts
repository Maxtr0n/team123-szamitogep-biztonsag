import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommentsDialogData } from 'src/app/entities/CommentsDialogData';
import { DeleteDialogData, EntityType } from 'src/app/entities/DeleteCaffDialogData';
import { CommentService } from 'src/app/services/comment.service';
import { SessionData } from 'src/app/services/sessionData';
import { DeleteCaffDialogComponent } from '../delete-caff-dialog/delete-caff-dialog.component';

@Component({
  selector: 'app-comments-dialog',
  templateUrl: './comments-dialog.component.html',
  styleUrls: ['./comments-dialog.component.css']
})
export class CommentsDialogComponent implements OnInit {

  userId: number = +sessionStorage.getItem(SessionData.USER_ID);

  constructor(public dialogRef: MatDialogRef<CommentsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CommentsDialogData,
    private dialog: MatDialog,
    private commentService: CommentService) { }

  ngOnInit() {    
  }

  sendComment() {
    console.log(this.data.newComment);
    this.commentService.addCommentForGif(this.data.caffId, this.data.newComment).then(
      response => {
        this.commentService.getCommentsByGifId(this.data.caffId).then(
          response => {            
            this.data.comments = response.items;
            this.data.newComment = '';            
          },
          error => {
    
          }
        )
      },
      error => {

      }
    );    
  }

  sendCommentByEnter() {
    if (this.data.newComment.length != 0) {
      this.sendComment();
    }
  }

  deleteComment(commentId: number) {
    console.log('Clicked: ' + commentId);
    const dialogConfig = this.setDeleteCaffDialogConfigs(commentId);
    const dialogRefDeleteComment = this.dialog.open(
      DeleteCaffDialogComponent,
      dialogConfig
    );

    dialogRefDeleteComment.afterClosed().subscribe((data: string) => {
      if (data == 'Success.') {
        this.commentService.getCommentsByGifId(this.data.caffId).then(
          response => {            
            this.data.comments = response.items;
            this.data.newComment = '';            
          },
          error => {
    
          }
        )
      }
    });
  }

  setDeleteCaffDialogConfigs(commentId: number) {
    const dialogConfig = this.setCommonConfig('450px');
    var dialogData = new DeleteDialogData();
    dialogData.entityId = commentId;
    dialogData.entityType = EntityType.COMMENT;
    dialogData.parentCaffId = this.data.caffId;
    dialogConfig.data = dialogData;    
    return dialogConfig;
  }

  setCommonConfig(width) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.hasBackdrop = true;
    dialogConfig.closeOnNavigation = true;
    dialogConfig.disableClose = true;
    dialogConfig.width = width;
    
    return dialogConfig;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
