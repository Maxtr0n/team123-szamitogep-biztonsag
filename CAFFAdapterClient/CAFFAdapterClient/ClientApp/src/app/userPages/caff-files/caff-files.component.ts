import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { CommentsDialogComponent } from 'src/app/dialogs/comments-dialog/comments-dialog.component';
import { CommentData } from 'src/app/entities/CommentData';
import { CommentsDialogData } from 'src/app/entities/CommentsDialogData';
import { GifResponse } from 'src/app/entities/GifResponse';
import { CommentService } from 'src/app/services/comment.service';
import { UserService } from 'src/app/services/user.service';
import { LoadingComponent } from 'src/app/loading/loading/loading.component';
import { GetGifResponse } from 'src/app/entities/gif/GetGifResponse';

@Component({
  selector: 'app-caff-files',
  templateUrl: './caff-files.component.html',
  styleUrls: ['./caff-files.component.css']
})
export class CaffFilesComponent implements OnInit {

  isLoading = true;
  base64gif = '';
  gifs: GifResponse[] = [];
  gifContainer: GetGifResponse[];

  constructor(private userService: UserService,
    private commentService: CommentService,
    private dialog: MatDialog,
    private toast: ToastrService) { }

  ngOnInit() {
    // this.userService.getGifs().then(response => {
    //   var gif = response as GifResponse;      
    //   this.base64gif = gif.file;
    //   for (var i = 0; i < 6; i++) {
    //     this.gifs.push(gif);
    //   }
    //   this.isLoading = false;
    // });

    this.userService.getAllGifs().then(
      response => {
        this.gifContainer = response.items;
        this.isLoading = false;
      },
      error => {

      }
    )
  }

  downloadCaffFile() {
    this.showDownloadInfo();
  }

  showDownloadInfo() {
    this.toast.info('Download has been started.', 'Info');
  }

  async seeComments(gifId: number) {
    const dialogConfig = await this.setSeeCommentsDialogConfigs(gifId);
    const dialogRef = this.dialog.open(
      CommentsDialogComponent,
      dialogConfig
    );
  }

  async setSeeCommentsDialogConfigs(gifId: number) {
    const dialogConfig = this.setCommonConfig('550px');     
    
    await this.commentService.getCommentsByGifId(gifId).then(
      response => {
        console.log(response);
        var dialogData = new CommentsDialogData();
        dialogData.comments = response.items;
        dialogData.newComment = '';
        dialogConfig.data = dialogData; 
      },
      error => {

      }
    )

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

}
