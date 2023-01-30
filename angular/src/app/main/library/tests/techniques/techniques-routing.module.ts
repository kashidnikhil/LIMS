import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TechiquesComponent } from './technique-list/techniques.component';

const routes: Routes = [
    {
        path: '',
        component: TechiquesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TechniquesRoutingModule {}
