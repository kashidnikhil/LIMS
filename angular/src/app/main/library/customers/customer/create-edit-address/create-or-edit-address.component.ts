import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CustomerAddressDto, CustomerMasterDto, CustomerMasterInputDto, CustomerMasterServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { map as _map, filter as _filter } from 'lodash-es';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'create-edit-address',
    templateUrl: './create-or-edit-address.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['create-or-edit-address.component.less']
})
export class CreateOrEditAddressComponent implements OnInit {
    @Input() customerAddressItem : CustomerAddressDto ;
    @Input() customerAddressIndex : number;
    @Output() modalSave: EventEmitter<number> = new EventEmitter<number>();

    customerItem : CustomerMasterDto = new CustomerMasterDto();
    
    constructor() {
    }


    ngOnInit(){
        console.log(this.customerAddressIndex);
    }

   }
