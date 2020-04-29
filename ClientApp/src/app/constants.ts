export const apiRoute = 'https://localhost:5001/api/v1';
// export const apiRoute = 'https://api.dragonflytracker.com:5001/api/v1';

export const pageSizeConst = 50;

export const tokenGetter = () => {
  const storageItem = localStorage.getItem('auth_app_token');
  if (!storageItem) {
    return null;
  }
  return JSON.parse(storageItem)['value'];
};

export interface Project {
  name?: string;
  description?: string;
  isPublic?: boolean;
  issueStages?: string[];
  issueTypes?: string[];
  creator?: User;
  admins?: User[];
  maintainers?: User[];
}

export interface Issue {
  title?: string;
  content?: string;
  number?: number;
  isPublic?: boolean;
  parentProject?: Project;
  author?: User;
  stages?: string[];
  types?: string[];
}

export interface IssuePost {
  content?: string;
}

export interface User {
  username?: string;
  email?: string;
}
