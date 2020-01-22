"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var auth_1 = require("@nebular/auth");
exports.apiRoute = "https://localhost:5001/api/v1";
exports.pageSizeConst = 50;
exports.myNbPasswordAuthStrategyOptions = {
    name: "email",
    baseEndpoint: "https://localhost:5001/api/v1/identity/",
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
        class: auth_1.NbAuthJWTToken,
        key: 'token',
        getter: function (module, res, options) { return auth_1.getDeepFromObject(res.body, options.token.key); },
    },
    errors: {
        key: 'data.errors',
        getter: function (module, res, options) { return auth_1.getDeepFromObject(res.error, options.errors.key, options[module].defaultErrors); },
    },
    messages: {
        key: 'data.messages',
        getter: function (module, res, options) { return auth_1.getDeepFromObject(res.body, options.messages.key, options[module].defaultMessages); },
    },
    validation: {
        password: {
            required: true,
            minLength: 6,
            maxLength: 32,
        },
        email: {
            required: true,
        },
        fullName: {
            required: true,
        },
    },
};
//# sourceMappingURL=constants.js.map