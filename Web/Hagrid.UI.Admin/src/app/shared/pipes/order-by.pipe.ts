import { Pipe, PipeTransform } from '@angular/core';
/**
 * Using orderBy
 * orderBy: field code with apostrophe : ascending
 *
 * e.g.
 * ```
 * {{ collection | orderBy:'name':true}}
 * ```
 * @export
 * @class OrderByPipe
 * @implements {PipeTransform}
 */
@Pipe({
	name: 'orderBy'
})
export class OrderByPipe implements PipeTransform {

	transform(array, orderBy, asc = true) {
		if (array && orderBy) {

			if (!orderBy || orderBy.trim() == "") {
				return array;
			}

			if (asc) {
				return Array.from(array).sort((item1: any, item2: any) => {
					return this.orderByComparator(item1[orderBy], item2[orderBy]);
				});
			}
			else {
				return Array.from(array).sort((item1: any, item2: any) => {
					return this.orderByComparator(item2[orderBy], item1[orderBy]);
				});
			}
		}

		return array;
	}

	orderByComparator(a: any, b: any): number {

		if (!b && !a) return 0;

		if ((isNaN(parseFloat(a)) || !isFinite(a)) || (isNaN(parseFloat(b)) || !isFinite(b))) {
			//Isn't a number so lowercase the string to properly compare
			if (a.toLowerCase() < b.toLowerCase()) return -1;
			if (a.toLowerCase() > b.toLowerCase()) return 1;
		}
		else {
			//Parse strings as numbers to compare properly
			if (parseFloat(a) < parseFloat(b)) return -1;
			if (parseFloat(a) > parseFloat(b)) return 1;
		}

		return 0; //equal each other
	}
}
