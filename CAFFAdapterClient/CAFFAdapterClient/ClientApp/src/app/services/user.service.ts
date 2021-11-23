import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccessJWTToken } from '../entities/login/AccessJWTToken';
import { EndPoint } from './endpoints';
import { SessionData } from './sessionData';
import jwt_decode  from 'jwt-decode';
import { EditProfileDialogData } from '../entities/EditProfileDialogData';
import { ChangePasswordDialogData } from '../entities/ChangePasswordDialogData';

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

  decodeToken() {
    var tokenInfo = new AccessJWTToken();
    var token = sessionStorage.getItem(SessionData.TOKEN);
    tokenInfo = jwt_decode(token);    
    return tokenInfo;     
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
