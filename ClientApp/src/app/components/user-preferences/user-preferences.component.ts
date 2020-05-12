import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProjectsService } from 'src/app/services/projects.service';
import { UserService } from 'src/app/services/user.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { User } from '../../models/api/user';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-user-preferences',
  templateUrl: './user-preferences.component.html',
  styleUrls: ['./user-preferences.component.scss'],
})
export class UserPreferencesComponent implements OnInit, OnDestroy {
  ngUnsubscribe = new Subject<void>();
  username: string;
  user: User = new User();

  constructor(
    private projectsService: ProjectsService,
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
  ) {}

  async ngOnInit() {
    this.authService
      .getUsername()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((newUsername) => {
        console.log(newUsername);
        this.username = newUsername;
        this.userService
          .getUser(this.username)
          .pipe(takeUntil(this.ngUnsubscribe))
          .subscribe((response) => {
            this.user = response.data;
            console.log(this.user);
            console.log(this.user.userName);
          });
      });
  }

  onProfileSaveChangesClick() {}
  onAccountSaveChangesClick() {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
