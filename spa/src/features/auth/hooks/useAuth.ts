import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { refresh, setUser, clearAuthState } from '../authSlice';
import type { RootState, AppDispatch } from '@/app/store';

export default function useAuth() {
  const dispatch = useDispatch<AppDispatch>();
  const { expiresAt, token, user } = useSelector((state: RootState) => state.auth);

  // Check local storage for token on mount
  useEffect(() => {
    const savedToken = localStorage.getItem('auth_token');
    const savedUser = localStorage.getItem('auth_user');

    if (savedToken && savedUser)
      dispatch(setUser({ token: savedToken, user: JSON.parse(savedUser) })); // Set user from local storage
  }, [dispatch]);

  // Token refresh interval
  useEffect(() => {
    if (!token) return;
    const now = Date.now();
    const timeLeft = expiresAt - now;
    const timeToRepeatRefresh = 5 * 60 * 1000; // 5 minutes
    const minTimeToRefresh = 60 * 1000; // 1 minute

    if (timeLeft < minTimeToRefresh) {
      dispatch(refresh());
    }

    const timer = setInterval(
      () => {
        dispatch(refresh());
      },
      timeToRepeatRefresh, // Refresh token every 5 minutes
    );

    return () => clearInterval(timer);
  }, [dispatch, expiresAt, token]); // Run effect when expiresAt changes

  // Logout function (clears localStorage + Redux state)
  const logout = () => {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('auth_user');
    dispatch(clearAuthState());
  };

  return { token, user, logout };
}
