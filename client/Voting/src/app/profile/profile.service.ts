import { Injectable } from '@angular/core';
import { ApiService } from '../core/services/api.service';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private apiService: ApiService) { }

  getUserInfo(){
    return this.apiService.get('/account/get-user');
  }

  getAllQuestions(){
    return this.apiService.get('/question/get-questions-user')
  }

  getAllComments(){
    return this.apiService.get('/comment/get-comments-by-user')
  }

  getAllVotes(){
    return this.apiService.get('/question/get-votes-by-user')
  }
  
  changePassword(model: any){
    return this.apiService.post('/account/change-password', model);
  }

  deleteAccount(model: any){
    return this.apiService.post('/account/delete-account', model);
  }
}
