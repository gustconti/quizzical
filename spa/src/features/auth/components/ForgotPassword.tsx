import React, { useState } from 'react';

const ForgotPassword: React.FC = () => {
    const [email, setEmail] = useState('');
    const [isSubmitted, setIsSubmitted] = useState(false);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        // Handle forgot password logic here
        setIsSubmitted(true);
    };

    return (
        <div className="forgot-password">
            <h2>Forgot Password</h2>
            {isSubmitted ? (
                <p>Please check your email for password reset instructions.</p>
            ) : (
                <form onSubmit={handleSubmit}>
                    <div>
                        <label htmlFor="email">Email Address</label>
                        <input
                            type="email"
                            id="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                    </div>
                    <button type="submit">Reset Password</button>
                </form>
            )}
        </div>
    );
};

export default ForgotPassword;