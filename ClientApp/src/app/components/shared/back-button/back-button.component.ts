import { Component, OnInit } from '@angular/core';
import { RouteStateService } from 'src/app/services/route-state.service';
import { IsBrowserService } from 'src/app/helpers/services/is-browser.service';

@Component({
  selector: 'app-back-button',
  templateUrl: './back-button.component.html',
  styleUrls: ['./back-button.component.scss'],
})
export class BackButtonComponent implements OnInit {
  previousUrl: string;

  constructor(private routeStateService: RouteStateService, private isBrowserService: IsBrowserService) {}

  ngOnInit() {
    this.previousUrl = this.routeStateService.getPreviousUrl();
    if (!this.isBrowserService.isInBrowser()) {
      return;
    }
  }
}
