import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';
import { Role } from '../../enums/role';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  public Role = Role;

  constructor(private authService: AuthService) { }

  ngOnInit() {}

  logout(){
    this.authService.logout();
  }
}
