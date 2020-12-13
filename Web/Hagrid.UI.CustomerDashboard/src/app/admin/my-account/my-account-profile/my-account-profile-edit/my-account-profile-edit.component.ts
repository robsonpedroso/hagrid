import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MyAccountService } from '../../../../shared/services/myaccount.service';
import { SelectComponent } from 'ng2-select';
import { NotificationsService } from 'angular2-notifications';

@Component({
  selector: 'app-my-account-profile-edit',
  templateUrl: './my-account-profile-edit.component.html',
  styleUrls: ['./my-account-profile-edit.component.scss']
})
export class MyAccountProfileEditComponent implements OnInit {
  account: any = {};

  constructor(
    private accountService: MyAccountService,
    private router: Router,
    private notify: NotificationsService
  ) {
    this.router = router;
  }

  ngOnInit() {
    this.accountService.get().subscribe(result => {
      this.account = result;
    });
  }

  public save() {
    this.accountService.update(this.account).subscribe(x => {
      this.notify.success("Sucesso", "Conta atualizada com sucesso!");
      this.router.navigate(['/meus-dados']);
    });
  }

  public panelOut() {
    let overPanel = document.querySelector('.overpanel');
    overPanel.classList.add('remove');
    this.router.navigate(['/meus-dados']);
  }

}
