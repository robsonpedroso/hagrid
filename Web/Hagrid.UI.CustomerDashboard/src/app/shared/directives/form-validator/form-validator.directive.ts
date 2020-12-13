import { Directive, Input } from '@angular/core';
import { NG_VALIDATORS, AbstractControl, Validator } from '@angular/forms';
import { ValidatorService } from '../../services/validator.service';
import { CreditCardValidatorService } from "../../services/credit-card-validator.service";

@Directive({
  selector: '[validate]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: FormValidatorDirective,
      multi: true
    }]
})
export class FormValidatorDirective implements Validator {

  @Input('validate') fieldType: string;

  constructor(private creditCardValidator: CreditCardValidatorService) { }

  validate(control: AbstractControl): { [key: string]: any } | null {
    if(this.fieldType == "creditcard"){
      return ValidatorService.validateCreditCard(control, this.creditCardValidator);
    } else {
      return ValidatorService.validate(control, this.fieldType);
    }
  }

}
