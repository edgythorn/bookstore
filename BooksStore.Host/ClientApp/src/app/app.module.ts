import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CustomFormsModule } from 'ng2-validation'
import { HttpClientModule }    from '@angular/common/http';

import { MatPaginatorModule, MatPaginatorIntl } from '@angular/material/paginator';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { BookServiceAbstract } from './book.service/book.service.abstract';

import { IsbnValidator } from './isbn/isbn.directive';

import { AppComponent } from './app.component';
import { BooksComponent } from './books/books.component';
import { BookFormComponent } from './book-form/book-form.component';
import { AppRoutingModule } from './app-routing.module';
import { MatPaginatorIntlRus } from './mat-paginator-rus';
import { SortIconsComponent } from './sort-icons/sort-icons.component';

import { BookServiceMock } from './book.service/book.service.mock';
import { BookServiceHttp } from './book.service/book.service.http';

@NgModule({
  declarations: [
    AppComponent,
    BooksComponent,
    BookFormComponent,
    IsbnValidator,
    SortIconsComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    CustomFormsModule,
    AppRoutingModule,
    MatPaginatorModule,
    NoopAnimationsModule,
    HttpClientModule
  ],
  providers: [
    {
      provide: MatPaginatorIntl,
      useClass: MatPaginatorIntlRus
    },
    {
      provide: BookServiceAbstract,
      useClass: BookServiceHttp
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
