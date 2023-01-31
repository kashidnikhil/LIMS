import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { TaxRoutingModule } from "./tax-routing.module";
import { TaxesComponent } from "./tax-list/taxes.component";
import { CreateOrEditTaxModalComponent } from "./create-edit-tax/create-or-edit-tax-modal.component";

@NgModule({
    declarations: [
        TaxesComponent, 
        CreateOrEditTaxModalComponent
    ],
    imports: [
        AppSharedModule,  
        TaxRoutingModule,
        SubheaderModule
    ],
    providers: [],
})
export class TaxModule {}