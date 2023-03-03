import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    ApplicationsServiceProxy,
    ApplicationsDto,
    ApplicationInputDto,
    ResponseDto,
} from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-application-modal',
    templateUrl: './create-or-edit-application-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-application-modal.component.less']
})
export class CreateOrEditApplicationModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() restoreApplication: EventEmitter<ResponseDto> = new EventEmitter<ResponseDto>();

    active = false;
    saving = false;
    applicationItem : ApplicationsDto = new ApplicationsDto();
    
    constructor(
        injector: Injector,
        private _applicationsService : ApplicationsServiceProxy
    ) {
        super(injector);
    }

    show(applicationId?: string): void {
        if (!applicationId) {
            this.applicationItem = new ApplicationsDto({id : null, name: "", description: ""}); 
            this.active = true;
            this.modal.show();
        }
        else{
            this._applicationsService.getApplicationById(applicationId).subscribe((response : ApplicationsDto)=> {
                this.applicationItem = response;
                this.active = true;
                this.modal.show();
            });
        }        
    }

   
    onShown(): void {
        document.getElementById('name').focus();
    }

    save(): void {
        let input = new ApplicationInputDto();
        input = this.applicationItem;
        input.name = input.name.trim();
        this.saving = true;
        this._applicationsService
            .insertOrUpdateApplication(input)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe((response : ResponseDto) => {
                console.log(response);
                if(!response.existingData){
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modalSave.emit(null);
                }
                else{
                    this.close();
                    this.restoreApplication.emit(response);
                }
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
