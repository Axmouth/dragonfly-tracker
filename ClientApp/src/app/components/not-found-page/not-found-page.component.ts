import { Component, OnInit } from '@angular/core';
import { RouteStateService } from 'src/app/services/route-state.service';

@Component({
  selector: 'app-not-found-page',
  templateUrl: './not-found-page.component.html',
  styleUrls: ['./not-found-page.component.scss'],
})
export class NotFoundPageComponent implements OnInit {
  previousUrl: string;

  constructor(private routeStateService: RouteStateService) {}

  ngOnInit() {
    this.previousUrl = this.routeStateService.getPreviousUrl();
  }
}
