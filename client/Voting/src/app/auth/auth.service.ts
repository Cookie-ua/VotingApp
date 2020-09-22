import { Injectable } from '@angular/core';
import { ApiService } from '../core/services/api.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private user: boolean;
  
  constructor(private apiService: ApiService,
              private router: Router) { }

  login(path: string, body: any){
    this.user = true;
    return this.apiService.post(path, body);
  }
  
  logout(){
    this.router.navigate(['/login']);
    localStorage.removeItem('auth_token');
  }
}
