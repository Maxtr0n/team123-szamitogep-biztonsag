import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EndPoint } from './endpoints';
import { SessionData } from './sessionData';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class HeaderInterceptorService implements HttpInterceptor {

  constructor(private router: Router, private toast: ToastrService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    var url = req.url;
    var unAuthorizedEndpoint = [];
    unAuthorizedEndpoint.push(EndPoint.LOGIN_URL, EndPoint.REGISTER_URL);
    if (!unAuthorizedEndpoint.includes(url)) {
      var token = sessionStorage.getItem(SessionData.TOKEN);
      const modifiedReq = req.clone({
        headers: req.headers.set('Authorization', 'Bearer ' + token),
      });
      return next.handle(modifiedReq).pipe(tap(() => { },
      (err: any) => {
        if (err instanceof HttpErrorResponse) {
          this.handleErrorResponse(err);
        }
      }));;
    }
    return next.handle(req);
  }

  handleErrorResponse(errorResponse: HttpErrorResponse) {
    if (errorResponse.status === 401 || errorResponse.status === 403) {
      sessionStorage.clear();
      this.showError('You have logged out.')
      this.router.navigate(['signin']);
    }
  }

  showError(message: string) {
    this.toast.error(message, 'Error');
  }
}
