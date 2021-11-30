import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { AddAdminDialogComponent } from 'src/app/dialogs/add-admin-dialog/add-admin-dialog.component';
import { DeleteUserDialogComponent } from 'src/app/dialogs/delete-user-dialog/delete-user-dialog.component';
import { EditProfileDialogComponent } from 'src/app/dialogs/edit-profile-dialog/edit-profile-dialog.component';
import { DeleteProfileDialogData } from 'src/app/entities/DeleteProfileDialogData';
import { EditProfileDialogData } from 'src/app/entities/EditProfileDialogData';
import { ReqisterResponseErrorData } from 'src/app/entities/register/RegisterResponseErrorData';
import { RegisterUserDto } from 'src/app/entities/user/RegisterUserDto';
import { UserRowViewModel } from 'src/app/entities/user/UserRowViewModel';
import { AdminService } from 'src/app/services/admin.service';

@Component({
    selector: 'app-admin-users',
    templateUrl: './admin-users.component.html',
    styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent implements OnInit {

    users: UserRowViewModel[] = [];
    isLoading = true;


    constructor(private dialog: MatDialog,
        private adminService: AdminService,
        private toast: ToastrService,
    ) { }

    ngOnInit() {
        this.getData();
    }

    getData() {
        this.adminService.getUsers().then(response => {
            this.users = response.items;
            this.isLoading = false;
        },
            error => {

            });
    }

    deleteProfile(userId: number) {
        const dialogConfig = this.setDeleteProfileConfigs();
        const dialogRef = this.dialog.open(
            DeleteUserDialogComponent,
            dialogConfig
        );
        dialogRef.afterClosed().subscribe((data: String) => {
            if (data == 'Success.') {
                this.adminService.deleteAccount(userId).then(
                    success => {
                        this.showSuccess('User deleted.');
                        this.getData();
                    },
                    err => {
                        var response = err as ReqisterResponseErrorData;
                        this.showError(response.error);
                    });
            }
        });
    }

    editProfile(userId: number) {
        const dialogConfig = this.setEditProfileConfigs();
        const dialogRef = this.dialog.open(
            EditProfileDialogComponent,
            dialogConfig
        );
        dialogRef.afterClosed().subscribe((data: EditProfileDialogData) => {
            if (data) {
                this.adminService.editUserProfile(userId, data).then(
                    success => {
                        this.showSuccess('User edited.');
                        this.getData();
                    },
                    err => {
                        var response = err as ReqisterResponseErrorData;
                        this.showError(response.error);
                    });
            }
        });
    }

    addAdmin() {
        const dialogConfig = this.setAddAdminConfigs();
        const dialogRef = this.dialog.open(
            AddAdminDialogComponent,
            dialogConfig
        );
        dialogRef.afterClosed().subscribe((data: RegisterUserDto) => {
            if (data) {
                this.adminService.registerAdmin(data).then(
                    success => {
                        this.showSuccess('Admin registered.');
                        this.getData();
                    },
                    err => {
                        var response = err as ReqisterResponseErrorData;
                        this.showError(response.error);
                    });
            }
        });
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

    setAddAdminConfigs() {
        const dialogConfig = this.setCommonConfig('450px');
        var dialogData = new RegisterUserDto();
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

    showSuccess(message: string) {
        this.toast.success(message, 'Success');
    }

    showError(message: string) {
        this.toast.error(message, 'Error');
    }
}
