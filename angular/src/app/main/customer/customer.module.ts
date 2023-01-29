import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { customerComponent } from './customer.component';
import {customerRoutingModule} from './customer-routing.module';
import { CreateCustomerModalComponent } from './create-customer-modal.component';



@NgModule({
    declarations: [customerComponent,CreateCustomerModalComponent],
    imports: [AppSharedModule, customerRoutingModule]
})
export class customerModule {}
