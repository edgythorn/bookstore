import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { Book } from '../models/book';
import { Author } from '../models/author';
import { ROUTE_ADD_SEGMENT, ROUTE_ID_PARAMETER } from '../constants';
import { BookServiceAbstract } from '../book.service/book.service.abstract';

const NOIMAGE = 'img/noimage.jpg';

@Component({
  selector: 'app-book-form',
  templateUrl: './book-form.component.html',
  styleUrls: ['./book-form.component.css']
})
export class BookFormComponent implements OnInit {

  maxImageSize = 512000;

  @Input() book: Book;

  showErrors: boolean;
  imagePath: string;
  imageFile: File;
  imageSizeIsTooLarge = false;

  constructor(
    private bookService: BookServiceAbstract,
    private route: ActivatedRoute,
    private location: Location
  ) { }

  ngOnInit() {
    if (this.isAppendMode) {
      this.book = new Book([new Author()]);
      this.imagePath = NOIMAGE;
    }
    else {
      const id = this.route.snapshot.paramMap.get(ROUTE_ID_PARAMETER);
      this.bookService.getBook(id).subscribe(b => {
        this.book = b;
        this.imagePath = b.image ? b.image : NOIMAGE;
      });
    }
  }

  onFilesChanged(files: FileList, img: any) {
    this.imageSizeIsTooLarge = files.length != 0 && files[0].size > this.maxImageSize;

    if (files.length == 0) {
      this.imageFile = null;
      img.src = this.book.image ? this.book.image : NOIMAGE;
    }
    else if (this.imageSizeIsTooLarge) {
      return;
    }
    else {
      this.imageFile = files[0];

      var reader = new FileReader();
      reader.readAsDataURL(this.imageFile);
      reader.onloadend = function () {
        img.src = reader.result
      };
    }
  }

  get isAppendMode(): boolean {
    //TODO Найти нормальный способ определения режима формы
    return this.route.snapshot.url.find(seg => seg.path == ROUTE_ADD_SEGMENT) != undefined;
  }


  addAuthor() {
    this.book.authors.push(new Author());
  }

  deleteAuthor(index: number) {
    this.book.authors.splice(index, 1);
  }


  submit() {
    if (this.isAppendMode) {
      this.bookService.addBook(this.book, this.imageFile).subscribe(() => this.cancel());
    }
    else {
      this.bookService.updateBook(this.book, this.imageFile).subscribe(() => this.cancel());
    }
  }

  cancel() {
    this.location.back();
  }
}
