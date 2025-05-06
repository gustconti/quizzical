import { useRegister } from '../hooks/useRegister';

export default function Register() {
    const { formData, handleChange, handleSubmit } = useRegister();

    const fields = [
        { name: 'username', label: 'Username', type: 'text' },
        { name: 'email', label: 'Email', type: 'email' },
        { name: 'password', label: 'Password', type: 'password' },
        { name: 'confirmPassword', label: 'Confirm Password', type: 'password' },
    ] as const;

    return (
        <form
            onSubmit={handleSubmit}
            className="max-w-md mx-auto p-6 shadow-md rounded-md bg-gray-900"
        >
            <h2 className="text-2xl font-bold mb-4 text-center text-amber-300">Register</h2>

            {fields.map(({ name, label, type }) => (
                <div className="mb-4" key={name}>
                    <label htmlFor={name} className="block text-sm font-medium text-amber-200">
                        {label}
                    </label>
                    <input
                        type={type}
                        id={name}
                        name={name}
                        value={formData[name]}
                        onChange={handleChange}
                        className="mt-1 block w-full px-3 py-2 border border-gray-700 bg-gray-800 text-white rounded-md shadow-sm focus:ring-orange-500 focus:border-orange-500"
                        required
                    />
                </div>
            ))}

            <button
                type="submit"
                className="w-full bg-orange-500 text-white py-2 px-4 rounded-md hover:bg-orange-600 focus:outline-none focus:ring-2 focus:ring-orange-500 focus:ring-offset-2"
            >
                Register
            </button>
        </form>
    );
}
