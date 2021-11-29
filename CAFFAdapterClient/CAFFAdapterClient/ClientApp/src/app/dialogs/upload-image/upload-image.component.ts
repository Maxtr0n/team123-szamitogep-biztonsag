import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { GifResponse } from 'src/app/entities/GifResponse';
import { ReqisterResponseErrorData } from 'src/app/entities/register/RegisterResponseErrorData';
import { UploadImageDialogData } from 'src/app/entities/UploadImageDialogData';
import { CaffFileService } from 'src/app/services/caff-file.service';

@Component({
  selector: 'app-upload-image',
  templateUrl: './upload-image.component.html',
  styleUrls: ['./upload-image.component.css']
})
export class UploadImageComponent implements OnInit {

  isFileValid: boolean = false;
  preViewWasClicked: boolean = false;  
  success: boolean = false;
  preview: string = '';
  description: string = '';
  uploadStarted: boolean = false;
  showLoading: boolean = false;
 
  constructor(public dialogRef: MatDialogRef<UploadImageComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UploadImageDialogData,
    private toast: ToastrService,
    private caffFileService: CaffFileService) { }

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
  
  fileUpload(files: FileList) {
    this.data.caffFile = null;
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
    this.showLoading = true;
    this.caffFileService.previewCaff(this.data.caffFile).then(
      (response: Blob) => {
        this.success = true;
        this.createImageFromBlob(response);
        this.showLoading = false;
      },
      error => {        
        var customError = error.error as ReqisterResponseErrorData;
        this.showError('Not a valid CAFF File.');    
        this.showLoading = false;    
      }
    )
  }

  uploadFile() {
    this.uploadStarted = true;
    this.showInfo();
    this.caffFileService.uploadCaff(this.data.caffFile, this.description).then(
      response => {
        this.dialogRef.close('Success.');
      },
      error => {
        var customError = error.error as ReqisterResponseErrorData;
        this.showError(customError.error);    
        this.showLoading = false; 
        this.dialogRef.close();
      }
    );
  }

  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {      
      this.preview = reader.result as string;      
    }, false); 
    if (image) {
       reader.readAsDataURL(image);
    }
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

  showError(message: string) {
    this.toast.error(message, 'Error');
  }

  showInfo() {
    this.toast.info('Your upload has been started.', 'Please wait.');
  }
}
