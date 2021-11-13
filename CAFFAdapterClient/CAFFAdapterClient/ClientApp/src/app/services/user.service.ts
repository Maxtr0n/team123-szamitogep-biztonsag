import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EndPoint } from './endpoints';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  public getGifs() {
    return this.http.get(EndPoint.BASE_URL + '/gif').toPromise();
  }
}
