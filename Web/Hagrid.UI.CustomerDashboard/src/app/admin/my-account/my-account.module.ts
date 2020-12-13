import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MyAccountComponent } from './my-account.component';
import { MyAccountRoutingModule } from './my-account.routing';
import { OnlyNumbersAndLettersDirective } from '../../shared/directives/only-numbers-and-letters.directive';
import { TextMaskModule } from 'angular2-text-mask';
import { MyAccountService } from '../../shared/services/myaccount.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxMaskModule } from 'ngx-mask';
import { BsModalModule } from 'ng2-bs3-modal';
import { ShowErrorsModule } from '../../shared/components/show-erros/show-errors.module';
import { FormValidatorModule } from './../../shared/directives/form-validator/form-validator.module';
import { MatTabsModule, MatDatepickerModule, MatFormFieldModule, MatInputModule } from '@angular/material';
import { MyAccountHomeComponent } from './my-account-home/my-account-home.component';
import { MyAccountProfileComponent } from './my-account-profile/my-account-profile.component';
import { MyAccountMailComponent } from './my-account-mail/my-account-mail.component';
import { MyAccountAddressComponent } from './my-account-address/my-account-address.component';
import { MyAccountPhoneComponent } from './my-account-phone/my-account-phone.component';
import { MyAccountRegisterComponent } from './my-account-register/my-account-register.component';
import { MyAccountProfileEditComponent } from './my-account-profile/my-account-profile-edit/my-account-profile-edit.component';
import { MyAccountAdressAddComponent } from './my-account-address/my-account-adress-add/my-account-adress-add.component';
import { MyAccountAddressAdmComponent } from './my-account-address/my-account-address-adm/my-account-address-adm.component';
import { MyAccountMailAdmComponent } from './my-account-mail/my-account-mail-adm/my-account-mail-adm.component';
import { MyAccountPhoneAdmComponent } from './my-account-phone/my-account-phone-adm/my-account-phone-adm.component';
import { MyAccountPasswordComponent } from './my-account-password/my-account-password.component';
import { MyAccountPasswordAdmComponent } from './my-account-password/my-account-password-adm/my-account-password-adm.component';
import { SelectModule } from 'ng2-select';
import { PipesModule } from '../../shared/pipes/pipes.module';
import { HttpModule } from '@angular/http';
import { LogisticsService } from '../../shared/services/logistics.service';


@NgModule({
  imports: [
    CommonModule,
    MyAccountRoutingModule,
    CommonModule,
    TextMaskModule,
    FormsModule,
    NgxMaskModule.forRoot(),
    BsModalModule,
    ReactiveFormsModule,
    FormValidatorModule,
    ShowErrorsModule,
    MatTabsModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    SelectModule,
    PipesModule,
    HttpModule
  ],
  declarations: [
    MyAccountHomeComponent,
    MyAccountComponent,
    MyAccountProfileComponent,
    MyAccountMailComponent,
    MyAccountAddressComponent,
    MyAccountPhoneComponent,
    MyAccountRegisterComponent,
    MyAccountProfileEditComponent,
    MyAccountAdressAddComponent,
    MyAccountAddressAdmComponent,
    MyAccountMailAdmComponent,
    MyAccountPhoneAdmComponent,
    MyAccountPasswordComponent,
    MyAccountPasswordAdmComponent
  ],
  providers: [MyAccountService, LogisticsService]
})
export class MyAccountModule { }
