"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.apiRoute = "https://localhost:5001/api/v1";
exports.pageSizeConst = 50;
exports.tokenGetter = function () {
    var storageItem = localStorage.getItem("auth_app_token");
    if (!storageItem) {
        return null;
    }
    return JSON.parse(storageItem)["value"];
};
//# sourceMappingURL=constants.js.map