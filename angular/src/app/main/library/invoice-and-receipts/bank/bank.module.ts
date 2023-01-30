import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { BankRoutingModule } from "./bank-routing.module";
import { BanksComponent } from "./bank-list/banks.component";
import { CreateOrEditBankModalComponent } from "./create-edit-bank/create-or-edit-bank-modal.component";

@NgModule({
    declarations: [
        BanksComponent, 
        CreateOrEditBankModalComponent
    ],
    imports: [
        AppSharedModule,  
        BankRoutingModule,
        SubheaderModule
    ],
    providers: [],
})
export class BankModule {}