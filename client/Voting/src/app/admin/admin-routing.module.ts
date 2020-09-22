import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './admin/admin.component';
import { AuthGuard } from '../auth/auth.guard';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { Role } from '../core/enums/role';
import { ManageQuestionListComponent } from './manage-question/question-list/question-list.component';
import { ManageQuestionDetailComponent } from './manage-question/question-detail/question-detail.component';
import { ManageQuestionAddComponent } from './manage-question/question-add/question-add.component';
import { ManageUserListComponent } from './manage-user/user-list/user-list.component';
import { ManageUserDetailComponent } from './manage-user/user-detail/user-detail.component';
import { ManageAdminListComponent } from './manage-admin/admin-list/admin-list.component';
import { ManageAdminAddComponent } from './manage-admin/admin-add/admin-add.component';
import { ManageAdminDetailComponent } from './manage-admin/admin-detail/admin-detail.component';


const adminRoutes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AuthGuard],
    children: [
      {path: '', component: AdminDashboardComponent},
      {path: 'manage-question/add', component: ManageQuestionAddComponent},
      {path: 'manage-question/list', component: ManageQuestionListComponent},
      {path: 'manage-question/:id', component: ManageQuestionDetailComponent},
      {path: 'manage-user/list', component: ManageUserListComponent},
      {path: 'manage-user/:id', component: ManageUserDetailComponent},
      {path: 'manage-admin/list', component: ManageAdminListComponent, canActivate: [AuthGuard], data: { roles: [ Role.SuperAdmin ] } },
      {path: 'manage-admin/add', component: ManageAdminAddComponent, canActivate: [AuthGuard], data: { roles: [ Role.SuperAdmin ] } },
      {path: 'manage-admin/:id', component: ManageAdminDetailComponent, canActivate: [AuthGuard], data: { roles: [ Role.SuperAdmin ] } }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(adminRoutes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
