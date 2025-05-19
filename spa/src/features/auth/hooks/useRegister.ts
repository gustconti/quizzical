import { useState } from 'react';
import { useDispatch } from 'react-redux';
import { register } from '../store/authSlice';
import type { AppDispatch } from '@/app/store';
import type { RegisterModel } from '../types/RegisterModel';
import { useNavigate } from 'react-router-dom';
import { completeLogin } from '../utils/authUtils';

export function useRegister() {
    const dispatch = useDispatch<AppDispatch>();
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        username: '',
        email: '',
        password: '',
        confirmPassword: '',
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        const { confirmPassword, ...registerData } = formData;
        
        if (registerData.password !== confirmPassword) {
            alert('Passwords do not match');
            return;
        }

        console.log("GC: ReigsterData: ", registerData)

        try {
            const resultAction = await dispatch(register(registerData as RegisterModel));

            if (register.fulfilled.match(resultAction)) {
                console.log('Registration successful:', resultAction.payload);
                completeLogin(resultAction.payload, dispatch, navigate);
            } else {
                console.error('Registration failed:', resultAction.payload || resultAction.error);
            }
        } catch (error) {
            console.error('Unexpected error during registration:', error);
        }
    };

    return { formData, handleChange, handleSubmit };
}
