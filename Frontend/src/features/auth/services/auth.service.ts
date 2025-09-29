import type { LoginRequest } from '../types';

const API_BASE_URL = 'http://localhost:5000/api';

const createAuthHeader = (username: string, password: string): string => {
  return 'Basic ' + btoa(`${username}:${password}`);
};

const getCurrentLanguage = (): string => {
  return localStorage.getItem('i18nextLng') || 'en-US';
};

export const authService = {
  authenticate: async (credentials: LoginRequest): Promise<boolean> => {
    try {
      const response = await fetch(`${API_BASE_URL}/authenticate`, {
        method: 'POST',
        headers: {
          'Authorization': createAuthHeader(credentials.username, credentials.password),
          'Content-Type': 'application/json',
          'Accept-Language': getCurrentLanguage()
        }
      });

      return response.ok;
    } catch (error) {
      console.error('Authentication error:', error);
      return false;
    }
  }
};