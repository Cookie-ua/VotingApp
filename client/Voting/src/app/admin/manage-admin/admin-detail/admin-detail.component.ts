import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from 'src/app/core/enums/status';
import { User } from 'src/app/core/models/User';
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-admin-detail',
  templateUrl: './admin-detail.component.html',
  styleUrls: ['./admin-detail.component.css']
})
export class ManageAdminDetailComponent implements OnInit {

  private userId: string;
  public user: User;
  public Status = Status;
  resetPasswordForm: FormGroup;
  

  constructor(private route: ActivatedRoute,
              private router: Router,
              private adminService: AdminService,
              private fb: FormBuilder) { 
                this.getUserId();
                this.resetPasswordForm = this.fb.group({
                  'userId': [this.userId, Validators.required],
                  'password': [null, Validators.required],
                  'confirmPassword': [null, Validators.required]
                });
              }

  ngOnInit() {
    this.getAdminById();
  }

  private getUserId(){
    this.userId = this.route.snapshot.paramMap.get('id');
  }

  private getAdminById(){
    this.adminService.getAdminById(this.userId).subscribe(res => {
      this.user = res;
    });
  }

  public resetPassword(){
    if(this.resetPasswordForm.valid){
      this.adminService.resetPassword(this.resetPasswordForm.value).subscribe(res => {
        this.resetPasswordForm.reset();
      });
    }  
  }

  public changeAdminStatus(user: User ,status: Status){
    this.adminService.changeAdminStatus({userId: user.id, status: status}).subscribe(res => {
      user.isBlocked = status === Status.Block;
    });
  }

  public makeUser(){
    this.adminService.makeUserAdmin({userId: this.user.id, role: 'User'}).subscribe(res => {
      this.router.navigate(['./admin/manage-admin/list']);
    });
  }
}
