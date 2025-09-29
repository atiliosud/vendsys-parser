export interface LoginRequest {
  username: string;
  password: string;
}

export interface AuthState {
  isAuthenticated: boolean;
  username: string | null;
  password: string | null;
}