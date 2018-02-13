import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import 'hammerjs';
import { SharedModule } from './core/modules/shared.module';
import { AppComponent } from './app.component';
import { FuseMainModule } from './main/main.module';
import { FuseSplashScreenService } from './core/services/splash-screen.service';
import { FuseConfigService } from './core/services/config.service';
import { FuseNavigationService } from './core/components/navigation/navigation.service';
import { FuseSampleModule } from './main/content/sample/sample.module';

import { LoginModule } from './main/content/authentication/login/login.module';
import { RegisterModule } from './main/content/authentication/register/register.module';
import { ForgotPasswordModule } from './main/content/authentication/forgot-password/forgot-password.module';
import { ResetPasswordModule } from './main/content/authentication/reset-password/reset-password.module';
import { MailConfirmModule } from './main/content/authentication/mail-confirm/mail-confirm.module';
import { ComingSoonModule } from './main/content/coming-soon/coming-soon.module';
import { Error404Module } from './main/content/errors/404/error-404.module';
import { Error500Module } from './main/content/errors/500/error-500.module';
import { MaintenanceModule } from './main/content/maintenance/maintenence.module';
import { TranslateModule } from '@ngx-translate/core';
import { FuseLoginComponent } from "./main/content/authentication/login/login.component";

const appRoutes: Routes = [
  {
    path: 'login',
    component: FuseLoginComponent
  },
  {
    path: '**', redirectTo: 'sample',
  },
];

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(appRoutes),
    SharedModule,
    TranslateModule.forRoot(),
    FuseMainModule,

    FuseSampleModule,

    // Auth
    LoginModule,
    RegisterModule,
    ForgotPasswordModule,
    ResetPasswordModule,
    MailConfirmModule,

    // Coming-soon
    ComingSoonModule,

    // Errors
    Error404Module,
    Error500Module,

    // Maintenance
    MaintenanceModule,
  ],
  providers: [
    FuseSplashScreenService,
    FuseConfigService,
    FuseNavigationService
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule {
}
