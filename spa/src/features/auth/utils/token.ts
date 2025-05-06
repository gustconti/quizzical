// utils/token.ts
import { STORAGE_KEYS } from './storageKeys';

export const isTokenExpired = (expiresAt: number): boolean => {
  return Date.now() > expiresAt;
};

export const persistAuth = (token: string, user: object) => {
  localStorage.setItem(STORAGE_KEYS.authToken, token);
  localStorage.setItem(STORAGE_KEYS.authUser, JSON.stringify(user));
};

export const getToken = () => {
  return localStorage.getItem(STORAGE_KEYS.authToken);
};

export const getUser = () => {
  return localStorage.getItem(STORAGE_KEYS.authUser);
};

export const removeToken = () => {
  localStorage.removeItem(STORAGE_KEYS.authToken);
  localStorage.removeItem(STORAGE_KEYS.authUser);
};
