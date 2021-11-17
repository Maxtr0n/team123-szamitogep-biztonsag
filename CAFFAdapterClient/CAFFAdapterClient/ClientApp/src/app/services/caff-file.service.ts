import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CaffFileService {

  url: '';

  constructor(private http: HttpClient) { }

  previewCaff(file: File) {
    var fData: FormData = new FormData();
    fData.append("content", file);    
    console.log(fData.get("content"));
  }
}
