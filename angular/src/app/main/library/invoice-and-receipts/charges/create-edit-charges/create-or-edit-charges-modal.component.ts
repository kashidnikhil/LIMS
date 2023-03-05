import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ChargesDto, ChargesInputDto, ChargesServiceProxy, ResponseDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-charges-modal',
    templateUrl: './create-or-edit-charges-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-charges-modal.component.less']
})
export class CreateOrEditChargesModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() restoreCharge: EventEmitter<ResponseDto> = new EventEmitter<ResponseDto>();
    

    active = false;
    saving = false;
    chargesItem : ChargesDto = new ChargesDto();
    
    constructor(
        injector: Injector,
        private _chargesService : ChargesServiceProxy
    ) {
        super(injector);
    }

    show(chargesId?: string): void {
        if (!chargesId) {
            this.chargesItem = new ChargesDto({id : null, name: "", description: ""}); 
            this.active = true;
            this.modal.show();
        }
        else{
            this._chargesService.getChargesById(chargesId).subscribe((response : ChargesDto)=> {
                this.chargesItem = response;
                this.active = true;
                this.modal.show();
            });
        }        
    }

   
    onShown(): void {
        document.getElementById('name').focus();
    }

    save(): void {
        let input = new ChargesInputDto();
        input = this.chargesItem;
        this.saving = true;
        this._chargesService
            .insertOrUpdateCharges(input)
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
                    this.restoreCharge.emit(response);
                }
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
