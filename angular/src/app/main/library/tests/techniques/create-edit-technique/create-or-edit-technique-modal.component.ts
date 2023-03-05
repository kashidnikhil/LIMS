import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    ResponseDto,
    TechniqueDto,
    TechniqueInputDto,
    TechniqueServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-technique-modal',
    templateUrl: './create-or-edit-technique-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-technique-modal.component.less']
})
export class CreateOrEditTechniqueModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() restoreTechnique: EventEmitter<ResponseDto> = new EventEmitter<ResponseDto>();
    

    active = false;
    saving = false;
    techniqueItem : TechniqueDto = new TechniqueDto();
    
    constructor(
        injector: Injector,
        private _techniqueService : TechniqueServiceProxy
    ) {
        super(injector);
    }

    show(techniqueId?: string): void {
        if (!techniqueId) {
            this.techniqueItem = new TechniqueDto({id : null, name: "", description: ""}); 
            this.active = true;
            this.modal.show();
        }
        else{
            this._techniqueService.getTechniqueById(techniqueId).subscribe((response : TechniqueDto)=> {
                this.techniqueItem = response;
                this.active = true;
                this.modal.show();
            });
        }        
    }

   
    onShown(): void {
        document.getElementById('name').focus();
    }

    save(): void {
        let input = new TechniqueInputDto();
        input = this.techniqueItem;
        this.saving = true;
        this._techniqueService
            .insertOrUpdateTechnique(input)
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
                    this.restoreTechnique.emit(response);
                }
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    
}
