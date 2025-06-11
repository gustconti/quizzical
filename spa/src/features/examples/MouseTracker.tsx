import { useEffect, useState } from 'react';

export function MouseTracker() {
    const [pos, setPos] = useState({x: 0, y: 0});

    useEffect(() => {
        const handleMove = (e: MouseEvent) => {
            setPos({x: e.clientX, y: e.clientY});
        }
        window.addEventListener("mousemove", handleMove);
        return () => window.removeEventListener("mousemove", handleMove);

    }, []);

    return (
        <p>Mouse is at: {pos.x}, {pos.y}</p>
    );
}
