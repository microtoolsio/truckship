import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Subscriber } from 'rxjs/Subscriber';
import { ExecutionResult } from '../execution-result';
import * as Rx from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { ErrorInfo } from "../error-info";
import { HttpErrorResponse } from "@angular/common/http/src/response";

@Injectable()
export class HttpService {

  constructor(private http: HttpClient) {
    this.showLoadingIndicatorObservable = new Subject<boolean>();
  }


  public authHeaderValue: string;
  public showLoadingIndicator: boolean = false;

  private showLoadingIndicatorObservable: Subject<boolean>;
  private currentRequestsNumber: number = 0;

  public setAuthHeader(value: string) {
    this.authHeaderValue = value;
  }

  public post<T>(url: string): Observable<ExecutionResult<T>>;

  public post<T>(url: string, body: any): Observable<ExecutionResult<T>>;

  public post<T>(url: string, body: any, params: {}): Observable<ExecutionResult<T>>;

  public post<T>(url: string, body?: any, params?: {}): Observable<ExecutionResult<T>> {
    this.showLoader();
    var obs = Rx.Observable.create(o => {

      let headers = new HttpHeaders();
      if (this.authHeaderValue) {
        headers = headers.set('Authorization', this.authHeaderValue);
      }

      this.http.post<T>(url, body, { observe: 'response', headers: headers, params: params }).subscribe(data => {
        o.next(new ExecutionResult(data.body));
        o.complete();
      },
        (err: HttpErrorResponse) => {
          this.handleError(err, o);
        });
    });

    return obs.finally(() => {
      this.hideLoader();
    });

  }

  public patch<T>(url: string, body: any): Observable<ExecutionResult<T>>;

  public patch<T>(url: string, body: any, params: {}): Observable<ExecutionResult<T>>;

  public patch<T>(url: string, body?: any, params?: {}): Observable<ExecutionResult<T>> {
    var obs = Rx.Observable.create(o => {

      let headers = new HttpHeaders();
      if (this.authHeaderValue) {
        headers = headers.set('Authorization', this.authHeaderValue);
      }

      this.http.patch<T>(url, body, { observe: 'response', headers: headers, params: params }).subscribe(data => {
        o.next(new ExecutionResult(data.body));
        o.complete();
      },
        (err: HttpErrorResponse) => {
          this.handleError(err, o);
        });
    });

    return obs.finally(() => {
      this.hideLoader();
    });
  }

  public delete<T>(url: string, params?: any): Observable<ExecutionResult<T>> {
    var obs = Rx.Observable.create(o => {

      let headers = new HttpHeaders();
      if (this.authHeaderValue) {
        headers = headers.set('Authorization', this.authHeaderValue);
      }

      this.http.delete<T>(url, { observe: 'response', headers: headers, params: params }).subscribe(data => {
        o.next(new ExecutionResult(data.body));
        o.complete();
      },
        (err: HttpErrorResponse) => {
          this.handleError(err, o);
        });
    });

    return obs.finally(() => {
      this.hideLoader();
    });
  }

  public get<T>(url: string, params?: any): Observable<ExecutionResult<T>> {
    this.showLoader();
    var obs = Rx.Observable.create(o => {

      let headers = new HttpHeaders();
      if (this.authHeaderValue) {
        headers = headers.set('Authorization', this.authHeaderValue);
      }

      this.http.get<T>(url, { params: params, observe: 'response', headers: headers }).subscribe(data => {
        o.next(new ExecutionResult(data.body));
        o.complete();
      },
        (err: HttpErrorResponse) => {
          this.handleError(err, o);
        });
    });
    return obs.finally(() => {
      this.hideLoader();
    });
  }

  private handleError<T>(response: HttpErrorResponse, o: Subscriber<ExecutionResult<T>>) {
    if (response.error instanceof Error) {
      // A client-side or network error occurred. Handle it accordingly.
      console.log('An error occurred:', response.error.message);
      o.next(new ExecutionResult(null, new Array<ErrorInfo>(new ErrorInfo(response.error.message, ""))));
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.log(`Backend returned code ${response.status}, body was: ${response.error}`);
      o.next(new ExecutionResult(null, new Array<ErrorInfo>(new ErrorInfo(response.error.error, response.status.toString()))));
    }
    o.complete();
  }

  public getLoadingIndicatorState(): Observable<boolean> {
    return this.showLoadingIndicatorObservable;
  }

  private showLoader() {
    this.currentRequestsNumber++;
    if (this.currentRequestsNumber === 1) {
      this.showLoadingIndicator = true;
      console.log('loader shown');
      this.showLoadingIndicatorObservable.next(this.showLoadingIndicator);
    }
  }

  private hideLoader() {
    this.currentRequestsNumber--;
    if (this.currentRequestsNumber === 0) {
      this.showLoadingIndicator = false;
      console.log('loader hid');
      this.showLoadingIndicatorObservable.next(this.showLoadingIndicator);
    }
  }
}
