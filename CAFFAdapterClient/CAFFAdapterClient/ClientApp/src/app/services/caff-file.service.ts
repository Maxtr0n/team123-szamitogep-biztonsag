import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EndPoint } from './endpoints';

@Injectable({
  providedIn: 'root'
})
export class CaffFileService {

  url: '';

  constructor(private http: HttpClient) { }

  async previewCaff(file: File) {       
    var source = await this.convertToByArray(file);    

    var tmp = source as string;
    var img = tmp.substring(tmp.indexOf(",") + 1);
        
    var requestBody = {
      file: img
    };
    
    return this.http.post(EndPoint.BASE_URL_2 + '/CaffFiles/preview', requestBody, { responseType: 'blob' }).toPromise();
  }

  async uploadCaff(file: File, description: string) {
    var source = await this.convertToByArray(file);    

    var tmp = source as string;
    var img = tmp.substring(tmp.indexOf(",") + 1);
        
    var requestBody = {
      description: description,
      file: img
    };
    
    return this.http.post(EndPoint.BASE_URL_2 + '/CaffFiles', requestBody).toPromise();
  }

  async convertToByArray(file: File) {
    return new Promise((resolve, reject) => {
      var reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = (event: any) => {
        var data = reader.result;
        resolve(data);
      };
      //reader.readAsBinaryString(excelFile);
    });
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
