import { configureStore } from '@reduxjs/toolkit';
import authReducer from '@/features/auth/store/authSlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    // other slices can go here
  },
});

// ✅ Use this in your components
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;