import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AdminService } from '../../admin.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-add',
  templateUrl: './admin-add.component.html',
  styleUrls: ['./admin-add.component.css']
})
export class ManageAdminAddComponent implements OnInit {

  addAdminForm: FormGroup;

  constructor(private adminService: AdminService,
              private fb: FormBuilder,
              private router: Router) { 
                this.addAdminForm = this.fb.group({
                  'email': [null, Validators.required],
                  'password': [null, Validators.required],
                  'confirmPassword': [null, Validators.required]
                })
              }
  
  ngOnInit() {
  }

  addAdmin(){
    if (this.addAdminForm.valid) {
      this.adminService.addAdmin(this.addAdminForm.value).subscribe(res => {
        this.router.navigate(['./admin/manage-admin/list']);
      });
    }
  }
}
