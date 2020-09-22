import { Component, Input, OnInit } from '@angular/core';
import { Comment } from 'src/app/core/models/Comment';
import { Question } from 'src/app/core/models/Question';
import { QuestionService } from '../question.service';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import * as signalR from '@microsoft/signalr';
import { PageResult } from 'src/app/core/models/pageResult';

@Component({
  selector: 'app-question-item',
  templateUrl: './question-item.component.html',
  styleUrls: ['./question-item.component.css']
})
export class QuestionItemComponent implements OnInit {

  private questionId: number;
  public question: Question;
  public comments: Comment[];
  public pageIndex: number = 1;
  public loading: boolean = false;
  
  private hubConnection: signalR.HubConnection;

  commentForm: FormGroup;

  constructor(private questionService: QuestionService,
              private route: ActivatedRoute,
              private fb: FormBuilder) { 
                this.commentForm = this.fb.group({
                  'message': ['', Validators.required]
                }); 
              }

  ngOnInit() {
    this.getQuestionId();
    this.startConnection();
    this.addedCommentListener();
    this.updatedCommentListener();
    this.deletedCommentListener();
    this.getQuestion();
    this.getComments();
  }

  private getQuestionId(){
    this.questionId = Number.parseInt(this.route.snapshot.paramMap.get('id'));
  }

  private getQuestion(){
    this.questionService.getQuestion(this.questionId).subscribe((res: Question) => {
      this.question = res;
    });
  }

  private getComments(){
    this.loading = true;
    this.questionService.getComments(null, this.questionId).subscribe((res: PageResult<Comment>) => {
      this.comments = res.items;
      this.loading = false;
    }, error => {
      this.loading = false;
    });
  }

  onScroll() {
    this.loading = true;
    this.questionService.getComments(++this.pageIndex, this.questionId).subscribe((res: PageResult<Comment>) => {
      this.comments = this.comments.concat(res.items);
      // res.items.forEach(x => {
      //   this.comments.push(x);
      // });
      this.loading = false;
    }, error => {
      this.loading = false;
    });
  }

  public startConnection = () => {
    var auth_token = localStorage.getItem("auth_token");
    this.hubConnection = new signalR.HubConnectionBuilder()
                          .withUrl(`${environment.hub_url}/comment-hub`, { accessTokenFactory: () => auth_token })
                          .withAutomaticReconnect()
                          .configureLogging(signalR.LogLevel.Information)  
                          .build();

    this.hubConnection
      .start()
      .catch(err => console.error(err.toString()));
  }
  public addedCommentListener = () => {
    this.hubConnection.on('AddCommentAsync', (comment: Comment) => {
      if (comment.questionId === this.questionId) {
        if (comment.commentId !== null) {
          var c = this.comments.find(x => x.id == comment.commentId);
          if (c.comments !== undefined) {
            c.comments.unshift(comment);            
          }
          c.answersCount++;
          return;          
        }

        this.comments.unshift(comment);   
        this.commentForm.reset();     
        console.log(this.comments)
      };
    });
  }
  public updatedCommentListener = () => {
    this.hubConnection.on('UpdateComment', (comment: Comment) => {
      if (comment.questionId === this.questionId) {
        if (comment.commentId !== null) {
          let parentComment = this.comments.find(x => x.id === comment.commentId);
          let childComment = parentComment.comments.find(x => x.id === comment.id);
          childComment.message = comment.message;
          return;
        }
        let updatedComment = this.comments.find(x => x.id === comment.id);
        updatedComment.message = comment.message;
      };
    });
  }
  public deletedCommentListener = () => {
    this.hubConnection.on('CommentDeletedAsync', (id: number, commentId: any) => {
      if(commentId !== null){
        let el = this.comments.find(x => x.id === commentId);
        let del = el.comments.map(x => x.id).indexOf(id);
        el.comments.splice(del, 1);
        el.answersCount--;
        return;
      }
       let el = this.comments.map(x => x.id).indexOf(id);
       this.comments.splice(el, 1);
    });
  }
  
  sendComment(){
    if (this.commentForm.valid) {
      this.hubConnection.invoke("AddComment", { 
        "message": this.commentForm.get("message").value, 
        "questionId": this.questionId
      });
    }
  }

  sendChildComment({message, commentId}){
    this.hubConnection.invoke("AddComment", { 
      "commentId": commentId,
      "message": message, 
      "questionId": this.questionId
    });
  }

  loadAnswers(commentId: any){
    this.questionService.loadAllComments(commentId).subscribe((res: Comment[]) => {
      this.comments.find(x => x.id == commentId).comments = res;
    })
  }

  updateComment(comment: Comment){
    this.hubConnection.invoke("UpdateComment", {
      "id": comment.id, 
      "message": comment.message, 
      "questionId": comment.questionId
    })
  }
  
  deleteComment(commentId: any){
    this.questionService.deleteComment(commentId).subscribe();
  }
}