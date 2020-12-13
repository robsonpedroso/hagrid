import { environment } from './../../../../environments/environment';
import { Component, Input } from '@angular/core';
import { AbstractControlDirective, AbstractControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'show-errors',
  template: `
    <div *ngIf="shouldShowErrors()" class="invalid-feedback d-block">
      <span *ngFor="let error of listOfErrors()">{{error}}</span>
    </div>
  `,
})
export class ShowErrorsComponent {

  errors_messages: any[];

  constructor(private http: HttpClient) {
    this.getErrosMessages().subscribe(data => {
        this.errors_messages = data;
    });
}

public getErrosMessages(): Observable<any> {
  return this.http.get(environment.urls.accountCustomerDashboardURL + "/assets/json/errors-messages.json");
}

  @Input() private control: AbstractControlDirective | AbstractControl;

  shouldShowErrors(): boolean {
    return this.control && this.control.errors && this.control.touched;
  }

  listOfErrors(): string[] {
    return Object.keys(this.control.errors)
      .map(field => this.getMessage(field, this.control.errors[field]));
  }

  private getMessage(type: string, params: any) {

    let error = this.errors_messages[type];

    if(error != null && error != ""){
      switch(type){
        case "minlength":
        case "maxlength":
          error = error.replace("#param", params.requiredLength)
          break;
        default:
          break;
      }

      return error;

    } else {
      return "required";
    }
  }


}
