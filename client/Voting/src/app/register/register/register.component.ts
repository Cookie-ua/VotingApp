import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService } from '../register.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;

  constructor(private fb: FormBuilder,
              private registerService: RegisterService,
              private router: Router) { 
    this.registerForm = this.fb.group({
      'email': [null, Validators.required],
      'password': [null, Validators.required],
      'confirmPassword': [null, Validators.required]
    });
  }

  ngOnInit() {
  }

  register(){
    if(this.registerForm.valid){
      this.registerService.register(this.registerForm.value).subscribe(res => {
        this.router.navigate(['/login']);
      });
    }
  }
}