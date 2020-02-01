import { NbPasswordStrategyModule, NbAuthStrategyOptions, NbPasswordStrategyMessage, NbPasswordStrategyToken, getDeepFromObject, NbAuthSimpleToken, NbPasswordStrategyReset, NbPasswordAuthStrategyOptions, NbAuthJWTToken } from "@nebular/auth";
import { HttpErrorResponse, HttpResponse } from "@angular/common/http";

export const apiRoute = "https://localhost:5001/api/v1";
export const pageSizeConst = 50;

export const myNbPasswordAuthStrategyOptions = {
  name: "email",
  baseEndpoint: `${apiRoute}/identity/`,
  login: {
    alwaysFail: false,
    endpoint: 'login',
    method: 'post',
    requireValidToken: true,
    redirect: {
      success: '/',
      failure: null,
    },
    defaultErrors: ['Login/Email combination is not correct, please try again.'],
    defaultMessages: ['You have been successfully logged in.'],
  },
  register: {
    alwaysFail: false,
    endpoint: 'register',
    method: 'post',
    requireValidToken: true,
    redirect: {
      success: '/',
      failure: null,
    },
    defaultErrors: ['Something went wrong, please try again.'],
    defaultMessages: ['You have been successfully registered.'],
  },
  requestPass: {
    endpoint: 'request-pass',
    method: 'post',
    redirect: {
      success: '/',
      failure: null,
    },
    defaultErrors: ['Something went wrong, please try again.'],
    defaultMessages: ['Reset password instructions have been sent to your email.'],
  },
  resetPass: {
    endpoint: 'reset-pass',
    method: 'put',
    redirect: {
      success: '/',
      failure: null,
    },
    resetPasswordTokenKey: 'reset_password_token',
    defaultErrors: ['Something went wrong, please try again.'],
    defaultMessages: ['Your password has been successfully changed.'],
  },
  logout: {
    alwaysFail: false,
    endpoint: 'logout',
    method: 'delete',
    redirect: {
      success: '/',
      failure: null,
    },
    defaultErrors: ['Something went wrong, please try again.'],
    defaultMessages: ['You have been successfully logged out.'],
  },
  refreshToken: {
    endpoint: 'refresh',
    method: 'post',
    requireValidToken: false,
    redirect: {
      success: null,
      failure: null,
    },
    defaultErrors: ['Something went wrong, please try again.'],
    defaultMessages: ['Your token has been successfully refreshed.'],
  },
  token: {
    class: NbAuthJWTToken,
    key: 'token',
    getter: (module: string, res: HttpResponse<Object>, options: NbPasswordAuthStrategyOptions) => getDeepFromObject(
      res.body,
      options.token.key,
    ),
  },
  errors: {
    key: 'data.errors',
    getter: (module: string, res: HttpErrorResponse, options: NbPasswordAuthStrategyOptions) => getDeepFromObject(
      res.error,
      options.errors.key,
      options[module].defaultErrors,
    ),
  },
  messages: {
    key: 'data.messages',
    getter: (module: string, res: HttpResponse<Object>, options: NbPasswordAuthStrategyOptions) => getDeepFromObject(
      res.body,
      options.messages.key,
      options[module].defaultMessages,
    ),
  },
  validation: {
    password: {
      required: true,
      minLength: 6,
      maxLength: 32,
      // regexp: ,
    },
    email: {
      required: true,
      // regexp: string | null;
    },
    fullName: {
      required: true,
      //minLength?: number | null;
      //maxLength?: number | null;
      //regexp?: string | null;
    },
  },
}

export const myRefreshNbPasswordAuthStrategyOptions = {
  name: "refreshToken",
  baseEndpoint: `${apiRoute}/identity/`,
  login: {
    alwaysFail: false,
    endpoint: 'login',
    method: 'post',
    requireValidToken: true,
    redirect: {
      success: '/',
      failure: null,
    },
    defaultErrors: ['Login/Email combination is not correct, please try again.'],
    defaultMessages: ['You have been successfully logged in.'],
  },
  register: {
    alwaysFail: false,
    endpoint: 'register',
    method: 'post',
    requireValidToken: true,
    redirect: {
      success: '/',
      failure: null,
    },
    defaultErrors: ['Something went wrong, please try again.'],
    defaultMessages: ['You have been successfully registered.'],
  },
  requestPass: {
    endpoint: 'request-pass',
    method: 'post',
    redirect: {
      success: '/',
      failure: null,
    },
    defaultErrors: ['Something went wrong, please try again.'],
    defaultMessages: ['Reset password instructions have been sent to your email.'],
  },
  resetPass: {
    endpoint: 'reset-pass',
    method: 'put',
    redirect: {
      success: '/',
      failure: null,
    },
    resetPasswordTokenKey: 'reset_password_token',
    defaultErrors: ['Something went wrong, please try again.'],
    defaultMessages: ['Your password has been successfully changed.'],
  },
  logout: {
    alwaysFail: false,
    endpoint: 'logout',
    method: 'delete',
    redirect: {
      success: '/',
      failure: null,
    },
    defaultErrors: ['Something went wrong, please try again.'],
    defaultMessages: ['You have been successfully logged out.'],
  },
  refreshToken: {
    endpoint: 'refresh',
    method: 'post',
    requireValidToken: false,
    redirect: {
      success: null,
      failure: null,
    },
    defaultErrors: ['Something went wrong, please try again.'],
    defaultMessages: ['Your token has been successfully refreshed.'],
  },
  token: {
    class: NbAuthJWTToken,
    key: 'token',
    getter: (module: string, res: HttpResponse<Object>, options: NbPasswordAuthStrategyOptions) => getDeepFromObject(
      res.body,
      options.token.key,
    ),
  },
  errors: {
    key: 'data.errors',
    getter: (module: string, res: HttpErrorResponse, options: NbPasswordAuthStrategyOptions) => getDeepFromObject(
      res.error,
      options.errors.key,
      options[module].defaultErrors,
    ),
  },
  messages: {
    key: 'data.messages',
    getter: (module: string, res: HttpResponse<Object>, options: NbPasswordAuthStrategyOptions) => getDeepFromObject(
      res.body,
      options.messages.key,
      options[module].defaultMessages,
    ),
  },
  validation: {
    password: {
      required: true,
      minLength: 6,
      maxLength: 32,
      // regexp: ,
    },
    email: {
      required: true,
      // regexp: string | null;
    },
    fullName: {
      required: true,
      //minLength?: number | null;
      //maxLength?: number | null;
      //regexp?: string | null;
    },
  },
}


export function tokenGetter() {
  const storageItem = localStorage.getItem("auth_app_token");
  if (!storageItem) {
    return null;
  }
  return JSON.parse(storageItem)["value"];
}

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
}

export interface IssuePost {
  content?: string;
}

export interface User {
  username?: string;
  email?: string;
}
