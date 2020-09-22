import { Directive, TemplateRef, ViewContainerRef, OnInit, Input } from '@angular/core';
import { JwtHelperService } from '../services/jwt-helper.service';
import { Role } from '../enums/role';

@Directive({
  selector: '[appUserRole]'
})
export class UserRoleDirective implements OnInit {

  userRoles: Role[];

  constructor(private templateRef: TemplateRef<any>,
              private jwtHelperService: JwtHelperService,
              private viewContainer: ViewContainerRef) { }

  @Input() 
  set appUserRole(roles: Role[]) {
      if (!roles || !roles.length) {
          throw new Error('Roles value is empty or missed');
      }

      this.userRoles = roles;
  }

  ngOnInit(){
    let hasAccess = false;
    let token = localStorage.getItem('auth_token');
    if (token && this.userRoles) {
        hasAccess = this.userRoles.some(r => this.jwtHelperService.hasRole(r, token));
    }
    
    if (hasAccess) {
        this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
        this.viewContainer.clear();
    }
  }

}
