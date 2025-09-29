import { useState, useCallback } from 'react';
import { dexService } from '../services/dex.service';
import type { UploadResponse } from '../types';

export const useDexUpload = () => {
  const [loading, setLoading] = useState(false);
  const [uploadResult, setUploadResult] = useState<UploadResponse | null>(null);

  const uploadFile = useCallback(async (
    file: File,
    username: string,
    password: string
  ): Promise<UploadResponse> => {
    setLoading(true);
    setUploadResult(null);

    try {
      const result = await dexService.uploadFile(file, username, password);
      setUploadResult(result);
      return result;
    } catch {
      const errorResult: UploadResponse = {
        message: 'Upload failed',
        success: false
      };
      setUploadResult(errorResult);
      return errorResult;
    } finally {
      setLoading(false);
    }
  }, []);

  const clearResult = useCallback(() => {
    setUploadResult(null);
  }, []);

  return {
    loading,
    uploadResult,
    uploadFile,
    clearResult
  };
};