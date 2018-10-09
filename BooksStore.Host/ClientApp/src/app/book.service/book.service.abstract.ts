import { Book } from "../models/book";
import { Observable } from "rxjs";
import { Pagination } from "../models/pagination";
import { SortOrder } from "../models/sortorder";
import { BooksPage } from "../models/books.page";

export abstract class BookServiceAbstract {

    abstract deleteBook(book: Book): Observable<{}>;

    abstract getBooks(pagination: Pagination, sort?: SortOrder): Observable<BooksPage>;

    abstract getBook(id: string): Observable<Book>;

    abstract addBook(book: Book, image: File): Observable<{}>;

    abstract updateBook(book: Book, image: File): Observable<{}>;
}