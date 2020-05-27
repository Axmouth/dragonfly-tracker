import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-view-with-loading-state',
  templateUrl: './view-with-loading-state.component.html',
  styleUrls: ['./view-with-loading-state.component.scss'],
})
export class ViewWithLoadingStateComponent implements OnInit {
  @Input()
  found = true;
  @Input()
  loading = true;
  @Input()
  loadingText = 'Loading';

  constructor() {}

  ngOnInit() {}
}
