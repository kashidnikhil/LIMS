import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestPlanListComponent } from './test-plan-list/test-plan-list.component';


const routes: Routes = [
    {
        path: '',
        component: TestPlanListComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TestPlanRoutingModule {}
