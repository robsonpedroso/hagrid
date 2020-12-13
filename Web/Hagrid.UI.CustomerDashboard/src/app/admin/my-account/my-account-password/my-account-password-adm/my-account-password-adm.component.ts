import { Component, OnInit } from '@angular/core';
import { MyDataService } from '../../../../shared/services/mydata.service';
import { NotificationsService } from 'angular2-notifications';

@Component({
  selector: 'app-my-account-password-adm',
  templateUrl: './my-account-password-adm.component.html',
  styleUrls: ['./my-account-password-adm.component.scss']
})
export class MyAccountPasswordAdmComponent implements OnInit {
  password: string = "";
  new_password: string = "";
  new_password_confirm: string = "";

  constructor(
    private MyDataService: MyDataService,
    private notify: NotificationsService
  ) { }

  ngOnInit() {
  }

  public save() {

    if (this.new_password == "") {
      this.notify.warn("Atenção", "Nova senha não pode ser branca.");
      return;
    }

    if (this.new_password != this.new_password_confirm) {
      this.notify.warn("Atenção", "Combinação de senha diferente da nova senha.");
      return;
    }

    this.MyDataService.changePassword(this.password, this.new_password).then(res => {
      this.notify.success("Sucesso", "Senha alterada com sucesso.");
    });
  }

}
