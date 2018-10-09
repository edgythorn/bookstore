import { Directive, forwardRef } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl } from '@angular/forms';

import { isbn } from './isbn.validator';

const ISBN_VALIDATOR: any = {
  provide: NG_VALIDATORS,
  useExisting: forwardRef(() => IsbnValidator),
  multi: true
};

@Directive({
  selector: '[isbn][formControlName],[isbn][formControl],[isbn][ngModel]',
  providers: [ISBN_VALIDATOR]
})
export class IsbnValidator implements Validator {
  validate(c: AbstractControl): {[key: string]: any} {
    return isbn(c);
  }
}