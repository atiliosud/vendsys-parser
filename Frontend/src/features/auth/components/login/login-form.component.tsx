import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { Button, Input, Card } from '../../../../components/ui';
import { AppFooter } from '../../../../components/layout';
import { useAuth } from '../../hooks/use-auth.hook';
import type { LoginRequest } from '../../types';
import nayaxLogo from '../../../../assets/nayax-logo.png';
import './login-form.component.css';

interface LoginFormProps {
  onLogin: (username: string, password: string) => void;
}

export const LoginForm: React.FC<LoginFormProps> = ({ onLogin }) => {
  const [credentials, setCredentials] = useState<LoginRequest>({
    username: '',
    password: ''
  });

  const navigate = useNavigate();
  const { t } = useTranslation();
  const { login, loading, error, setError } = useAuth();

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setCredentials(prev => ({
      ...prev,
      [name]: value
    }));
    if (error) setError('');
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!credentials.username || !credentials.password) {
      setError(t('login.invalidCredentials'));
      return;
    }

    const success = await login(credentials);
    if (success) {
      onLogin(credentials.username, credentials.password);
      navigate('/upload');
    }
  };

  const customTitle = (
    <div className="login-title-container">
      <img src={nayaxLogo} alt="Nayax" className="login-logo" />
      <span className="login-title-text">{t('login.title')}</span>
    </div>
  );

  return (
    <div className="app-container-with-footer">
      <div className="page-container">
        <Card title={customTitle}>
          {error && (
            <div className="message error">
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit}>
            <Input
              label={t('login.username')}
              type="text"
              name="username"
              value={credentials.username}
              onChange={handleInputChange}
              disabled={loading}
              autoComplete="username"
            />

            <Input
              label={t('login.password')}
              type="password"
              name="password"
              value={credentials.password}
              onChange={handleInputChange}
              disabled={loading}
              autoComplete="current-password"
            />

            <Button
              type="submit"
              disabled={loading || !credentials.username || !credentials.password}
              loading={loading}
            >
              {t('login.button')}
            </Button>
          </form>
        </Card>
      </div>
      <AppFooter />
    </div>
  );
};