import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor() { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const headersConfig = {
            'Accept': 'application/json'
          };
        
        let access_token = localStorage.getItem('auth_token');
        if (access_token) {
            headersConfig['Authorization'] = `Bearer ${access_token}`;
        }  
        request =  request.clone({setHeaders: headersConfig});

        return next.handle(request);
    }
}