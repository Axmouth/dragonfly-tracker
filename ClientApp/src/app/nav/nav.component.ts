import { Component, OnInit } from '@angular/core';
import { NbAuthService } from '@nebular/auth';
import { NbMenuService } from '@nebular/theme';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  isLoggedIn = false;
  items = [
    {
      title: 'Profile',
      link: '/profile',
      icon: "person-outline"
    },
    {
      title: 'Settings',
      link: '/settings',
      icon: "settings-outline"
    },
    {
      title: 'Logout',
      link: '/auth/logout',
      icon: "log-out-outline"
    },
  ];

  constructor(protected authService: NbAuthService, protected menuService: NbMenuService) { }

  async ngOnInit() {
    // console.log(await this.authService.isAuthenticatedOrRefresh().toPromise());
    this.authService.isAuthenticatedOrRefresh().subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
    this.authService.onAuthenticationChange().subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
    this.menuService.onItemClick()
      .pipe(
        filter(({ tag }) => tag === "nav-user-context-menu"),
        map(({ item: { title } }) => title),
      )
      .subscribe(title => {
        console.log(title)
      });
  }

}
