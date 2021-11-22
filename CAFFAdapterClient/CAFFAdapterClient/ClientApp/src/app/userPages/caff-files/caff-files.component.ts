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

@Component({
  selector: 'app-caff-files',
  templateUrl: './caff-files.component.html',
  styleUrls: ['./caff-files.component.css']
})
export class CaffFilesComponent implements OnInit {

  isLoading = true;
  base64gif = '';
  gifs: GifResponse[] = [];

  constructor(private userService: UserService,
    private commentService: CommentService,
    private dialog: MatDialog,
    private toast: ToastrService) { }

  ngOnInit() {
    this.userService.getGifs().then(response => {
      var gif = response as GifResponse;      
      this.base64gif = gif.file;
      for (var i = 0; i < 6; i++) {
        this.gifs.push(gif);
      }
      this.isLoading = false;
    });
  }

  downloadCaffFile() {
    this.showDownloadInfo();
  }

  showDownloadInfo() {
    this.toast.info('Download has been started.', 'Info');
  }

  async seeComments() {
    const dialogConfig = await this.setSeeCommentsDialogConfigs();
    const dialogRef = this.dialog.open(
      CommentsDialogComponent,
      dialogConfig
    );
  }

  async setSeeCommentsDialogConfigs() {
    const dialogConfig = this.setCommonConfig('550px');
    await this.commentService.getCommentsForGif('1').then(response => {
      var responseEntity = response as CommentData[];     
      console.log(responseEntity); 
      var dialogData = new CommentsDialogData();
      dialogData.comments = responseEntity;
      dialogData.newComment = '';
      dialogConfig.data = dialogData;            
    });    
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
