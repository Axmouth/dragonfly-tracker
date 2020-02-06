import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-logout-page',
  templateUrl: './logout-page.component.html',
  styleUrls: ['./logout-page.component.scss']
})
export class LogoutPageComponent implements OnInit {
  isSuccess = false;
  isFailure = false;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.authService.logout().subscribe(result => {
      console.log(result);
      if (result.isSuccess) {
        this.isFailure = false;
        this.isSuccess = true;
        this.router.navigateByUrl('');
      } else {
        this.isFailure = true;
        this.isSuccess = false;
      }
    });
  }

}
