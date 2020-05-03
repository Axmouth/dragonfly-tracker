import { Project } from './project';
import { User } from './user';
import { IssueStage } from './issue-stage';
import { IssueType } from './issue-type';

export class Issue {
  title?: string;
  content?: string;
  number?: number;
  isPublic?: boolean;
  parentProject?: Project;
  author?: User;
  stage?: IssueStage;
  types?: IssueType[];
}
