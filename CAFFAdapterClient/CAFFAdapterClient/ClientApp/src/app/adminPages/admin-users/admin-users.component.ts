import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { DeleteUserDialogComponent } from 'src/app/dialogs/delete-user-dialog/delete-user-dialog.component';
import { EditProfileDialogComponent } from 'src/app/dialogs/edit-profile-dialog/edit-profile-dialog.component';
import { DeleteProfileDialogData } from 'src/app/entities/DeleteProfileDialogData';
import { EditProfileDialogData } from 'src/app/entities/EditProfileDialogData';
import { UserProfileResponse } from 'src/app/entities/UserProfileResponse';
import { UserProfilesResponse } from 'src/app/entities/UserProfilesResponse';
import { AdminService } from 'src/app/services/admin.service';

@Component({
    selector: 'app-admin-users',
    templateUrl: './admin-users.component.html',
    styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent implements OnInit {

    users: UserProfileResponse[] = [];

    constructor(private dialog: MatDialog,
        private adminService: AdminService,
    ) { }

    ngOnInit() {
        this.adminService.getUsers().then(response => {
            var user = response as UserProfileResponse;
            for (var i = 0; i < 10; i++) {
                this.users.push(user);
            }
        });
    }

    deleteProfile() {
        const dialogConfig = this.setDeleteProfileConfigs();
        const dialogRef = this.dialog.open(
            DeleteUserDialogComponent,
            dialogConfig
        );
    }

    editProfile() {
        const dialogConfig = this.setEditProfileConfigs();
        const dialogRef = this.dialog.open(
            EditProfileDialogComponent,
            dialogConfig
        );
    }

    setDeleteProfileConfigs() {
        const dialogConfig = this.setCommonConfig('450px');
        var dialogData = new DeleteProfileDialogData();
        dialogConfig.data = dialogData;

        return dialogConfig;
    }

    setEditProfileConfigs() {
        const dialogConfig = this.setCommonConfig('450px');
        var dialogData = new EditProfileDialogData();
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
}
