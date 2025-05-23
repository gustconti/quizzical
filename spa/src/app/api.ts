// src/app/api.ts
import { getToken } from '@/features/auth/utils/token';
import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5000/',
});

api.interceptors.request.use((config) => {
    const token = getToken();
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export default api;
