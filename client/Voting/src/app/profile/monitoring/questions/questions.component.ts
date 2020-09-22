import { Component, OnInit } from '@angular/core';
import { Question } from 'src/app/core/models/Question';
import { ProfileService } from '../../profile.service';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.css']
})
export class ProfileQuestionsComponent implements OnInit {

  public questions: Question[];

  constructor(private profileService: ProfileService) { }

  ngOnInit() {
    this.getAllQuestionsByUser();
  }
  
  private getAllQuestionsByUser(){
    this.profileService.getAllQuestions().subscribe((res: Question[]) => {
      this.questions = res;
    });
  }
}
