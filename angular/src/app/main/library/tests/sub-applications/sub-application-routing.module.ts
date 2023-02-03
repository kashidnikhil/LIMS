import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SubApplicationsComponent } from './sub-application-list/sub-applications.component';

const routes: Routes = [
    {
        path: '',
        component: SubApplicationsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SubApplicationRoutingModule {}
