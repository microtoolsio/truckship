import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Subscriber } from 'rxjs/Subscriber';
import { ExecutionResult } from '../execution-result';
import * as Rx from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { of } from 'rxjs/observable/of';
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import { catchError, retry } from 'rxjs/operators';
import { ErrorInfo } from "../error-info";
import { HttpErrorResponse } from "@angular/common/http/src/response";

@Injectable()
export class HttpService {
  private currentRequestsNumber: number = 0;

  private authHeaderValue: string;

  public errorsObservable: Subject<ErrorInfo>;

  public showLoadingIndicatorObservable: Subject<boolean>;

  //TODO: CONSIDER REFACTORING OF ERROR HANDLING
  constructor(private http: HttpClient) {
    this.showLoadingIndicatorObservable = new Subject<boolean>();
    this.errorsObservable = new Subject<ErrorInfo>();
  }

  //public setAuthHeader(value: string) {
  //  this.authHeaderValue = value;
  //}

  //private getAuthHeaders(): HttpHeaders {
  //  let headers = new HttpHeaders();
  //  if (this.authHeaderValue) {
  //    headers = headers.set('Authorization', this.authHeaderValue);
  //  }
  //  return headers;
  //}

  public post<T>(url: string): Observable<ExecutionResult<T>>;

  public post<T>(url: string, body: any): Observable<ExecutionResult<T>>;

  public post<T>(url: string, body: any, params: {}): Observable<ExecutionResult<T>>;

  public post<T>(url: string, body?: any, params?: {}, extractSetCookie?: boolean): Observable<ExecutionResult<T>> {
    this.showLoader();

    return this.http.post<T>(url, body, { observe: 'response', params: params })
      .map(x => {
        this.hideLoader();
        return new ExecutionResult<T>(x.body);
      })
      .catch(x => {
        this.hideLoader();
        return of(new ExecutionResult<T>(null, [this.handleError(x)]));
      });
  }

  public patch<T>(url: string, body: any): Observable<ExecutionResult<T>>;
  public patch<T>(url: string, body: any, params: {}): Observable<ExecutionResult<T>>;
  public patch<T>(url: string, body?: any, params?: {}): Observable<ExecutionResult<T>> {

    this.showLoader();

    return this.http.patch<T>(url, body, { observe: 'response', params: params })
      .map(x => {
        this.hideLoader();
        return new ExecutionResult<T>(x.body);
      })
      .catch(x => {
        this.hideLoader();
        return of(new ExecutionResult<T>(null, [this.handleError(x)]));
      });
  }

  public delete<T>(url: string, params?: any): Observable<ExecutionResult<T>> {

    this.showLoader();

    return this.http.delete<T>(url, { observe: 'response', params: params })
      .map(x => {
        this.hideLoader();
        return new ExecutionResult<T>(x.body);
      })
      .catch(x => {
        this.hideLoader();
        return of(new ExecutionResult<T>(null, [this.handleError(x)]));
      });
  }


  public get<T>(url: string, params?: any): Observable<ExecutionResult<T>> {

    this.showLoader();

    return this.http.get<T>(url, { params: params, observe: 'response', })
      .map(x => {
        this.hideLoader();
        return new ExecutionResult<T>(x.body);
      })
      .catch(x => {
        this.hideLoader();
        return of(new ExecutionResult<T>(null, [this.handleError(x)]));
      });
  }

  //private handleError<T>(response: HttpErrorResponse, o: Subscriber<ExecutionResult<T>>) {
  private handleError<T>(response: HttpErrorResponse): ErrorInfo {
    if (response.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.log('An error occurred:', response.error.message);
      let errorInfo = new ErrorInfo(response.error.message, "");
      //o.next(new ExecutionResult(null, new Array<ErrorInfo>(errorInfo)));
      this.errorsObservable.next(errorInfo);
      return errorInfo;
      //return of(new ExecutionResult<T>(null, [errorInfo]));
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.log(`Backend returned code ${response.status}, body was: ${response.error || response.statusText}`);
      let errorInfo = new ErrorInfo(response.error ? response.error.error : response.statusText, response.status.toString());
      //o.next(new ExecutionResult(null, new Array<ErrorInfo>(errorInfo)));
      this.errorsObservable.next(errorInfo);
      return errorInfo;
      //return of(new ExecutionResult<T>(null, [errorInfo]));
    }
    //o.complete();

  }

  private showLoader() {
    this.currentRequestsNumber++;
    if (this.currentRequestsNumber === 1) {
      console.log('loader shown');
      this.showLoadingIndicatorObservable.next(true);
    }
  }

  private hideLoader() {
    this.currentRequestsNumber--;
    if (this.currentRequestsNumber === 0) {
      console.log('loader hid');
      this.showLoadingIndicatorObservable.next(false);
    }
  }
}
