import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CreatePersonModalComponent } from './create-person-modal.component';
import { EditPersonModalComponent } from './edit-person-modal.component';
import {PhoneBookRoutingModule} from './phonebook-routing.module';
import {PhoneBookComponent} from './phonebook.component';

@NgModule({
    declarations: [PhoneBookComponent,CreatePersonModalComponent,EditPersonModalComponent],
    imports: [AppSharedModule, PhoneBookRoutingModule]
})
export class PhoneBookModule {}
