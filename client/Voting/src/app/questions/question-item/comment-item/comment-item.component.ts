import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Role } from 'src/app/core/enums/role';
import { Comment } from 'src/app/core/models/Comment';

@Component({
  selector: 'app-comment-item',
  templateUrl: './comment-item.component.html',
  styleUrls: ['./comment-item.component.css']
})
export class CommentItemComponent implements OnInit {

  @Input() comment: Comment;
  @Input() parentId: any;
  @Output() delete = new EventEmitter<number>();
  @Output() update = new EventEmitter<any>();
  @Output() load = new EventEmitter<any>();
  @Output() send = new EventEmitter<any>();

  public showCommentForm: boolean = false;
  public showChildComments: boolean = false;
  public readMore: boolean = false;
  public isUpdate: boolean = false;
  public Role = Role;
  public replyComment: string;
  public updateMessage: string;
  public comm: Comment;

  constructor() { }

  ngOnInit() {
    this.comm = this.comment;
  }

  public updateComment(updatedComment: string, id: number){
    // this.comment.message = comment;
    this.update.emit({message: updatedComment, id: id});
    this.isUpdate = false;
  }

  public updateChildComment(updatedComment: string){
    // this.comment.message = comment;
    this.update.emit(updatedComment);
    this.isUpdate = false;
  }

  public deleteComment(commentId: any){
    this.delete.emit(commentId);
  }

  public loadAnswers(commentId: any){
    this.load.emit(commentId);
    this.showChildComments = true;
  }

  public sendComment(){
    this.send.emit({message: this.replyComment, commentId: this.comment.id});
    this.showCommentForm = false;
    this.replyComment = null;
  }
}
