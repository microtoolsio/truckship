import { NgModule } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { RouterModule } from '@angular/router';
import { FuseNavigationComponent } from './navigation.component';
import { FuseNavVerticalItemComponent } from './vertical/nav-item/nav-vertical-item.component';
import { FuseNavVerticalCollapseComponent } from './vertical/nav-collapse/nav-vertical-collapse.component';
import { FuseNavVerticalGroupComponent } from './vertical/nav-group/nav-vertical-group.component';

@NgModule({
    imports     : [
        SharedModule,
        RouterModule
    ],
    exports     : [
        FuseNavigationComponent
    ],
    declarations: [
        FuseNavigationComponent,
        FuseNavVerticalGroupComponent,
        FuseNavVerticalItemComponent,
        FuseNavVerticalCollapseComponent,
    ]
})
export class FuseNavigationModule
{
}
