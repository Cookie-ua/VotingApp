import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  
  constructor(private fb: FormBuilder,
              private authServise: AuthService,
              private router: Router) { 
                this.loginForm = this.fb.group({
                  'email': [''],
                  'password': ['']
                }); 
              }

  ngOnInit() {
  }

  onSubmit(){
    this.authServise.login('/auth/login', this.loginForm.value).subscribe(res => {
      if(res){
        localStorage.setItem('auth_token', res.auth_token);
        this.router.navigate(['/question-list']);
      }
    });
  }

}
