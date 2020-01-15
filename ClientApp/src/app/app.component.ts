import { Component } from '@angular/core';
import { NbIconConfig } from '@nebular/theme';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
    title = 'app';
    disabledIconConfig: NbIconConfig = { icon: 'settings-2-outline', pack: 'eva' };
}
