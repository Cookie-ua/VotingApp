import { Component, OnInit } from '@angular/core';
import { ProfileService } from '../profile.service';
import { User } from 'src/app/core/models/User';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class ProfileMainComponent implements OnInit {

  public user: User;
  changePasswordForm: FormGroup;
  public showDeletedForm: boolean = false;
  deleteAccountForm: FormGroup;
  
  constructor(private profileService: ProfileService,
              private fb: FormBuilder) { 
                this.changePasswordForm = this.fb.group({
                  'currentPassword': [null, Validators.required],
                  'newPassword': [null, Validators.required],
                  'confirmPassword': [null, Validators.required]
                });

                this.deleteAccountForm = this.fb.group({
                  'password': [null, Validators.required]
                })
              }

  ngOnInit() {
    this.getUserInfo();
  }

  private getUserInfo(){
    this.profileService.getUserInfo().subscribe((res: User) => {
      this.user = res;
    });
  }

  changePassword(){
    if (this.changePasswordForm.valid) {
      this.profileService.changePassword(this.changePasswordForm.value).subscribe(res => {
        this.changePasswordForm.reset();
        
      });
    }
  }

  resetDeleteForm(){
    this.showDeletedForm = !this.showDeletedForm;
    this.deleteAccountForm.reset();
  }

  deleteAccount(){
    if (this.deleteAccountForm.valid) {
      this.profileService.deleteAccount(this.deleteAccountForm.value).subscribe();
    }
  }
}
