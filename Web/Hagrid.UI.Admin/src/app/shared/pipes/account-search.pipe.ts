import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'accountSearch'
})
export class AccountSearchPipe implements PipeTransform {
    transform(items: any[], searchText: string): any[] {
        if (!items) return [];
		if (!searchText) return items;

		searchText = searchText.toLowerCase().replace(/\s/g, "");
		return items.filter(item => {
			let document = item.account.document ? item.account.document.toLowerCase().replace(/\s/g, "").includes(searchText)
												 : "";
			return item.account.code.toLowerCase().replace(/\s/g, "").includes(searchText) ||
				   item.account.email.toLowerCase().replace(/\s/g, "").includes(searchText) ||
				   document
        });
    }
}
