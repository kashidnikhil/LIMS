import { Component, EventEmitter, Injector, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    StandardRemarkDto,
    StandardRemarkInputDto,
    StandardRemarkServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {OrganizationUnitsTreeComponent} from '../../../../../admin/shared/organization-unit-tree.component';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-standard-remark-modal',
    templateUrl: './create-or-edit-standard-remark-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-standard-remark-modal.component.less']
})
export class CreateOrEditStandardRemarkModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('organizationUnitTree') organizationUnitTree: OrganizationUnitsTreeComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    standardRemarkItem : StandardRemarkDto = new StandardRemarkDto();
    
    constructor(
        injector: Injector,
        private _standardRemarkService : StandardRemarkServiceProxy
    ) {
        super(injector);
    }

    show(standardReferenceId?: string): void {
        if (!standardReferenceId) {
            this.standardRemarkItem = new StandardRemarkDto({id : null, name: "", description: ""}); 
            this.active = true;
            this.modal.show();
        }
        else{
            this._standardRemarkService.getStandardRemarkById(standardReferenceId).subscribe((response : StandardRemarkDto)=> {
                this.standardRemarkItem = response;
                this.active = true;
                this.modal.show();
            });
        }        
    }

   
    onShown(): void {
        document.getElementById('name').focus();
    }

    save(): void {
        let input = new StandardRemarkInputDto();
        input = this.standardRemarkItem;
        this.saving = true;
        this._standardRemarkService
            .insertOrUpdateStandardRemark(input)
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
