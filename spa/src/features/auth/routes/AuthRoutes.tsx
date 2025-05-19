import { Route, Routes } from 'react-router-dom';
import Login from '../components/Login';
import Register from '../components/Register';
import ForgotPassword from '../components/ForgotPassword';
import PrivateRoute from '../components/PrivateRoute';
import UserProfile from '../components/UserProfile';

export default function AuthRoutes() {
    return (
        <Routes>
            <Route path="login" element={<Login />} />
            <Route path="register" element={<Register />} />
            <Route path="forgot-password" element={<ForgotPassword />} />
            <Route element={<PrivateRoute />}>
                <Route path="profile" element={<UserProfile />} />
            </Route>
        </Routes>
    );
}
