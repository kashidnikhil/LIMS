import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from "@angular/core";
import { FormArray, FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ApplicationsDto, ApplicationsServiceProxy, DropdownDto, SubApplicationDto, SubApplicationServiceProxy, TechniqueDto, TechniqueServiceProxy, TestMasterDto, TestMasterInputDto, TestMasterServiceProxy, TestSubApplicationDto, TestVariableDto, UnitDto, UnitServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from "rxjs/operators";
import * as XLSX from 'xlsx';


@Component({
    selector: 'create-edit-test-modal',
    templateUrl: './create-edit-test-master-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-edit-test-master-modal.component.less']
})
export class CreateOrEditTestModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;
    editMode: boolean = false;
    testMasterForm!: FormGroup;
    testMasterList: DropdownDto[] = [];
    filteredTestMasterList: DropdownDto[] = [];
    selectedTestMaster: DropdownDto;

    unitList: UnitDto[] = [];
    techniqueList: TechniqueDto[] = [];
    applicationList: ApplicationsDto[] = [];
    subApplicationList: SubApplicationDto[] = [];

    testMasterInput: TestMasterDto = new TestMasterDto();

    worksheetList: any = [];
    currentWorkBook: XLSX.WorkBook = { SheetNames: [], Sheets: {} };
    currentUploadedExcelSheets: any[] = []; // this variable should only have an array of title value pair. Where title would be a sheet name and vlaue should be indexNumber of the sheet.
   
    constructor(
        injector: Injector,
        private _testMasterService: TestMasterServiceProxy,
        private _unitService: UnitServiceProxy,
        private _techniqueService: TechniqueServiceProxy,
        private _subApplicationService: SubApplicationServiceProxy,
        private _applicationService: ApplicationsServiceProxy,
        private formBuilder: FormBuilder
    ) {
        super(injector);
    }


    async show(testMasterId?: string) {
        this.currentUploadedExcelSheets = [];
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
        await this.loadUnitList();
        await this.loadTechniqueList();
        await this.loadApplicationList();
        await this.loadTestMasterList();
    }

    async loadUnitList() {
        let unitList = await this._unitService.getUnitList().toPromise();
        if (unitList.length > 0) {
            this.unitList = [];
            unitList.forEach((unitItem: UnitDto) => {
                this.unitList.push(unitItem);
            });
        }
    }

    async loadTechniqueList() {
        let techniqueList = await this._techniqueService.getTechniqueList().toPromise();
        if (techniqueList.length > 0) {
            this.techniqueList = [];
            techniqueList.forEach((tehniqueItem: TechniqueDto) => {
                this.techniqueList.push(tehniqueItem);
            });
        }
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

    deleteSubApplicationItem(indexValue: number){
        const testSubApplicationArray = this.testSubApplications;
        testSubApplicationArray.removeAt(indexValue);
    }

    deleteTestVariableItem(indexValue: number){
        const testVariableArray = this.testVariables;
        testVariableArray.removeAt(indexValue);
    }


    async onApplicationSelect(applicationId: string) {
        this.subApplicationList = [];
        let subApplicationList = await this._subApplicationService.getSubApplicationList(applicationId).toPromise();
        if (subApplicationList.length > 0) {
            subApplicationList.forEach((subApplicationItem: SubApplicationDto) => {
                this.subApplicationList.push(subApplicationItem);
            });
        }
    }

    onSubApplicationSelect(subApplicationId: string) {
        let tempSubApplicationList = this.testSubApplications.value;
        let existingSubApplicationItem = tempSubApplicationList.some(item => item.subApplicationId == subApplicationId);
        if (!existingSubApplicationItem) {
            let subApplication = this.subApplicationList.find(x => x.id == subApplicationId);
            let subApplicationItem: TestSubApplicationDto = new TestSubApplicationDto({ id: "", isMOEF: false, isNABL: false, name: subApplication.name, subApplicationId: subApplicationId, testId: "" });
            let subApplicationForm = this.createTestSubApplications(subApplicationItem);
            this.testSubApplications.push(subApplicationForm);
        }
    }

    onExcelFileUpload(evt: any) {
        /* wire up file reader */
        const target: DataTransfer = <DataTransfer>(evt.target);
        if (target.files.length !== 1) throw new Error('Cannot use multiple files');
        const reader: FileReader = new FileReader();
        reader.onload = (e: any) => {
            /* read workbook */
            const bstr: string = e.target.result;
            this.currentWorkBook = XLSX.read(bstr, { type: 'binary' });
            let tempSheetNameList = this.currentWorkBook.SheetNames;

            //This exercise is required because we need to give the user a flexibility to select a sheet from the uploaded excel file. This is for giving a custom selection of excel, data cell for fetching the data
            if (tempSheetNameList.length > 0) {
                this.currentUploadedExcelSheets = [];
                for (let i: number = 0; i < tempSheetNameList.length; i++) {
                    let sheetItem = { title: tempSheetNameList[i], value: tempSheetNameList[i] };
                    this.currentUploadedExcelSheets.push(sheetItem);
                }
            }
        };
        reader.readAsBinaryString(target.files[0]);
    }

    getProductsData() {
        return [
            {
                id: '1000',
                code: 'f230fh0g3',
                name: 'Bamboo Watch',
                description: 'Product Description',
                image: 'bamboo-watch.jpg',
                price: 65,
                category: 'Accessories',
                quantity: 24,
                inventoryStatus: 'INSTOCK',
                rating: 5
            },
            {
                id: '1001',
                code: 'nvklal433',
                name: 'Black Watch',
                description: 'Product Description',
                image: 'black-watch.jpg',
                price: 72,
                category: 'Accessories',
                quantity: 61,
                inventoryStatus: 'OUTOFSTOCK',
                rating: 4
            },
            {
                id: '1002',
                code: 'zz21cz3c1',
                name: 'Blue Band',
                description: 'Product Description',
                image: 'blue-band.jpg',
                price: 79,
                category: 'Fitness',
                quantity: 2,
                inventoryStatus: 'LOWSTOCK',
                rating: 3
            },
            {
                id: '1003',
                code: '244wgerg2',
                name: 'Blue T-Shirt',
                description: 'Product Description',
                image: 'blue-t-shirt.jpg',
                price: 29,
                category: 'Clothing',
                quantity: 25,
                inventoryStatus: 'INSTOCK',
                rating: 5
            },
            {
                id: '1004',
                code: 'h456wer53',
                name: 'Bracelet',
                description: 'Product Description',
                image: 'bracelet.jpg',
                price: 15,
                category: 'Accessories',
                quantity: 73,
                inventoryStatus: 'INSTOCK',
                rating: 4
            },
            {
                id: '1005',
                code: 'av2231fwg',
                name: 'Brown Purse',
                description: 'Product Description',
                image: 'brown-purse.jpg',
                price: 120,
                category: 'Accessories',
                quantity: 0,
                inventoryStatus: 'OUTOFSTOCK',
                rating: 4
            }
        ];
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