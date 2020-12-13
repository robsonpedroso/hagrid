import { Component, OnInit, ViewChild } from '@angular/core';
import { LocalStorageService } from 'ngx-webstorage';
import { MetadatasService } from '../metadatas.service';
import { NotificationsService } from 'angular2-notifications';
import { ResolveEmit } from '@jaspero/ng2-confirmations/src/interfaces/resolve-emit';
import { ConfirmationService } from '@jaspero/ng2-confirmations';
import { FormControl } from '@angular/forms';
import * as _ from 'lodash';
import { AuthPermissionService } from '../../shared/auth/auth-permission.service';
import { Keys } from '../../shared/models/keys';
import { Router } from '@angular/router';

@Component({
	selector: 'app-metadata-field',
	templateUrl: './metadata-field.component.html',
	styles: []
})
export class MetadataFieldComponent implements OnInit {

	metadata_field: any = {
		format: '',
		type: '',
		validator: {
			type: '',
			values: []
		}
	};

	show_modal: boolean;
	validator_types: any[] = [];
	metadata_formats: any[];
	metadatas_fields: any[] = [];
	validate_by_format: any[] = [];
	error_messages_validator = {
		'isDecimal': 'Por favor, apenas números decimais',
		'isInteger': 'Por favor, apenas números interios'
	};

	is_loaded: boolean;
	currentMetadadosPermission: any = {};
	alert: any = {
		timeOut: 3000,
		showProgressBar: false,
		pauseOnHover: true,
		clickToClose: true,
		animate: 'fromLeft'
	};

	constructor(
		private metadatasService: MetadatasService,
		private notify: NotificationsService,
		private notifyConfirm: ConfirmationService,
		private authPermissionService: AuthPermissionService,
		private router: Router) {
		this.metadata_formats = metadatasService.MetadataFormats;
	}

	ngOnInit() {
		this.currentMetadadosPermission = this.authPermissionService.getCurrentPermission(Keys.MetadadosModule.Metadados);
		this.get();
	}

	public get(): void {
		this.metadatasService.search({ type: null, skip: 0, take: 9999 }).subscribe((response) => {

			this.metadatas_fields = response.items.map(field => {
				if (!field.validator) {
					field.validator = { type: '' };
				}
				return field;
			});

			this.metadatasService.buildAttributesMetadata(this.metadatas_fields);
			this.is_loaded = true;
		});
	}

	public save(): void {
		this.metadatasService.save(this.metadata_field).subscribe((response) => {

			if (!this.metadata_field.code) {
				let field = response;
				field.is_new = true;
				setTimeout(() => { field.is_new = false; }, 3000);

				this.metadatas_fields.unshift(field);
				this.metadatasService.buildAttributesMetadata(this.metadatas_fields);
				this.metadatas_fields = this.metadatas_fields.slice();
			}
			else {

				let index = this.metadatas_fields.findIndex(m => m.code == this.metadata_field.code);
				let field = this.metadatas_fields[index];

				field.is_new = true;
				setTimeout(() => { field.is_new = false; }, 3000);
				field.name = this.metadata_field.name;
			}

			this.cancel();
			this.notify.success('Sucesso!', 'Campo salvo com sucesso =)');
		});
	}

	public cancel(): void {
		this.metadata_field = {
			format: '',
			type: '',
			validator: {
				type: '',
				values: []
			}
		};

		this.validator_types = [];
		this.validate_by_format = [];
		this.show_modal = false;
	}

	public edit(code: string): void {
		if (!this.currentMetadadosPermission.Insert)
		{
			this.show_modal = false;
			this.notify.create('Atenção!', 'Usuário sem permissão para executar essa ação', 'warn', this.alert);
			this.router.navigate(['/home']);
		}
		let index = this.metadatas_fields.findIndex(m => m.code == code);
		let field = this.metadatas_fields[index];
		this.changeFormat(field.format);
		this.metadata_field = _.cloneDeep(field);
	}

	public delete(code: string, type: string): void {
		this.notifyConfirm.create('Atenção!', 'Deseja realmente remover esse campo?').subscribe((ans: ResolveEmit) => {
			if (ans.resolved) {
				this.metadatasService.delete(code, type).subscribe((response) => {
					this.get();
					this.notify.success('Sucesso!', 'Campo removido com sucesso =)');
				});
			}
		});
	}

	public changeFormat(format: number) {

		this.validator_types = [];
		this.metadata_field.validator.type = '';
		this.metadata_field.validator.values = [];
		this.validate_by_format = [];

		switch (format) {

			case 1: //Texto
			case 2: //Inteiro
			case 3: //Decimal
				this.validator_types = [{ id: 'Options', text: 'Lista de valores' }]

				if (format == 2) {
					this.validate_by_format = [this.validatorIntegerValue];
				}
				else if (format == 3) {
					this.validate_by_format = [this.validatorDecimalValue];
				}

				break;

			case 5: //JSON
				this.validator_types = [{ id: 'JsonSchema', text: 'Formato de Json' }]
				break;
		}

		if (this.validator_types.length > 0) {
			this.validator_types.unshift(
				{ id: '', text: 'Selecione...' }
			);
		}
	}

	public validateOnlyLetters(evt) {
		var theEvent = evt || window.event;
		var key = theEvent.keyCode || theEvent.which;

		key = String.fromCharCode(key);

		var regex = /[a-z]|\_/;
		if (!regex.test(key)) {
			theEvent.returnValue = false;
			if (theEvent.preventDefault) theEvent.preventDefault();
		}
	}

	private validatorDecimalValue(control: FormControl) {

		const value = control.value;

		const result: any = isNaN(value) ? {
			isDecimal: true
		} : null;

		return result;
	}

	private validatorIntegerValue(control: FormControl) {

		const value = control.value;

		var regX = /^\d+$/;

		const result: any = !regX.test(value) ? {
			isInteger: true
		} : null;

		return result;
	}
}
