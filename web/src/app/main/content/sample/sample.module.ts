import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../../../core/modules/shared.module';

import { FuseSampleComponent } from './sample.component';

import { AuthGuard } from "../../../core/services/auth/auth-guard.service";

const routes = [
    {
        path     : 'sample',
        component: FuseSampleComponent,
        canActivate: [AuthGuard],
    }
];

@NgModule({
    declarations: [
        FuseSampleComponent
    ],
    imports     : [
        SharedModule,
        RouterModule.forChild(routes)
    ],
    exports     : [
        FuseSampleComponent
    ]
})

export class FuseSampleModule
{
}
