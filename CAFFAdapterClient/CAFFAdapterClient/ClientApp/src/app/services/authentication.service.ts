import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { EndPoint } from './endpoints';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  @Output() userLoggedIn: EventEmitter<any> = new EventEmitter();
  @Output() adminLoggedIn: EventEmitter<any> = new EventEmitter();

  url: string = EndPoint.BASE_URL;

  constructor(private http: HttpClient) { }


  userLogin(email:string, password:string) {
    return this.http.get(this.url + '/login/user/' + email + '/' + password).toPromise();
  }

  userLoggedin() {
    this.userLoggedIn.emit(true);
  }

  adminLogin(email:string, password:string) {
    return this.http.get(this.url + '/login/admin/' + email + '/' + password).toPromise();
  }

  adminLoggedin() {
    this.adminLoggedIn.emit(true);
  }
}
