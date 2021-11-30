import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginResponse } from '../entities/login/LoginResponse';
import { AuthenticationService } from '../services/authentication.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { SessionData } from '../services/sessionData';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

    loginForm: FormGroup;
    adminLoginForm: FormGroup;

    emailValue = '';

    constructor(
        private formBuilder: FormBuilder,
        private toast: ToastrService,
        private authService: AuthenticationService,
        private router: Router) { }

    ngOnInit() {

        this.loginForm = this.formBuilder.group({
            email: [null, [Validators.required, Validators.email]],
            password: [null, Validators.required],
        });

        this.adminLoginForm = this.formBuilder.group({
            email: [null, [Validators.required, Validators.email]],
            password: [null, Validators.required],
        });
    }

    userLogin() {
        var email = this.loginForm.value.email;
        var password = this.loginForm.value.password;

        this.authService.login(email, password).then(response => {
            var responseEntity = response as LoginResponse;
            if (responseEntity.isSuccess == true && responseEntity.role == 1) {
                this.handleSuccessUserLogin(responseEntity.token);
            } else {
                this.showError('Invalid email or password.');
            }
        });
    }

    handleSuccessUserLogin(jwtToken: string) {
        this.showSuccess();
        sessionStorage.setItem(SessionData.USER_LOGGED_IN, 'true');
        sessionStorage.setItem(SessionData.TOKEN, jwtToken);
        sessionStorage.setItem(SessionData.USER_ID, this.authService.getUserIdFromToken());
        this.authService.userLoggedin();
        this.router.navigate(['user/caff']);
    }

    adminLogin() {
        var email = this.adminLoginForm.value.email;
        var password = this.adminLoginForm.value.password;

        this.authService.login(email, password).then(response => {
            var responseEntity = response as LoginResponse;
            if (responseEntity.isSuccess == true && responseEntity.role == 0) {
                this.handleSuccessAdminLogin(responseEntity.token);
            } else {
                this.showError('Invalid email or password.');
            }
        });
    }

    handleSuccessAdminLogin(jwtToken: string) {
        this.showSuccess();
        sessionStorage.setItem(SessionData.ADMIN_LOGGED_IN, 'true');
        sessionStorage.setItem(SessionData.TOKEN, jwtToken);
        this.authService.adminLoggedin();
        this.router.navigateByUrl('admin/caff');
    }

    showSuccess() {
        this.toast.success('You have successfully logged in.', 'Success!', {
            timeOut: 3000,
        });
    }

    showError(message) {
        this.toast.error(message, 'Login failed');
    }

}
