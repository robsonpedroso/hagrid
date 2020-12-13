import { Component, ViewChild, Output, EventEmitter, OnInit, Input, setTestabilityGetter } from '@angular/core';
import { SelectComponent } from 'ng2-select';
import { StoresService } from '../../../stores/stores.service';

@Component({
	selector: 'app-application-select',
	templateUrl: './application-select.component.html',
	styles: []
})
export class ApplicationSelectComponent implements OnInit{
	@ViewChild('selectApplication') selectApplication: SelectComponent;
	@Output() appResult = new EventEmitter<any>();

	constructor(
		private storeService: StoresService
	) { }

	ngOnInit() {
		this.getApplications();
	}

	public getApplications(): void {
        this.storeService.getApplications().subscribe((response) => {
            let members = response.filter(x => x.member.toLowerCase() === "merchant");
            this.selectApplication.items = members.map(result => {
                return { id: result.code, text: result.name }
            });
        });
    }

	public setApplication(value: any): void {
		this.appResult.emit(value);
	}
}
