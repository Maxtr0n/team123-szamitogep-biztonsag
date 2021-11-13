import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { ChangePasswordDialogComponent } from 'src/app/dialogs/change-password-dialog/change-password-dialog.component';
import { CommentsDialogComponent } from 'src/app/dialogs/comments-dialog/comments-dialog.component';
import { DeleteCaffDialogComponent } from 'src/app/dialogs/delete-caff-dialog/delete-caff-dialog.component';
import { UploadImageComponent } from 'src/app/dialogs/upload-image/upload-image.component';
import { ChangePasswordDialogData } from 'src/app/entities/ChangePasswordDialogData';
import { CommentsDialogData } from 'src/app/entities/CommentsDialogData';
import { DeleteCaffDialogData } from 'src/app/entities/DeleteCaffDialogData';
import { GifResponse } from 'src/app/entities/GifResponse';
import { UploadImageDialogData } from 'src/app/entities/UploadImageDialogData';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-userprofile',
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css']
})
export class UserprofileComponent implements OnInit {

  email:string = 'test@test.com';
  gifs: GifResponse[] = [];

  constructor(private dialog: MatDialog, private userService: UserService) { }

  ngOnInit() {
    this.userService.getGifs().then(response => {
      var gif = response as GifResponse;      
      this.base64gif = gif.file;
      for (var i = 0; i < 6; i++) {
        this.gifs.push(gif);
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
        console.log(result.oldPassword);
        console.log(result.newPassword);
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

  seeComments() {
    const dialogConfig = this.setSeeCommentsDialogConfigs();
    const dialogRef = this.dialog.open(
      CommentsDialogComponent,
      dialogConfig
    );
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

  setSeeCommentsDialogConfigs() {
    const dialogConfig = this.setCommonConfig('450px');
    var dialogData = new CommentsDialogData();
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

  base64gif = ''
}
