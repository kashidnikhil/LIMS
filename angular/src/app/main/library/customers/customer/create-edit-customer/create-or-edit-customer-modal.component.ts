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
    submitted = false;
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
        this.customerForm = this.formBuilder.group({
            title: new FormControl(this.customerItem.title, []),
            name: new FormControl(this.customerItem.name, [
                // Validators.required,
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
                [this.createCustomerAddress()])

            // new FormControl(this.customerItem.gstNumber, []),
            // customerPOs: new FormControl(this.customerItem.customerPOs, []),
            // customerContactPersons: new FormControl(this.customerItem.customerContactPersons.gstNumber, []),
        });
    }

    get customerAddresses(): FormArray {
        return (<FormArray>this.customerForm.get('customerAddresses'));
    }

    createCustomerAddress() {
        return this.formBuilder.group({
            id: new FormControl('', []),
            addressLine1: new FormControl('', Validators.required),
            addressLine2: new FormControl('', []),
            city: new FormControl('', []),
            state: new FormControl('', []),
            customerId: new FormControl('', [])
        });
    }

    addCustomerAddress() {
        let addressForm = this.createCustomerAddress();
        this.customerAddresses.push(addressForm);
    }

    deleteAddressGroup(index: number) {
       this.customerAddresses.removeAt(index);
    }

    onShown(): void {
        document.getElementById('title').focus();
    }

    save(): void {
        let input = new CustomerMasterInputDto();
        input = this.customerItem;
        this.saving = true;
        this.submitted = true;
        this.saving = false;
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
