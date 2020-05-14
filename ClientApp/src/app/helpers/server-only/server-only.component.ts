import { Component, OnInit, PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-server-only',
  templateUrl: './server-only.component.html',
  styleUrls: ['./server-only.component.scss'],
})
export class ServerOnlyComponent implements OnInit {
  isBrowser = false;

  constructor(@Inject(PLATFORM_ID) private platform: Object) {
    this.isBrowser = isPlatformBrowser(platform);
  }

  ngOnInit() {}
}
