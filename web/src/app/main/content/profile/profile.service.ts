import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { HttpService } from "../../../core/services/http.service";
import { environment } from "../../../../environments/environment";
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/mergeMap';

@Injectable()
export class ProfileService implements Resolve<any>
{
  timeline: any;
  about: any;
  photosVideos: any;

  timelineOnChanged: BehaviorSubject<any> = new BehaviorSubject({});
  aboutOnChanged: BehaviorSubject<any> = new BehaviorSubject({});
  photosVideosOnChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(private http: HttpClient, private httpService: HttpService) {
    this.resolve(null, null);
  }

  /**
   * Resolve
   * @param {ActivatedRouteSnapshot} route
   * @param {RouterStateSnapshot} state
   * @returns {Observable<any> | Promise<any> | any}
   */
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise((resolve, reject) => {
      Promise.all([
        this.getTimeline(),
        this.getAbout(),
        this.getPhotosVideos()
      ]).then(
        () => {
          resolve();
        },
        reject
        );
    });
  }

  /**
   * Get timeline
   */
  getTimeline(): Promise<any[]> {
    return new Promise((resolve, reject) => {
      this.httpService.get(`${environment.api}/values`)
        .mergeMap(x => {
          if (!x.success) {
            reject(x.errors);
          }
          return this.http.get('api/profile-timeline');
        })
        //this.http.get('api/profile-timeline')
        .subscribe((timeline: any) => {
          this.timeline = timeline;
          //this.timelineOnChanged.next(this.timeline);
          resolve(this.timeline);
        }, reject);
    });
  }

  /**
   * Get about
   */
  getAbout(): Promise<any[]> {
    return new Promise((resolve, reject) => {

      this.http.get('api/profile-about')
        .subscribe((about: any) => {
          this.about = about;
          //this.aboutOnChanged.next(this.about);
          resolve(this.about);
        }, reject);
    });
  }

  /**
   * Get photos & videos
   */
  getPhotosVideos(): Promise<any[]> {
    return new Promise((resolve, reject) => {

      this.http.get('api/profile-photos-videos')
        .subscribe((photosVideos: any) => {
          this.photosVideos = photosVideos;
          //this.photosVideosOnChanged.next(this.photosVideos);
          resolve(this.photosVideos);
        }, reject);
    });
  }

}
