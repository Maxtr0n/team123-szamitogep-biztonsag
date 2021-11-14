import { Component, OnInit } from '@angular/core';
import { UserProfileResponse } from 'src/app/entities/UserProfileResponse';
import { UserProfilesResponse } from 'src/app/entities/UserProfilesResponse';
import { AdminService } from 'src/app/services/admin.service';

@Component({
    selector: 'app-admin-users',
    templateUrl: './admin-users.component.html',
    styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent implements OnInit {

    users: UserProfileResponse[] = [];

    constructor(
        private adminService: AdminService,
    ) { }

    ngOnInit() {
        this.adminService.getUsers().then(response => {
            var user = response as UserProfileResponse;
            for (var i = 0; i < 10; i++) {
                this.users.push(user);
            }
        });
    }

}
