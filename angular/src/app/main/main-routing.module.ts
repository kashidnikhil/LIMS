import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    {
                        path: 'dashboard',
                        loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
                        data: { permission: 'Pages.Tenant.Dashboard' },
                    },
                    {
                        path: 'applications',
                        loadChildren: () => import('./library/tests/applications/applications.module').then(m => m.ApplicationsModule)
                    },
                    {
                        path: 'standard-remarks',
                        loadChildren: () => import('./library/tests/standard-remarks/standard-remarks.module').then(m => m.StandardRemarksModule)
                    },
                    {
                        path: 'standard-references',
                        loadChildren: () => import('./library/tests/standard-references/standard-references.module').then(m => m.StandardReferencesModule)
                    },
                    {
                        path: 'techniques',
                        loadChildren: () => import('./library/tests/techniques/techniques.module').then(m => m.TechniquesModule)
                    },
                    {
                        path: 'sources',
                        loadChildren: () => import('./library/customer/source/source.module').then(m => m.SourceModule)
                    },
                    {
                        path: 'banks',
                        loadChildren: () => import('./library/invoice-and-receipts/bank/bank.module').then(m => m.BankModule)
                    },{
                        path: 'charges',
                        loadChildren: () => import('./library/invoice-and-receipts/charges/charges.module').then(m => m.ChargesModule)
                    },
                    // {
                    //     path: 'tests',
                    //     loadChildren: () => import('./library/tes').then(m => m.customerModule)
                    // },
                    // {
                    //     path: 'test-plans',
                    //     loadChildren: () => import('./customer/customer.module').then(m => m.customerModule)
                    // },
                    {
                        path: 'units',
                        loadChildren: () => import('./library/tests/units/units.module').then(m => m.UnitsModule)
                    },                  
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'dashboard' },
                ],
            },
        ]),
    ],
    exports: [RouterModule],
})
export class MainRoutingModule {}
