import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { SubApplicationRoutingModule } from "./sub-application-routing.module";
import { SubApplicationsComponent } from "./sub-application-list/sub-applications.component";
import { CreateOrEditApplicationModalComponent } from "./create-edit-sub-application/create-or-edit-sub-application-modal.component";

@NgModule({
    declarations: [
        SubApplicationsComponent, 
        CreateOrEditApplicationModalComponent
    ],
    imports: [
        AppSharedModule,  
        SubApplicationRoutingModule,
        SubheaderModule
    ],
    providers: [],
})
export class SubApplicationsModule {}