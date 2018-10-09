import { Book } from "./book";

export class BooksPage {
    constructor(
        public books: Book[],
        public total: number
    ) { }
}