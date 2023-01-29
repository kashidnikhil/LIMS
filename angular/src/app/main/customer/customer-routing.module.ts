import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { customerComponent } from './customer.component';


const routes: Routes = [{
    path: '',
    component: customerComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class customerRoutingModule {}
