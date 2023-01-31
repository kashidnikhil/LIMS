import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChargesComponent } from './charges-list/charges.component';

const routes: Routes = [
    {
        path: '',
        component: ChargesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ChargesRoutingModule {}
