import { Component, Injector, ViewChild, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { StandardRemarkDto, StandardRemarkServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { CreateOrEditStandardRemarkModalComponent } from '../create-edit-standard-remark/create-or-edit-standard-remark-modal.component';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './standard-remarks.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./standard-remarks.component.less'],
    animations: [appModuleAnimation()],
})
export class StandardRemarksComponent extends AppComponentBase implements AfterViewInit {
    @ViewChild('createOrEditStandardRemarkModal', { static: true }) createOrEditStandardRemarkModal: CreateOrEditStandardRemarkModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
    //Filters
    filterText = '';

    constructor(
        injector: Injector,
        private _standardRemarkService: StandardRemarkServiceProxy,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
        this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    }

    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
    }

    getStandardRemarks(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._standardRemarkService
            .getStandardRemarks(
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

    createStandardRemark(): void {
        this.createOrEditStandardRemarkModal.show();
    }

    deleteStandardRemark(standardRemark: StandardRemarkDto): void {
        this.message.confirm(this.l('StandardRemarkDeleteWarningMessage', standardRemark.name), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._standardRemarkService.deleteStandardRemark(standardRemark.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
}
