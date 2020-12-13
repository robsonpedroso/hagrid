import { Component, OnInit, Input } from '@angular/core';
import { MyAccountService } from '../../../shared/services/myaccount.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'my-account-profile',
  templateUrl: './my-account-profile.component.html',
  styleUrls: ['./my-account-profile.component.scss']
})
export class MyAccountProfileComponent implements OnInit {
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
