import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CustomerMasterDto, CustomerMasterInputDto, CustomerMasterServiceProxy } from '@shared/service-proxies/service-proxies';
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
    customerItem : CustomerMasterDto = new CustomerMasterDto();
    
    constructor(
        injector: Injector,
        private _customerService : CustomerMasterServiceProxy
    ) {
        super(injector);
    }

    show(customerId?: string): void {
        if (!customerId) {
            //this.sourceItem = new SourceDto({id : null, name: "", description: ""}); 
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

   
    onShown(): void {
        document.getElementById('name').focus();
    }

    save(): void {
        let input = new CustomerMasterInputDto();
        input = this.customerItem;
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

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
