import { Component, OnInit } from '@angular/core';
import { AccountService } from '../accounts.service';
import { LocalStorageService } from 'ngx-webstorage';

@Component({
    selector: 'app-account-list',
    templateUrl: './account-list.component.html',
    styles: []
})
export class AccountListComponent implements OnInit {

    accounts: any[];
    is_loaded: boolean;
    filters: any = {}

    constructor(
        private accountService: AccountService,
        private localStorageService: LocalStorageService) { }

    ngOnInit() {

        let search = this.localStorageService.retrieve('Search');
        
        if (search && search.account) {
            this.filters = search.account;
            this.search(this.filters);
        }
        else {
            this.is_loaded = true;
        }
    }

    public filter(): void {

        if (this.filters.code || this.filters.email || this.filters.document) {

            let copy = Object.assign({}, this.filters);
            delete copy.skip; delete copy.take;

            let search = this.localStorageService.retrieve('Search');

            if (!search) {
                this.localStorageService.store('Search', { account: copy });
            }
            else {
                search.account = copy;
                this.localStorageService.store('Search', search);
            }

            this.search(this.filters);
        }
    }

    public search(filters): void {
        this.accountService.search(filters).subscribe((response) => {
            this.accounts = response.items;
            this.is_loaded = true;
        });
    }
}
