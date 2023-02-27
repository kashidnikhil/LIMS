import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { CustomerRoutingModule } from "./customer-routing.module";
import { CustomersComponent } from "./customer-list/customers.component";
import { CreateOrEditCustomerModalComponent } from "./create-edit-customer/create-or-edit-customer-modal.component";
import { ReactiveFormsModule } from "@angular/forms";
import { ContactPersonsComponent } from "../contact-person/contact-person-list/contact-persons.component";
import { CreateOrEditContactPersonModalComponent } from "../contact-person/create-edit-contact-person/create-or-edit-contact-person-modal.component";

@NgModule({
    declarations: [
        CustomersComponent, 
        CreateOrEditCustomerModalComponent,
        ContactPersonsComponent,
        CreateOrEditContactPersonModalComponent
    ],
    imports: [
        AppSharedModule,  
        CustomerRoutingModule,
        SubheaderModule,
        ReactiveFormsModule 
    ],
    providers: [],
})
export class CustomerModule {}