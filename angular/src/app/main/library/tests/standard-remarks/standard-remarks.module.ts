import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { CreateOrEditStandardRemarkModalComponent } from "./create-edit-standard-remark/create-or-edit-standard-remark-modal.component";
import { StandardRemarksComponent } from "./standard-remarks-list/standard-remarks.component";
import { StandardRemarksRoutingModule } from "./standard-remarks-routing.module";

@NgModule({
    declarations: [
        StandardRemarksComponent,
        CreateOrEditStandardRemarkModalComponent
    ],
    imports: [
        AppSharedModule,  
        StandardRemarksRoutingModule,
        SubheaderModule,
    ],
    providers: [],
})

export class StandardRemarksModule {}