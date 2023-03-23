import { Component, Injector, ViewChild, ViewEncapsulation } from "@angular/core";
import { FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ApplicationsDto, ApplicationsServiceProxy, SubApplicationDto, SubApplicationServiceProxy, TechniqueDto, TechniqueServiceProxy, TestMasterDto, TestMasterServiceProxy, UnitDto, UnitServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from 'ngx-bootstrap/modal';
import * as XLSX from 'xlsx';

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
    techniqueList : TechniqueDto[] =[];
    applicationList : ApplicationsDto[] = [];
    subApplicationList : SubApplicationDto[] =[];

    worksheetList: any = [];
    currentWorkBook : XLSX.WorkBook = {SheetNames : [], Sheets : {}};
    currentUploadedExcelSheets : any[] = []; // this variable should only have an array of title value pair. Where title would be a sheet name and vlaue should be indexNumber of the sheet.


    constructor(
        injector: Injector,
        private _testMasterService: TestMasterServiceProxy,
        private _unitService : UnitServiceProxy,
        private _techniqueService: TechniqueServiceProxy,
        private _subApplicationService : SubApplicationServiceProxy,
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
        await this.loadTechniqueList();
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

     async loadTechniqueList(){
        let techniqueList = await this._techniqueService.getTechniqueList().toPromise();
        if(techniqueList.length > 0){
            this.techniqueList = [];
            techniqueList.forEach((tehniqueItem : TechniqueDto) => {
                this.techniqueList.push(tehniqueItem);
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
            method: new FormControl(testItem.method, []),
            methodDescription: new FormControl(testItem.methodDescription, []),
            isSC: new FormControl(testItem.isSC, []),
            rate: new FormControl(testItem.rate, []),
            worksheetName :  new FormControl('', []),
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

    async onApplicationSelect(applicationId : string){
        this.subApplicationList = [];
        let subApplicationList = await this._subApplicationService.getSubApplicationList(applicationId).toPromise();
        if(subApplicationList.length > 0){
            subApplicationList.forEach((subApplicationItem : SubApplicationDto) => {
                this.subApplicationList.push(subApplicationItem);
            });
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
          if(tempSheetNameList.length > 0){
            this.currentUploadedExcelSheets = [];
            for ( let i: number = 0; i < tempSheetNameList.length ; i++){
              let sheetItem = { title : tempSheetNameList[i], value : i};
              this.currentUploadedExcelSheets.push(sheetItem);
            }
            console.log(this.currentUploadedExcelSheets);
          }
          /* grab first sheet */
          // const wsname: string = wb.SheetNames[0];
          // const ws: XLSX.WorkSheet = wb.Sheets[wsname];
    
        };
        reader.readAsBinaryString(target.files[0]);
      }

    save() : void {

    }



}