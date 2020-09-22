import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router, CanLoad, Route } from '@angular/router';
import { Observable } from 'rxjs';
import * as jwt_decode from 'jwt-decode';
import { JwtHelperService } from '../core/services/jwt-helper.service';
import { Role } from '../core/enums/role';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanLoad {
  
  constructor(private router: Router, 
              private jwtHelperService: JwtHelperService){}
  
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
  
    let token = localStorage.getItem('auth_token'); 
    if (token === null) {
      this.router.navigate(['/login']);
    }

    if (this.jwtHelperService.isTokenExpired(token)) {
      localStorage.removeItem('auth_token');
      this.router.navigate(['/login']);  
      return false;
    }

    const roles = route.data.roles as Role[];
    if (roles && !roles.some(r => this.jwtHelperService.hasRole(r, token))) {
        this.router.navigate(['/']);
        return false;
    }
    
    return true;
  }
  
  canLoad(route: Route): boolean | Observable<boolean> | Promise<boolean> {
    let token = localStorage.getItem('auth_token');    
    if (!token && this.jwtHelperService.isTokenExpired(token)) {
      localStorage.removeItem('auth_token');
      this.router.navigate(['/login']);  
      return false;
    }
    const roles = route.data.roles as Role[];
    if (roles && !roles.some(r => this.jwtHelperService.hasRole(r, token))) {
        this.router.navigate(['/']);
        return false;
    }
    
    return true;
  }

}
