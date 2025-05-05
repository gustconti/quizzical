import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { refresh } from '../authSlice';

export default function useAuth() {
  const dispatch = useDispatch();
  const { accessToken, expiresAt } = useSelector(state => state.auth);

  useEffect(() => {
    const now = Date.now();
    const timeLeft = expiresAt - now;

    if (timeLeft < 60000) { // < 1min
      dispatch(refresh()); // uses refresh token behind the scenes
    }

    const timer = setInterval(() => {
      dispatch(refresh());
    }, 5 * 60 * 1000); // every 5 mins

    return () => clearInterval(timer);
  }, [dispatch, expiresAt]);
}