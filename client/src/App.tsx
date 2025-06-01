import { Routes, Route } from 'react-router-dom'
import Home from './pages/home'
import ProtectedPage from './pages/protected'
import Navbar from './components/Navbar'
import PrivateRoute from './components/PrivateRoute'
import RegisterRecord from './pages/register'

export default function App() {
    return (
        <>
            <Navbar />
            <Routes>
                <Route path="/" element={<Home />} />

                {/* Rota protegida */}
                <Route element={<PrivateRoute />}>
                    <Route path="/protected" element={<ProtectedPage />} />
                    <Route path="/register" element={<RegisterRecord />} />
                </Route>
            </Routes>
        </>
    )
}