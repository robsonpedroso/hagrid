import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'phone'
})
export class PhonePipe implements PipeTransform {

    transform(value: any): any {
        if (value) {
            value = value.toString().replace("-", "");
            if (value.length === 8) {
                return value.substr(0, 4)
                        .concat("-")
                        .concat(value.substr(4, 4));
            }

            if (value.length === 9) {
                return value.substr(0, 5)
                        .concat("-")
                        .concat(value.substr(5, 4));
            }

            if (value.length === 10) {
                return "(".concat(value.substr(0, 2))
                        .concat(")")
                        .concat(value.substr(2, 4))
                        .concat("-")
                        .concat(value.substr(6, 4));
            }

            if (value.length === 11) {
                return "(".concat(value.substr(0, 2))
                        .concat(")")
                        .concat(value.substr(2, 5))
                        .concat("-")
                        .concat(value.substr(7, 4));
            }
        }
        return value;
    }
}
