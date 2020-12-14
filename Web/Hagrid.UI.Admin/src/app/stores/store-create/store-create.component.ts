import { Component, OnInit } from "@angular/core";
import { Router } from '@angular/router';
import { StoresService } from "../stores.service";
import { NotificationsService } from "angular2-notifications";
import { LocalStorageService } from "ngx-webstorage";

import {
	FormGroup,
	FormBuilder
} from "@angular/forms";

@Component({
	selector: "app-store-create",
	templateUrl: "./store-create.component.html",
	styles: []
})
export class StoreCreateComponent implements OnInit {
	is_loaded: boolean = false;
	checkboxLogistics: boolean = false;
	store: any = {
		addresses: []
	};
	address: any = {};

	states: any[] = [
		"SP", "AM", "RR", "AC",
		"RO", "PA",	 "AP", "TO",
		"GO", "MT",	 "MS", "PI",
		"CE", "AL",	 "PB", "SE",
		"RN", "PE",	 "MG", "ES",
		"RJ", "RS",	 "SC", "PR",
		"DF"
	];

	public ngOnInit() {
		this.is_loaded = true;
	}

	constructor(
		private storeService: StoresService,
		private notify: NotificationsService,
		private localStorageService: LocalStorageService,
		private router: Router
	) { }

	public create(): void {
		this.store.addresses.push(this.address);
		this.storeService.createStore(this.store).subscribe((response) => {
		 	this.notify.success("Sucesso!", "Loja criada com sucesso!");
		 	this.router.navigate(['stores']);
		});
	}

	isFieldValid(field: string) {
		return field == '';
	}

	displayFieldCss(field: string) {
		return {
			'has-error': this.isFieldValid(field),
			'has-feedback': this.isFieldValid(field)
		};
	}
}
