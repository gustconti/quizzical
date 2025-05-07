import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import * as authService from '../services/authService';
import type { User } from '@/types/User';
import type { LoginModel } from '@/features/auth/types/LoginModel';
import { AuthResponse } from '../types/AuthResponse';
import { AuthState } from '../types/AuthState';
import { RegisterModel } from '../types/RegisterModel';
import { removeToken } from '../utils/token';
import { RefreshTokenModel } from '../types/RefreshTokenModel';

export const login = createAsyncThunk<AuthResponse, LoginModel>(
    'auth/login', // Action type prefix
    async (credentials: LoginModel) => {
        const { data } = await authService.login(credentials); // Call the login service
        return data;
    },
);

export const register = createAsyncThunk<AuthResponse, RegisterModel>(
    'auth/register',
    async (registerData: RegisterModel) => {
        const { data } = await authService.register(registerData);
        return data;
    },
);
export const refresh = createAsyncThunk<AuthResponse, RefreshTokenModel>(
    'auth/refresh',
    async (refreshData: RefreshTokenModel) => {
        const { data } = await authService.refreshToken(refreshData); 
        return data;
    },
);

const initialState: AuthState = {
    token: null,
    user: null,
    expiresAt: 0,
    isLoading: false,
    error: '',
    guestName: 'GUEST',
};

// `createSlice` is used to define a Redux slice, which includes the state, reducers, and extra reducers.
const authSlice = createSlice({
    name: 'auth', // Name of the slice
    initialState, // Initial state for the slice
    reducers: {
        // Reducer for logging out the user
        logout: (state) => {
            console.log('GC: LOGOUT! state: ', state)
            state.token = null; // Clear the access token
            state.user = null; // Clear the user data
            state.expiresAt = 0; // Reset the expiration timestamp
            removeToken();
        },
        // Reducer storing the user
        setUser: (state, action: PayloadAction<{ token: string; user: User }>) => {
            state.token = action.payload.token;
            state.user = action.payload.user;
        },
    },
    extraReducers: (builder) => {
        // Handle fulfilled state of the `login` thunk
        builder
            .addCase(login.pending, (state) => {
                state.isLoading = true;
            })
            .addCase(login.fulfilled, (state, action) => {
                state.token = action.payload.token; // Set the access token
                state.user = action.payload.user; // Set the user data
                state.expiresAt = Date.now() + action.payload.expiresIn * 1000; // Calculate and set the token expiration timestamp
                state.isLoading = false;
            })
            .addCase(login.rejected, (state, action) => {
                state.error = action.error.message;
                state.isLoading = false;
            })
            // Handle fulfilled state of the `refresh` thunk
            .addCase(refresh.fulfilled, (state, action) => {
                state.token = action.payload.token; // Update the access token
                state.expiresAt = Date.now() + action.payload.expiresIn * 1000; // Update the token expiration timestamp
            });
    },
});

// Export the `logout` action for use in components
export const { logout, setUser } = authSlice.actions;

// Export the reducer to be included in the Redux store
export default authSlice.reducer;
