import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { Button, Card } from '../../../components/ui';
import { AppLayout } from '../../../components/layout';
import { useDexUpload } from '../hooks/use-dex-upload.hook';

interface DexUploadFormProps {
  username: string;
  password: string;
  onLogout: () => void;
}

export const DexUploadForm: React.FC<DexUploadFormProps> = ({ username, password, onLogout }) => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const navigate = useNavigate();
  const { t } = useTranslation();
  const { loading, uploadResult, uploadFile, clearResult } = useDexUpload();

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0] || null;
    setSelectedFile(file);
    if (uploadResult) {
      clearResult();
    }
  };

  const handleUpload = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!selectedFile) {
      return;
    }

    const result = await uploadFile(selectedFile, username, password);

    if (result.success) {
      setSelectedFile(null);
      const fileInput = document.getElementById('file-input') as HTMLInputElement;
      if (fileInput) fileInput.value = '';
    }
  };

  const handleLogout = () => {
    onLogout();
    navigate('/login');
  };

  return (
    <AppLayout showLogout onLogout={handleLogout}>
      <div className="page-container">
        <Card title={t('upload.title')}>
          {uploadResult && (
            <div className={`message ${uploadResult.success ? 'success' : 'error'}`}>
              {uploadResult.success ? t('upload.success') : uploadResult.message}
            </div>
          )}

          <form onSubmit={handleUpload}>
            <div className="form-group">
              <label htmlFor="file-input" className="form-label">
                {t('upload.selectFile')}
              </label>
              <div className="file-input-container">
                <input
                  type="file"
                  id="file-input"
                  className="file-input"
                  accept=".dex,.txt"
                  onChange={handleFileChange}
                  disabled={loading}
                />
                <label
                  htmlFor="file-input"
                  className={`file-input-label ${selectedFile ? 'has-file' : ''}`}
                >
                  {selectedFile ? selectedFile.name : t('upload.selectFile')}
                </label>
              </div>
            </div>

            <Button
              type="submit"
              disabled={loading || !selectedFile}
              loading={loading}
            >
              {t('upload.uploadButton')}
            </Button>
          </form>
        </Card>
      </div>
    </AppLayout>
  );
};