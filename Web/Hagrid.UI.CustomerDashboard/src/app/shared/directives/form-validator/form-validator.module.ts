import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormValidatorDirective } from './form-validator.directive';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [FormValidatorDirective],
  exports:[FormValidatorDirective]
})
export class FormValidatorModule { }
