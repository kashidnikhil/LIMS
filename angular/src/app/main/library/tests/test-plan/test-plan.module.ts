import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { TestPlanRoutingModule } from "./test-plan-routing.module";
import { ReactiveFormsModule } from "@angular/forms";
import { FileUploadModule as PrimeNgFileUploadModule } from 'primeng/fileupload';
@NgModule({
    declarations: [
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