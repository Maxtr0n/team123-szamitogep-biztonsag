import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { LoginRequest } from '../entities/login/LoginRequest';
import { RegisterRequestData } from '../entities/register/RegisterRequestData';
import { EndPoint } from './endpoints';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  @Output() userLoggedIn: EventEmitter<any> = new EventEmitter();
  @Output() adminLoggedIn: EventEmitter<any> = new EventEmitter();

  url: string = EndPoint.BASE_URL;

  constructor(private http: HttpClient) { }

  registerUser(firstname: string, lastname: string, email:string, password: string) {
    var requestBody = new RegisterRequestData();
    requestBody.firstname = firstname;
    requestBody.lastname = lastname;
    requestBody.email = email;
    requestBody.password = password;
    return this.http.post(EndPoint.REGISTER_URL, requestBody);
  }

  login(email:string, password:string) {
    var loginRequest = new LoginRequest();
    loginRequest.email = email;
    loginRequest.password = password;
    return this.http.post(EndPoint.LOGIN_URL, loginRequest).toPromise();
  }

  userLoggedin() {
    this.userLoggedIn.emit(true);
  } 

  adminLoggedin() {
    this.adminLoggedIn.emit(true);
  }  
}
