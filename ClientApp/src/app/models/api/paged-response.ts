export class PagedResponse<T> {
  data: T[];
  pageNumber?: number;
  total?: number;
  pageSize?: number;
  nextPage: string;
  previousPage: string;
}
