import { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { LoginForm, DexUploadForm } from './features';
import './styles/nayax.css';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [credentials, setCredentials] = useState<{ username: string; password: string }>({
    username: '',
    password: ''
  });

  const handleLogin = (username: string, password: string) => {
    setIsAuthenticated(true);
    setCredentials({ username, password });
  };

  const handleLogout = () => {
    setIsAuthenticated(false);
    setCredentials({ username: '', password: '' });
  };

  return (
    <div className="app-container">
      <Router>
        <Routes>
          <Route
            path="/login"
            element={
              isAuthenticated ?
                <Navigate to="/upload" replace /> :
                <LoginForm onLogin={handleLogin} />
            }
          />
          <Route
            path="/upload"
            element={
              isAuthenticated ?
                <DexUploadForm
                  username={credentials.username}
                  password={credentials.password}
                  onLogout={handleLogout}
                /> :
                <Navigate to="/login" replace />
            }
          />
          <Route path="/" element={<Navigate to="/login" replace />} />
        </Routes>
      </Router>
    </div>
  );
}

export default App;
