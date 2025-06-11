import { useState } from 'react';

export default function Counter() {
    const [count, setCount] = useState(0);

    const increment = () => setCount((prev) => prev + 1);

    return (
        <div>
            {count}
            <button onClick={increment}>+1</button>
        </div>
    );
}
