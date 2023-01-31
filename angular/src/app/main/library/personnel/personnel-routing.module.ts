import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PersonnelsComponent } from './personnel-list/personnels.component';

const routes: Routes = [
    {
        path: '',
        component: PersonnelsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PersonnelRoutingModule {}
