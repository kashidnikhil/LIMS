import { Component, Injector, ViewChild, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { BankDto, BankServiceProxy, ResponseDto, SourceDto, SourceServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { CreateOrEditBankModalComponent } from '../create-edit-bank/create-or-edit-bank-modal.component';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './banks.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./banks.component.less'],
    animations: [appModuleAnimation()],
})
export class BanksComponent extends AppComponentBase implements AfterViewInit {
    @ViewChild('createOrEditBankModal', { static: true }) createOrEditBankModal: CreateOrEditBankModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
    //Filters
    filterText = '';

    constructor(
        injector: Injector,
        private _bankService : BankServiceProxy,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
        this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    }

    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
    }

    getBanks(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._bankService
            .getBanks(
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

    createBank(): void {
        this.createOrEditBankModal.show();
    }

    deleteSource(bankItem: BankDto): void {
        this.message.confirm(this.l('BankDeleteWarningMessage', bankItem.name), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._bankService.deleteBank(bankItem.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    restoreBank(bankResponse: ResponseDto):void {
        if(bankResponse.id == null){
            if(bankResponse.isExistingDataAlreadyDeleted){
                this.message.confirm(this.l('BankRestoreMessage', bankResponse.name), this.l('AreYouSure'), async (isConfirmed) => {
                    if (isConfirmed) {
                        this._bankService.restoreBank(bankResponse.restoringItemId).subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('BankSuccessfullyRestored'));
                        });
                    }
                });
            }
            else{
                this.notify.error(this.l('ExistingBankErrorMessage',bankResponse.name));
            }
        }
        else{
            if(bankResponse.isExistingDataAlreadyDeleted){
                this.message.confirm(this.l('NewBankErrorMessage', bankResponse.name), this.l('AreYouSure'), async (isConfirmed) => {
                    if (isConfirmed) {
                        this._bankService.restoreBank(bankResponse.restoringItemId).subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('BankSuccessfullyRestored'));
                        });
                    }
                });
            }   
            else{
                this.notify.error(this.l('ExistingBankErrorMessage',bankResponse.name));
            }
        }
    }
}
