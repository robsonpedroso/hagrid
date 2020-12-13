import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'document'
})
export class DocumentPipe implements PipeTransform {

  transform(value: any): any {
    if(value) {
        value = value.toString();
        if(value.length === 11){
            return value.substring(0,3).concat(".")
                                 .concat(value.substring(3,6))
                                 .concat(".")
                                 .concat(value.substring(6,9))
                                 .concat("-")
                                 .concat(value.substring(9,11))
        } 
        
        if (value.length === 14) {
            return value.substr(0, 2).concat(".")
                        .concat(value.substr(2, 3))
                        .concat(".")
                        .concat(value.substr(5, 3))
                        .concat("/")
                        .concat(value.substr(8, 4))
                        .concat("-")
                        .concat(value.substr(12, 2));
        }
    }
    
    return value;
  }

}
