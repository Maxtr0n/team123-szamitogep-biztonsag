import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccessJWTToken } from '../entities/login/AccessJWTToken';
import { EndPoint } from './endpoints';
import { SessionData } from './sessionData';
import jwt_decode  from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  getUserData() {
    var userId = sessionStorage.getItem(SessionData.USER_ID);
    return this.http.get(EndPoint.BASE_URL_2 + '/account/' + userId).toPromise();
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
}
