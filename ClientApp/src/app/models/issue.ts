import { Project } from './project';
import { User } from './user';

export class Issue {
  title?: string;
  content?: string;
  number?: number;
  isPublic?: boolean;
  parentProject?: Project;
  author?: User;
  stages?: string[];
  types?: string[];
}
