import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PersonServiceProxy, PersonListDto, ListResultDtoOfPersonListDto, AddPhoneInput, PhoneType, PhoneInPersonListDto } from '@shared/service-proxies/service-proxies';

import { remove as _remove } from 'lodash-es';
@Component({
    templateUrl: './phonebook.component.html',
    styleUrls: ['./phonebook.component.less'],
    animations: [appModuleAnimation()]
})

export class PhoneBookComponent extends AppComponentBase implements OnInit {

    people: PersonListDto[] = [];
    filter: string = '';

    editingPerson: PersonListDto = null;
    newPhone: AddPhoneInput = null;
    
    constructor(
        injector: Injector,
        private _personService: PersonServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.getPeople();
    }

    getPeople(): void {
        this._personService.getPeople(this.filter).subscribe((result) => {
            this.people = result.items;
        });
    }

    deletePerson(person: PersonListDto): void {
        
                if (person.id) {
                    this._personService.deletePerson(person.id).subscribe(() => {
                        this.notify.info(this.l('SuccessfullyDeleted'));
                        _remove(this.people, person);
                    });
                }
            }
            
            editPerson(person: PersonListDto): void {
                if (person === this.editingPerson) {
                    this.editingPerson = null;
                } else {
                    this.editingPerson = person;
        
                    this.newPhone = new AddPhoneInput();
                    this.newPhone.type = PhoneType.Mobile;
                    this.newPhone.personId = person.id;
                }
            };

            getPhoneTypeAsString(phoneType: PhoneType): string {
                switch (phoneType) {
                    case PhoneType.Mobile:
                        return this.l('PhoneType_Mobile');
                    case PhoneType.Home:
                        return this.l('PhoneType_Home');
                    case PhoneType.Business:
                        return this.l('PhoneType_Business');
                    default:
                        return '?';
                }
            };

            deletePhone(phone, person): void {
                this._personService.deletePhone(phone.id).subscribe(() => {
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    _remove(person.phones, phone);
                });
            };

            savePhone(): void {
                if (!this.newPhone.number) {
                    this.message.warn('Please enter a number!');
                    return;
                }
                this._personService.addPhone(this.newPhone).subscribe(result => {
                    this.editingPerson.phones.push(result);
                    this.newPhone.number = '';
        
                    this.notify.success(this.l('SavedSuccessfully'));
                });
            };
    
}
