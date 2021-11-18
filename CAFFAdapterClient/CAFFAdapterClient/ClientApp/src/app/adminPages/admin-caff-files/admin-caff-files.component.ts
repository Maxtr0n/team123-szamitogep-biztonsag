import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { CommentsDialogComponent } from 'src/app/dialogs/comments-dialog/comments-dialog.component';
import { DeleteCaffDialogComponent } from 'src/app/dialogs/delete-caff-dialog/delete-caff-dialog.component';
import { CommentData } from 'src/app/entities/CommentData';
import { CommentsDialogData } from 'src/app/entities/CommentsDialogData';
import { DeleteCaffDialogData } from 'src/app/entities/DeleteCaffDialogData';
import { GifResponse } from 'src/app/entities/GifResponse';
import { AdminService } from 'src/app/services/admin.service';
import { CommentService } from 'src/app/services/comment.service';

@Component({
    selector: 'app-admin-caff-files',
    templateUrl: './admin-caff-files.component.html',
    styleUrls: ['./admin-caff-files.component.css']
})
export class AdminCaffFilesComponent implements OnInit {

    gifs: GifResponse[] = [];
    base64gif = '';

    constructor(private dialog: MatDialog,
        private adminService: AdminService,
        private commentService: CommentService,
        private toast: ToastrService
    ) { }

    ngOnInit() {
        this.adminService.getGifs().then(response => {
            var gif = response as GifResponse;
            this.base64gif = gif.file;
            for (var i = 0; i < 24; i++) {
                this.gifs.push(gif);
            }
        });
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

    async seeComments() {
        const dialogConfig = await this.setSeeCommentsDialogConfigs();
        const dialogRef = this.dialog.open(
            CommentsDialogComponent,
            dialogConfig
        );
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


}