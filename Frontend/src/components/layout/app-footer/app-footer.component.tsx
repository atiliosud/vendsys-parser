import React from 'react';
import { LanguageSelector } from '../ui';
import './app-footer.component.css';

export const AppFooter: React.FC = () => {
  return (
    <footer className="app-footer">
      <LanguageSelector />
    </footer>
  );
};