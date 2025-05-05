import api from '@/app/api';

export const login = (credentials: { email: string; password: string }) =>
  api.post('/login', credentials);

export const register = (data: {
  email: string;
  password: string;
  confirmPassword: string;
}) =>
  api.post('/register', data);

export const refreshToken = () => api.post('/refresh');
