import { Component, Injector, ViewChild, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ResponseDto, UnitDto, UnitServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { finalize } from 'rxjs/operators';
import { CreateOrEditUnitModalComponent } from '../create-edit-unit/create-or-edit-unit-modal.component';

@Component({
    templateUrl: './units.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./units.component.less'],
    animations: [appModuleAnimation()],
})
export class UnitsComponent extends AppComponentBase implements AfterViewInit {
    @ViewChild('createOrEditUnitModal', { static: true }) createOrEditUnitModal: CreateOrEditUnitModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
    //Filters
    filterText = '';

    constructor(
        injector: Injector,
        private _unitService: UnitServiceProxy,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
        this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    }

    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
    }

    getUnits(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._unitService
            .getUnits(
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

    createUnit(): void {
        this.createOrEditUnitModal.show();
    }

    deleteTechnique(unit: UnitDto): void {
        this.message.confirm(this.l('UnitDeleteWarningMessage', unit.name), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._unitService.deleteUnit(unit.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    restoreUnit(unitResponse: ResponseDto):void {
        if(unitResponse.id == null){
            if(unitResponse.isExistingDataAlreadyDeleted){
                this.message.confirm(this.l('UnitRestoreMessage', unitResponse.name), this.l('AreYouSure'), async (isConfirmed) => {
                    if (isConfirmed) {
                        this._unitService.restoreUnit(unitResponse.restoringItemId).subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('UnitSuccessfullyRestored'));
                        });
                    }
                });
            }
            else{
                this.notify.error(this.l('ExistingUnitErrorMessage',unitResponse.name));
            }
        }
        else{
            if(unitResponse.isExistingDataAlreadyDeleted){
                this.message.confirm(this.l('NewUnitErrorMessage', unitResponse.name), this.l('AreYouSure'), async (isConfirmed) => {
                    if (isConfirmed) {
                        this._unitService.restoreUnit(unitResponse.restoringItemId).subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('UnitSuccessfullyRestored'));
                        });
                    }
                });
            }   
            else{
                this.notify.error(this.l('ExistingUnitErrorMessage',unitResponse.name));
            }
        }
    }
}
