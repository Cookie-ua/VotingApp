import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoreModule } from '../core/core.module';

import { AdminRoutingModule } from './admin-routing.module';

import { AdminComponent } from './admin/admin.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { ManageQuestionListComponent } from './manage-question/question-list/question-list.component';
import { ManageQuestionDetailComponent } from './manage-question/question-detail/question-detail.component';
import { ManageQuestionAddComponent } from './manage-question/question-add/question-add.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ManageUserListComponent } from './manage-user/user-list/user-list.component';
import { ManageUserDetailComponent } from './manage-user/user-detail/user-detail.component';
import { ManageAdminListComponent } from './manage-admin/admin-list/admin-list.component';
import { ManageAdminAddComponent } from './manage-admin/admin-add/admin-add.component';
import {NgxPaginationModule} from 'ngx-pagination';
import { ManageAdminDetailComponent } from './manage-admin/admin-detail/admin-detail.component';

@NgModule({
  declarations: [
    AdminComponent,
    AdminDashboardComponent,
    ManageQuestionListComponent,
    ManageQuestionDetailComponent,
    ManageQuestionAddComponent,
    ManageUserListComponent,
    ManageUserDetailComponent,
    ManageAdminListComponent,
    ManageAdminAddComponent,
    ManageAdminDetailComponent
  ],
  imports: [
    CommonModule,
    CoreModule,
    AdminRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    NgxPaginationModule
  ]
})
export class AdminModule { }
