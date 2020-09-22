import { Injectable } from '@angular/core';
import { ApiService } from '../core/services/api.service';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {

  getQuest(){
    return this.apiService.get('/question/test');
  }

  constructor(private apiService: ApiService) { }
  
  getQuestions(page: any = null){
    return this.apiService.get(`/question/get-questions?page=${page}`);
  }
  
  getQuestion(id: any){
    return this.apiService.get(`/question/get-question/${id}`);
  }

  addQuestion(question: any){
    return this.apiService.post('/question/add-question-user', question);
  }

  getComments(page: any = null, questionId: number){
    return this.apiService.get(`/comment/get-comments?page=${page}&questionId=${questionId}`)
  }

  voteForQuestion(vote: any){
    return this.apiService.post('/question/vote-for-question', vote)
  }
  
  loadAllComments(commentId: any){
    return this.apiService.get(`/comment/get-all-child-comments/${commentId}`)
  }

  deleteComment(id: any){
    return this.apiService.delete(`/comment/delete-comment/${id}`);
  }
}
