import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'permissionSearch'
})
export class PermissionSearchPipe implements PipeTransform {
    transform(items: any[], permissionText: string): any[] {
        if (!items) return [];
        if (!permissionText) return items;

		permissionText = permissionText.toLowerCase();
		return items.filter(item => {
			return item.resource.application.name.toLowerCase().includes(permissionText) ||
				   item.resource.name.toLowerCase().includes(permissionText);
        });
    }
}
