import './App.css';
import { Link, Route, Routes, useNavigate } from 'react-router-dom';
import { AnimationTest } from './features/animationTest/AnimationTest';
import { QuizRoom } from './features/quizRoom/QuizRoom';
import AuthRoutes from './features/auth/routes/AuthRoutes';
import Button from './components/Button';
import { logout } from './features/auth/store/authSlice';
import { useDispatch, useSelector } from 'react-redux';
import { selectIsLoggedIn } from './features/auth/utils/authUtils';
import Counter from './features/examples/Counter';
import { NameLengthBad } from './features/examples/DerivedState';
import { MouseTracker } from './features/examples/MouseTracker';
import { Child } from './features/examples/propsDemo';


export default function App() {
    const dispatch = useDispatch();
    const isLoggedIn = useSelector(selectIsLoggedIn);
    const navigate = useNavigate();
    return (
        <>
            <nav className="w-full flex items-center gap-5 px-20">
                <Link to="/">Home</Link>
                <Link to="/animation-test">Animation Test</Link>
                <Link to="/signalr-test">SignalR Test</Link>
                <Button label={isLoggedIn ? 'logout' : 'login'} onClick={isLoggedIn ? () => dispatch(logout()) : () => navigate('/login')} />
            </nav>
            <Counter />
            <NameLengthBad />
            <MouseTracker />
            <Child value={12} />
            <Routes>
                <Route path="/animation-test" element={<AnimationTest />} />
                <Route path="/signalr-test" element={<QuizRoom />} />
                <Route path="/*" element={<AuthRoutes />} />
            </Routes>
        </>
    );
}
