import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MyDataComponent } from './my-data.component';
import { MyDataRoutingModule } from './my-data-routing.module';
import { OnlyNumbersAndLettersDirective } from '../../shared/directives/only-numbers-and-letters.directive';
import { TextMaskModule } from 'angular2-text-mask';
import { MyDataService } from '../../shared/services/mydata.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxMaskModule } from 'ngx-mask';
import { BsModalModule } from 'ng2-bs3-modal';
import { ShowErrorsModule } from '../../shared/components/show-erros/show-errors.module';
import { FormValidatorModule } from './../../shared/directives/form-validator/form-validator.module';
import { MatTabsModule, MatDatepickerModule, MatFormFieldModule, MatInputModule } from '@angular/material';
import { MyAccountModule } from '../my-account/my-account.module';


@NgModule({
  imports: [
    CommonModule,
    MyDataRoutingModule,
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
    MyAccountModule

  ],
  declarations: [MyDataComponent, OnlyNumbersAndLettersDirective],
  providers: [MyDataService]
})
export class MyDataModule {

  }
