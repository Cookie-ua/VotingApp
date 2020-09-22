import { Injectable } from '@angular/core';
import * as jwt_decode from 'jwt-decode';
import { Role } from '../enums/role';

@Injectable({
  providedIn: 'root'
})
export class JwtHelperService {

  constructor() { }

  isTokenExpired(token: string): boolean{
    let decode = jwt_decode(token);
    return !(new Date() <= new Date(decode.exp * 1000));
  }
  
  hasRole(role: Role, token: string){
    let decode = jwt_decode(token);
    let hasRole: boolean = false;
    for (const key in decode) {
      if(key.indexOf('role') !== -1){
        hasRole = decode[key].includes(role);
        break;
      }
    }  
    return hasRole;
  }

  isAdmin(token: string){
    let decode = jwt_decode(token);
    let isAdmin: boolean = false;
    for (const key in decode) {
      if(key.indexOf('role') !== -1){
        isAdmin = decode[key].includes(Role.SuperAdmin);
        break;
      }
    }
    return isAdmin;
  }
}
