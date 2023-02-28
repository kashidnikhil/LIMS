import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ContactPersonDto, CustomerAddressDto, CustomerMasterDto, CustomerMasterInputDto, CustomerMasterServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
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
    submitted = false;
    editMode : boolean = false;
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
            let customerItem : CustomerMasterDto = new CustomerMasterDto();
            this.initialiseCustomerForm(customerItem);
            this.editMode = false;
            this.active = true;
            this.modal.show();
        }
        else {
            this._customerService.getCustomerById(customerId).subscribe((response: CustomerMasterDto) => {
                let customerItem = response;
                this.initialiseCustomerForm(customerItem);
                this.editMode = true;
                this.active = true;
                this.modal.show();
            });
        }
    }


    initialiseCustomerForm(customerItem : CustomerMasterDto) {
        let addressItem : CustomerAddressDto = new CustomerAddressDto();
        let contactPersonItem: ContactPersonDto = new ContactPersonDto();
        this.customerForm = this.formBuilder.group({
            title: new FormControl(customerItem.title, []),
            name: new FormControl(customerItem.name, [
                // Validators.required,
            ]),
            initials: new FormControl(customerItem.initials, []),
            gstNumber: new FormControl(customerItem.gstNumber, []),
            mobileNumber: new FormControl(customerItem.mobileNumber, []),
            emailId: new FormControl(customerItem.emailId, [Validators.email]),
            discount: new FormControl(customerItem.discount, []),
            industry: new FormControl(customerItem.industry, []),
            website: new FormControl(customerItem.website, []),
            isSEZ: new FormControl(customerItem.isSEZ, []),
            vendorCode : new FormControl(customerItem.vendorCode, []),        
            commonDescription : new FormControl(customerItem.commonDescription, []),
            commercialDescription: new FormControl(customerItem.commercialDescription, []),
            specialDescription: new FormControl(customerItem.specialDescription, []),
            id: new FormControl(customerItem.id, []),
            customerAddresses: customerItem.id ? this.formBuilder.array(
                customerItem.customerAddresses.map((x : CustomerAddressDto) => 
                    this.createCustomerAddress(x)
                  )
            ) : this.formBuilder.array([this.createCustomerAddress(addressItem)]),
            customerContactPersons: customerItem.id ?  this.formBuilder.array(
                customerItem.customerContactPersons.map((x : ContactPersonDto) => 
                    this.createContactPerson(x)
                  )
            ) : this.formBuilder.array([this.createContactPerson(contactPersonItem)])

        });
    }

    get customerAddresses(): FormArray {
        return (<FormArray>this.customerForm.get('customerAddresses'));
    }

    get customerContactPersons(): FormArray{
        return (<FormArray>this.customerForm.get('customerContactPersons'));
    }

    createCustomerAddress(customerAddress : CustomerAddressDto) : FormGroup {
        return this.formBuilder.group({
            id: new FormControl(customerAddress.id, []),
            addressLine1: new FormControl(customerAddress.addressLine1, Validators.required),
            addressLine2: new FormControl(customerAddress.addressLine2, []),
            city: new FormControl(customerAddress.city, []),
            state: new FormControl(customerAddress.state, []),
            isTemporaryDelete: new FormControl(customerAddress.isTemporaryDelete, []),
            customerId: new FormControl(customerAddress.customerId, [])
        });
    }

    createContactPerson(contactPersonItem : ContactPersonDto) : FormGroup {
        return this.formBuilder.group({
            id: new FormControl(contactPersonItem.id, []),
            name: new FormControl(contactPersonItem.name, Validators.required),
            designation: new FormControl(contactPersonItem.designation, Validators.required),
            department: new FormControl(contactPersonItem.department, Validators.required),
            emailId: new FormControl(contactPersonItem.emailId, [Validators.required, Validators.email]),
            isTemporaryDelete: new FormControl(contactPersonItem.isTemporaryDelete, []),
            mobileNumber: new FormControl(contactPersonItem.mobileNumber,[]),
            customerId: new FormControl(contactPersonItem.customerId, [])
        });
    }

    addCustomerAddress() {
        let customerAddressItem : CustomerAddressDto = new CustomerAddressDto();
        let addressForm = this.createCustomerAddress(customerAddressItem);
        this.customerAddresses.push(addressForm);
    }

    // deleteAddressGroup(index: number) {
    //    this.customerAddresses.removeAt(index);
    // }

    addContactPerson() {
        let contactPersonItem : ContactPersonDto = new ContactPersonDto();
        let contactPersonForm = this.createContactPerson(contactPersonItem);
        this.customerContactPersons.push(contactPersonForm);
    }

    // deleteContactPersonGroup(index: number) {
    //    this.customerContactPersons.removeAt(index);
    // }

    onShown(): void {
        document.getElementById('title').focus();
    }

    save(): void {
        this.submitted = true;
        if(this.customerForm.valid){
            let input = new CustomerMasterInputDto();
            input = this.customerForm.value;
            this.saving = true;
           
            this._customerService
                .insertOrUpdateCustomer(input)
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

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
