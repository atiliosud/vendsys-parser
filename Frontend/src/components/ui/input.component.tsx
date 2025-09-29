import React from 'react';

interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
  helperText?: string;
}

export const Input: React.FC<InputProps> = ({
  label,
  error,
  helperText,
  className = '',
  id,
  ...props
}) => {
  const inputId = id || `input-${Math.random().toString(36).substr(2, 9)}`;
  const inputClass = `form-input ${error ? 'error' : ''} ${className}`;

  return (
    <div className="form-group">
      {label && (
        <label htmlFor={inputId} className="form-label">
          {label}
        </label>
      )}
      <input
        id={inputId}
        className={inputClass}
        {...props}
      />
      {error && (
        <div className="message error">
          {error}
        </div>
      )}
      {helperText && !error && (
        <p className="text-muted">
          {helperText}
        </p>
      )}
    </div>
  );
};