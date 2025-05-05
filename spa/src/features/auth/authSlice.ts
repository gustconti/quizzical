import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import * as authService from './services/authService';
import type { User } from '@/types/User';
import type { LoginCredentials } from '@/types/LoginCredentials';

type AuthResponse = {
  token: string;
  user: User;
  expiresIn: number;
};

type AuthState = {
  token: string | null;
  user: User | null;
  expiresAt: number;
  isLoading: boolean;
  error: string | undefined;
};

// `createAsyncThunk` is used to define asynchronous actions.
// `login` thunk handles the login process by calling the `authService.login` API.
export const login = createAsyncThunk<AuthResponse, LoginCredentials>(
  'auth/login', // Action type prefix
  async (credentials) => {
    const { data } = await authService.login(credentials); // Call the login service
    return data; // Return the response data as the payload
  },
);

// Another `createAsyncThunk` for refreshing the access token.
// This handles the token refresh process by calling the `authService.refreshToken` API.
export const refresh = createAsyncThunk<Pick<AuthResponse, 'token' | 'expiresIn'>>(
  'auth/refresh',
  async () => {
    const { data } = await authService.refreshToken(); // Call the refresh token service
    return data; // Return the response data as the payload
  },
);

// Initial state for the authentication slice
const initialState: AuthState = {
  token: null, // No token initially
  user: null, // No user initially
  expiresAt: 0, // Token expiration timestamp is 0 initially
  isLoading: false,
  error: "",
};

// `createSlice` is used to define a Redux slice, which includes the state, reducers, and extra reducers.
const authSlice = createSlice({
  name: 'auth', // Name of the slice
  initialState, // Initial state for the slice
  reducers: {
    // Reducer for logging out the user
    logout: (state) => {
      state.token = null; // Clear the access token
      state.user = null; // Clear the user data
      state.expiresAt = 0; // Reset the expiration timestamp
    },
    // Reducer storing the user
    setUser: (state, action: PayloadAction<{token: string, user: User}>) => {
      state.token = action.payload.token;
      state.user = action.payload.user;
    },
    // Reducer to clear the authentication state
    clearAuthState: (state) => {
      state.token = null;
      state.user = null;
      state.expiresAt = 0;
    }

  },
  extraReducers: (builder) => {
    // Handle fulfilled state of the `login` thunk
    builder
    .addCase(login.pending, (state) => {
      state.isLoading = true;
    }).addCase(login.fulfilled, (state, action) => {
      state.token = action.payload.token; // Set the access token
      state.user = action.payload.user; // Set the user data
      state.expiresAt = Date.now() + action.payload.expiresIn * 1000; // Calculate and set the token expiration timestamp
    })
    .addCase(login.rejected, (state, action) => {
      state.error = action.error.message;
    })
    // Handle fulfilled state of the `refresh` thunk
    .addCase(refresh.fulfilled, (state, action) => {
      state.token = action.payload.token; // Update the access token
      state.expiresAt = Date.now() + action.payload.expiresIn * 1000; // Update the token expiration timestamp
    });
  },
});

// Export the `logout` action for use in components
export const { logout, setUser, clearAuthState } = authSlice.actions;

// Export the reducer to be included in the Redux store
export default authSlice.reducer;
