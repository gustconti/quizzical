type ButtonProps = {
    label: string;
    onClick?: () => void;
    href?: string;
    className?: string;
    type?: 'submit' | 'button' | 'reset';
};

export default function Button({
    label,
    onClick,
    href,
    className = '',
    type = 'button',
}: ButtonProps) {
    return (
        <button
            type={type}
            className={`cursor-pointer px-4 py-2 rounded-md font-semibold text-white bg-gradient-to-r from-orange-500 to-yellow-500 hover:from-orange-600 hover:to-yellow-600 focus:outline-none focus:ring-2 focus:ring-orange-400 focus:ring-offset-2 ${className}`}
            onClick={onClick}
        >
            {href ? (
                <a href={href} className="block w-full text-center">
                    {label}
                </a>
            ) : (
                label
            )}
        </button>
    );
}
