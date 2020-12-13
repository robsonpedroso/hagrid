import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'zipcode'
})
export class ZipcodePipe implements PipeTransform {

    transform(value: any): any {
        if (value) {
            value = value.toString();
            if (value.length === 8) {
                return value.substr(0, 5)
                            .concat("-")
                            .concat(value.substr(5, 3));
            }
        }
        return value;
    }
}
