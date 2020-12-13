import { Component, OnInit } from '@angular/core';
import { MyAccountService } from '../../../../shared/services/myaccount.service';
import { CustomerService } from '../../../../shared/services/customer.service';
import { NotificationsService } from 'angular2-notifications';
import { ConfirmationService } from '@jaspero/ng2-confirmations';
import { ResolveEmit } from '@jaspero/ng2-confirmations/src/interfaces/resolve-emit';

@Component({
  selector: 'my-account-address-adm',
  templateUrl: './my-account-address-adm.component.html',
  styleUrls: ['./my-account-address-adm.component.scss']
})
export class MyAccountAddressAdmComponent implements OnInit {
  account: any = {};
  isLoad: boolean = false;

  constructor(
    private accountService: MyAccountService,
    private cusomterService: CustomerService,
    private notify: NotificationsService,
    private notifyConfirm: ConfirmationService
  ) { }

  ngOnInit() {
    this.accountService.get().subscribe(result => {
      this.account = result;
      this.isLoad = true;
    });
  }

  public delete(code: string): void {
    this.notifyConfirm.create('Atenção!', 'Deseja realmente remover esse endereço?').subscribe((ans: ResolveEmit) => {
      if (ans.resolved) {
        this.cusomterService.deleteAddress(code).subscribe(result => {
          this.notify.success("Sucesso", "Endereço removido com sucesso!");
        });
      }
    });
  }
}
