import { ValidatorService } from './../../shared/services/validator.service';
import { Observable } from "rxjs/Observable";
import { Component, OnInit, ViewChild } from "@angular/core";
import { FormControl, Validators, NgForm } from "@angular/forms";
import { NotificationsService } from "angular2-notifications";
import { ActivatedRoute, Router } from "@angular/router";
import { MyDataService } from "../../shared/services/mydata.service";
import { ToolsService } from './../../shared/services/tools.service';
import { MaskService } from './../../shared/services/mask.service';
import { LogisticsService } from "./../../shared/services/logistics.service";
import { MatTabsModule, MatDatepickerModule, MatFormFieldModule, MatInputModule } from '@angular/material';

@Component({
  selector: 'app-my-account',
  templateUrl: './my-account.component.html'
})
export class MyAccountComponent implements OnInit {

  myData: any = {};
  response_address: any = {};
  address: any = {};
  is_loaded: boolean = false;
  is_correct: boolean = true;
  states: any[];
  selectedState: any[];
  isPerson: boolean;
  decimalSeparator = ".";
  groupSeparator = ",";
  pureResult: any;
  maskedId: any;
  val: any;
  v: any;
  state: string;
  street: string;
  district: string;
  city: string;
  contact_phone: string;
  commercial_phone: string;
  mobile_phone: string;
  password: string;
  new_password: string;
  confirm_password: string;


  constructor(
    private notify: NotificationsService,
    private route: ActivatedRoute,
    private MyDataService: MyDataService,
    private router: Router,
    private toolsService: ToolsService,
    private LogisticsService: LogisticsService,
  ) { }

  ngOnInit() {
    this.states = this.toolsService.getStates();
    this.GetMyData();
  }


  public GetMyData() {
    this.MyDataService.get().subscribe(response => {

      let _address = response.customer.addresses.filter(
        x => x.type == "HomeAddress"
      )[0];

      if (_address != null) {
        let _contact_phone = _address.phones.filter(
          x => x.phone_type == "Residencial"
        )[0];

        if (_contact_phone) {
          this.contact_phone = this.toolsService.setPhone(_contact_phone);
        }

        let _mobile_phone = _address.phones.filter(
          x => x.phone_type == "Celular"
        )[0];

        if (_mobile_phone) {
          this.mobile_phone = this.toolsService.setPhone(_mobile_phone);
        }

        let _commercial_phone = _address.phones.filter(
          x => x.phone_type == "Comercial"
        )[0];

        if (_commercial_phone) {
          this.commercial_phone = this.toolsService.setPhone(_commercial_phone);
        }
      }

      if (response.customer.type == 'Person') {
        this.isPerson = true;
      }

      this.myData = response;
      this.address = _address;

      this.is_loaded = true;
    });

  }

  public update() {

    this.validateSaveDocument();
    if (this.is_correct) {

      if (this.myData != null || this.address != null) {
        if (this.address.phones[0] != null) {

          this.address.phones[0] = this.toolsService.getPhone(this.contact_phone, 2); //principal (residencial)
          this.address.phones[1] = this.toolsService.getPhone(this.commercial_phone, 1); //comercial
          this.address.phones[2] = this.toolsService.getPhone(this.mobile_phone, 4); // celular
          this.address.phones[3] = this.toolsService.getPhone("", 3); // faz (obrigatório por causa do shopping)
        } else {
          this.address.phones[0] = [];

          this.address.phones[0].push(this.toolsService.getPhone(this.contact_phone, 2)); //principal (residencial)
          this.address.phones[1].push(this.toolsService.getPhone(this.commercial_phone, 1)); //comercial
          this.address.phones[2].push(this.toolsService.getPhone(this.mobile_phone, 4)); // celular
          this.address.phones[3].push(this.toolsService.getPhone("", 3)); // faz (obrigatório por causa do shopping)
        }

        this.myData.customer.addresses.filter(
          x => x.type == "HomeAddress"
        )[0] = this.address;

        this.MyDataService.update(this.myData).subscribe(response => {
          this.notify.success('Sucesso!', 'Usuário atualizado com sucesso.');
          this.router.navigate(['dashboard']);
        });
      }
    }
  }

  public validateSaveDocument() {

    if(this.isPerson){
      let regex: RegExp = new RegExp(/^-?[0-9a-zA-Z.-]*/g);
      this.myData.customer.rg = String(this.myData.customer.rg).match(regex)[0];
    }

    if (this.isPerson && this.myData.document.replace(/\D/g, '').length > 11) {
      this.is_correct = false;
      this.notify.error("Atenção", "Não é possível alterar o documento para CNPJ");
    }
    if (!this.isPerson && this.myData.document.replace(/\D/g, '').length < 14){
      this.is_correct = false;
      this.notify.error("Atenção", "Não é possível alterar o documento para CPF");
    }
  }

  public changePassword(form: NgForm) {
    this.MyDataService.changePassword(this.password, this.new_password).then(res => {

      this.notify.success("Sucesso", "Senha alterada com sucesso.", {
        timeOut: 2500,
        showProgressBar: true,
        pauseOnHover: false,
        clickToClose: false
      });

      form.reset();
    }).catch(ex => form.reset());
  }

  public getAddressByZipCode() {
    var zipCode = this.address.zip_code.replace(/\(|\)|\_|\ |\-/gi, "")
    if (zipCode == null || zipCode == "" || zipCode.length != 8) {
      this.notify.error("Atenção!", "CEP inválido");
    } else {
      this.LogisticsService
        .getAddress(zipCode.replace("-", ""))
        .subscribe(response => {
          this.address.street = response.street,
            this.address.district = response.district,
            this.address.city = response.city,
            this.address.state = response.state
        });
    }
  }

  //Mask's
  public documentMask(rawValue) {
    return MaskService.documentMask(rawValue);
  }

  public phoneMask(rawValue) {
    return MaskService.phoneMask(rawValue);
  }

  public zipCodeMask(rawValue) {
    return MaskService.zipCodeMask();
  }

  //Validate CPF and CNPJ
  public format(valString) {
    if (!valString) {
      return '';
    }
    let val = valString.toString();
    const parts = this.unFormat(val).split(this.decimalSeparator);
    this.pureResult = parts;
    if (parts[0].length <= 11) {
      this.maskedId = this.cpf_mask(parts[0]);
      return this.maskedId;
    } else {
      this.maskedId = this.cnpj(parts[0]);
      return this.maskedId;
    }
  };

  unFormat(val) {
    if (!val) {
      return '';
    }
    val = val.replace(/\D/g, '');

    if (this.groupSeparator === ',') {
      return val.replace(/,/g, '');
    } else {
      return val.replace(/\./g, '');
    }
  };

  cpf_mask(v) {
    v = v.replace(/\D/g, ''); //Remove tudo o que não é dígito
    v = v.replace(/(\d{3})(\d)/, '$1.$2'); //Coloca um ponto entre o terceiro e o quarto dígitos
    v = v.replace(/(\d{3})(\d)/, '$1.$2'); //Coloca um ponto entre o terceiro e o quarto dígitos
    //de novo (para o segundo bloco de números)
    v = v.replace(/(\d{3})(\d{1,2})$/, '$1-$2'); //Coloca um hífen entre o terceiro e o quarto dígitos
    return v;
  }

  cnpj(v) {
    v = v.replace(/\D/g, ''); //Remove tudo o que não é dígito
    v = v.replace(/^(\d{2})(\d)/, '$1.$2'); //Coloca ponto entre o segundo e o terceiro dígitos
    v = v.replace(/^(\d{2})\.(\d{3})(\d)/, '$1.$2.$3'); //Coloca ponto entre o quinto e o sexto dígitos
    v = v.replace(/\.(\d{3})(\d)/, '.$1/$2'); //Coloca uma barra entre o oitavo e o nono dígitos
    v = v.replace(/(\d{4})(\d)/, '$1-$2'); //Coloca um hífen depois do bloco de quatro dígitos
    return v;
  }
}

