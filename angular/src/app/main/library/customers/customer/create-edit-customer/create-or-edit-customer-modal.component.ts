import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CustomerAddressDto, CustomerMasterDto, CustomerMasterInputDto, CustomerMasterServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-customer-modal',
    templateUrl: './create-or-edit-customer-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-customer-modal.component.less']
})
export class CreateOrEditCustomerModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    customerItem : CustomerMasterDto;
    
    constructor(
        injector: Injector,
        private _customerService : CustomerMasterServiceProxy
    ) {
        super(injector);
    }

    show(customerId?: string): void {
        if (!customerId) {
            this.initialiseCutomerData();
            this.addCustomerAddress();
            this.active = true;
            this.modal.show();
        }
        else{
            this._customerService.getCustomerById(customerId).subscribe((response : CustomerMasterDto)=> {
                this.customerItem = response;
                this.active = true;
                this.modal.show();
            });
        }        
    }

    initialiseCutomerData()
    {
        this.customerItem = new CustomerMasterDto();
        this.customerItem.customerAddresses = [];
        this.customerItem.customerContactPersons = [];
        this.customerItem.customerPOs = [];
        // this.customerItem = new CustomerMasterDto(
        //     {
        //         commercialDescription : "",
        //         commonDescription : "",
        //         customerAddresses : [],
        //         customerContactPersons : [],
        //         customerPOs : [],
        //         discount : 0,
        //         emailId : "",
        //         gstNumber : "",
        //         industry: "",
        //         initials : "",
        //         isSEZ: false,
        //         mobileNumber :"",
        //         name:"",
        //         referenceId : "",
        //         specialDescription: "",
        //         title : "",
        //         vendorCode :"",
        //         website : "",
        //         id : ""
        //     }
        // );
    }

    addCustomerAddress(){
        if(this.customerItem.customerAddresses?.length == 0){
            this.customerItem.customerAddresses = [];
        }
       
        let customerAddressItem = new CustomerAddressDto(
            {
                id :"",
                addressLine1 : "",
                addressLine2 : "",
                city : "",
                state:"",
                customerId : ""
            });
        this.customerItem.customerAddresses.push(customerAddressItem);
    }

    onShown(): void {
        document.getElementById('title').focus();
    }

    save(): void {
        let input = new CustomerMasterInputDto();
        input = this.customerItem;
        console.log(input);
        // this.saving = true;
        // this._customerService
        //     .insertOrUpdateCustomer(input)
        //     .pipe(
        //         finalize(() => {
        //             this.saving = false;
        //         })
        //     )
        //     .subscribe(() => {
        //         this.notify.info(this.l('SavedSuccessfully'));
        //         this.close();
        //         this.modalSave.emit(null);
        //     });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
