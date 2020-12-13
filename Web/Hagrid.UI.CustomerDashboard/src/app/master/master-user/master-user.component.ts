import { Component, OnInit, AfterContentInit, AfterContentChecked } from '@angular/core';
import { AuthService } from '../../shared/auth/auth.service';
import { MyDataService } from '../../shared/services/mydata.service';
import { LocalStorageService } from 'ngx-webstorage';

@Component({
  selector: 'app-master-user',
  templateUrl: './master-user.component.html',
  styleUrls: ['./master-user.component.scss'],
})

export class MasterUserComponent implements OnInit, AfterContentChecked {
  public nameAccount: string = "";
  public isLoad: boolean = false;
  public isLoadInfoUser: boolean = false;

  constructor(
    private authService: AuthService,
    private myDataService: MyDataService,
    private storage: LocalStorageService
  ) {

  }

  ngAfterContentChecked(): void {
    if (this.storage.retrieve('nameAccount') == null) {

      if (this.authService.isAuthenticated()) {
        this.myDataService.get().subscribe(x => {
          if (x.customer != null)
          {
            this.nameAccount = x.customer.name;
            this.isLoadInfoUser = true;
          }
          else
          {
            this.nameAccount = x.email;
            this.isLoadInfoUser = true;
          }

          this.storage.store("nameAccount", this.nameAccount);
        });
      }
    }
    else {
      this.isLoadInfoUser = true;
      this.nameAccount = this.storage.retrieve('nameAccount');
    }

    this.storage.observe('nameAccount')
      .subscribe((value) => {
        this.nameAccount = value;
      });
  }

  public load() {
    if (this.authService.isAuthenticated()) {
      this.isLoad = true;
    }
    else {
      this.isLoad = false;
    }

    return this.isLoad;
  }

  public ngOnInit() {
  }

  public logout(): void {
    this.authService.logout();
  }

  public login(): void {
    this.authService.redirectLoginPage();
  }
}
