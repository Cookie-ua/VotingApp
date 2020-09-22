import { Component, OnInit } from '@angular/core';
// import { QuestionModel } from 'src/app/core/models/question.model';
import { AdminService } from '../../admin.service';
import { Status } from 'src/app/core/enums/status';
import { PageResult } from 'src/app/core/models/pageResult';
import { Question } from 'src/app/core/models/Question';

@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.css']
})
export class ManageQuestionListComponent implements OnInit {

  public Status = Status; 
  public questions: Question[];
  public pageResult: PageResult<Question>;

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.getQuestions();
  }
  
  private getQuestions(){
    this.adminService.getQuestions().subscribe((res: PageResult<Question>) => {
      this.pageResult = res;
    });
  }
  
  public onPageChange = (pageNumber) => {
    this.adminService.getQuestions(pageNumber).subscribe((res: PageResult<Question>) => {
      this.pageResult = res;
    })
  }

  public changeQuestionStatus(question: Question, status: Status){
    this.adminService.changeQuestionStatus({id: question.id, status: status}).subscribe(res => {
      question.isActive = status !== Status.Block;
    });
  }

  public isExpired(question: Question): boolean{
    return new Date(question.expiryDate).getTime() < new Date().getTime();
  }
}
