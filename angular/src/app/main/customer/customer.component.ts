import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CustomerListDto, CustomerServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './customer.component.html',
    animations: [appModuleAnimation()]
})

export class customerComponent extends AppComponentBase implements OnInit{
    
    people: CustomerListDto[] = [];
    filter: string = '';

    constructor(
        injector: Injector,
        private _customerService: CustomerServiceProxy
    ) {
            super(injector);
        }

        ngOnInit(): void {
            this.getPeople();
        }

        getPeople(): void {
            this._customerService.getPeople(this.filter).subscribe((result) => {
                this.people = result.items;
            });
        }
}
