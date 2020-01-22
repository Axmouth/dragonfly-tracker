import { Component, OnInit } from '@angular/core';
import { ProjectsService } from '../projects.service';
import { NbTokenService, NbAuthService } from '@nebular/auth';

@Component({
  selector: 'app-view-my-projects',
  templateUrl: './view-my-projects.component.html',
  styleUrls: ['./view-my-projects.component.scss']
})
export class ViewMyProjectsComponent implements OnInit {
  projectsList: any[] = [];

  constructor(private projectsService: ProjectsService, private authService: NbAuthService, private tokenService: NbTokenService) { }

  async ngOnInit() {
    // console.log(await this.tokenService.get().toPromise());
    const username = (await (await this.tokenService.get().toPromise()).getPayload()).sub;
    //console.log(username);
    // console.log(await this.projectsService.getUsersProjects(username).catch(error => console.log(error)));
    const projectsResult = await this.projectsService.getUsersProjects(username).catch(error => console.log(error));
    console.log(projectsResult);
    this.projectsList = projectsResult["Data"];
    console.log(this.projectsList);
    console.log(this.projectsList);
  }

}
