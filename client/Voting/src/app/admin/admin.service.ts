import { Injectable } from '@angular/core';
import { ApiService } from '../core/services/api.service';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private apiService: ApiService) { }

  getDashboardInfo(){
    return this.apiService.get('/admin/get-admin-info');
  }

  getUserById(id: any){
    return this.apiService.get(`/admin/get-user/${id}`);
  }

  getAllUsers(){
    return this.apiService.get('/admin/get-all-users');
  }
  
  getAdminById(id: any){
    return this.apiService.get(`/admin/get-admin/${id}`);
  }

  getAllAdmins(){
    return this.apiService.get('/admin/get-all-admins');
  }

  addAdmin(admin: any){
    return this.apiService.post('/account/register-admin', admin)
  }

  changeAdminStatus(model: any){
    return this.apiService.post('/admin/change-admin-status', model)
  }

  makeUserAdmin(model: any){
    return this.apiService.post('/admin/change-role-admin', model);
  }
  
  resetPassword(model: any){
    return this.apiService.post('/admin/reset-password', model)
  }

  changeUserStatus(model: any){
    return this.apiService.post('/admin/change-user-status', model)
  }

  getQuestions(page: any = null){
    return this.apiService.get(`/question/get-questions-admin?page=${page}`);
  }

  getQuestionById(id: any){
    return this.apiService.get(`/question/get-question-admin/${id}`);
  }
  
  addQuestion(question: any){
    return this.apiService.post('/question/add-question-admin', question)
  }

  updateQuestion(question: any){
    return this.apiService.post('/question/update-question-admin', question)
  }

  changeQuestionStatus(model: any){
    return this.apiService.post('/question/change-question-status', model)
  } 

  deleteQuestionById(id: any){
    return this.apiService.delete(`/question/delete-question-admin/${id}`)
  }
}
