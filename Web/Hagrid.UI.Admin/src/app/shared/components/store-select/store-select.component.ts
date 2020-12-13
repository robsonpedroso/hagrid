import { Component, ViewChild, Output, EventEmitter, Input, OnInit } from '@angular/core';
import { SelectComponent } from 'ng2-select';
import { StoresService } from '../../../stores/stores.service';

@Component({
	selector: 'app-store-select',
	templateUrl: './store-select.component.html',
	styles: []
})
export class StoreSelectComponent implements OnInit {
	@ViewChild('selectStore') selectStore: SelectComponent;
	@Output() storeResult = new EventEmitter<any>();

	ngOnInit(){}

	constructor(
		private storeService: StoresService
	) {}

	public getStores(term: any): void {
		if (term && term.length == 2) {
			this.storeService.getByTerm(term).subscribe((response) => {
				this.selectStore.items = response.map(result => {
					return { id: result.code, text: result.name }
				});
			});
		}
	}

	public setStore(value: any): void {
		this.storeResult.emit(value);
	}
}
