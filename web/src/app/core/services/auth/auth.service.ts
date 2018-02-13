import { Injectable } from '@angular/core';
import { HttpService } from '../http.service';
import { Observable } from 'rxjs/Observable';
import { ExecutionResult } from '../../execution-result';
import { Jwt } from './jwt';

import { environment } from "../../../../environments/environment";

@Injectable()
export class AuthService {

  constructor(private httpService: HttpService) { }

  public isAuthenticated: boolean = false;

  public loginUser(email: string, password: string): Observable<ExecutionResult<void>> {

    return this.httpService.post<Jwt>(`${environment.api}/auth/login`, { email: email, password: password }).map(x => {
      if (x.success) {
        let tokenString = `Bearer ${x.value.access_token}`;
        this.httpService.setAuthHeader(tokenString);
        this.isAuthenticated = true;
        return new ExecutionResult(null);
      }
      return new ExecutionResult(null, x.errors);
    });
  }

  public logOut(): Observable<ExecutionResult<void>> {
    return this.httpService.post<void>(`${environment.api}/auth/logout`).map(x => {
      this.isAuthenticated = false;
      return x;
    });
  }
}
