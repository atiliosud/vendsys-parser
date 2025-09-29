export interface UploadResponse {
  message: string;
  success: boolean;
}

export interface ApiError {
  message: string;
  status: number;
}