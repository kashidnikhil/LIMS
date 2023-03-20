import { Component, Injector, ViewChild, ViewEncapsulation } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TestMasterServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'create-edit-test-modal',
    templateUrl: './create-edit-test-master-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-edit-test-master-modal.component.less']
})
export class CreateOrEditTestModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    active = false;
    saving = false;

    constructor(
        injector: Injector,
        private _testMasterService: TestMasterServiceProxy,
        private formBuilder: FormBuilder
    ) {
        super(injector);
    }


    show(techniqueId?: string): void {
        if (!techniqueId) {
           // this.techniqueItem = new TechniqueDto({id : null, name: "", description: ""}); 
            this.active = true;
            this.modal.show();
        }
        else{
            // this._techniqueService.getTechniqueById(techniqueId).subscribe((response : TechniqueDto)=> {
            //    this.techniqueItem = response;
            //     this.active = true;
            //     this.modal.show();
            // });
        }        
    }



}