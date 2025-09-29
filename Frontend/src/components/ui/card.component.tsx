import React from 'react';

interface CardProps {
  children: React.ReactNode;
  className?: string;
  title?: string | React.ReactNode;
}

export const Card: React.FC<CardProps> = ({ children, className = '', title }) => {
  return (
    <div className={`form-container ${className}`}>
      {title && (
        <h1 className="page-title">
          {title}
        </h1>
      )}
      {children}
    </div>
  );
};