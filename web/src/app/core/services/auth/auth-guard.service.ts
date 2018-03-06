import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Router } from '@angular/router';
import { AuthService } from "./auth.service";

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private loginService: AuthService) {

  }

  canActivate() {
    console.log('AuthGuard#canActivate called');
    
    //if (this.loginService.isAuthenticated) {
      return true;
    //}

    //this.router.navigate(['/login']);
    //return false;
  }
}
