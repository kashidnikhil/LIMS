import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { BankDto, BankInputDto, BankServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-bank-modal',
    templateUrl: './create-or-edit-bank-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-bank-modal.component.less']
})
export class CreateOrEditBankModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    bankItem : BankDto = new BankDto();
    
    constructor(
        injector: Injector,
        private _bankService : BankServiceProxy
    ) {
        super(injector);
    }

    show(applicationId?: string): void {
        if (!applicationId) {
            this.bankItem = new BankDto({id : null, name: "", description: ""}); 
            this.active = true;
            this.modal.show();
        }
        else{
            this._bankService.getBankById(applicationId).subscribe((response : BankDto)=> {
                this.bankItem = response;
                this.active = true;
                this.modal.show();
            });
        }        
    }

   
    onShown(): void {
        document.getElementById('name').focus();
    }

    save(): void {
        let input = new BankInputDto();
        input = this.bankItem;
        this.saving = true;
        this._bankService
            .insertOrUpdateBank(input)
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
