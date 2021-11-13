import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EndPoint } from './endpoints';

@Injectable({
  providedIn: 'root'
})
export class HeaderInterceptorService implements HttpInterceptor {

  constructor() { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    var url = req.url;
    if (url !== EndPoint.LOGIN_URL) {
      const modifiedReq = req.clone({ 
        headers: req.headers.set('Authorization', 'Basic '),
      });
      return next.handle(modifiedReq);
    }
    return next.handle(req);
  }
}
