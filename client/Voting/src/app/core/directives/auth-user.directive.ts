import { Directive, OnInit, ViewContainerRef, TemplateRef } from '@angular/core';
import { JwtHelperService } from '../services/jwt-helper.service';

@Directive({
  selector: '[appAuthUser]'
})
export class AuthUserDirective implements OnInit{

  constructor(private templateRef: TemplateRef<any>,
              private viewContainer: ViewContainerRef,
              private jwtHelperService: JwtHelperService) { }

  ngOnInit() {
    let token = localStorage.getItem('auth_token');
    const hasAccess = token && !this.jwtHelperService.isTokenExpired(token);;
    
    if (hasAccess) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
        this.viewContainer.clear();
    }
  }

}
