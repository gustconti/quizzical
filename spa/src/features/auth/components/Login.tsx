import Button from '@/components/Button';
import { useLogin } from '../hooks/useLogin';

export default function Login() {
    const { email, setEmail, password, setPassword, isLoading, error, handleSubmit } = useLogin();

    const fields = [
        { name: 'email', label: 'Email', type: 'email' },
        { name: 'password', label: 'Password', type: 'password' },
    ] as const;

    return (
        <div className="flex items-center justify-center min-h-screen">
            <form
                onSubmit={handleSubmit}
                className="w-full max-w-sm p-6 bg-gray-900 rounded-lg shadow-md"
            >
                <h2 className="mb-6 text-2xl font-bold text-center text-gray-200">Quizzical</h2>

                {fields.map(({ name, label, type }) => (
                    <div className="mb-4" key={name}>
                        <label
                            htmlFor={name}
                            className="block mb-2 text-left text-sm font-medium text-gray-200"
                        >
                            {label}
                        </label>
                        <input
                            type={type}
                            id={name}
                            name={name}
                            value={name === 'email' ? email : password}
                            onChange={(e) =>
                                name === 'email'
                                    ? setEmail(e.target.value)
                                    : setPassword(e.target.value)
                            }
                            className="w-full px-3 py-2 border border-gray-700 bg-gray-800 text-white rounded-md shadow-sm focus:ring-orange-500 focus:border-orange-500"
                            required
                        />
                    </div>
                ))}

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
