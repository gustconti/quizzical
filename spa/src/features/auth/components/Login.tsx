import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { login } from '../authSlice';
import type { AppDispatch, RootState } from '@/app/store';
import type {} from '../authSlice';
import { useNavigate } from 'react-router-dom';

export default function Login() {
    const dispatch = useDispatch<AppDispatch>();
    const navigate = useNavigate();
    const { isLoading, error } = useSelector((state: RootState) => state.auth);

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        console.log('GC: Login', { email, password });

        try {
            const resultAction = await dispatch(login({ email, password }));

            if (login.fulfilled.match(resultAction)) {
                const { token, user } = resultAction.payload;

                // Persist to localStorage
                localStorage.setItem('auth_token', token);
                localStorage.setItem('auth_user', JSON.stringify(user));

                // Navigate to protected route
                navigate('/');
            } else {
                console.error('GC: Login Failed: ', resultAction.payload || resultAction.error);
            }
        } catch (error) {
            console.error('Unexpected error during login: ', error);
        }
    };

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100">
            <form
                onSubmit={handleSubmit}
                className="w-full max-w-sm p-6 bg-white rounded-lg shadow-md"
            >
                <h2 className="mb-6 text-2xl font-bold text-center text-gray-800">Login</h2>
                <div className="mb-4">
                    <label htmlFor="email" className="block mb-2 text-sm font-medium text-gray-700">
                        Email
                    </label>
                    <input
                        type="email"
                        id="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                        placeholder="Enter your email"
                        required
                    />
                </div>
                <div className="mb-6">
                    <label
                        htmlFor="password"
                        className="block mb-2 text-sm font-medium text-gray-700"
                    >
                        Password
                    </label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                        placeholder="Enter your password"
                        required
                    />
                </div>
                <button
                    type="submit"
                    className="w-full px-4 py-2 text-white bg-blue-500 rounded-lg hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500"
                >
                    {isLoading ? 'Logging in...' : 'Login'}
                </button>
                {error && <p className="text-red-500">Error: {error}</p>}
            </form>
        </div>
    );
}
