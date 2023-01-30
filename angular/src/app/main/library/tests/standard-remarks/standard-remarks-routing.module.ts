import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StandardRemarksComponent } from './standard-remarks-list/standard-remarks.component';

const routes: Routes = [
    {
        path: '',
        component: StandardRemarksComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StandardRemarksRoutingModule {}
