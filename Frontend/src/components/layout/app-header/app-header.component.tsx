import React from 'react';
import { useTranslation } from 'react-i18next';
import { Button } from '../../ui';
import nayaxLightLogo from '../../../assets/nayax-light-logo.png';
import './app-header.component.css';

interface AppHeaderProps {
  showLogout?: boolean;
  onLogout?: () => void;
}

export const AppHeader: React.FC<AppHeaderProps> = ({ showLogout = false, onLogout }) => {
  const { t } = useTranslation();

  return (
    <header className="app-header">
      <div className="header-brand-container">
        <img src={nayaxLightLogo} alt="Nayax" className="header-logo" />
        <div className="header-text-container">
          <div className="app-logo">VendSys Parser</div>
          <div className="app-tagline">Simplify Payments. Maximize Loyalty.</div>
        </div>
      </div>
      {showLogout && onLogout && (
        <Button
          variant="secondary"
          onClick={onLogout}
          style={{ width: 'auto', padding: '0.5rem 1rem', fontSize: '0.875rem' }}
        >
          {t('upload.logout')}
        </Button>
      )}
    </header>
  );
};