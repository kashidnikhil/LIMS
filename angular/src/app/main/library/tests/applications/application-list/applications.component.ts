import { Component, Injector, ViewChild, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    ApplicationsDto,
    ApplicationsServiceProxy
} from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { CreateOrEditApplicationModalComponent } from '../create-edit-application/create-or-edit-application-modal.component';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './applications.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./applications.component.less'],
    animations: [appModuleAnimation()],
})
export class ApplicationsComponent extends AppComponentBase implements AfterViewInit {
    @ViewChild('createOrEditAppicationModal', { static: true }) createOrEditAppicationModal: CreateOrEditApplicationModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
    //Filters
    filterText = '';

    constructor(
        injector: Injector,
        private _applicationService : ApplicationsServiceProxy,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
        this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    }

    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
    }

    getApplications(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._applicationService
            .getApplications(
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
        this.createOrEditAppicationModal.show();
    }

    deleteApplication(application: ApplicationsDto): void {
        this.message.confirm(this.l('UserDeleteWarningMessage', application.name), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._applicationService.deleteApplication(application.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
}
