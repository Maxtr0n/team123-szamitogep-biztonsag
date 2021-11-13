import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { UploadImageDialogData } from 'src/app/entities/UploadImageDialogData';

@Component({
  selector: 'app-upload-image',
  templateUrl: './upload-image.component.html',
  styleUrls: ['./upload-image.component.css']
})
export class UploadImageComponent implements OnInit {

  isFileValid: boolean = false;
  preViewWasClicked: boolean = false;
  inProgress: boolean = false;
 
  constructor(public dialogRef: MatDialogRef<UploadImageComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UploadImageDialogData,
    private toast: ToastrService) { }

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
  
  fileUpload(files: FileList) {
    if (files.length == 0) {
      this.data.caffFile = null;
      return;
    }
    var file = files.item(0);
    this.isFileValid = this.validateFileExtension(file.name);
    if (!this.isFileValid) {
      this.showBadExtensionError();
      return;
    }
    this.data.caffFile = file;
  }

  preViewCaffFile() {
    this.preViewWasClicked = true;
    this.inProgress = true;
  }
  
  validateFileExtension(fileName) {
    var dotIndex = fileName.indexOf('.');
    var extension = fileName.substring(dotIndex);
    if (extension !== '.caff') {      
      return false;
    }
    return true;
  }

  showBadExtensionError() {
    this.toast.error('You can only upload CAFF files.', 'Error');
  }
}
