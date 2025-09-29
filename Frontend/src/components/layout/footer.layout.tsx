import React from 'react';
import { LanguageSelector } from '../ui';

export const Footer: React.FC = () => {
  return (
    <footer className="app-footer">
      <LanguageSelector />
    </footer>
  );
};