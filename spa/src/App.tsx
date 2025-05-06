import './App.css';
import { Link, Route, Routes } from 'react-router-dom';
import { AnimationTest } from './features/animationTest/AnimationTest';
import { QuizRoom } from './features/quizRoom/QuizRoom';
import AuthRoutes from './features/auth/routes/AuthRoutes';

export default function App() {
    return (
        <>
            <nav className="w-full flex gap-5 px-20">
                <Link to="/">Home</Link>
                <Link to="/animation-test">Animation Test</Link>
                <Link to="/signalr-test">SignalR Test</Link>
            </nav>
            <Routes>
                <Route path="/animation-tesdt" element={<AnimationTest />} />
                <Route path="/signalr-test" element={<QuizRoom />} />
                <Route path="/*" element={<AuthRoutes />} />
            </Routes>
        </>
    );
}
