import { Component, OnInit } from '@angular/core';
import { ProjectsService } from '../projects.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-view-user-project',
  templateUrl: './view-user-project.component.html',
  styleUrls: ['./view-user-project.component.scss']
})
export class ViewUserProjectComponent implements OnInit {
  project = {};
  targetUsername: string;
  targetProjectName: string;
  loading = true;
  notFound = false;

  constructor(private projectsService: ProjectsService, private route: ActivatedRoute) { }

  async ngOnInit() {
    const params = this.route.snapshot.paramMap;
    this.targetUsername = params.get("username");
    this.targetProjectName = params.get("projectname");
    this.project = await this.projectsService.getUsersProject(this.targetUsername, this.targetProjectName);
    console.log(this.project);
  }

}
