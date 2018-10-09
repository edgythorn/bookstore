import { AbstractControl, Validators, ValidatorFn } from '@angular/forms';

export const isbn: ValidatorFn = (control: AbstractControl): { [key: string]: boolean } => {
    if (isValidISBN(control.value))
        return null;

    return { isbn: true };
};

function isValidISBN(isbn) {
    if (!/(^978-(\d+-){3}\d$)|(^(?!978-)(\d+-){3}[\dx]$)/i.test(isbn))
        return false;

    isbn = isbn.replace(/[^\dX]/gi, '');
    if (isbn.length == 10) {
        var chars = isbn.split('');
        if (chars[9].toUpperCase() == 'X') {
            chars[9] = 10;
        }
        var sum = 0;
        for (var i = 0; i < chars.length; i++) {
            sum += ((10 - i) * parseInt(chars[i]));
        }
        return (sum % 11 == 0);
    } else if (isbn.length == 13) {
        var chars = isbn.split('');
        var sum = 0;
        for (var i = 0; i < chars.length; i++) {
            if (i % 2 == 0) {
                sum += parseInt(chars[i]);
            } else {
                sum += parseInt(chars[i]) * 3;
            }
        }
        return (sum % 10 == 0);
    } else {
        return false;
    }
}