import { Component, OnInit } from '@angular/core';
import { MyAccountService } from '../../../shared/services/myaccount.service';
import { Observable } from 'rxjs';
import { LocalStorageService } from 'ngx-webstorage';

@Component({
  selector: 'app-my-account-home',
  templateUrl: './my-account-home.component.html',
  styleUrls: ['./my-account-home.component.scss']
})
export class MyAccountHomeComponent implements OnInit {
  account: Observable<any>;
  loaded: boolean = false;

  constructor(
    private accountService: MyAccountService,
    private storage: LocalStorageService
  ) {
  }

  ngOnInit() {
    this.accountService.get().subscribe(acc => {
      this.account = new Observable((observer) => {
        observer.next(acc);
        return () => acc;
      });

      if (acc.customer != null)
        this.storage.store("nameAccount", acc.customer.name);
      else
        this.storage.store("nameAccount", acc.email);

      this.loaded = true;
    });
  }
}
