import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { CommentsDialogComponent } from 'src/app/dialogs/comments-dialog/comments-dialog.component';
import { DeleteCaffDialogComponent } from 'src/app/dialogs/delete-caff-dialog/delete-caff-dialog.component';
import { CommentData } from 'src/app/entities/CommentData';
import { CommentsDialogData } from 'src/app/entities/CommentsDialogData';
import { DeleteDialogData, EntityType } from 'src/app/entities/DeleteCaffDialogData';
import { GetGifResponse } from 'src/app/entities/gif/GetGifResponse';
import { GifResponse } from 'src/app/entities/GifResponse';
import { AdminService } from 'src/app/services/admin.service';
import { CaffFileService } from 'src/app/services/caff-file.service';
import { CommentService } from 'src/app/services/comment.service';

@Component({
    selector: 'app-admin-caff-files',
    templateUrl: './admin-caff-files.component.html',
    styleUrls: ['./admin-caff-files.component.css']
})
export class AdminCaffFilesComponent implements OnInit {

    isLoading = true;
    base64gif = '';
    gifs: GifResponse[] = [];
    gifContainer: GetGifResponse[];

    constructor(private dialog: MatDialog,
        private adminService: AdminService,
        private commentService: CommentService,
        private toast: ToastrService,
        private caffService: CaffFileService,
        private datePipe: DatePipe,
    ) { }

    ngOnInit() {
        this.getData();

    }

    getData() {
        this.adminService.getAllGifs().then(
            response => {
                this.gifContainer = response.items;
                this.isLoading = false;
            },
            error => {

            }
        )
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
                this.getData();
            }
        });
    }

    downloadCaffFile(caffId: number) {
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

    async seeComments(gifId: number) {
        const dialogConfig = await this.setSeeCommentsDialogConfigs(gifId);
        const dialogRef = this.dialog.open(
            CommentsDialogComponent,
            dialogConfig
        );
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


}
