import { MatPaginatorIntl } from "@angular/material/paginator";
import { Injectable } from "@angular/core";

@Injectable()
export class MatPaginatorIntlRus extends MatPaginatorIntl {
    itemsPerPageLabel = 'Элементов на странице: ';
    nextPageLabel = 'Следующая';
    previousPageLabel = 'Предыдущая';

    getRangeLabel = (page: number, pageSize: number, length: number) => {
        let above = page * pageSize;
        let currentMax = above + pageSize;
        if(currentMax > length) currentMax = length;
        return (above + 1) + ' - ' + currentMax + ' из ' + length;
    }
}