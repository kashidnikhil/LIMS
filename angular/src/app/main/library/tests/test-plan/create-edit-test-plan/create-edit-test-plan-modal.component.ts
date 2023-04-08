import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { FormArray, FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ApplicationsDto, ApplicationsServiceProxy, DropdownDto, TestMasterDto, TestMasterInputDto, TestMasterListDto, TestMasterServiceProxy, TestPlanMasterDto, TestPlanMasterInputDto, TestPlanMasterServiceProxy, TestPlanTestMasterDto, TestPlanTestMasterInputDto, TestSubApplicationDto, TestVariableDto, UnitDto, UnitServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from "rxjs/operators";

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
    testPlanMasterForm!: FormGroup;
    testMasterList: TestMasterListDto[] = [];
    filteredTestMasterList: TestMasterListDto[] = [];
    selectedTestMaster: TestMasterListDto;

    applicationList: ApplicationsDto[] = [];
    testMasterInput: TestMasterDto = new TestMasterDto();

    constructor(
        injector: Injector,
        private _testPlanMasterService: TestPlanMasterServiceProxy,
        private _testMasterService: TestMasterServiceProxy,
        private _applicationService: ApplicationsServiceProxy,
        private formBuilder: FormBuilder
    ) {
        super(injector);
    }


    async show(testPlanMasterId?: string) {
        await this.loadDropdownList();
        if (!testPlanMasterId) {
            let testMasterItem: TestPlanMasterDto = new TestPlanMasterDto();
            this.initialiseTestPlanMasterForm(testMasterItem);
            this.editMode = false;
            this.active = true;
            this.modal.show();
        }
        else {
            this.loadTestMasterData(testPlanMasterId);
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
        let testMasterList = await this._testMasterService.getTestList().toPromise();
        if (testMasterList.length > 0) {
            this.testMasterList = [];
            testMasterList.forEach((testMasterItem: TestMasterListDto) => {
                this.testMasterList.push(testMasterItem);
            });
        }
    }

    filterTestMasterList(event) {
        let filtered: TestMasterListDto[] = [];
        let query = event.query;
        for (let i = 0; i < this.testMasterList.length; i++) {
            let testMasterItem = this.testMasterList[i];
            if (testMasterItem.name.toLowerCase().indexOf(query.toLowerCase()) == 0) {
                filtered.push(testMasterItem);
            }
        }

        this.filteredTestMasterList = filtered;
    }

    initialiseTestPlanMasterForm(testPlanItem: TestPlanMasterDto) {
        this.testPlanMasterForm = this.formBuilder.group({
            id: new FormControl(testPlanItem.id, []),
            name: new FormControl(testPlanItem.name, []),
            applicationsId: new FormControl(testPlanItem.applicationsId, []),
            standardReferenceId : new FormControl(testPlanItem.standardReferenceId, []),
            testPlanTestMasters: testPlanItem.testPlanTestMasters && testPlanItem.testPlanTestMasters.length > 0 ? this.formBuilder.array(
                testPlanItem.testPlanTestMasters.map((x: TestPlanTestMasterDto) =>
                    this.createTestMasters(x))
            ) : this.formBuilder.array([])
        });
    }

    loadTestMasterData(testPlanMasterId: string) {
        this._testPlanMasterService.getTestPlanMasterById(testPlanMasterId).subscribe((response: TestPlanMasterDto) => {
            let testPlanMasterItem = response;
            this.initialiseTestPlanMasterForm(testPlanMasterItem);
        });
    }

    createTestMasters(testPlanTestMasterItem: TestPlanTestMasterDto): FormGroup {

        let existingTestMasterItem = this.testMasterList.find(x=> x.id == testPlanTestMasterItem.testId);

        return this.formBuilder.group({
            id: new FormControl(testPlanTestMasterItem.id, []),
            testName : new FormControl(existingTestMasterItem? existingTestMasterItem.name : '', []),
            unitName: new FormControl(existingTestMasterItem? existingTestMasterItem.unitName : '', []),
            techniqueName : new FormControl(existingTestMasterItem? existingTestMasterItem.techniqueName : '', []),
            rate : new FormControl(existingTestMasterItem? existingTestMasterItem.rate : '', []),
            method : new FormControl(existingTestMasterItem? existingTestMasterItem.method : '', []),
            testId: new FormControl(testPlanTestMasterItem.testId, [])
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

    get testPlanTestMasters(): FormArray {
        return (<FormArray>this.testPlanMasterForm.get('testPlanTestMasters'));
    }

    onShown(): void {
        document.getElementById('name').focus();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    addTestVariable() {
        let testVariableItem: TestVariableDto = new TestVariableDto();
        let testVariableForm = this.createTestVariables(testVariableItem);
        this.testPlanTestMasters.push(testVariableForm);
    }


    save(): void {
        if (this.testPlanMasterForm.valid) {
            let input = new TestPlanMasterInputDto ();
            input = this.testPlanMasterForm.value;
            this.saving = true;

            this._testPlanMasterService
                .insertOrUpdateTestPlan(input)
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