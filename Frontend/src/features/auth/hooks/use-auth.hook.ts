import { useState, useCallback } from 'react';
import { authService } from '../services/auth.service';
import type { LoginRequest, AuthState } from '../types';

export const useAuth = () => {
  const [authState, setAuthState] = useState<AuthState>({
    isAuthenticated: false,
    username: null,
    password: null
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');

  const login = useCallback(async (credentials: LoginRequest): Promise<boolean> => {
    setLoading(true);
    setError('');

    try {
      const isAuthenticated = await authService.authenticate(credentials);

      if (isAuthenticated) {
        setAuthState({
          isAuthenticated: true,
          username: credentials.username,
          password: credentials.password
        });
        return true;
      } else {
        setError('Invalid credentials');
        return false;
      }
    } catch {
      setError('Authentication error');
      return false;
    } finally {
      setLoading(false);
    }
  }, []);

  const logout = useCallback(() => {
    setAuthState({
      isAuthenticated: false,
      username: null,
      password: null
    });
    setError('');
  }, []);

  return {
    authState,
    loading,
    error,
    login,
    logout,
    setError
  };
};