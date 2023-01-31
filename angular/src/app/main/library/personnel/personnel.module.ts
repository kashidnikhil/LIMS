import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { PersonnelRoutingModule } from "./personnel-routing.module";
import { PersonnelsComponent } from "./personnel-list/personnels.component";
import { CreateOrEditPersonnelModalComponent } from "./create-edit-personnel/create-or-edit-personnel-modal.component";

@NgModule({
    declarations: [
        PersonnelsComponent, 
        CreateOrEditPersonnelModalComponent
    ],
    imports: [
        AppSharedModule,  
        PersonnelRoutingModule,
        SubheaderModule
    ],
    providers: [],
})
export class PersonnelModule {}