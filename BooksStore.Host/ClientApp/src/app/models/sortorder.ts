export enum SortType { Unsorted, Ascending, Descending }

export class SortOrder {
    constructor(
        public field: string,
        public sort: SortType) { }
}