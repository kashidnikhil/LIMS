import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { DynamicEntityPropertyManagerModule } from "@app/shared/common/dynamic-entity-property-manager/dynamic-entity-property-manager.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { ApplicationRoutingModule } from "./application-routing.module";
import { ApplicationsComponent } from "./application-list/applications.component";
import { ApplicationsService } from "./applications.service";
import { CreateOrEditApplicationModalComponent } from "./create-edit-application/create-or-edit-application-modal.component";

@NgModule({
    declarations: [
        ApplicationsComponent, 
        CreateOrEditApplicationModalComponent
    ],
    imports: [
        AppSharedModule,  
        ApplicationRoutingModule, 
        DynamicEntityPropertyManagerModule,
        SubheaderModule
    ],
    providers: [ApplicationsService],
})
export class ApplicationsModule {}