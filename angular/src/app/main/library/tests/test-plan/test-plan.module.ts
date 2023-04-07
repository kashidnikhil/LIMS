import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { TestPlanRoutingModule } from "./test-plan-routing.module";
import { ReactiveFormsModule } from "@angular/forms";
import { FileUploadModule as PrimeNgFileUploadModule } from 'primeng/fileupload';
import { TestPlanListComponent } from "./test-plan-list/test-plan-list.component";
import { CreateOrEditTestPlanModalComponent } from "./create-edit-test-plan/create-edit-test-plan-modal.component";
@NgModule({
    declarations: [
        TestPlanListComponent,
        CreateOrEditTestPlanModalComponent
    ],
    imports: [
        AppSharedModule,  
        TestPlanRoutingModule,
        SubheaderModule,
        ReactiveFormsModule,
        PrimeNgFileUploadModule 
    ],
    providers: [],
})
export class TestPlanModule {}