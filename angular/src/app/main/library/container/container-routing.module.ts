import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaxesComponent } from './container-list/containers.component';

const routes: Routes = [
    {
        path: '',
        component: TaxesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContainerRoutingModule {}
