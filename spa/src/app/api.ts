// src/app/api.ts
import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/api', // or whatever your API base URL is
  withCredentials: true, // if you're using cookies for auth
});

export default api;