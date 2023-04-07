import { Component, Injector, ViewChild, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TestMasterDto, TestMasterServiceProxy, TestPlanMasterDto, TestPlanMasterServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
//import { CreateOrEditCustomerModalComponent } from '../create-edit-customer/create-or-edit-customer-modal.component';
import { finalize } from 'rxjs/operators';
//import { CreateOrEditTestModalComponent } from '../create-edit-test-master/create-edit-test-master-modal.component';

@Component({
    templateUrl: './test-plan-list.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./test-plan-list.component.less'],
    animations: [appModuleAnimation()],
})
export class TestPlanListComponent extends AppComponentBase implements AfterViewInit {
    @ViewChild('createOrEditTestPlanModal', { static: true }) createOrEditTestPlanModal: any; //CreateOrEditTestPlanModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
    //Filters
    filterText = '';

    constructor(
        injector: Injector,
        private _testPlanService : TestPlanMasterServiceProxy,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
        this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    }

    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
    }

    getTestPlans(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._testPlanService
            .getTestPlanMasters(
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

    createCustomer(): void {
        this.createOrEditTestPlanModal.show();
    }

    deleteTestMaster(testPlanItem: TestPlanMasterDto): void {
        this.message.confirm(this.l('TestPlanDeleteWarningMessage', testPlanItem.name), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._testPlanService.deleteTestPlanMasterData(testPlanItem.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
}
