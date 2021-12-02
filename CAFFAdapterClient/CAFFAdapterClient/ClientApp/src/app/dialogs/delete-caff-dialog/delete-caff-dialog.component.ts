import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { DeleteDialogData, EntityType } from 'src/app/entities/DeleteCaffDialogData';
import { AdminService } from 'src/app/services/admin.service';
import { CaffFileService } from 'src/app/services/caff-file.service';
import { CommentService } from 'src/app/services/comment.service';

@Component({
    selector: 'app-delete-caff-dialog',
    templateUrl: './delete-caff-dialog.component.html',
    styleUrls: ['./delete-caff-dialog.component.css']
})
export class DeleteCaffDialogComponent implements OnInit {

    entity: string = '';

    constructor(public dialogRef: MatDialogRef<DeleteCaffDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DeleteDialogData,
        private caffService: CaffFileService,
        private commentService: CommentService,
        private adminService: AdminService,
        private toast: ToastrService) {
        this.entity = this.data.entityType == EntityType.CAFF ? 'gif' : 'comment';
    }

    ngOnInit() {
    }

    deleteEntity() {
        if (this.data.entityType == EntityType.CAFF) {
            this.caffService.deleteCaff(this.data.entityId).then(
                response => {
                    this.dialogRef.close('Success.');
                },
                error => {
                    this.toast.error('Please try again later.', 'Something went wrong.')
                }
            )
        }
        if (this.data.entityType == EntityType.COMMENT && this.data.isAdminDelete == false) {
            this.commentService.deleteComment(this.data.entityId).then(
                response => {                    
                    this.dialogRef.close('Success.');
                },
                error => {
                    this.toast.error('Please try again later.', 'Something went wrong.')
                }
            )
        }
        if (this.data.entityType == EntityType.COMMENT && this.data.isAdminDelete == true) {
            this.adminService.deleteComment(this.data.parentCaffId, this.data.entityId).then(
                response => {                    
                    this.dialogRef.close('Success.');
                },
                error => {
                    this.toast.error('Please try again later.', 'Something went wrong.')
                }
            )
        }
    }

    onNoClick(): void {
        this.dialogRef.close();
    }
}
