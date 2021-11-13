import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from "@angular/material/input";
import { ReactiveFormsModule } from "@angular/forms";
import { MatIconModule } from "@angular/material/icon";
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';

import { ToastrModule } from 'ngx-toastr';

import { RegistrationComponent } from './registration/registration.component';
import { LoginComponent } from './login/login.component';
import { HeaderInterceptorService } from './services/header-interceptor.service';
import { UserHomeComponent } from './userPages/user-home/user-home.component';
import { AdminHomeComponent } from './adminPages/admin-home/admin-home.component';

import { AdminAuthGuard } from './guard/admin-auth.guard';
import { UserAuthGuard } from './guard/user-auth.guard';
import { UserprofileComponent } from './userPages/userprofile/userprofile.component';
import { CaffFilesComponent } from './userPages/caff-files/caff-files.component';
import { ChangePasswordDialogComponent } from './dialogs/change-password-dialog/change-password-dialog.component';
import { UploadImageComponent } from './dialogs/upload-image/upload-image.component';
import { CommentsDialogComponent } from './dialogs/comments-dialog/comments-dialog.component';
import { DeleteCaffDialogComponent } from './dialogs/delete-caff-dialog/delete-caff-dialog.component';
import { DownloadCaffDialogComponent } from './dialogs/download-caff-dialog/download-caff-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    RegistrationComponent,
    LoginComponent,
    UserHomeComponent,
    AdminHomeComponent,
    UserprofileComponent,
    CaffFilesComponent,
    ChangePasswordDialogComponent,
    UploadImageComponent,
    CommentsDialogComponent,
    DeleteCaffDialogComponent,
    DownloadCaffDialogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'signin', component: LoginComponent },
      { path: 'signup', component: RegistrationComponent },
      
      { path: 'admin/home', component: AdminHomeComponent, canActivate: [AdminAuthGuard] },

      { path: 'user/home', component: UserHomeComponent, canActivate: [UserAuthGuard] },
      { path: 'user/caff', component: CaffFilesComponent, canActivate: [UserAuthGuard] },
      { path: 'user/profile', component: UserprofileComponent, canActivate: [UserAuthGuard] },
    ]),
    BrowserAnimationsModule,
    ToastrModule.forRoot({      
      positionClass: 'toast-top-center',
      closeButton: true
    }),

    MatButtonModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatIconModule,
    MatSnackBarModule,
    MatDialogModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HeaderInterceptorService, multi: true },
  ],
  bootstrap: [AppComponent],
  entryComponents: [ChangePasswordDialogComponent,
    UploadImageComponent,
    CommentsDialogComponent,
    DeleteCaffDialogComponent,
    DownloadCaffDialogComponent
  ],
})
export class AppModule { }
