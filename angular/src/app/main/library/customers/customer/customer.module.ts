import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { CustomerRoutingModule } from "./customer-routing.module";
import { CustomersComponent } from "./customer-list/customers.component";
import { CreateOrEditCustomerModalComponent } from "./create-edit-customer/create-or-edit-customer-modal.component";

@NgModule({
    declarations: [
        CustomersComponent, 
        CreateOrEditCustomerModalComponent
    ],
    imports: [
        AppSharedModule,  
        CustomerRoutingModule,
        SubheaderModule
    ],
    providers: [],
})
export class CustomerModule {}