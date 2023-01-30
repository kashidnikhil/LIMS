import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StandardReferencesComponent } from './standard-references-list/standard-references.component';

const routes: Routes = [
    {
        path: '',
        component: StandardReferencesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StandardReferencesRoutingModule {}
