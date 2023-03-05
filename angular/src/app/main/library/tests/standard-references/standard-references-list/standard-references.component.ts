import { Component, Injector, ViewChild, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    ResponseDto,
    StandardReferenceDto,
    StandardReferenceServiceProxy
} from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { CreateOrEditStandardReferenceModalComponent } from '../create-edit-standard-reference/create-or-edit-standard-reference-modal.component';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './standard-references.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./standard-references.component.less'],
    animations: [appModuleAnimation()],
})
export class StandardReferencesComponent extends AppComponentBase implements AfterViewInit {
    @ViewChild('createOrEditStandardReferenceModal', { static: true }) createOrEditStandardReferenceModal: CreateOrEditStandardReferenceModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
    //Filters
    filterText = '';

    constructor(
        injector: Injector,
        private _standardReferenceService: StandardReferenceServiceProxy,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
        this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    }

    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
    }

    getStandardReferences(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._standardReferenceService
            .getStandardReferences(
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

    createApplication(): void {
        this.createOrEditStandardReferenceModal.show();
    }

    deleteStandardReference(standardReference: StandardReferenceDto): void {
        this.message.confirm(this.l('StandardReferenceDeleteWarningMessage', standardReference.name), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._standardReferenceService.deleteStandardReference(standardReference.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    restoreStandardReference(standardReferenceResponse: ResponseDto):void {
        if(standardReferenceResponse.id == null){
            if(standardReferenceResponse.isExistingDataAlreadyDeleted){
                this.message.confirm(this.l('StandardReferenceRestoreMessage', standardReferenceResponse.name), this.l('AreYouSure'), async (isConfirmed) => {
                    if (isConfirmed) {
                        this._standardReferenceService.restoreStandardReference(standardReferenceResponse.restoringItemId).subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('StandardReferenceSuccessfullyRestored'));
                        });
                    }
                });
            }
            else{
                this.notify.error(this.l('ExistingStandardReferenceErrorMessage',standardReferenceResponse.name));
            }
        }
        else{
            if(standardReferenceResponse.isExistingDataAlreadyDeleted){
                this.message.confirm(this.l('NewStandardReferenceErrorMessage', standardReferenceResponse.name), this.l('AreYouSure'), async (isConfirmed) => {
                    if (isConfirmed) {
                        this._standardReferenceService.restoreStandardReference(standardReferenceResponse.restoringItemId).subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('StandardReferenceSuccessfullyRestored'));
                        });
                    }
                });
            }   
            else{
                this.notify.error(this.l('ExistingStandardReferenceErrorMessage',standardReferenceResponse.name));
            }
        }
    }
}
