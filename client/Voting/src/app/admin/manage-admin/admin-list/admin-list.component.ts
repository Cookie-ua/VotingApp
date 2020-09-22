import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/core/models/User';
import { AdminService } from '../../admin.service';
import { Status } from 'src/app/core/enums/status';

@Component({
  selector: 'app-admin-list',
  templateUrl: './admin-list.component.html',
  styleUrls: ['./admin-list.component.css']
})
export class ManageAdminListComponent implements OnInit {

  public users: User[];
  public Status = Status; 

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.getAllAdmins();
  }

  private getAllAdmins() {
    this.adminService.getAllAdmins().subscribe((res: User[]) => {
      this.users = res;
    });
  }

  changeStatus(user: User ,status: Status){
    this.adminService.changeAdminStatus({userId: user.id, status: status}).subscribe(res => {
      user.isBlocked = status === Status.Block;
    });
  }
}
