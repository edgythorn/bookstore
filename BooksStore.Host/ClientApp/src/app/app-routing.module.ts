import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { BooksComponent } from './books/books.component';
import { BookFormComponent } from './book-form/book-form.component';

// сегменты и параметры путей используются в компонентах, т.ч. хардкод может привести к ошибкам
import { ROUTE_BOOKS_SEGMENT, ROUTE_SORT_SEGMENT, ROUTE_FIELD_PARAMETER, ROUTE_SORT_PARAMETER, ROUTE_ID_PARAMETER, ROUTE_ADD_SEGMENT, ROUTE_PAGE_SEGMENT, ROUTE_PAGE_PARAMETER, ROUTE_EDIT_SEGMENT } from './constants';

const routes: Routes = [
  // defaults (books)
  { path: '', redirectTo: `/${ROUTE_BOOKS_SEGMENT}`, pathMatch: 'full' },
  { path: ROUTE_BOOKS_SEGMENT, component: BooksComponent },
  // books sorted
  { path: `${ROUTE_BOOKS_SEGMENT}/${ROUTE_SORT_SEGMENT}/:${ROUTE_FIELD_PARAMETER}/:${ROUTE_SORT_PARAMETER}`, component: BooksComponent },
  // books pagination
  { path: `${ROUTE_BOOKS_SEGMENT}/${ROUTE_PAGE_SEGMENT}/:${ROUTE_PAGE_PARAMETER}`, component: BooksComponent },
  // books sort + pagination
  { path: `${ROUTE_BOOKS_SEGMENT}/${ROUTE_SORT_SEGMENT}/:${ROUTE_FIELD_PARAMETER}/:${ROUTE_SORT_PARAMETER}/${ROUTE_PAGE_SEGMENT}/:${ROUTE_PAGE_PARAMETER}`, component: BooksComponent },
  // edit
  { path: `${ROUTE_EDIT_SEGMENT}/:${ROUTE_ID_PARAMETER}`, component: BookFormComponent },
  // add
  { path: ROUTE_ADD_SEGMENT, component: BookFormComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }