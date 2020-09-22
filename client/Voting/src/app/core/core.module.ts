import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthUserDirective } from './directives/auth-user.directive';
import { UserRoleDirective } from './directives/user-role.directive';

@NgModule({
  declarations: [
    AuthUserDirective,
    UserRoleDirective
  ],
  imports: [
    CommonModule
  ],
  exports: [
    AuthUserDirective,
    UserRoleDirective
  ]
})
export class CoreModule { }
