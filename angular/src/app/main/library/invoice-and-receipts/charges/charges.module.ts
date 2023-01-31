import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { ChargesRoutingModule } from "./charges-routing.module";
import { ChargesComponent } from "./charges-list/charges.component";
import { CreateOrEditChargesModalComponent } from "./create-edit-charges/create-or-edit-charges-modal.component";

@NgModule({
    declarations: [
        ChargesComponent, 
        CreateOrEditChargesModalComponent
    ],
    imports: [
        AppSharedModule,  
        ChargesRoutingModule,
        SubheaderModule
    ],
    providers: [],
})
export class ChargesModule {}