import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ResponseDto, TaxDto, TaxInputDto, TaxServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-tax-modal',
    templateUrl: './create-or-edit-tax-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-tax-modal.component.less']
})
export class CreateOrEditTaxModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() restoreTax: EventEmitter<ResponseDto> = new EventEmitter<ResponseDto>();

    active = false;
    saving = false;
    taxItem : TaxDto = new TaxDto();
    
    constructor(
        injector: Injector,
        private _taxService : TaxServiceProxy
    ) {
        super(injector);
    }

    show(taxId?: string): void {
        if (!taxId) {
            this.taxItem = new TaxDto({id : null, name: "", percentage : 0}); 
            this.active = true;
            this.modal.show();
        }
        else{
            this._taxService.getTaxById(taxId).subscribe((response : TaxDto)=> {
                this.taxItem = response;
                this.active = true;
                this.modal.show();
            });
        }        
    }

   
    onShown(): void {
        document.getElementById('name').focus();
    }

    save(): void {
        let input = new TaxInputDto();
        input = this.taxItem;
        this.saving = true;
        this._taxService
            .insertOrUpdateTax(input)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((response : ResponseDto) => {
                if(!response.dataMatchFound){
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modalSave.emit(null);
                }
                else{
                    this.close();
                    this.restoreTax.emit(response);
                }
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
