import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'my-account-register',
  templateUrl: './my-account-register.component.html',
  styleUrls: ['./my-account-register.component.scss']
})
export class MyAccountRegisterComponent implements OnInit {
  @Input() currentAccount: Observable<any>;
  account: any = {};
  isLoad: boolean = false;
  addressContact: any = {};

  constructor() {
   }

  ngOnInit() {
    this.currentAccount.subscribe( x => {
      this.account = x;
      this.isLoad = true;

      if (this.account.customer)
        this.addressContact = this.account.customer.addresses.find(ad => ad.purpose.toLowerCase() == 'contact');
    });
  }
}
