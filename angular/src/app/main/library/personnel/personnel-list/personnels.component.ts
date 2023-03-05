import { Component, Injector, ViewChild, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PersonnelDto, PersonnelServiceProxy, ResponseDto } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { CreateOrEditPersonnelModalComponent } from '../create-edit-personnel/create-or-edit-personnel-modal.component';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './personnels.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./personnels.component.less'],
    animations: [appModuleAnimation()],
})
export class PersonnelsComponent extends AppComponentBase implements AfterViewInit {
    @ViewChild('createOrEditPesonnelModal', { static: true }) createOrEditPersonnelModal: CreateOrEditPersonnelModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
    //Filters
    filterText = '';

    constructor(
        injector: Injector,
        private _personnelService : PersonnelServiceProxy,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
        this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    }

    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
    }

    getPersonnels(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._personnelService
            .getPersonnels(
                    this.filterText,
                    this.primengTableHelper.getSorting(this.dataTable),
                    this.primengTableHelper.getMaxResultCount(this.paginator, event),
                    this.primengTableHelper.getSkipCount(this.paginator, event)
            )
            .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createPersonnel(): void {
        this.createOrEditPersonnelModal.show();
    }

    deletePersonnel(personnelItem: PersonnelDto): void {
        this.message.confirm(this.l('PersonnelDeleteWarningMessage', personnelItem.name), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._personnelService.deletePersonnel(personnelItem.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    restorePersonnel(personnelResponse: ResponseDto):void {
        if(personnelResponse.id == null){
            if(personnelResponse.isExistingDataAlreadyDeleted){
                this.message.confirm(this.l('PersonnelRestoreMessage', personnelResponse.name), this.l('AreYouSure'), async (isConfirmed) => {
                    if (isConfirmed) {
                        this._personnelService.restorePersonnel(personnelResponse.restoringItemId).subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('PersonnelSuccessfullyRestored'));
                        });
                    }
                });
            }
            else{
                this.notify.error(this.l('ExistingPersonnelErrorMessage',personnelResponse.name));
            }
        }
        else{
            if(personnelResponse.isExistingDataAlreadyDeleted){
                this.message.confirm(this.l('NewPersonnelErrorMessage', personnelResponse.name), this.l('AreYouSure'), async (isConfirmed) => {
                    if (isConfirmed) {
                        this._personnelService.restorePersonnel(personnelResponse.restoringItemId).subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('PersonnelSuccessfullyRestored'));
                        });
                    }
                });
            }   
            else{
                this.notify.error(this.l('ExistingPersonnelErrorMessage',personnelResponse.name));
            }
        }
    }
}
