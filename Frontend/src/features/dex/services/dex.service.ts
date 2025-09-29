import type { UploadResponse, ApiError } from '../types';

const API_BASE_URL = 'http://localhost:5000/api';

const createAuthHeader = (username: string, password: string): string => {
  return 'Basic ' + btoa(`${username}:${password}`);
};

const getCurrentLanguage = (): string => {
  return localStorage.getItem('i18nextLng') || 'en-US';
};

export const dexService = {
  uploadFile: async (file: File, username: string, password: string): Promise<UploadResponse> => {
    try {
      const formData = new FormData();
      formData.append('file', file);

      const response = await fetch(`${API_BASE_URL}/dex`, {
        method: 'POST',
        headers: {
          'Authorization': createAuthHeader(username, password),
          'Accept-Language': getCurrentLanguage()
        },
        body: formData
      });

      if (response.ok) {
        const text = await response.text();
        return {
          message: text,
          success: true
        };
      } else {
        const errorText = await response.text();
        throw new Error(errorText);
      }
    } catch (error) {
      const apiError = error as ApiError;
      return {
        message: apiError.message || 'Error uploading file',
        success: false
      };
    }
  }
};