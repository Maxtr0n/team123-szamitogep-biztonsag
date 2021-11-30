import { Injectable } from '@angular/core';
import { EndPoint } from './endpoints';
import { HttpClient } from '@angular/common/http';
import { GetGifContainerResponse } from '../entities/gif/GetGifContainerResponse';
import { UserContainerResponse } from '../entities/user/UserContainerResponse';
import { EditProfileDialogData } from '../entities/EditProfileDialogData';
import { RegisterUserDto } from '../entities/user/RegisterUserDto';

@Injectable({
    providedIn: 'root'
})
export class AdminService {

    constructor(private http: HttpClient) { }

    getUsers() {
        return this.http.get<UserContainerResponse>(EndPoint.BASE_URL_2 + '/account').toPromise();
    }

    deleteAccount(userId: number) {
        return this.http.delete(EndPoint.BASE_URL_2 + '/account/' + userId).toPromise();
    }

    editUserProfile(userId: number, data: EditProfileDialogData) {
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
        return this.http.patch(EndPoint.BASE_URL_2 + '/account/' + userId, jsonPatchDocument).toPromise();
    }

    getAllGifs() {
        return this.http.get<GetGifContainerResponse>(EndPoint.BASE_URL_2 + '/getAllGifs').toPromise();
    }

    registerAdmin(dto: RegisterUserDto) {
        console.log(dto);
        console.log()
        return this.http.post(EndPoint.BASE_URL_2 + '/account/registerAdmin', dto).toPromise();
    }

    deleteComment(caffId: number, commentId: number) {
        return this.http.delete(EndPoint.BASE_URL_2 + '/CaffFiles/' + caffId + '/comments/' + commentId).toPromise();
    }
}
