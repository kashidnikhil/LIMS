import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { CreateOrEditTechniqueModalComponent } from "./create-edit-technique/create-or-edit-technique-modal.component";
import { TechiquesComponent } from "./technique-list/techniques.component";
import { TechniquesRoutingModule } from "./techniques-routing.module";

@NgModule({
    declarations: [
        TechiquesComponent,
        CreateOrEditTechniqueModalComponent
    ],
    imports: [
        AppSharedModule,  
        TechniquesRoutingModule,
        SubheaderModule,
    ],
    providers: [],
})

export class TechniquesModule {}