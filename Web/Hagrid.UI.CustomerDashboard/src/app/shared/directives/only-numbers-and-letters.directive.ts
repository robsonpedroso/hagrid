import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[onlyNumbersAndLetters]'
})
export class OnlyNumbersAndLettersDirective {

    private regex: RegExp = new RegExp(/^-?[0-9a-zA-Z.-]*/g);
    private specialKeys: Array<string> = ["Backspace", "Tab", "End", "Home", "-"];

    constructor(private el: ElementRef) {}

    @HostListener("keydown", ["$event"]) onKeyDown(event: KeyboardEvent) {

    if (this.specialKeys.indexOf(event.key) !== -1) {
      return;
    }

    let current: string = this.el.nativeElement.value;
    let next: string = current.concat(event.key);

    if (next && String(next).match(this.regex)[0] != String(next)) {
      event.preventDefault();
      this.el.nativeElement.value = String(next).match(this.regex)[0];
    }
  }

  @HostListener("blur") onBlur() {
    this.el.nativeElement.value = String(this.el.nativeElement.value).match(this.regex)[0];
  }
}
