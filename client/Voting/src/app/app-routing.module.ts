import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { QuestionListComponent } from './questions/question-list/question-list.component';
import { QuestionItemComponent } from './questions/question-item/question-item.component';
import { AuthGuard } from './auth/auth.guard';
import { QuestionAddComponent } from './questions/question-add/question-add.component';
import { RegisterComponent } from './register/register/register.component';
import { Role } from './core/enums/role';

const routes: Routes = [
  {path: "", redirectTo: "/question/list", pathMatch: "full"},
  {path: "register", component: RegisterComponent},
  {path: "login", component: LoginComponent},
  {path: "question/add", component: QuestionAddComponent, canActivate: [AuthGuard]},
  {path: "question/list", component: QuestionListComponent, canActivate: [AuthGuard]},
  {path: 'question/:id', component: QuestionItemComponent, canActivate: [AuthGuard]},
  {
    path: 'admin', 
    loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule),
    canLoad: [AuthGuard],
    data: {
      roles: [
        Role.SuperAdmin,
        Role.Admin
      ]
    }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
