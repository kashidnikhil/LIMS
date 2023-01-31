import { Component, Injector, ViewChild, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ChargesDto, ChargesServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { CreateOrEditChargesModalComponent } from '../create-edit-charges/create-or-edit-charges-modal.component';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './charges.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./charges.component.less'],
    animations: [appModuleAnimation()],
})
export class ChargesComponent extends AppComponentBase implements AfterViewInit {
    @ViewChild('createOrEditChargesModal', { static: true }) createOrEditChargesModal: CreateOrEditChargesModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
    //Filters
    filterText = '';

    constructor(
        injector: Injector,
        private _chargesService : ChargesServiceProxy,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
        this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    }

    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
    }

    getCharges(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._chargesService
            .getCharges(
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

    createCharges(): void {
        this.createOrEditChargesModal.show();
    }

    deleteCharge(charges: ChargesDto): void {
        this.message.confirm(this.l('ChargesDeleteWarningMessage', charges.name), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._chargesService.deleteCharge(charges.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
}
