import { Component, OnInit } from "@angular/core";
import { Router } from '@angular/router';
import { StoresService } from "../stores.service";
import { NotificationsService } from "angular2-notifications";
import { LocalStorageService } from "ngx-webstorage";
import { LogisticsService } from "../../shared/services/logistics.service";

import {
	FormGroup,
	FormBuilder,
	Validators,
	FormControl,
	AbstractControl
} from "@angular/forms";

@Component({
	selector: "app-store-create",
	templateUrl: "./store-create.component.html",
	styles: []
})
export class StoreCreateComponent implements OnInit {
	is_loaded: boolean = false;
	private formSumitAttempt: boolean;
	formStore: FormGroup;
	checkboxLogistics: boolean = false;
	store: any = {};


	public ngOnInit() {
		this.is_loaded = true;
		this.createForm();
		this.formControlValueChanged();
	}

	constructor(
		private storeService: StoresService,
		private notify: NotificationsService,
		private localStorageService: LocalStorageService,
		private logisticService: LogisticsService,
		private fb: FormBuilder,
		private router: Router
	) { }

	private createForm() {
		this.formStore = this.fb.group({
			company_name: ['', [Validators.required, Validators.maxLength(300)]],
			store_name: ['', Validators.required],
			cnpj: ['', Validators.required],
			ie: [],
			email: ['', [Validators.required, Validators.email]],
			user_name: ['', Validators.required],
			subdomain: ['', Validators.required],
			siteUrl: ['', Validators.required],
			billing: this.fb.group({
				finance_contact_name: ['', Validators.required],
				account_number: ['', Validators.required],
				branch_number: ['', Validators.required],
				bank_name: ['', Validators.required],
				bank_code: ['', Validators.required]
			}),
			store_address: this.fb.group({
				contact_name: [""],
				zip_code: ['', [Validators.required, Validators.minLength(8)]],
				state: [],
				district: ['', [Validators.required, Validators.maxLength(50)]],
				city: ['', [Validators.required, Validators.maxLength(50)]],
				street: ['', [Validators.required, Validators.maxLength(300)]],
				number: ['', Validators.required],
				complement: [null],
				phone_ddd: [],
				phone_number: ['', Validators.required]
			}),
			marketplace: this.fb.group({
				store_type: ['', Validators.required]
			}),
			reviews: this.fb.group({
				review_approval_email: [],
				review_approval_name: []
			}),
			logistics: this.fb.group({
				postage_address: this.fb.group({
					zip_code: [],
					state: [],
					district: [],
					city: [],
					street: [],
					number: [],
					complement: [],
					phone_ddd: [],
					phone_number: [],
				})
			}),
			accounts: this.fb.group({
				metadata: this.fb.group({
				})
			})
		});
	}

	formControlValueChanged() {

		var state = this.formStore.controls.store_address.get("state");
		var stateLog = this.formStore.controls.logistics.get("postage_address");
		var stLog = stateLog.get("state");

		state.valueChanges.subscribe(
			(mode: string) => { });

		stLog.valueChanges.subscribe(
			(mode: string) => { });
	}

	public create(): void {
		this.formSumitAttempt = true;

		if (this.formStore.valid) {

			this.store = this.formStore.value;

			this.store.store_address.contact_name = this.store.user_name;

			if (this.checkboxLogistics) {
				if (this.store.logistics.postage_address.zip_code != null) {
					var phoneLog = this.store.logistics.postage_address.phone_number;
					this.store.logistics.postage_address.phone_number = phoneLog.substring(2);
					this.store.logistics.postage_address.phone_ddd = phoneLog.substring(0, 2);
				}
			}
			else{
				this.store.logistics = null;
			}

			var phoneForSave = this.store.store_address.phone_number;
			this.store.store_address.phone_number = phoneForSave.substring(2);
			this.store.store_address.phone_ddd = phoneForSave.substring(0, 2);

			this.storeService.createStore(this.store).subscribe((response) => {
				this.notify.success("Sucesso!", "Loja criada com sucesso!");
				this.router.navigate(['stores']);
			});
		}
		else {
			this.validateAllFormFields(this.formStore);
		}
	}

	validateAllFormFields(formGroup: FormGroup) {
		Object.keys(formGroup.controls).forEach(field => {
			const control = formGroup.get(field);
			if (control instanceof FormControl) {
				control.markAsTouched({ onlySelf: true });
			} else if (control instanceof FormGroup) {
				this.validateAllFormFields(control);
			}
		});
	}

	public loadAddress(): void {
		if (this.formStore.controls.store_address.get("zip_code").value.length == 8) {
			this.logisticService.getAddressByZipCode(this.formStore.controls.store_address.get("zip_code").value).subscribe(response => {
				this.formStore.controls.store_address.patchValue({
					city: response.city,
					district: response.district,
					street: response.street,
					state: response.state
				});
			});
		}
	}

	public loadAddressLog(): void {
		var postage = this.formStore.controls.logistics.get("postage_address");
		var zipCode = postage.get("zip_code").value;

		if (zipCode.length == 8) {
			this.logisticService.getAddressByZipCode(zipCode).subscribe(response => {
				postage.patchValue({
					state: response.state,
					city: response.city,
					district: response.district,
					street: response.street,
				});
			});
		}
	}

	showBlockLog() {
		this.checkboxLogistics = !this.checkboxLogistics;
		var postageAddress = this.formStore.controls.logistics.get("postage_address");

		if (this.checkboxLogistics) {
			//Add validators case this.checkboxLogistics == true
			postageAddress.get("zip_code").setValidators([Validators.required]);
			postageAddress.get("street").setValidators([Validators.required, Validators.maxLength(200)]);
			postageAddress.get("number").setValidators([Validators.required]);
			postageAddress.get("state").setValidators([Validators.required]);
			postageAddress.get("district").setValidators([Validators.required, Validators.maxLength(50)]);
			postageAddress.get("city").setValidators([Validators.required, Validators.maxLength(50)]);
			postageAddress.get("phone_number").setValidators([Validators.required]);
		}
		else {
			postageAddress.get("zip_code").clearValidators();
			postageAddress.get("street").clearValidators();
			postageAddress.get("number").clearValidators();
			postageAddress.get("state").clearValidators();
			postageAddress.get("district").clearValidators();
			postageAddress.get("city").clearValidators();
			postageAddress.get("phone_number").clearValidators();
		}

		if (!this.checkboxLogistics) {
			postageAddress.reset();
			this.formSumitAttempt = false;
		}
	}

	isFieldValid(field: string) {
		return (
			(!this.formStore.get(field).valid && this.formStore.get(field).touched) ||
			(this.formStore.get(field).untouched && this.formSumitAttempt && this.formStore.get(field).value == '')
		);
	}

	displayFieldCss(field: string) {
		return {
			'has-error': this.isFieldValid(field),
			'has-feedback': this.isFieldValid(field)
		};
	}
}
