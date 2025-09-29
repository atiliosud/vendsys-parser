export const API_BASE_URL = 'http://localhost:5000/api';

export const createAuthHeader = (username: string, password: string): string => {
  return 'Basic ' + btoa(`${username}:${password}`);
};