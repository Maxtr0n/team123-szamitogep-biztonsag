import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { ChangePasswordDialogComponent } from 'src/app/dialogs/change-password-dialog/change-password-dialog.component';
import { CommentsDialogComponent } from 'src/app/dialogs/comments-dialog/comments-dialog.component';
import { DeleteCaffDialogComponent } from 'src/app/dialogs/delete-caff-dialog/delete-caff-dialog.component';
import { UploadImageComponent } from 'src/app/dialogs/upload-image/upload-image.component';
import { ChangePasswordDialogData } from 'src/app/entities/ChangePasswordDialogData';
import { CommentData } from 'src/app/entities/CommentData';
import { CommentsDialogData } from 'src/app/entities/CommentsDialogData';
import { DeleteDialogData, EntityType } from 'src/app/entities/DeleteCaffDialogData';
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
import { EditDescriptionDialogData } from 'src/app/entities/dialogData/EditDescriptionDialogData';
import { EditCaffDescriptionDialogComponent } from 'src/app/dialogs/edit-caff-description-dialog/edit-caff-description-dialog.component';
import { CssSelector } from '@angular/compiler';
import { GetGifResponse } from 'src/app/entities/gif/GetGifResponse';
import { DatePipe } from '@angular/common';

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
  gifContainer: GetGifResponse[] = [];

  constructor(private dialog: MatDialog, 
    private userService: UserService, 
    private commentService: CommentService,
    private caffService: CaffFileService,
    private toast: ToastrService,
    private datePipe: DatePipe) { }

  ngOnInit() {   

    // this.userService.getGifs().then(response => {
    //   var gif = response as GifResponse;      
    //   this.base64gif = gif.file;
    //   for (var i = 0; i < 6; i++) {
    //     this.gifs.push(gif);
    //   }
    //   this.isLoading = false;
    // });

    // this.userService.getGifVersion2().then((response: Blob) => {
    //   console.log('VÁLASZ');
    //   console.log(response);
    //   console.log('BASE64:');   
    //   this.createImageFromBlob(response);
    //   this.isLoading = false;
    // });

    this.userService.getGifsByUser().then(
      response => {
        if (response.count == 0) {
          this.gifContainer = [];
        } else {
          this.gifContainer = response.items;
        }        
        this.isLoading = false;
      },
      error => {

      }
    );

    this.loadUserInfo();
  }

  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      var gifResponse = new GifResponse();
      gifResponse.file = reader.result as string;
      this.gifs.push(gifResponse);
    }, false);
 
    if (image) {
       reader.readAsDataURL(image);
    }
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

  editDescription(gif: GetGifResponse) {  
    const dialogConfig = this.setEditDescriptionDialogConfigs(gif);
    const dialogRef = this.dialog.open(
      EditCaffDescriptionDialogComponent,
      dialogConfig
    );
    dialogRef.afterClosed().subscribe((data: EditDescriptionDialogData) => {
      if (data) {
        console.log(data.description);
        this.caffService.editDescription(gif.id, data.description).then(
          response => {
            this.isLoading = true;
            this.userService.getGifsByUser().then(
              response => {
                if (response.count == 0) {
                  this.gifContainer = [];
                } else {
                  this.gifContainer = response.items;
                }        
                this.isLoading = false;
              },
              error => {
        
              }
            );
          },
          error => {

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

  deleteCaffFile(gifId: number) {
    const dialogConfig = this.setDeleteCaffDialogConfigs(gifId);
    const dialogRef = this.dialog.open(
      DeleteCaffDialogComponent,
      dialogConfig
    );

    dialogRef.afterClosed().subscribe((data: string) => {
      if (data == 'Success.') {
        this.isLoading = true;
        this.userService.getGifsByUser().then(
          response => {
            if (response.count == 0) {
              this.gifContainer = [];
            } else {
              this.gifContainer = response.items;
            }        
            this.isLoading = false;
          },
          error => {
    
          }
        )
      }
    });
  }

  downloadCaffFile(caffId: number) {
    //this.showDownloadInfo();
    this.caffService.downloadCaff(caffId).then(
      (response: Blob) => {
        console.log(response);
        this.downloadFile(response, caffId);
      },
      error => {

      }
    );
  }

  showDownloadInfo() {
    this.toast.info('Download has been started.', 'Info');
  }

  downloadFile(data: Blob, caffId: number) {
    console.log('ITTEN');
    const blob = data;
    const url = window.URL.createObjectURL(blob);    
    var anchor = document.createElement("a");
    var now = new Date();
    var date = this.datePipe.transform(now, 'yyyyMMddhhmm_' + caffId);
    anchor.download = date + '.caff';
    anchor.href = url;
    anchor.click();
    URL.revokeObjectURL(url);
  }

  showSuccess(message: string) {
    this.toast.success(message, 'Success');
  }

  showError(message: string) {
    this.toast.error(message, 'Error');
  }

  async seeComments(gifId: number) {
    const dialogConfig = await this.setSeeCommentsDialogConfigs(gifId);
    const dialogRef = this.dialog.open(
      CommentsDialogComponent,
      dialogConfig
    );
  }

  setEditDescriptionDialogConfigs(gif: GetGifResponse) {
    const dialogConfig = this.setCommonConfig('550px');
    var dialogData = new EditDescriptionDialogData();
    dialogData.description = gif.description;
    dialogConfig.data = dialogData;
    
    return dialogConfig;
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

  setDeleteCaffDialogConfigs(gifId: number) {
    const dialogConfig = this.setCommonConfig('450px');
    var dialogData = new DeleteDialogData();
    dialogData.entityId = gifId;
    dialogData.entityType = EntityType.CAFF;
    dialogConfig.data = dialogData;    
    return dialogConfig;
  }

  async setSeeCommentsDialogConfigs(gifId: number) {
    const dialogConfig = this.setCommonConfig('550px');

    // await this.commentService.getCommentsForGif('1').then(response => {
    //   var responseEntity = response as CommentData[];     
    //   console.log(responseEntity); 
    //   var dialogData = new CommentsDialogData();
    //   dialogData.comments = responseEntity;
    //   dialogData.newComment = '';
    //   dialogConfig.data = dialogData;            
    // });    
    
    await this.commentService.getCommentsByGifId(gifId).then(
      response => {
        console.log(response);
        var dialogData = new CommentsDialogData();
        dialogData.comments = response.items;
        dialogData.caffId = gifId;
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

  base64gif = ''
}
