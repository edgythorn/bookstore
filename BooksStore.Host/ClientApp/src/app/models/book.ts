import { Author } from "./author";

export class Book {
    
    constructor(authors: Author[]) {
        this.authors = authors;
    }

    id: string;
    title: string;
    authors: Author[];
    pagesCount: number;
    publisher: string;
    publishYear: number;
    isbn: string;
    image: string;
    imagePreview: string;
}