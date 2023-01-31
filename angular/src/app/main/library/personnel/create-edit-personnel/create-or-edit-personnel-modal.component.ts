import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PersonnelDto, PersonnelInputDto, PersonnelServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-personnel-modal',
    templateUrl: './create-or-edit-personnel-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-personnel-modal.component.less']
})
export class CreateOrEditPersonnelModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    personnelItem : PersonnelDto = new PersonnelDto();
    
    constructor(
        injector: Injector,
        private _personnelService : PersonnelServiceProxy
    ) {
        super(injector);
    }

    show(taxId?: string): void {
        if (!taxId) {
            this.personnelItem = new PersonnelDto({id : null, name: "", description: ""}); 
            this.active = true;
            this.modal.show();
        }
        else{
            this._personnelService.getPersonnelById(taxId).subscribe((response : PersonnelDto)=> {
                this.personnelItem = response;
                this.active = true;
                this.modal.show();
            });
        }        
    }

   
    onShown(): void {
        document.getElementById('name').focus();
    }

    save(): void {
        let input = new PersonnelInputDto();
        input = this.personnelItem;
        this.saving = true;
        this._personnelService
            .insertOrUpdatePersonnel(input)
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
