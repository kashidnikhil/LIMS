import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    ApplicationsServiceProxy,
    ApplicationsDto,
    ApplicationInputDto,
} from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {OrganizationUnitsTreeComponent} from '../../../../../admin/shared/organization-unit-tree.component';
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
    @ViewChild('organizationUnitTree') organizationUnitTree: OrganizationUnitsTreeComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

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
        this.saving = true;
        this._applicationsService
            .insertOrUpdateApplication(input)
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


    isSMTPSettingsProvided(): boolean {
        return !(
            this.s('Abp.Net.Mail.DefaultFromAddress') === '' ||
            this.s('Abp.Net.Mail.Smtp.Host') === '' ||
            this.s('Abp.Net.Mail.Smtp.UserName') === '' ||
            this.s('Abp.Net.Mail.Smtp.Password') === ''
        );
    }
}
