import { ShowErrorsComponent } from './show-errors.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [ShowErrorsComponent],
  exports:[ShowErrorsComponent]
})
export class ShowErrorsModule { }
