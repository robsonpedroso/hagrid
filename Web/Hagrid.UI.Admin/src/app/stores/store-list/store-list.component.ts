import { Component, OnInit } from '@angular/core';
import { StoresService } from '../stores.service';
import { NotificationsService } from 'angular2-notifications';
import { LocalStorageService } from 'ngx-webstorage';

@Component({
    selector: 'app-store-list',
    templateUrl: './store-list.component.html',
    styles: []
})
export class StoreListComponent implements OnInit {
    stores: any = {};
    filters: any = {};
	is_loaded: boolean = false;
	p: number = 1;

    constructor(
        private storeService: StoresService,
        private notify: NotificationsService,
        private localStorageService: LocalStorageService
    ) { }

    public ngOnInit() {
        let search = this.localStorageService.retrieve('Search');
        this.is_loaded = true;

        if (search && search.store) {
            this.filters = search.store;
            this.search(this.filters);
        }
    }

    public filter(): void {
        if (this.filters.code || this.filters.term || this.filters.document) {
            let copy = Object.assign({}, this.filters);
            delete copy.skip; delete copy.take;

            let search = this.localStorageService.retrieve('Search');

            if (!search) {
                this.localStorageService.store('Search', { store: copy });
            }
            else {
                search.store = copy;
                this.localStorageService.store('Search', search);
            }

            this.search(copy);
        }
    }

    public search(filters): void {
        this.storeService.search(filters).subscribe(
            response => {
                response.items.forEach(function (store) {
                    store.logo = store.logo.concat("?", new Date().getTime());
				  });

				this.p = response.skip + 1;
				this.stores = response;
			}
		);
	}

	public getPage(page: number) {
		this.filters.skip = page - 1;
        this.search(this.filters);
    }
}
