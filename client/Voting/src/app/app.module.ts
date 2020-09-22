import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { QuestionItemComponent } from './questions/question-item/question-item.component';
import { QuestionListComponent } from './questions/question-list/question-list.component';
import { LoginComponent } from './auth/login/login.component';
import { JwtInterceptor } from './core/interceptors/jwt.interceptors';
import { QuestionAddComponent } from './questions/question-add/question-add.component';
import { RegisterComponent } from './register/register/register.component';
import { CommentItemComponent } from './questions/question-item/comment-item/comment-item.component';
import { HeaderComponent } from './core/share/header/header.component';
import { CoreModule } from './core/core.module';
import { ProfileModule } from './profile/profile.module';
import {NgxPaginationModule} from 'ngx-pagination';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

@NgModule({
  declarations: [
    AppComponent,
    QuestionItemComponent,
    QuestionListComponent,
    LoginComponent,
    QuestionAddComponent,
    RegisterComponent,
    CommentItemComponent,
    HeaderComponent
  ],
  imports: [
    CoreModule,
    ProfileModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    NgxPaginationModule,
    InfiniteScrollModule
  ],
  exports: [],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
