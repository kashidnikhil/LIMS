import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { ContainerRoutingModule } from "./container-routing.module";
import { TaxesComponent } from "./container-list/containers.component";
import { CreateOrEditContainerModalComponent } from "./create-edit-container/create-or-edit-container-modal.component";

@NgModule({
    declarations: [
        TaxesComponent, 
        CreateOrEditContainerModalComponent
    ],
    imports: [
        AppSharedModule,  
        ContainerRoutingModule,
        SubheaderModule
    ],
    providers: [],
})
export class ContainerModule {}