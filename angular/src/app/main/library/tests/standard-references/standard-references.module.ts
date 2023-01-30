import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { CreateOrEditStandardReferenceModalComponent } from "./create-edit-standard-reference/create-or-edit-standard-reference-modal.component";
import { StandardReferencesComponent } from "./standard-references-list/standard-references.component";
import { StandardReferencesRoutingModule } from "./standard-references-routing.module";
import { StandardReferencesService } from "./standard-references.service";

@NgModule({
    declarations: [
        StandardReferencesComponent,
        CreateOrEditStandardReferenceModalComponent
    ],
    imports: [
        AppSharedModule,  
        StandardReferencesRoutingModule,
        SubheaderModule,
    ],
    providers: [StandardReferencesService],
})


export class StandardReferencesModule {}