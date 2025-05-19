import { Navigate, Outlet } from 'react-router-dom';
import useAuth from '../hooks/useAuth'; // Import your custom hook to check authentication

export default function PrivateRoute() {
  const { token } = useAuth(); // Use your custom hook to check if the user is authenticated

  // If there's a token, render the nested route. Otherwise, redirect.
  return token ? <Outlet /> : <Navigate to="/login" replace />;
};