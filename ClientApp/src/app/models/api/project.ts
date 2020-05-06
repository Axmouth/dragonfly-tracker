import { User } from './user';
import { IssueStage } from './issue-stage';
import { IssueType } from './issue-type';

export class Project {
  name?: string;
  description?: string;
  isPublic?: boolean;
  stages?: IssueStage[];
  types?: IssueType[];
  creator?: User;
  admins?: User[];
  maintainers?: User[];
}
