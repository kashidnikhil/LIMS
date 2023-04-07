import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { FormArray, FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ApplicationsDto, ApplicationsServiceProxy, DropdownDto, SubApplicationDto, SubApplicationServiceProxy, TechniqueDto, TechniqueServiceProxy, TestMasterDto, TestMasterInputDto, TestMasterServiceProxy, TestSubApplicationDto, TestVariableDto, UnitDto, UnitServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from "rxjs/operators";

export interface Product {
    id?: string;
    code?: string;
    name?: string;
    description?: string;
    price?: number;
    quantity?: number;
    inventoryStatus?: string;
    category?: string;
    image?: string;
    rating?: number;
}

@Component({
    selector: 'create-edit-test-plan-modal',
    templateUrl: './create-edit-test-plan-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-edit-test-plan-modal.component.less']
})
export class CreateOrEditTestPlanModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;
    editMode: boolean = false;
    testMasterForm!: FormGroup;
    testMasterList: DropdownDto[] = [];
    filteredTestMasterList: DropdownDto[] = [];
    selectedTestMaster: DropdownDto;

    applicationList: ApplicationsDto[] = [];
    testMasterInput: TestMasterDto = new TestMasterDto();



    constructor(
        injector: Injector,
        private _testMasterService: TestMasterServiceProxy,
        private _applicationService: ApplicationsServiceProxy,
        private formBuilder: FormBuilder
    ) {
        super(injector);
    }


    async show(testMasterId?: string) {
        await this.loadDropdownList();
        if (!testMasterId) {
            let testMasterItem: TestMasterDto = new TestMasterDto();
            this.initialiseTestMasterForm(testMasterItem);
            this.editMode = false;
            this.active = true;
            this.modal.show();
        }
        else {
            this.loadTestMasterData(testMasterId, false);
            this.editMode = true;
            this.active = true;
            this.modal.show();
        }
    }

    async loadDropdownList() {
        await this.loadApplicationList();
        await this.loadTestMasterList();
    }

    async loadApplicationList() {
        let applicationList = await this._applicationService.getApplicationList().toPromise();
        if (applicationList.length > 0) {
            this.applicationList = [];
            applicationList.forEach((applicationItem: ApplicationsDto) => {
                this.applicationList.push(applicationItem);
            });
        }
    }

    async loadTestMasterList() {
        let testMasterList = await this._testMasterService.getTestMasterList().toPromise();
        if (testMasterList.length > 0) {
            this.testMasterList = [];
            testMasterList.forEach((testMasterItem: DropdownDto) => {
                this.testMasterList.push(testMasterItem);
            });
        }
    }

    filterTestMasterList(event) {
        let filtered: DropdownDto[] = [];
        let query = event.query;
        for (let i = 0; i < this.testMasterList.length; i++) {
            let testMasterItem = this.testMasterList[i];
            if (testMasterItem.title.toLowerCase().indexOf(query.toLowerCase()) == 0) {
                filtered.push(testMasterItem);
            }
        }

        this.filteredTestMasterList = filtered;
    }

    initialiseTestMasterForm(testItem: TestMasterDto) {
        this.testMasterForm = this.formBuilder.group({
            name: new FormControl(testItem.name, []),
            unitId: new FormControl(testItem.unitId, []),
            techniqueId: new FormControl(testItem.techniqueId, []),
            isDefaultTechnique: new FormControl(testItem.isDefaultTechnique ? testItem.isDefaultTechnique : false, []),
            applicationId: new FormControl(testItem.applicationId, []),
            method: new FormControl(testItem.method, []),
            methodDescription: new FormControl(testItem.methodDescription, []),
            worksheetName: new FormControl(testItem.worksheetName, []),
            isSC: new FormControl(testItem.isSC ? testItem.isSC : false, []),
            rate: new FormControl(testItem.rate ? testItem.rate : 0.0, []),
            id: new FormControl(testItem.id, []),
            testSubApplications: testItem.testSubApplications && testItem.testSubApplications.length > 0 ? this.formBuilder.array(
                testItem.testSubApplications.map((x: TestSubApplicationDto) =>
                    this.createTestSubApplications(x))
            ) : this.formBuilder.array([]),
            testVariables: testItem.testVariables && testItem.testVariables.length > 0 ? this.formBuilder.array(
                testItem.testVariables.map((x: TestVariableDto) =>
                    this.createTestVariables(x))
            ) : this.formBuilder.array([]),

        });
    }

    loadSelectedTestMasterData() {
        if (this.selectedTestMaster.value) {
            this.loadTestMasterData(this.selectedTestMaster.value, true);
            this.selectedTestMaster = new DropdownDto();
        }
    }

    loadTestMasterData(testMasterId: string, isTestDataCopied: boolean) {
        this._testMasterService.getTestMasterById(testMasterId).subscribe((response: TestMasterDto) => {
            let testMasterItem = response;
            if (isTestDataCopied) {
                testMasterItem.id = null;

                //It is to make new entry when an existing test is being newly copied
                if (testMasterItem.testSubApplications && testMasterItem.testSubApplications.length > 0) {
                    testMasterItem.testSubApplications.forEach(subApplicationItem => {
                        subApplicationItem.testId = null;
                        subApplicationItem.id = null;
                    });
                }

                //It is to make new entry when an existing test is being newly copied
                if (testMasterItem.testVariables && testMasterItem.testVariables.length > 0) {
                    testMasterItem.testVariables.forEach(testVariableItem => {
                        testVariableItem.testId = null;
                        testVariableItem.id = null;
                    });
                }
            }
            this.initialiseTestMasterForm(testMasterItem);

        });
    }

    get testSubApplications(): FormArray {
        return (<FormArray>this.testMasterForm.get('testSubApplications'));
    }

    createTestSubApplications(subApplicationItem: TestSubApplicationDto): FormGroup {
        return this.formBuilder.group({
            id: new FormControl(subApplicationItem.id, []),
            subApplicationId: new FormControl(subApplicationItem.subApplicationId, []),
            subApplicationName: new FormControl(subApplicationItem.name, []),
            isNABL: new FormControl(subApplicationItem.isNABL, []),
            isMOEF: new FormControl(subApplicationItem.isMOEF, []),
            testId: new FormControl(subApplicationItem.testId, [])
        });
    }

    createTestVariables(testVariable: TestVariableDto): FormGroup {
        return this.formBuilder.group({
            id: new FormControl(testVariable.id, []),
            variable: new FormControl(testVariable.variable, []),
            description: new FormControl(testVariable.description, []),
            cellValue: new FormControl(testVariable.cellValue, [])
        });
    }

    get testVariables(): FormArray {
        return (<FormArray>this.testMasterForm.get('testVariables'));
    }

    onShown(): void {
        document.getElementById('name').focus();
    }

    close(): void {
        this.selectedTestMaster = new DropdownDto();
        this.active = false;
        this.modal.hide();
    }

    addTestVariable() {
        let testVariableItem: TestVariableDto = new TestVariableDto();
        let testVariableForm = this.createTestVariables(testVariableItem);
        this.testVariables.push(testVariableForm);
    }


    save(): void {
        if (this.testMasterForm.valid) {
            let input = new TestMasterInputDto();
            input = this.testMasterForm.value;
            this.saving = true;

            this._testMasterService
                .insertOrUpdateTest(input)
                .pipe(
                    finalize(() => {
                        this.saving = false;
                    })
                )
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modalSave.emit(null);
                });
        }
    }
}