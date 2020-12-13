import { AbstractControl } from '@angular/forms';

export class ValidationService {

	static Number(control: AbstractControl): { [key: string]: any } | null {
		const valid = /^\d+$/.test(control.value);
		return valid ? null : { invalidNumber: { valid: false, value: control.value } };
	}

	static Email(control: AbstractControl): { [key: string]: any } | null {
		const valid = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/.test(control.value);
		return valid ? null : { invalidEmail: { valid: false, value: control.value } };
	}
}
