import { User } from './user';

export class Project {
  name?: string;
  description?: string;
  isPublic?: boolean;
  issueStages?: string[];
  issueTypes?: string[];
  creator?: User;
  admins?: User[];
  maintainers?: User[];
}
