import './App.css'
import { Link, Route, Routes } from 'react-router-dom'
import { AnimationTest } from './features/AnimationTest/AnimationTest';
import { QuizRoom } from './features/QuizRoom/QuizRoom';
import { Counter } from './features/Counter/Counter';

function App() {
    return (
    <>
      <nav className="p-4 space-x-4 bg-gray-200">
        <Link to="/">Home</Link>
        <Link to="/animation-test">Animation Test</Link>
        <Link to="/signalr-test">SignalR Test</Link>
      </nav>
      <Routes>
        <Route path="/" element={<Counter />} />
        <Route path="/animation-test" element={<AnimationTest />} />
        <Route path="/signalr-test" element={<QuizRoom />} />
      </Routes>
    </>
  )
}

export default App
