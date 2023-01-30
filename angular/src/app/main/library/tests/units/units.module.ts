import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { CreateOrEditUnitModalComponent } from "./create-edit-unit/create-or-edit-unit-modal.component";
import { UnitsComponent } from "./unit-list/units.component";
import { UnitsRoutingModule } from "./units-routing.module";

@NgModule({
    declarations: [
        UnitsComponent,
        CreateOrEditUnitModalComponent
    ],
    imports: [
        AppSharedModule,  
        UnitsRoutingModule,
        SubheaderModule,
    ],
    providers: [],
})

export class UnitsModule {}