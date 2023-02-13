import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { SourceRoutingModule } from "./source-routing.module";
import { SourcesComponent } from "./source-list/sources.component";
import { CreateOrEditSourceModalComponent } from "./create-edit-source/create-or-edit-source-modal.component";

@NgModule({
    declarations: [
        SourcesComponent, 
        CreateOrEditSourceModalComponent
    ],
    imports: [
        AppSharedModule,  
        SourceRoutingModule,
        SubheaderModule
    ],
    providers: [],
})
export class SourceModule {}