import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/core/models/User';
import { AdminService } from '../../admin.service';
import { Status } from 'src/app/core/enums/status';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class ManageUserListComponent implements OnInit {

  public Status = Status;
  public users: User[];
  
  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.getAllUsers();
  }

  private getAllUsers(){
    this.adminService.getAllUsers().subscribe((res: User[]) => {
      this.users = res;
    });
  }

  public changeStatus(user: User ,status: Status){
    this.adminService.changeUserStatus({userId: user.id, status: status}).subscribe(res => {
      user.isBlocked = status === Status.Block;
    });
  }
}
