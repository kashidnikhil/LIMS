import { Component, Injector, ViewChild, ViewEncapsulation } from "@angular/core";
import { FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ApplicationsDto, ApplicationsServiceProxy, SubApplicationDto, SubApplicationServiceProxy, TechniqueDto, TechniqueServiceProxy, TestMasterDto, TestMasterServiceProxy, UnitDto, UnitServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from 'ngx-bootstrap/modal';
import * as XLSX from 'xlsx';

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
    products: Product[];


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
        this.products = this.getProductsData();
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
            id: new FormControl(testItem.id, []),
            // testVariables: testItem.id ? this.formBuilder.array(
            //     testItem.customerAddresses.map((x : CustomerAddressDto) => 
            //         this.createTestVariables(x)
            //       )
            // ) : this.formBuilder.array([this.createTestVariables(addressItem)])
            
        });
    }

    // createTestVariables(customerAddress : CustomerAddressDto) : FormGroup {
    //     return this.formBuilder.group({
    //         id: new FormControl(customerAddress.id, []),
    //         addressLine1: new FormControl(customerAddress.addressLine1, Validators.required),
    //         addressLine2: new FormControl(customerAddress.addressLine2, []),
    //         city: new FormControl(customerAddress.city, []),
    //         state: new FormControl(customerAddress.state, []),
    //         isTemporaryDelete: new FormControl(customerAddress.isTemporaryDelete ? customerAddress.isTemporaryDelete : false, []),
    //         customerId: new FormControl(customerAddress.customerId, [])
    //     });
    // }

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
            },
            {
                id: '1006',
                code: 'bib36pfvm',
                name: 'Chakra Bracelet',
                description: 'Product Description',
                image: 'chakra-bracelet.jpg',
                price: 32,
                category: 'Accessories',
                quantity: 5,
                inventoryStatus: 'LOWSTOCK',
                rating: 3
            },
            {
                id: '1007',
                code: 'mbvjkgip5',
                name: 'Galaxy Earrings',
                description: 'Product Description',
                image: 'galaxy-earrings.jpg',
                price: 34,
                category: 'Accessories',
                quantity: 23,
                inventoryStatus: 'INSTOCK',
                rating: 5
            },
            {
                id: '1008',
                code: 'vbb124btr',
                name: 'Game Controller',
                description: 'Product Description',
                image: 'game-controller.jpg',
                price: 99,
                category: 'Electronics',
                quantity: 2,
                inventoryStatus: 'LOWSTOCK',
                rating: 4
            },
            {
                id: '1009',
                code: 'cm230f032',
                name: 'Gaming Set',
                description: 'Product Description',
                image: 'gaming-set.jpg',
                price: 299,
                category: 'Electronics',
                quantity: 63,
                inventoryStatus: 'INSTOCK',
                rating: 3
            },
            {
                id: '1010',
                code: 'plb34234v',
                name: 'Gold Phone Case',
                description: 'Product Description',
                image: 'gold-phone-case.jpg',
                price: 24,
                category: 'Accessories',
                quantity: 0,
                inventoryStatus: 'OUTOFSTOCK',
                rating: 4
            },
            {
                id: '1011',
                code: '4920nnc2d',
                name: 'Green Earbuds',
                description: 'Product Description',
                image: 'green-earbuds.jpg',
                price: 89,
                category: 'Electronics',
                quantity: 23,
                inventoryStatus: 'INSTOCK',
                rating: 4
            },
            {
                id: '1012',
                code: '250vm23cc',
                name: 'Green T-Shirt',
                description: 'Product Description',
                image: 'green-t-shirt.jpg',
                price: 49,
                category: 'Clothing',
                quantity: 74,
                inventoryStatus: 'INSTOCK',
                rating: 5
            },
            {
                id: '1013',
                code: 'fldsmn31b',
                name: 'Grey T-Shirt',
                description: 'Product Description',
                image: 'grey-t-shirt.jpg',
                price: 48,
                category: 'Clothing',
                quantity: 0,
                inventoryStatus: 'OUTOFSTOCK',
                rating: 3
            },
            {
                id: '1014',
                code: 'waas1x2as',
                name: 'Headphones',
                description: 'Product Description',
                image: 'headphones.jpg',
                price: 175,
                category: 'Electronics',
                quantity: 8,
                inventoryStatus: 'LOWSTOCK',
                rating: 5
            },
            {
                id: '1015',
                code: 'vb34btbg5',
                name: 'Light Green T-Shirt',
                description: 'Product Description',
                image: 'light-green-t-shirt.jpg',
                price: 49,
                category: 'Clothing',
                quantity: 34,
                inventoryStatus: 'INSTOCK',
                rating: 4
            },
            {
                id: '1016',
                code: 'k8l6j58jl',
                name: 'Lime Band',
                description: 'Product Description',
                image: 'lime-band.jpg',
                price: 79,
                category: 'Fitness',
                quantity: 12,
                inventoryStatus: 'INSTOCK',
                rating: 3
            },
            {
                id: '1017',
                code: 'v435nn85n',
                name: 'Mini Speakers',
                description: 'Product Description',
                image: 'mini-speakers.jpg',
                price: 85,
                category: 'Clothing',
                quantity: 42,
                inventoryStatus: 'INSTOCK',
                rating: 4
            },
            {
                id: '1018',
                code: '09zx9c0zc',
                name: 'Painted Phone Case',
                description: 'Product Description',
                image: 'painted-phone-case.jpg',
                price: 56,
                category: 'Accessories',
                quantity: 41,
                inventoryStatus: 'INSTOCK',
                rating: 5
            },
            {
                id: '1019',
                code: 'mnb5mb2m5',
                name: 'Pink Band',
                description: 'Product Description',
                image: 'pink-band.jpg',
                price: 79,
                category: 'Fitness',
                quantity: 63,
                inventoryStatus: 'INSTOCK',
                rating: 4
            },
            {
                id: '1020',
                code: 'r23fwf2w3',
                name: 'Pink Purse',
                description: 'Product Description',
                image: 'pink-purse.jpg',
                price: 110,
                category: 'Accessories',
                quantity: 0,
                inventoryStatus: 'OUTOFSTOCK',
                rating: 4
            },
            {
                id: '1021',
                code: 'pxpzczo23',
                name: 'Purple Band',
                description: 'Product Description',
                image: 'purple-band.jpg',
                price: 79,
                category: 'Fitness',
                quantity: 6,
                inventoryStatus: 'LOWSTOCK',
                rating: 3
            },
            {
                id: '1022',
                code: '2c42cb5cb',
                name: 'Purple Gemstone Necklace',
                description: 'Product Description',
                image: 'purple-gemstone-necklace.jpg',
                price: 45,
                category: 'Accessories',
                quantity: 62,
                inventoryStatus: 'INSTOCK',
                rating: 4
            },
            {
                id: '1023',
                code: '5k43kkk23',
                name: 'Purple T-Shirt',
                description: 'Product Description',
                image: 'purple-t-shirt.jpg',
                price: 49,
                category: 'Clothing',
                quantity: 2,
                inventoryStatus: 'LOWSTOCK',
                rating: 5
            },
            {
                id: '1024',
                code: 'lm2tny2k4',
                name: 'Shoes',
                description: 'Product Description',
                image: 'shoes.jpg',
                price: 64,
                category: 'Clothing',
                quantity: 0,
                inventoryStatus: 'INSTOCK',
                rating: 4
            },
            {
                id: '1025',
                code: 'nbm5mv45n',
                name: 'Sneakers',
                description: 'Product Description',
                image: 'sneakers.jpg',
                price: 78,
                category: 'Clothing',
                quantity: 52,
                inventoryStatus: 'INSTOCK',
                rating: 4
            },
            {
                id: '1026',
                code: 'zx23zc42c',
                name: 'Teal T-Shirt',
                description: 'Product Description',
                image: 'teal-t-shirt.jpg',
                price: 49,
                category: 'Clothing',
                quantity: 3,
                inventoryStatus: 'LOWSTOCK',
                rating: 3
            },
            {
                id: '1027',
                code: 'acvx872gc',
                name: 'Yellow Earbuds',
                description: 'Product Description',
                image: 'yellow-earbuds.jpg',
                price: 89,
                category: 'Electronics',
                quantity: 35,
                inventoryStatus: 'INSTOCK',
                rating: 3
            },
            {
                id: '1028',
                code: 'tx125ck42',
                name: 'Yoga Mat',
                description: 'Product Description',
                image: 'yoga-mat.jpg',
                price: 20,
                category: 'Fitness',
                quantity: 15,
                inventoryStatus: 'INSTOCK',
                rating: 5
            },
            {
                id: '1029',
                code: 'gwuby345v',
                name: 'Yoga Set',
                description: 'Product Description',
                image: 'yoga-set.jpg',
                price: 20,
                category: 'Fitness',
                quantity: 25,
                inventoryStatus: 'INSTOCK',
                rating: 8
            }
        ];
    }

    save() : void {

    }



}