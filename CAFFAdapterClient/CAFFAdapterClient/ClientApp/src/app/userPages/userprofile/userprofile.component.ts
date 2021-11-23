import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { ChangePasswordDialogComponent } from 'src/app/dialogs/change-password-dialog/change-password-dialog.component';
import { CommentsDialogComponent } from 'src/app/dialogs/comments-dialog/comments-dialog.component';
import { DeleteCaffDialogComponent } from 'src/app/dialogs/delete-caff-dialog/delete-caff-dialog.component';
import { UploadImageComponent } from 'src/app/dialogs/upload-image/upload-image.component';
import { ChangePasswordDialogData } from 'src/app/entities/ChangePasswordDialogData';
import { CommentData } from 'src/app/entities/CommentData';
import { CommentsDialogData } from 'src/app/entities/CommentsDialogData';
import { DeleteCaffDialogData } from 'src/app/entities/DeleteCaffDialogData';
import { GifResponse } from 'src/app/entities/GifResponse';
import { UploadImageDialogData } from 'src/app/entities/UploadImageDialogData';
import { CommentService } from 'src/app/services/comment.service';
import { UserService } from 'src/app/services/user.service';
import { saveAs } from 'file-saver';
import { ToastrService } from 'ngx-toastr';
import { EditProfileDialogData } from 'src/app/entities/EditProfileDialogData';
import { EditProfileDialogComponent } from 'src/app/dialogs/edit-profile-dialog/edit-profile-dialog.component';
import { CaffFileService } from 'src/app/services/caff-file.service';
import { UserInfo } from 'src/app/entities/user/UserInfo';
import { ReqisterResponseErrorData } from 'src/app/entities/register/RegisterResponseErrorData';
import { UserProfileResponse } from 'src/app/entities/UserProfileResponse';

@Component({
  selector: 'app-userprofile',
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css']
})
export class UserprofileComponent implements OnInit {

  isLoading = true;
  firstname:string = '';
  lastname: string = '';
  email:string = '';  
  gifs: GifResponse[] = [];

  constructor(private dialog: MatDialog, 
    private userService: UserService, 
    private commentService: CommentService,
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
    this.loadUserInfo();
  }

  loadUserInfo() {        
    this.userService.getProfileData().then(
      success => {        
        var response = success as UserProfileResponse;
        this.firstname = response.firstname;
        this.lastname = response.lastname;
        this.email = response.email;
      },
      error => {
        var response = error as ReqisterResponseErrorData;
        this.showError(response.error);
      }
    )
  }

  editProfile() {
    const dialogConfig = this.setEditProfileConfigs();
    const dialogRef = this.dialog.open(
      EditProfileDialogComponent,
      dialogConfig
    );
    dialogRef.afterClosed().subscribe((data: EditProfileDialogData) => {      
      if (data) {
        this.userService.editProfile(data).then(
          success => {
            this.showSuccess('Profile data has successfully changed.');
            this.loadUserInfo();
          },
          err => {
            var response = err as ReqisterResponseErrorData;
            this.showError(response.error);
          });
      }     
    });
  }

  changePassword() {
    const dialogConfig = this.setPasswordDialogConfigs();
    const dialogRef = this.dialog.open(
      ChangePasswordDialogComponent,
      dialogConfig
    );
    dialogRef.afterClosed().subscribe((result: ChangePasswordDialogData) => {
      if (result) {
        this.userService.changePassword(result).then(
          success => {
            this.showSuccess('Password has been changed successfully.');
          },
          error => {            
            var response = error.error as ReqisterResponseErrorData;            
            this.showError(response.error);
          }
        )
      }      
    });
  }

  uploadCaffFile() {
    const dialogConfig = this.setUploadImageDialogConfigs();
    const dialogRef = this.dialog.open(
      UploadImageComponent,
      dialogConfig
    );
  }

  deleteCaffFile() {
    const dialogConfig = this.setDeleteCaffDialogConfigs();
    const dialogRef = this.dialog.open(
      DeleteCaffDialogComponent,
      dialogConfig
    );
  }

  downloadCaffFile() {
    this.showDownloadInfo();
  }

  showDownloadInfo() {
    this.toast.info('Download has been started.', 'Info');
  }

  showSuccess(message: string) {
    this.toast.success(message, 'Success');
  }

  showError(message: string) {
    this.toast.error(message, 'Error');
  }

  async seeComments() {
    const dialogConfig = await this.setSeeCommentsDialogConfigs();
    const dialogRef = this.dialog.open(
      CommentsDialogComponent,
      dialogConfig
    );
  }

  setEditProfileConfigs() {
    const dialogConfig = this.setCommonConfig('450px');
    var dialogData = new EditProfileDialogData();
    dialogData.firstName = this.firstname;
    dialogData.lastName = this.lastname;
    dialogConfig.data = dialogData;
    
    return dialogConfig;
  }

  setPasswordDialogConfigs() {
    const dialogConfig = this.setCommonConfig('450px');
    var dialogData = new ChangePasswordDialogData();
    dialogConfig.data = dialogData;
    
    return dialogConfig;
  }

  setUploadImageDialogConfigs() {
    const dialogConfig = this.setCommonConfig('450px');
    var dialogData = new UploadImageDialogData();
    dialogConfig.data = dialogData;
    
    return dialogConfig;
  }

  setDeleteCaffDialogConfigs() {
    const dialogConfig = this.setCommonConfig('450px');
    var dialogData = new DeleteCaffDialogData();
    dialogConfig.data = dialogData;
    
    return dialogConfig;
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

  base64gif = ''
}
