import { Component, Injector, ViewChild, ViewEncapsulation } from "@angular/core";
import { FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ApplicationsDto, ApplicationsServiceProxy, TestMasterDto, TestMasterServiceProxy, UnitDto, UnitServiceProxy } from "@shared/service-proxies/service-proxies";
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
    editMode : boolean = false;
    testMasterForm!: FormGroup;

    unitList : UnitDto[] = [];
    applicationList : ApplicationsDto[] = [];

    constructor(
        injector: Injector,
        private _testMasterService: TestMasterServiceProxy,
        private _unitService : UnitServiceProxy,
        private _applicationService : ApplicationsServiceProxy,
        private formBuilder: FormBuilder
    ) {
        super(injector);
    }


    async show(testMasterId?: string) {
        await this.loadDropdownList();
        if (!testMasterId) {
            let testMasterItem : TestMasterDto = new TestMasterDto();
            this.initialiseTestMasterForm(testMasterItem);
            this.editMode = false;
            this.active = true;
            this.modal.show();
        }
        else{
            this._testMasterService.getTestMasterById(testMasterId).subscribe((response : TestMasterDto)=> {
                let testMasterItem = response;
                this.initialiseTestMasterForm(testMasterItem);
                this.editMode = true;
                this.active = true;
                this.modal.show();
            });
        }        
    }

    async loadDropdownList(){
        await this.loadUnitList();
        await this.loadApplicationList();
    }

    async loadUnitList(){
        let unitList = await this._unitService.getUnitList().toPromise();
        if(unitList.length > 0){
            this.unitList = [];
            unitList.forEach((unitItem : UnitDto) => {
                this.unitList.push(unitItem);
            });
        }
    }

    async loadApplicationList(){
        let applicationList = await this._applicationService.getApplicationList().toPromise();
        if(applicationList.length > 0){
            this.applicationList = [];
            applicationList.forEach((applicationItem : ApplicationsDto) => {
                this.applicationList.push(applicationItem);
            });
        }
    }

    initialiseTestMasterForm(testItem : TestMasterDto){
        this.testMasterForm = this.formBuilder.group({
            name: new FormControl(testItem.name, []),
            unitId: new FormControl(testItem.unitId, []),
            techniqueId: new FormControl(testItem.techniqueId, []),
            isDefaultTechnique: new FormControl(testItem.isDefaultTechnique, []),
            applicationId: new FormControl(testItem.applicationId, []),
            // method: new FormControl(testItem.method, []),
            // methodDescription: new FormControl(testItem.methodDescription, []),
            // isSC: new FormControl(testItem.isSC, []),
            // rate: new FormControl(testItem.rate, []),
            // id: new FormControl(testItem.id, []),
            // customerAddresses: customerItem.id ? this.formBuilder.array(
            //     customerItem.customerAddresses.map((x : CustomerAddressDto) => 
            //         this.createCustomerAddress(x)
            //       )
            // ) : this.formBuilder.array([this.createCustomerAddress(addressItem)])
            
        });
    }

    onShown(): void {
        document.getElementById('name').focus();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    save() : void {

    }



}