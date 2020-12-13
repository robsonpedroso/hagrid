import { Component, OnInit, ViewChild } from '@angular/core';
import { AccountService } from '../accounts.service';
import { ActivatedRoute, Router } from '@angular/router';
import { StoresService } from '../../stores/stores.service';
import { validateConfig } from '@angular/router/src/config';
import { SelectComponent } from 'ng2-select';
import { NotificationsService } from 'angular2-notifications';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-account-create',
    templateUrl: './account-create.component.html'
})

export class AccountCreateComponent implements OnInit {

    @ViewChild('selectStore') selectStore: SelectComponent;
    @ViewChild('selectApplication') selectApplication: SelectComponent;

    account: any = {};
    applications: any[] = [];
    checkboxValue: boolean;
    dateNow = new Date();
	pass: Number;
	decimalSepatator = ".";
  	groupSeparator = ",";
  	pureResult: any;
  	maskedId: any;

    constructor(
        private accountservice: AccountService,
        private storeService: StoresService,
        private notify: NotificationsService,
        private router: Router) {}

    public ngOnInit(): void {
        this.getApplications();
        let day = this.dateNow.getUTCDate();
        let month = (this.dateNow.getMonth() + 1);
        let year = AccountCreateComponent.getUnidade(this.dateNow.getFullYear());
        let mil = this.dateNow.getMilliseconds();
        this.pass = (Number(day) * Number(month) * Number(year)) * mil * 5;
	}

	static getUnidade(value): string {
        let final = value.toString().length;
        return value.toString().substring((final - 2), final);
    }

    public save(): void {

        if (this.applications.length > 0 && this.account.store_code) {

			if (this.account.document != undefined){
				this.account.document = this.account.document.replace(/\D/g, '');
			}
			this.account.login = this.account.email;
			this.account.applications = this.applications;

			if (this.checkboxValue == true) {
				this.account.code_email_template = environment.create_user_rmail_template;
			}
			this.accountservice.save(this.account).subscribe((response) => {
                if (response.status == true) {
                    this.notify.success('Sucesso!', 'Usuário criado com sucesso.');

                    this.router.navigate(['accounts']);
                }
            });
        }
        else {
            this.notify.warn('Atenção!', 'Por favor selecione a loja ou aplicação');
        }
    }

    public getStores(term: any): void {
        if (term && term.length == 2) {
            this.storeService.getByTerm(term).subscribe((response) => {
                this.selectStore.items = response.map(result => {
                    return { id: result.code, text: result.name }
                });
            });
        }
    }

    public getApplications(): void {
        this.storeService.getApplications().subscribe((response) => {
            let members = response.filter(x => x.member.toLowerCase() === "merchant");
            this.selectApplication.items = members.map(result => {
                return { id: result.code, text: result.name }
            });
        });
    }

    public setStore(value: any): void {
        this.account.store_code = value.id;
    }

    public addApplications(value: any): void {
        this.applications = value.map(val => { return val.text });
	}

	format(value) {
		if (!value) {
			return '';
		}
		let val = value.toString();
		const parts = this.unFormat(val).split(this.decimalSepatator);
		this.pureResult = parts;
		if(parts[0].length <= 11){
		  this.maskedId = this.cpf_mask(parts[0]);
		  return this.maskedId;
		}else{
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
		v = v.replace(/\D/g, '');
		v = v.replace(/(\d{3})(\d)/, '$1.$2');
		v = v.replace(/(\d{3})(\d)/, '$1.$2');
		v = v.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
		return v;
	};

	 cnpj(v) {
		v = v.replace(/\D/g, '');
		v = v.replace(/^(\d{2})(\d)/, '$1.$2');
		v = v.replace(/^(\d{2})\.(\d{3})(\d)/, '$1.$2.$3');
		v = v.replace(/\.(\d{3})(\d)/, '.$1/$2');
		v = v.replace(/(\d{4})(\d)/, '$1-$2');
		return v;
	};

}





