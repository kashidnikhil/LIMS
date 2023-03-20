import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestMasterListComponent } from './test-master-list/test-master-list.component';


const routes: Routes = [
    {
        path: '',
        component: TestMasterListComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TestMasterRoutingModule {}
