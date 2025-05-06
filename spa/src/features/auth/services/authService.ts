import api from '@/app/api';
import { RegisterModel } from '../types/RegisterModel';
import { LoginModel } from '../types/LoginModel';

export const login = (credentials: LoginModel) =>
    api.post('/login', credentials);

export const register = (data: RegisterModel) =>
    api.post('/register', data);

export const refreshToken = () => api.post('/refresh');
