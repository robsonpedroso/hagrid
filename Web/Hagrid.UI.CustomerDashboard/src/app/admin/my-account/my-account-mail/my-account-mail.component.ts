import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'my-account-mail',
  templateUrl: './my-account-mail.component.html',
  styleUrls: ['./my-account-mail.component.scss']
})
export class MyAccountMailComponent implements OnInit {
  @Input() currentAccount: Observable<any>;
  account: any = {};
  isLoad: boolean = false;

  constructor() {
   }

  ngOnInit() {
    this.currentAccount.subscribe( x => {
      this.account = x;
      this.isLoad = true;
    });
  }

}
