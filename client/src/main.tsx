// src/main.tsx
import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import { BrowserRouter } from 'react-router-dom'
import { WalletProvider } from './contexts/WalletContext' // <== IMPORTANTE
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <BrowserRouter>
            <WalletProvider>
                <App />
            </WalletProvider>
        </BrowserRouter>
    </React.StrictMode>
)