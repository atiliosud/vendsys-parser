import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
// Using nayax.css from App.tsx
import './i18n'
import App from './app.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
