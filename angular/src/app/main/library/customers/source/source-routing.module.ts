import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SourcesComponent } from './source-list/sources.component';

const routes: Routes = [
    {
        path: '',
        component: SourcesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SourceRoutingModule {}
