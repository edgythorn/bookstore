import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { Book } from '../models/book';
import { SortOrder, SortType } from '../models/sortorder';
import { Pagination } from '../models/pagination';
import { BooksPage } from '../models/books.page';
import { BookServiceAbstract } from './book.service.abstract';
import { ROUTE_SORT_SEGMENT, ROUTE_PAGE_SEGMENT } from '../constants';

const url = 'api/books/'; //TODO

const httpOptions = {
  headers: new HttpHeaders({
    'Access-Control-Allow-Origin': '*'
  })
};

@Injectable({
  providedIn: 'root'
})
export class BookServiceHttp implements BookServiceAbstract {

  constructor(
    private http: HttpClient
  ) { }

  getBooks(pagination: Pagination, sort?: SortOrder): Observable<BooksPage> {
    var path = url;

    if (sort && sort.sort != SortType.Unsorted)
      path += `${ROUTE_SORT_SEGMENT}/${sort.field}/${sort.sort}/`;

    path += `${ROUTE_PAGE_SEGMENT}/${pagination.pageIndex}/${pagination.pageSize}`;

    return this.http.get<BooksPage>(path, httpOptions).pipe(
      tap(r => console.log(r))
    );
  }

  getBook(id: string): Observable<Book> {
    return this.http.get<Book>(url + id, httpOptions).pipe(
      tap(r => console.log(r))
    );
  }

  addBook(book: Book, image: File): Observable<{}> {
    const formData = this.getFormData(book, image);
    return this.http.put(url, formData, httpOptions).pipe(
      tap(r => console.log(r))
    );
  }

  updateBook(book: Book, image: File): Observable<{}> {
    const formData = this.getFormData(book, image);
    return this.http.post(url, formData, httpOptions).pipe(
      tap(r => console.log(r))
    );
  }

  deleteBook(book: Book): Observable<{}> {
    return this.http.delete(url + book.id).pipe(
      tap(r => console.log(r))
    );
  }

  private getFormData(book: Book, image: File) {
    const formData = new FormData();

    Object.keys(book).forEach(key => {
      if (key != 'authors' && book[key])
        formData.append(key, book[key]);
    });

    book.authors.forEach((a, i) => {
      var prefix = `authors[${i}].`;
      Object.keys(a).forEach(key => {        
        formData.append(prefix + key, a[key]);
      });
    })

    if (image) {
      formData.append('file', image);
    }

    return formData;
  }
}
