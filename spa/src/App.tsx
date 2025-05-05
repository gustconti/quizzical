import './App.css'
import { Link, Route, Routes } from 'react-router-dom'
import { AnimationTest } from './features/animationTest/AnimationTest';
import { QuizRoom } from './features/quizRoom/QuizRoom';
import Login from './features/auth/components/Login';
import Register from './features/auth/components/Register';
import UserProfile from './features/auth/components/UserProfile';
import PrivateRoute from './features/auth/components/PrivateRoute';

function App() {
    return (
    <>
      <nav className="p-4 space-x-4 bg-gray-200 d-flex gap-3">
        <Link to="/">Home</Link>
        <Link to="/animation-test">Animation Test</Link>
        <Link to="/signalr-test">SignalR Test</Link>
      </nav>
      <Routes>
        <Route path="/animation-test" element={<AnimationTest />} />
        <Route path="/signalr-test" element={<QuizRoom />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/signalr-test" element={<QuizRoom />} />
        <Route element={<PrivateRoute />}>
            <Route path='/profile' element={<UserProfile />} />
        </Route>
      </Routes>
    </>
  )
}

export default App
