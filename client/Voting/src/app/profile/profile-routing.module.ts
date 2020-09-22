import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuard } from '../auth/auth.guard';
import { Role } from '../core/enums/role';
import { ProfileQuestionsComponent } from './monitoring/questions/questions.component';
import { ProfileCommentsComponent } from './monitoring/comments/comments.component';
import { ProfileVotesComponent } from './monitoring/votes/votes.component';
import { ProfileMainComponent } from './main/main.component';


const profileRoutes: Routes = [
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard],
    data: { roles: [Role.User] },
    children: [
      {path: '', component: ProfileMainComponent},
      {path: 'my-questions', component: ProfileQuestionsComponent},
      {path: 'my-comments', component: ProfileCommentsComponent},
      {path: 'my-votes', component: ProfileVotesComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(profileRoutes)],
  exports: [RouterModule]
})
export class ProfileRoutingModule { }
