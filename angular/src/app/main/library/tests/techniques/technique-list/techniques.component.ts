import { Component, Injector, ViewChild, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TechniqueServiceProxy, TechniqueDto } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { CreateOrEditTechniqueModalComponent } from '../create-edit-technique/create-or-edit-technique-modal.component';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './techniques.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./techniques.component.less'],
    animations: [appModuleAnimation()],
})
export class TechiquesComponent extends AppComponentBase implements AfterViewInit {
    @ViewChild('createOrEditTechniqueModal', { static: true }) createOrEditTechniqueModal: CreateOrEditTechniqueModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
    //Filters
    filterText = '';

    constructor(
        injector: Injector,
        private _techniqueService: TechniqueServiceProxy,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
        this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    }

    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
    }

    getTechniques(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._techniqueService
            .getTechniques(
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

    createTechnique(): void {
        this.createOrEditTechniqueModal.show();
    }

    deleteTechnique(standardRemark: TechniqueDto): void {
        this.message.confirm(this.l('TechniqueDeleteWarningMessage', standardRemark.name), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._techniqueService.deleteTechnique(standardRemark.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
}
