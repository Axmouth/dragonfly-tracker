import { User } from './user';
import { IssueStage } from './issue-stage';
import { IssueType } from './issue-type';

export class Project {
  name?: string;
  description?: string;
  private?: boolean;
  stages?: IssueStage[];
  types?: IssueType[];
  creator?: User;
  owner?: User;
  admins?: User[];
  maintainers?: User[];
  createdAt?: string;
  updatedAt?: string;
}
