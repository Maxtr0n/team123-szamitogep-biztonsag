import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CaffCreationDate, CaffMetadata } from 'src/app/entities/gif/GetGifResponse';

class ArrayData {
  key: string;
  value: any;
  constructor(_key, _value) {
    this.key =_key;
    this.value =_value;
  }
}

@Component({
  selector: 'app-caff-metadata-dialog',
  templateUrl: './caff-metadata-dialog.component.html',
  styleUrls: ['./caff-metadata-dialog.component.css']
})
export class CaffMetadataDialogComponent implements OnInit {

  displayedColumnsFrames: string[] = ['counter', 'caption', 'tags'];
  displayedColumns: string[] = [ 'key', 'value'];
  
  array: ArrayData[] = [];

  constructor(public dialogRef: MatDialogRef<CaffMetadataDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CaffMetadata) {
      this.array.push(new ArrayData('Creation date', this.formatDate(data.creation_date)));
      this.array.push(new ArrayData('Creator', data.creator));
      this.array.push(new ArrayData('Height', data.height));
      this.array.push(new ArrayData('Width', data.width));
    }

  ngOnInit() {
  }

  formatDate(date: CaffCreationDate) {
    var year = date.year;
    var month = date.month < 10 ? '0' + date.month : date.month;
    var day = date.day < 10 ? '0' + date.day : date.day;
    var hour = date.hour < 10 ? '0' + date.hour : date.hour;
    var minute = date.minute < 10 ? '0' + date.minute : date.minute;
    return year + '. ' + month + '. ' + day + '. ' + hour + ':' + minute; 
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
