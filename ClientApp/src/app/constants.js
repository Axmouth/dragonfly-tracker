"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.apiRoute = "https://localhost:5001/api/v1";
exports.pageSizeConst = 50;
function tokenGetter() {
    var storageItem = localStorage.getItem("auth_app_token");
    if (!storageItem) {
        return null;
    }
    return JSON.parse(storageItem)["value"];
}
exports.tokenGetter = tokenGetter;
//# sourceMappingURL=constants.js.map