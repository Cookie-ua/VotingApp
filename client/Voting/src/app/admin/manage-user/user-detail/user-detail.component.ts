import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Role } from 'src/app/core/enums/role';
import { Status } from 'src/app/core/enums/status';
import { User } from 'src/app/core/models/User';
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class ManageUserDetailComponent implements OnInit {

  private userId: string;
  public user: User;
  public Status = Status;
  public Role = Role;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private adminService: AdminService) { }

  ngOnInit() {
    this.getUserId();
    this.getUserById();
  }

  private getUserId(){
    this.userId = this.route.snapshot.paramMap.get('id');
  }

  public getUserById(){
    this.adminService.getUserById(this.userId).subscribe((res: User) => {
      this.user = res;
    });
  }

  public changeStatus(user: User ,status: Status){
    this.adminService.changeUserStatus({userId: user.id, status: status}).subscribe(res => {
      user.isBlocked = status === Status.Block;
    });
  }

  public makeAdmin(){
    this.adminService.makeUserAdmin({userId: this.user.id, role: 'Admin'}).subscribe(res => {
      this.router.navigate(['./admin/manage-user/list']);
    });
  }
}
