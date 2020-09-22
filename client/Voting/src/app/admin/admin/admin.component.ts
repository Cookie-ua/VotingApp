import { Component, OnInit } from '@angular/core';
import { Role } from 'src/app/core/enums/role';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  public Role = Role;

  constructor() { }

  ngOnInit() {
  }

}
