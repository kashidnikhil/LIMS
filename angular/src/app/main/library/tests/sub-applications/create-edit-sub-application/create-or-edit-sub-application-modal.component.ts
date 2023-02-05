import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ApplicationsDto, ApplicationsServiceProxy, SubApplicationDto, SubApplicationInputDto, SubApplicationServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-sub-application-modal',
    templateUrl: './create-or-edit-sub-application-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-sub-application-modal.component.less']
})
export class CreateOrEditApplicationModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    subApplicationItem : SubApplicationDto = new SubApplicationDto();
    applicationList : ApplicationsDto[] = [];
    
    constructor(
        injector: Injector,
        private _applicationService : ApplicationsServiceProxy,
        private _subApplicationsService : SubApplicationServiceProxy
    ) {
        super(injector);
    }

    async show(applicationId?: string) {
        await this.loadApplicationsList();
        if (!applicationId) {
            this.subApplicationItem = new SubApplicationDto({id : null, name: "", description: "",applicationId : null}); 
            this.active = true;
            this.modal.show();
        }
        else{
            this._subApplicationsService.getSubApplicationById(applicationId).subscribe((response : SubApplicationDto)=> {
                this.subApplicationItem = response;
                this.active = true;
                this.modal.show();
            });
        }        
    }

    async loadApplicationsList(){
        this.applicationList = [];
        await this._applicationService.getApplicationList()
       .subscribe((response : ApplicationsDto[]) =>{
            this.applicationList = response;
        });
    }

    onShown(): void {
        document.getElementById('name').focus();
    }

    save(): void {
        let input = new SubApplicationInputDto();
        input = this.subApplicationItem;
        this.saving = true;
        this._subApplicationsService
            .insertOrUpdateSubApplication(input)
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
