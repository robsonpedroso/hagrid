import { Component, Input } from '@angular/core';
import { AbstractControlDirective, AbstractControl } from '@angular/forms';

@Component({
  selector: 'show-errors',
  template: `
    <div *ngIf="shouldShowErrors()">
      <span *ngFor="let error of listOfErrors()" style="color:red"><i class="fa fa-times-circle-o"></i> {{error}} </span>
    </div>
  `,
})
export class ShowErrorsComponent {

  private static readonly errorMessages = {
    'required': () => 'Campo obrigatório',
    'minlength': (params) => 'Mínimo de caracteres ' + params.requiredLength,
    'maxlength': (params) => 'A quantidade máxima de caracteres é ' + params.requiredLength,
    'pattern': (params) => 'Valor inválido'
  };

  @Input() private control: AbstractControlDirective | AbstractControl;

  shouldShowErrors(): boolean {
    return this.control && this.control.errors && this.control.touched  && !this.control.pristine;
    //return this.control && this.control.errors && (this.control.dirty || this.control.touched)  && !this.control.pristine;
  }

  listOfErrors(): string[] {
    return Object.keys(this.control.errors)
      .map(field => this.getMessage(field, this.control.errors[field]));
  }

  private getMessage(type: string, params: any) {
    return ShowErrorsComponent.errorMessages[type](params);
  }

}
