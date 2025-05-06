import Button from '@/components/Button';
import { useLogin } from '../hooks/useLogin';

export default function Login() {
    const { email, setEmail, password, setPassword, isLoading, error, handleSubmit } = useLogin();

    return (
        <div className="flex items-center justify-center min-h-screen">
            <form
                onSubmit={handleSubmit}
                className="w-full max-w-sm p-6 bg-gray-900 rounded-lg shadow-md"
            >
                <h2 className="mb-6 text-2xl font-bold text-center text-gray-200">Login</h2>
                <div className="mb-4">
                    <label htmlFor="email" className="block mb-2 text-sm font-medium text-gray-200">
                        Email
                    </label>
                    <input
                        type="email"
                        id="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-yellow-500"
                        placeholder="Enter your email"
                        required
                    />
                </div>
                <div className="mb-6">
                    <label
                        htmlFor="password"
                        className="block mb-2 text-sm font-medium text-gray-200"
                    >
                        Password
                    </label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-yellow-500"
                        placeholder="Enter your password"
                        required
                    />
                </div>
                <Button
                    type="submit"
                    className="w-full"
                    label={isLoading ? 'Logging in...' : 'Login'}
                />
                {error && <p className="text-red-500 mt-4 text-center">Error: {error}</p>}
            </form>
        </div>
    );
}
