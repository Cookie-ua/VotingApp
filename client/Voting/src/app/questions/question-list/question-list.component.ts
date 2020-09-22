import { Component, OnInit } from '@angular/core';
import { QuestionService } from '../question.service';
import { Question, Answer } from 'src/app/core/models/Question';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { VoteModel } from 'src/app/core/models/Vote';
import { Role } from 'src/app/core/enums/role';
import { PageResult } from 'src/app/core/models/pageResult';

@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.css']
})
export class QuestionListComponent implements OnInit {

  public date = Date;
  public Role = Role;
  get math() { return Math; };  
  public questionsPageResult: PageResult<Question>;
  private hubConnection: signalR.HubConnection

  constructor(private questionService: QuestionService) { }

  ngOnInit() {
    this.getQuestions();
    this.startConnection();
    this.addedQuestionListener();
    this.deleteQuestionListener();
    this.addVotedForQuestionListener();
  }

  //SignalR
  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                          .withUrl(`${environment.hub_url}/question-hub`)
                          .configureLogging(signalR.LogLevel.Information)  
                          .build();

    this.hubConnection
      .start()
      .catch(err => console.error(err.toString()));
  }
  public addedQuestionListener = () => {
    this.hubConnection.on('QuestionAdded', (question: Question) => {
      this.questionsPageResult.items.unshift(question);
    });
  }
  public deleteQuestionListener = () => {
    this.hubConnection.on('QuestionDeleted', (questionId: number) => {
      debugger;
      var el = this.questionsPageResult.items.map(x => x.id).indexOf(questionId);
      this.questionsPageResult.items.splice(el, 1);
    });
  }
  public addVotedForQuestionListener = () => {
    this.hubConnection.on('VotedForQuestion', (vote: VoteModel) => {
      let question: Question = this.questionsPageResult.items.find(x => x.id === vote.questionId);
      this.updateVote(vote, question);
    });
  }

  private getQuestions(){
    this.questionService.getQuestions().subscribe((res: PageResult<Question>) => {
      this.questionsPageResult = res;
      // res.items.forEach(el => {
      //   console.log()
      // }) 
    })
  }

  public onPageChange = (pageNumber) => {
    this.questionService.getQuestions(pageNumber).subscribe((res: PageResult<Question>) => {
      this.questionsPageResult = res;
    })
  }
  
  public voteForQuestion(question: Question, answer: any){
    if (new Date(question.expiryDate).getTime() > new Date().getTime()) {
      this.questionService.voteForQuestion({questionId: question.id, answer: answer}).subscribe();      
    }
  }

  private updateVote(vote: VoteModel, question: Question){
    if (vote.type) {
      let res = question.answers.find(x => x.answer === vote.newAnswer);
      res.count++;
      res.voted = true;
      question.totalCount++;
    } else {
      let oldAnswer = question.answers.find(x => x.answer === vote.oldAnswer); 
      oldAnswer.count--;
      oldAnswer.voted = false;
      let newAnswer = question.answers.find(x => x.answer === vote.newAnswer);
      newAnswer.count++;
      newAnswer.voted = true;
    }
  }

  public check(question: Question, count: number){
    let isExpired = new Date(question.expiryDate).getTime() < new Date().getTime();
    let res = Math.max(...question.answers.map(x => x.count)) === count;
    return isExpired && res
  }
}