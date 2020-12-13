import { Component, OnInit } from '@angular/core';
import { LogisticsService } from '../../../../shared/services/logistics.service';
import { NotificationsService } from 'angular2-notifications';
import { CustomerService } from '../../../../shared/services/customer.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToolsService } from '../../../../shared/services/core';
import { MaskService } from '../../../../shared/services/mask.service';

@Component({
  selector: 'app-my-account-adress-add',
  templateUrl: './my-account-adress-add.component.html',
  styleUrls: ['./my-account-adress-add.component.scss']
})
export class MyAccountAdressAddComponent implements OnInit {
  address: any = {};
  selectedState: any[];
  mainPhone: string;
  phone: string;
  mobile: string;
  is_loaded: boolean;
  states: any[];

  constructor(
    private logisticsService: LogisticsService,
    private notify: NotificationsService,
    private customerService: CustomerService,
    private routeActived: ActivatedRoute,
    private router: Router,
    private toolsService: ToolsService

  ) {
    this.states = this.toolsService.getStates();
  }

  ngOnInit() {
    if (this.routeActived.snapshot.params.id != null && this.routeActived.snapshot.params.id != "") {
      this.load();
    } else {
      this.is_loaded = true;
    }
  }

  public phoneMask(rawValue) {
    return MaskService.phoneMask(rawValue);
  }

  public zipCodeMask(rawValue) {
    return MaskService.zipCodeMask();
  }

  public getAddressByZipCode() {
    var zipCode = this.address.zip_code.replace(/\(|\)|\_|\ |\-/gi, "")
    if (zipCode == null || zipCode == "" || zipCode.length != 8) {
      this.notify.error("Atenção!", "CEP inválido");
    } else {
      this.logisticsService
        .getAddress(zipCode.replace("-", ""))
        .subscribe(response => {
          this.address.street = response.street,
            this.address.district = response.district,
            this.address.city = response.city,
            this.address.state = response.state
        });
    }
  }

  public save() {
    if (this.address.phones != null) {
      this.address.phones[0] = this.toolsService.getPhone(this.mainPhone, 2);
      this.address.phones[1] = this.toolsService.getPhone(this.phone, 1);
      this.address.phones[2] = this.toolsService.getPhone(this.mobile, 4);
      this.address.phones[3] = this.toolsService.getPhone("", 3);
    } else {
      this.address.phones = [];

      this.address.phones.push(this.toolsService.getPhone(this.mainPhone, 2));
      this.address.phones.push(this.toolsService.getPhone(this.phone, 1));
      this.address.phones.push(this.toolsService.getPhone(this.mobile, 4));
      this.address.phones.push(this.toolsService.getPhone("", 3));
    }

    if (this.address.purpose == null || this.address.purpose == "")
      this.address.purpose == 2;

    this.address.status = true;

    this.customerService.saveAddress(this.address).subscribe(response => {
      this.notify.success("Sucesso", "Endereço armazenado com sucesso");
    });
  }

  private load() {
    this.customerService
      .getAddress(this.routeActived.snapshot.params.id)
      .subscribe(response => {
        this.address = response;

        this.selectedState = this.states.filter(
          x => x.id == this.address.state
        );

        let _mainPhone = this.address.phones.filter(
          x => x.phone_type == "Residencial"
        );
        if (_mainPhone.length > 0) {
          this.mainPhone = this.toolsService.setPhone(_mainPhone[0]);
        }

        let _phone = this.address.phones.filter(
          x => x.phone_type == "Comercial"
        );
        if (_phone.length > 0) {
          this.phone = this.toolsService.setPhone(_phone[0]);
        }

        let _mobile = this.address.phones.filter(
          x => x.phone_type == "Celular"
        );
        if (_mobile.length > 0) {
          this.mobile = this.toolsService.setPhone(_mobile[0]);
        }

        this.is_loaded = true;
      });
  }
}
