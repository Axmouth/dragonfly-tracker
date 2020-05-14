import { Component, OnInit, PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-browser-only',
  templateUrl: './browser-only.component.html',
  styleUrls: ['./browser-only.component.scss'],
})
export class BrowserOnlyComponent implements OnInit {
  isBrowser = false;

  constructor(@Inject(PLATFORM_ID) private platform: Object) {
    this.isBrowser = isPlatformBrowser(platform);
  }

  ngOnInit() {}
}
