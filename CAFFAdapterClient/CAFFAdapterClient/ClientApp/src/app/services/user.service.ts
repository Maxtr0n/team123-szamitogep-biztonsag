import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EndPoint } from './endpoints';

import { EditProfileDialogData } from '../entities/EditProfileDialogData';
import { ChangePasswordDialogData } from '../entities/ChangePasswordDialogData';
import { GetGifContainerResponse } from '../entities/gif/GetGifContainerResponse';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }  

  getProfileData() {
    return this.http.get(EndPoint.GET_PROFILE_URL).toPromise();
  }

  getGifs() {
    return this.http.get(EndPoint.BASE_URL + '/gif').toPromise();
  }

  getGifVersion2() {
    return this.http.get('https://localhost:44343/CaffFiles/1/preview', { responseType: 'blob' }).toPromise();
  }

  getGifsByUser() {
    return this.http.get<GetGifContainerResponse>(EndPoint.BASE_URL_2 + '/getGifsByUserId').toPromise();
  }

  getAllGifs() {
    return this.http.get<GetGifContainerResponse>(EndPoint.BASE_URL_2 + '/getAllGifs').toPromise();
  }  

  editProfile(data: EditProfileDialogData) {
    var jsonPatchDocument = [
      {    
        "path": "firstname",
        "op": "replace",    
        "value": data.firstName
      },
      {    
        "path": "lastname",
        "op": "replace",    
        "value": data.lastName
      }
    ];
    console.log(jsonPatchDocument);
    return this.http.patch(EndPoint.EDIT_PROFILE_URL, jsonPatchDocument).toPromise();
  }

  changePassword(data: ChangePasswordDialogData) {
    return this.http.put(EndPoint.CHANGE_PASSWORD_URL, data).toPromise();
  }
}
