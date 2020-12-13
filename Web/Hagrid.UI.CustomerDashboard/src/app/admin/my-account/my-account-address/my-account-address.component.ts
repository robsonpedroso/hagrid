import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'my-account-address',
  templateUrl: './my-account-address.component.html',
  styleUrls: ['./my-account-address.component.scss']
})
export class MyAccountAddressComponent implements OnInit {
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
