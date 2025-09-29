export interface AppState {
  isAuthenticated: boolean;
  username: string | null;
  password: string | null;
}

export interface BaseProps {
  className?: string;
  children?: React.ReactNode;
}