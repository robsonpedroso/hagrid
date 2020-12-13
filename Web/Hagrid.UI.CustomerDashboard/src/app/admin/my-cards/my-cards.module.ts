import { CreditCardValidatorService } from './../../shared/services/credit-card-validator.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MyCardsComponent } from './my-cards.component';
import { MyCardsRoutingModule } from './my-cards.routing';
import { CardsService } from '../../shared/services/cards.service';
import { NewCreditCardComponent } from './new-credit-card/new-credit-card.component';
import { HomeCreditCardComponent } from './home-credit-card/home-credit-card.component';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';
import { TextMaskModule } from 'angular2-text-mask';
import { OnlyNumbersDirective } from '../../shared/directives/only-numbers.directive';
import { ShowErrorsModule } from '../../shared/components/show-erros/show-errors.module';
import { FormValidatorModule } from '../../shared/directives/form-validator/form-validator.module';
import { MaskService } from '../../shared/services/mask.service';
import { ToolsService } from '../../shared/services/tools.service';
import { BoxMyCardsComponent } from './box-my-cards/box-my-cards.component';
import { HttpModule } from '@angular/http';
import { LogisticsService } from '../../shared/services/logistics.service';

@NgModule({
  imports: [
    CommonModule,
    MyCardsRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    TextMaskModule,
    ShowErrorsModule,
    FormValidatorModule,
    HttpModule
  ],
  declarations: [MyCardsComponent, NewCreditCardComponent, HomeCreditCardComponent, OnlyNumbersDirective, BoxMyCardsComponent],
  providers: [CardsService, MaskService, ToolsService, CreditCardValidatorService, LogisticsService],
  exports: [BoxMyCardsComponent]
})
export class MyCardsModule { }
