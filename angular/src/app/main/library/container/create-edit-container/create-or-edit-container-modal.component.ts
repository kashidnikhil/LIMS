import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ContainerDto, ContainerInputDto, ContainerServiceProxy, ResponseDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-container-modal',
    templateUrl: './create-or-edit-container-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-container-modal.component.less']
})
export class CreateOrEditContainerModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() restoreContainer: EventEmitter<ResponseDto> = new EventEmitter<ResponseDto>();

    active = false;
    saving = false;
    containerItem : ContainerDto = new ContainerDto();
    
    constructor(
        injector: Injector,
        private _conatinerService : ContainerServiceProxy
    ) {
        super(injector);
    }

    show(taxId?: string): void {
        if (!taxId) {
            this.containerItem = new ContainerDto({id : null, name: "", description: ""}); 
            this.active = true;
            this.modal.show();
        }
        else{
            this._conatinerService.getContainerById(taxId).subscribe((response : ContainerDto)=> {
                this.containerItem = response;
                this.active = true;
                this.modal.show();
            });
        }        
    }

   
    onShown(): void {
        document.getElementById('name').focus();
    }

    save(): void {
        let input = new ContainerInputDto();
        input = this.containerItem;
        this.saving = true;
        this._conatinerService
            .insertOrUpdateContainer(input)
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
                    this.restoreContainer.emit(response);
                }
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
