import { Injectable } from '@angular/core';
import { Book } from '../models/book';
import { BOOKS } from './mock-books';
import { Observable, of } from 'rxjs';
import { SortOrder, SortType } from '../models/sortorder';
import { Pagination } from '../models/pagination';
import { BooksPage } from '../models/books.page';
import { BookServiceAbstract } from './book.service.abstract';

@Injectable({
  providedIn: 'root'
})
export class BookServiceMock implements BookServiceAbstract {

  deleteBook(book: Book): Observable<{}> {
    const index = BOOKS.findIndex(b => b == book);
    BOOKS.splice(index, 1);
    return of({});
  }

  getBooks(pagination: Pagination, sort?: SortOrder): Observable<BooksPage> {
    var result: Book[] = BOOKS.slice(0);

    if (sort != null && sort.sort != SortType.Unsorted) {

      if (sort.field == 'title')
        result = result.sort(this.compareTitles);
      else
        result = result.sort(this.comparePublishYears);

      if (sort.sort == SortType.Descending)
        result = result.reverse();
    }

    const start = pagination.pageIndex * pagination.pageSize;
    const end = start + pagination.pageSize;

    return of(new BooksPage(result.slice(start, end), BOOKS.length));
  }

  compareTitles(a: Book, b: Book): number {
    if (a.title > b.title) return 1;
    if (a.title < b.title) return -1;
    return 0;
  }

  comparePublishYears(a: Book, b: Book): number {
    if (a.publishYear > b.publishYear) return 1;
    if (a.publishYear < b.publishYear) return -1;
    return 0;
  }

  getBook(id: string): Observable<Book> {
    return of(JSON.parse(JSON.stringify(BOOKS.find(b => b.id === id))));
  }

  addBook(book: Book, image: File): Observable<{}> {
    book.id = this.newGuid();
    BOOKS.push(book);
    //TODO save image
    return of({});
  }

  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  updateBook(book: Book, image: File): Observable<{}> {
    const index = BOOKS.findIndex(b => b.id == book.id);
    BOOKS[index] = book;
    //TODO save image
    return of({});
  }
}
