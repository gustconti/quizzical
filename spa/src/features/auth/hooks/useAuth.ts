import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { refresh, setUser } from '../store/authSlice';
import type { RootState, AppDispatch } from '@/app/store';
import { getToken, getUser } from '../utils/token';

export default function useAuth() {
  const dispatch = useDispatch<AppDispatch>();
  const { expiresAt, token, user } = useSelector((state: RootState) => state.auth);

  // Check local storage for token and user info on mount
  useEffect(() => {
    const savedToken = getToken();
    const savedUser = getUser();

    if (savedToken && savedUser) {
      dispatch(setUser({ token: savedToken, user: JSON.parse(savedUser) }));
    }
  }, [dispatch]);

  // Handle token refresh
  useEffect(() => {
    if (!token) return; // If no token, do not refresh

    const now = Date.now();
    const timeLeft = expiresAt - now;
    const timeToRepeatRefresh = 5 * 60 * 1000; // Refresh every 5 minutes
    const minTimeToRefresh = 60 * 1000; // Minimum 1 minute before expiry

    if (timeLeft < minTimeToRefresh) {
      dispatch(refresh()); // Refresh token if near expiry
    }

    const timer = setInterval(() => {
      dispatch(refresh()); // Refresh token periodically
    }, timeToRepeatRefresh);

    return () => clearInterval(timer); // Clean up interval on unmount
  }, [dispatch, expiresAt, token]);

  return { token, user }; // Provide current auth state
}
