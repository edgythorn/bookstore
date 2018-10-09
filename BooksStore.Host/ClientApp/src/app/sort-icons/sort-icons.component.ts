import { Component, OnInit, Input } from '@angular/core';
import { SortOrder } from '../models/sortorder';

@Component({
  selector: 'app-sort-icons',
  templateUrl: './sort-icons.component.html',
  styleUrls: ['./sort-icons.component.css']
})
export class SortIconsComponent {

  @Input() sort: SortOrder;
  @Input() forField: string;

}
