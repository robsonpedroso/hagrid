import { Component, OnInit, OnDestroy } from '@angular/core';
import { environment } from '../environments/environment';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {

	public notification_options = {
		position: ['top', 'right'],
		timeOut: 3000,
		showProgressBar: true,
		pauseOnHover: true,
		clickToClose: false,
		maxLength: 10
	};

	public confirmation_options = {
		overlayClickToClose: false,
		showCloseButton: true,
		confirmText: 'Sim',
		declineText: 'Não'
	};

	constructor() { }

	body: HTMLBodyElement = document.getElementsByTagName('body')[0];

	ngOnInit() {
		// add the the body classes
		this.body.classList.add(environment.theme);
		this.body.classList.add('sidebar-mini');
	}

	ngOnDestroy() {
		// remove the the body classes
		this.body.classList.remove(environment.theme);
		this.body.classList.remove('sidebar-mini');
	}
}
