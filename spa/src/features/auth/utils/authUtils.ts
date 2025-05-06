import { AppDispatch, RootState } from '@/app/store';
import { setUser } from '../store/authSlice';
import { persistAuth } from '@/features/auth/utils/token';
import { AuthResponse } from '../types/AuthResponse';
import { NavigateFunction } from 'react-router-dom';

export const completeLogin = (
  { token, user }: AuthResponse,
  dispatch: AppDispatch,
  navigate: NavigateFunction
) => {
  persistAuth(token, user);
  dispatch(setUser({ token, user }));
  navigate('/');
};

export const selectIsLoggedIn = (state: RootState) => !!state.auth.token;
