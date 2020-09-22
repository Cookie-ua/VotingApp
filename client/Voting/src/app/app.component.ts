import { Component, OnInit } from '@angular/core';
import { AuthService } from './auth/auth.service';
import { Role } from './core/enums/role';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Voting';
  public Role = Role;
  
  constructor(private authService: AuthService){
  }

  ngOnInit(){
  }

  loginAsAdmin(){
    this.authService.login('/auth/login', {email: 'test@test.com', password: 'Qwerty_123'}).subscribe(res => {
      if(res){
        localStorage.setItem('auth_token', res.auth_token);
      }
    });
  }
  
  loginAsUser(){
    this.authService.login('/auth/login', {email: 'test-user-1@test.com', password: 'Qwerty_12345'}).subscribe(res => {
      if(res){
        localStorage.setItem('auth_token', res.auth_token);
      }
    });
  }

  logout(){
    this.authService.logout();
  }
}
