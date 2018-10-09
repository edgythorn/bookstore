import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';

import { PageEvent } from '@angular/material/paginator';

import { Book } from '../models/book';
import { ROUTE_BOOKS_SEGMENT, ROUTE_SORT_SEGMENT, ROUTE_FIELD_PARAMETER, ROUTE_SORT_PARAMETER, ROUTE_PAGE_PARAMETER, ROUTE_PAGE_SEGMENT } from '../constants';
import { SortType, SortOrder } from '../models/sortorder';
import { Pagination } from '../models/pagination';
import { BookServiceAbstract } from '../book.service/book.service.abstract';


@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.css']
})
export class BooksComponent implements OnInit {

  books: Book[];
  currentSort: SortOrder;
  pageSize = 5;
  total: number;
  pageIndex: number;

  constructor(
    private bookService: BookServiceAbstract,
    private route: ActivatedRoute,
    private router: Router,
  ) { }

  ngOnInit() {
    this.initializeAsync();
  }

  initializeAsync() {
    this.route.paramMap.subscribe(pm => {

      this.currentSort = this.getCurrentSort(pm);
      this.pageIndex = this.getCurrentPage(pm);

      this.bookService.getBooks(new Pagination(this.pageIndex, this.pageSize), this.currentSort)
        .subscribe(page => {
          this.books = page.books;
          this.total = page.total;
        });
    });
  }

  public onPageChanged(event?: PageEvent) {
    var url = this.buildRouteForPagination(event.pageIndex);
    this.router.navigateByUrl(url);
  }

  delete(book: Book): void {
    this.bookService.deleteBook(book).subscribe(() => {
      this.initializeAsync();
    });    
  }

  sort(field: string): void {
    const currentSort = this.getCurrentSort(this.route.snapshot.paramMap);

    // если поле для сортировки изменилось, не учитываем текущий режим сортировки
    var sortType = field === currentSort.field ? currentSort.sort : SortType.Unsorted;

    switch (sortType) {
      case SortType.Unsorted:
        sortType = SortType.Ascending;
        break;
      case SortType.Ascending:
        sortType = SortType.Descending;
        break;
      case SortType.Descending:
        sortType = SortType.Unsorted;
        break;
    }

    const url = this.buildRouteForSort(field, sortType);
    this.router.navigateByUrl(url);
  }

  getCurrentSort(paramMap: ParamMap): SortOrder {
    const field = paramMap.get(ROUTE_FIELD_PARAMETER);
    if (!field)
      return new SortOrder(null, SortType.Unsorted);

    const sortType = +paramMap.get(ROUTE_SORT_PARAMETER);
    return sortType == SortType.Ascending ? new SortOrder(field, SortType.Ascending) : new SortOrder(field, SortType.Descending);
  }

  getCurrentPage(paramMap: ParamMap): number {
    var page = +paramMap.get(ROUTE_PAGE_PARAMETER);
    console.log('page ' + page);
    return page;
  }
  
  buildRouteForSort(field: string, sort: SortType): string {
    var url = "/" + ROUTE_BOOKS_SEGMENT;

    if (sort != SortType.Unsorted)
      url += `/${ROUTE_SORT_SEGMENT}/${field}/${sort}`;

    return url;
  }

  buildRouteForPagination(pageIndex: number): string {    
    const currentSort = this.getCurrentSort(this.route.snapshot.paramMap);

    var url = this.buildRouteForSort(currentSort.field, currentSort.sort);
    url += `/${ROUTE_PAGE_SEGMENT}/${pageIndex}`;

    return url;
  }
}
