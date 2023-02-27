import { Component, Injector, ViewChild, ViewEncapsulation, AfterViewInit, Input, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ContactPersonDto, CustomerMasterDto, CustomerMasterServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { CreateOrEditContactPersonModalComponent } from '../create-edit-contact-person/create-or-edit-contact-person-modal.component';

@Component({
    selector: 'app-contact-persons-list',
    templateUrl: './contact-persons.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./contact-persons.component.less'],
    animations: [appModuleAnimation()],
})
export class ContactPersonsComponent extends AppComponentBase implements AfterViewInit {
    @ViewChild('createOrEditContactPersonModal', { static: true }) createOrEditContactPersonModal: CreateOrEditContactPersonModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    
    @Input() contactPersonList : ContactPersonDto[] = [];
   
    //Filters
    filterText = '';

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private cdref: ChangeDetectorRef
    ) {
        super(injector);
        this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    }

    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
    }

    getContactPersons(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this.primengTableHelper.totalRecordsCount = this.contactPersonList.length;
        this.primengTableHelper.records = this.contactPersonList;
        this.primengTableHelper.hideLoadingIndicator();
       
        // this._customerService
        //     .getCustomers(
        //             this.filterText,
        //             this.primengTableHelper.getSorting(this.dataTable),
        //             this.primengTableHelper.getMaxResultCount(this.paginator, event),
        //             this.primengTableHelper.getSkipCount(this.paginator, event)
        //     )
        //     .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
        //     .subscribe((result) => {
        //         this.primengTableHelper.totalRecordsCount = result.totalCount;
        //         this.primengTableHelper.records = result.items;
        //         this.primengTableHelper.hideLoadingIndicator();
        //     });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createContactPerson(): void {
        this.createOrEditContactPersonModal.show();
    }

    deleteContactPerson(customer: CustomerMasterDto): void {
        this.message.confirm(this.l('CustomerDeleteWarningMessage', customer.name), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this.reloadPage();
                this.notify.success(this.l('SuccessfullyDeleted'));
            }
        });

    }
}
