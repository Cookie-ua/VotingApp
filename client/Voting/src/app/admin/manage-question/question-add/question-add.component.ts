import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { AdminService } from '../../admin.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-question-add',
  templateUrl: './question-add.component.html',
  styleUrls: ['./question-add.component.css']
})
export class ManageQuestionAddComponent implements OnInit {

  public minutes: Array<number>;
  public hours: Array<number>;
  public days: Array<number>;
  addQuestionForm: FormGroup;
  
  constructor(private adminService: AdminService, 
              private fb: FormBuilder,
              private router: Router) { 
                this.minutes = Array(55).fill(0).map((x, i) => 5 + i);
                this.hours = Array(24).fill(0).map((x, i) => i);
                this.days = Array(31).fill(0).map((x, i) => i);
                
                this.addQuestionForm = this.fb.group({
                  'title': [null, Validators.required],
                  'description': [null, Validators.required],
                  'days': [30, Validators.required],
                  'hours': [23, Validators.required],
                  'minutes': [59, Validators.required],
                  'answers': fb.array([
                    ['', Validators.required],
                    ['', Validators.required]
                  ])
                })
              }

  ngOnInit() {
  }

  addAnswerInput(){
    const arrayControl = <FormArray>this.addQuestionForm.controls['answers'];
    arrayControl.push(new FormControl("", Validators.required));
  }

  deleteAnswerInput(index: number) {
    const arrayControl = <FormArray>this.addQuestionForm.controls['answers'];
    if (arrayControl.length > 2) {
      arrayControl.removeAt(index); 
    }    
  }

  addQuestion(){
    if (this.addQuestionForm.valid) {
      this.adminService.addQuestion(this.addQuestionForm.value).subscribe(res => {
        this.router.navigate(['./admin/manage-question/list']);
      });
    }
  }
}
