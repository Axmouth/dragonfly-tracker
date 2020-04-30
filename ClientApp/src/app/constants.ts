export const pageSizeConst = 50;

export const tokenGetter = () => {
  const storageItem = localStorage.getItem('auth_app_token');
  if (!storageItem) {
    return null;
  }
  return JSON.parse(storageItem)['value'];
};
