import React from 'react';
import { AppHeader } from './app-header.component';
import { AppFooter } from './app-footer.component';

interface AppLayoutProps {
  children: React.ReactNode;
  showLogout?: boolean;
  onLogout?: () => void;
}

export const AppLayout: React.FC<AppLayoutProps> = ({
  children,
  showLogout = false,
  onLogout
}) => {
  return (
    <div className="app-container-with-footer">
      <AppHeader showLogout={showLogout} onLogout={onLogout} />
      {children}
      <AppFooter />
    </div>
  );
};