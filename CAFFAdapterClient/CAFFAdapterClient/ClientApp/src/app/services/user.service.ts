import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EndPoint } from './endpoints';
import { SessionData } from './sessionData';

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
}
