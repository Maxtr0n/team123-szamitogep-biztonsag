import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EndPoint } from './endpoints';

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

  deleteCaff(gifId: number) {
    return this.http.delete(EndPoint.BASE_URL_2 + '/CaffFiles/' + gifId).toPromise();
  }

  editDescription(caffId: number, newDescription: string) {
    var requestBody = {
      description: newDescription
    };
    return this.http.put(EndPoint.BASE_URL_2 + '/CaffFiles/' + caffId, requestBody).toPromise();
  }

  downloadCaff(caffId: number) {
    return this.http.get(EndPoint.BASE_URL_2 + '/CaffFiles/' + caffId + '/download', { responseType: 'blob' }).toPromise();
  }
}
