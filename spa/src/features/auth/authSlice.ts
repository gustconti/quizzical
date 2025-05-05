import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import * as authService from './services/authService';

export const login = createAsyncThunk('auth/login', async (credentials) => {
  const { data } = await authService.login(credentials);
  return data;
});

export const refresh = createAsyncThunk('auth/refresh', async () => {
  const { data } = await authService.refreshToken();
  return data;
});

const authSlice = createSlice({
  name: 'auth',
  initialState: {
    accessToken: null,
    user: null,
    expiresAt: 0,
  },
  reducers: {
    logout: (state) => {
      state.accessToken = null;
      state.user = null;
      state.expiresAt = 0;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(login.fulfilled, (state, action) => {
        state.accessToken = action.payload.accessToken;
        state.user = action.payload.user;
        state.expiresAt = Date.now() + action.payload.expiresIn * 1000;
      })
      .addCase(refresh.fulfilled, (state, action) => {
        state.accessToken = action.payload.accessToken;
        state.expiresAt = Date.now() + action.payload.expiresIn * 1000;
      });
  },
});

export const { logout } = authSlice.actions;
export default authSlice.reducer;