import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../admin.service';
import { Answer, Question } from 'src/app/core/models/Question';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from 'src/app/core/enums/status';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-question-detail',
  templateUrl: './question-detail.component.html',
  styleUrls: ['./question-detail.component.css']
})
export class ManageQuestionDetailComponent implements OnInit {

  private questionId: number;
  public question: Question;
  public Status = Status; 
  public loading: boolean = false;
  public isUpdate: boolean = false;

  public minutes: Array<number>;
  public hours: Array<number>;
  public days: Array<number>;

  updateQuestionForm: FormGroup;
  
  constructor(private adminService: AdminService,
              private fb: FormBuilder,
              private route: ActivatedRoute,
              private router: Router) {
                this.minutes = Array(55).fill(0).map((x, i) => 5 + i);
                this.hours = Array(24).fill(0).map((x, i) => i);
                this.days = Array(31).fill(0).map((x, i) => i);
              }
  
  ngOnInit() {
    this.getQuestionId();
    this.getQuestion();
  }

  private getQuestionId(){
    this.questionId = Number.parseInt(this.route.snapshot.paramMap.get('id'));
  }

  private getQuestion(){
    this.loading = true;
    this.adminService.getQuestionById(this.questionId).subscribe((res: Question) => {
      this.loading = false;
      this.question = res;
    }, error => {
      this.loading = false;
    });
  }

  public initUpdateForm(){
    if(this.updateQuestionForm == null){
      var seconds = (new Date(this.question.expiryDate).getTime() - new Date().getTime()) / 1000;
      var d = Math.floor(seconds / (3600*24));
      var h = Math.floor(seconds % (3600*24) / 3600);
      var m = Math.floor(seconds % 3600 / 60);

      this.updateQuestionForm = this.fb.group({
        'id': [this.question.id, Validators.required],
        'title': [this.question.title, Validators.required],
        'description': [this.question.description, Validators.required],
        'days': [d < 0 ? 0 : d, Validators.required],
        'hours': [h < 0 ? 0 : h, Validators.required],
        'minutes': [m < 5 ? 5 : m, Validators.required],
        'answers': this.fb.array([])
      });
      this.question.answers.forEach(answer => {
        (<FormArray>this.updateQuestionForm.controls['answers']).push(new FormControl(answer.answer, Validators.required))
      })
    }
  }

  public addAnswerInput(){
    const arrayControl = <FormArray>this.updateQuestionForm.controls['answers'];
    arrayControl.push(new FormControl("", Validators.required));
  }

  public deleteAnswerInput(index: number) {
    const arrayControl = <FormArray>this.updateQuestionForm.controls['answers'];
    if (arrayControl.length > 2) {
      arrayControl.removeAt(index); 
    }    
  }

  public updateQuestion(){
    this.adminService.updateQuestion(this.updateQuestionForm.value).subscribe(res => {
      this.isUpdate = false;
      this.getQuestion();
    });
  }

  public changeQuestionStatus(question: Question, status: Status){
    this.adminService.changeQuestionStatus({id: question.id, status: status}).subscribe(res => {
      question.isActive = status !== Status.Block;
    });
  }

  public deleteQuestion(questionId: any){
    this.adminService.deleteQuestionById(questionId).subscribe(res => {
      this.router.navigate(['./admin/manage-question/list']);      
    });
  }

  public isExpired(expiryDate): boolean{
    return new Date(expiryDate).getTime() < new Date().getTime();
  }
  
  public check(question: Question, count: number){
    let isExpired = new Date(question.expiryDate).getTime() < new Date().getTime();
    let res = Math.max(...question.answers.map(x => x.count)) === count;
    return isExpired && res
  }
}