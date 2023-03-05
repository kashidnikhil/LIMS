import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ResponseDto, SourceDto, SourceInputDto, SourceServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-source-modal',
    templateUrl: './create-or-edit-source-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-source-modal.component.less']
})
export class CreateOrEditSourceModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() restoreSource: EventEmitter<ResponseDto> = new EventEmitter<ResponseDto>();

    active = false;
    saving = false;
    sourceItem : SourceDto = new SourceDto();
    
    constructor(
        injector: Injector,
        private _sourceService : SourceServiceProxy
    ) {
        super(injector);
    }

    show(applicationId?: string): void {
        if (!applicationId) {
            this.sourceItem = new SourceDto({id : null, name: "", description: ""}); 
            this.active = true;
            this.modal.show();
        }
        else{
            this._sourceService.getSourceById(applicationId).subscribe((response : SourceDto)=> {
                this.sourceItem = response;
                this.active = true;
                this.modal.show();
            });
        }        
    }

   
    onShown(): void {
        document.getElementById('name').focus();
    }

    save(): void {
        let input = new SourceInputDto();
        input = this.sourceItem;
        this.saving = true;
        this._sourceService
            .insertOrUpdateSource(input)
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
                    this.restoreSource.emit(response);
                }
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
