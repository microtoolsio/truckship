import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from "../../../core/modules/shared.module";
import { AccountComponent } from './account.component';
import { RouterModule } from '@angular/router';


const routes = [
  {
    path: 'account',
    component: AccountComponent,
    //resolve  : {
    //    profile: ProfileService
    //},
    //canActivate: [AuthGuard],
  }
];

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ],
  declarations: [AccountComponent]
})
export class AccountModule { }
