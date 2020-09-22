import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileRoutingModule } from './profile-routing.module';

import { ProfileComponent } from './profile/profile.component';
import { ProfileQuestionsComponent } from './monitoring/questions/questions.component';
import { ProfileCommentsComponent } from './monitoring/comments/comments.component';
import { ProfileVotesComponent } from './monitoring/votes/votes.component';
import { ProfileMainComponent } from './main/main.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    ProfileComponent,
    ProfileQuestionsComponent,
    ProfileCommentsComponent,
    ProfileVotesComponent,
    ProfileMainComponent
  ],
  imports: [
    CommonModule,
    ProfileRoutingModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class ProfileModule { }
