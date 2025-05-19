import { useState } from 'react';
import { useDispatch } from 'react-redux';
import { login } from '../store/authSlice';
import type { AppDispatch } from '@/app/store';
import { useNavigate } from 'react-router-dom';
import { completeLogin } from '../utils/authUtils';

export function useLogin() {
    const dispatch = useDispatch<AppDispatch>();
    const navigate = useNavigate();

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const resultAction = await dispatch(login({ email, password, remeberMe: false }));
            console.log('GC: ', resultAction);
            if (login.fulfilled.match(resultAction)) {
                completeLogin(resultAction.payload, dispatch, navigate);
            } else {
                console.error('Login failed:', resultAction.payload || resultAction.error);
            }
        } catch (error) {
            setError('Unexpected error occurred during login');
            console.error({ error }, error);
        } finally {
            setIsLoading(false);
        }
    };

    return {
        email,
        setEmail,
        password,
        setPassword,
        isLoading,
        error,
        handleSubmit,
    };
}
