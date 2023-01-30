import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
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
        SubheaderModule
    ],
    providers: [ApplicationsService],
})
export class ApplicationsModule {}