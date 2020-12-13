import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'groupPermission'
})
export class GroupPermissionPipe implements PipeTransform {
    transform(items: any[], groupText: string): any[] {
        if (!items) return [];
        if (!groupText) return items;

		groupText = groupText.toLowerCase();
		return items.filter(item => {
			return item.name.toLowerCase().includes(groupText) ||
				   item.store.name.toLowerCase().includes(groupText);
        });
    }
}
