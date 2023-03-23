import { NgModule } from "@angular/core";
import { AppSharedModule } from "@app/shared/app-shared.module";
import { SubheaderModule } from "@app/shared/common/sub-header/subheader.module";
import { TestMasterRoutingModule } from "./test-master-routing.module";
import { ReactiveFormsModule } from "@angular/forms";
import { TestMasterListComponent } from "./test-master-list/test-master-list.component";
import { CreateOrEditTestModalComponent } from "./create-edit-test-master/create-edit-test-master-modal.component";
import { FileUploadModule as PrimeNgFileUploadModule } from 'primeng/fileupload';
@NgModule({
    declarations: [
        TestMasterListComponent,
        CreateOrEditTestModalComponent
    ],
    imports: [
        AppSharedModule,  
        TestMasterRoutingModule,
        SubheaderModule,
        ReactiveFormsModule,
        PrimeNgFileUploadModule 
    ],
    providers: [],
})
export class TestMasterModule {}