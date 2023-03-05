import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    StandardReferenceServiceProxy,
    StandardReferenceDto,
    StandardReferenceInputDto,
    ResponseDto,
} from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-standard-reference-modal',
    templateUrl: './create-or-edit-standard-reference-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-standard-reference-modal.component.less']
})
export class CreateOrEditStandardReferenceModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() restoreStandardReference: EventEmitter<ResponseDto> = new EventEmitter<ResponseDto>();
    

    active = false;
    saving = false;
    standardReferenceItem : StandardReferenceDto = new StandardReferenceDto();
    
    constructor(
        injector: Injector,
        private _standardReferenceService : StandardReferenceServiceProxy
    ) {
        super(injector);
    }

    show(standardReferenceId?: string): void {
        if (!standardReferenceId) {
            this.standardReferenceItem = new StandardReferenceDto({id : null, name: "", description: ""}); 
            this.active = true;
            this.modal.show();
        }
        else{
            this._standardReferenceService.getStandardReferenceById(standardReferenceId).subscribe((response : StandardReferenceDto)=> {
                this.standardReferenceItem = response;
                this.active = true;
                this.modal.show();
            });
        }        
    }

   
    onShown(): void {
        document.getElementById('name').focus();
    }

    save(): void {
        let input = new StandardReferenceInputDto();
        input = this.standardReferenceItem;
        this.saving = true;
        this._standardReferenceService
            .insertOrUpdateStandardReference(input)
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
                    this.restoreStandardReference.emit(response);
                }
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
