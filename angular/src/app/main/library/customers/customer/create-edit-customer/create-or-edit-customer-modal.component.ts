import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CustomerMasterDto, CustomerMasterInputDto, CustomerMasterServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

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
    customerItem: CustomerMasterDto = new CustomerMasterDto();
    customerForm!: FormGroup;

    constructor(
        injector: Injector,
        private _customerService: CustomerMasterServiceProxy,
        private formBuilder: FormBuilder
    ) {
        super(injector);
    }

    show(customerId?: string): void {
        if (!customerId) {
            this.initialiseCustomerForm();
            //this.addCustomerAddress();
            this.active = true;
            this.modal.show();
        }
        else {
            this._customerService.getCustomerById(customerId).subscribe((response: CustomerMasterDto) => {
                this.customerItem = response;
                this.active = true;
                this.modal.show();
            });
        }
    }


    initialiseCustomerForm() {
        this.customerForm = new FormGroup({
            title: new FormControl(this.customerItem.title, []),
            name: new FormControl(this.customerItem.name, [
                // Validators.required,
                // Validators.minLength(1),
                // Validators.maxLength(250),
            ]),
            initials: new FormControl(this.customerItem.initials, []),
            gstNumber: new FormControl(this.customerItem.gstNumber, []),
            mobileNumber: new FormControl(this.customerItem.mobileNumber, []),
            emailId: new FormControl(this.customerItem.emailId, [Validators.email]),
            discount: new FormControl(this.customerItem.discount, []),
            industry: new FormControl(this.customerItem.industry, []),
            website: new FormControl(this.customerItem.website, []),
            isSEZ: new FormControl(this.customerItem.isSEZ, []),
            commercialDescription: new FormControl(this.customerItem.commercialDescription, []),
            specialDescription: new FormControl(this.customerItem.specialDescription, []),
            id: new FormControl(this.customerItem.id, []),
            customerAddresses: this.formBuilder.array(
				[this.createCustomerAddress()],
				[Validators.required])
                
            // new FormControl(this.customerItem.gstNumber, []),
            // customerPOs: new FormControl(this.customerItem.customerPOs, []),
            // customerContactPersons: new FormControl(this.customerItem.customerContactPersons.gstNumber, []),
        });
    }

    get addresses(): FormArray {
		return this.customerForm.get('customerAddresses') as FormArray;
  	}

    createCustomerAddress(){
        return this.formBuilder.group({
            id: ['',[]],
            addressLine1: ['',Validators.required],
            addressLine2: ['',[]],
            city: ['',[]],
            state: ['',[]],
            customerId: ['',[]]
        });
    }
    
    addCustomerAddress() {
        const addressList = this.customerForm.get('customerAddresses') as FormArray;
        let addressForm = this.createCustomerAddress();
		addressList.push(addressForm);
    }

    deleteAddressGroup(index: number) {
        const add = this.customerForm.get('customerAddresses') as FormArray;
        add.removeAt(index)
      }

    onShown(): void {
        document.getElementById('title').focus();
    }

    save(): void {
        let input = new CustomerMasterInputDto();
        input = this.customerItem;
        console.log(input);
        this.saving = true;
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
