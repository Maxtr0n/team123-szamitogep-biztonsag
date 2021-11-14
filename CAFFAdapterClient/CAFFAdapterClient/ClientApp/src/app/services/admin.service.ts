import { Injectable } from '@angular/core';
import { EndPoint } from './endpoints';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class AdminService {

    constructor(private http: HttpClient) { }

    getUsers() {
        return this.http.get(EndPoint.BASE_URL + '/users').toPromise();
    }

    getGifs() {
        return this.http.get(EndPoint.BASE_URL + '/gif').toPromise();
    }
}
