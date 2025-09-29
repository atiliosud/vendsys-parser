# VendSys DEX Parser - Frontend

React + TypeScript frontend application for parsing DEX files from vending machines following Nayax/VendSys specifications.

**Developed by:** Atilio Camargo Moreira
**Contact:** atiliosud@gmail.com

## Technologies

- **React 18** with TypeScript
- **Vite** for development and build
- **React Router** for navigation
- **Nayax Style Guide** for UI components
- **CSS Custom Properties** for theming

## Project Structure

```
Frontend/
├── public/
│   └── nayax-light.svg          # Nayax logo
├── src/
│   ├── components/
│   │   ├── layout/
│   │   │   ├── app-header/
│   │   │   │   ├── app-header.component.tsx
│   │   │   │   ├── app-header.css
│   │   │   │   └── index.ts
│   │   │   ├── app-footer/
│   │   │   │   ├── app-footer.component.tsx
│   │   │   │   ├── app-footer.css
│   │   │   │   └── index.ts
│   │   │   ├── app-layout/
│   │   │   │   ├── app-layout.component.tsx
│   │   │   │   └── index.ts
│   │   │   └── index.ts
│   │   └── ui/
│   │       ├── button/
│   │       │   ├── button.component.tsx
│   │       │   ├── button.css
│   │       │   └── index.ts
│   │       ├── language-selector/
│   │       │   ├── language-selector.component.tsx
│   │       │   ├── language-selector.css
│   │       │   └── index.ts
│   │       └── index.ts
│   ├── features/
│   │   ├── auth/
│   │   │   ├── components/
│   │   │   │   ├── login-form.component.tsx
│   │   │   │   └── login-form.css
│   │   │   └── index.ts
│   │   ├── dex-upload/
│   │   │   ├── components/
│   │   │   │   ├── dex-upload-form.component.tsx
│   │   │   │   └── dex-upload-form.css
│   │   │   └── index.ts
│   │   └── index.ts
│   ├── styles/
│   │   └── nayax.css             # Main stylesheet with Nayax Style Guide
│   ├── app.tsx                   # Main application component
│   ├── main.tsx                  # Application entry point
│   └── vite-env.d.ts
├── package.json
├── tsconfig.json
├── vite.config.ts
└── README.md
```

## Features

### Authentication

- Login form with username/password
- HTTP Basic Authentication integration
- Session management with React state

### DEX File Upload

- File upload component with drag-and-drop support
- File validation and preview
- Integration with backend API for DEX parsing
- Success/error message handling

### Layout Components

- **AppHeader**: Nayax branding with logout functionality
- **AppFooter**: Language selector with international support
- **AppLayout**: Consistent page structure

### UI Components

- **Button**: Primary and secondary variants following Nayax Style Guide
- **LanguageSelector**: Dropdown with scrollable country list and drag support

## Styling Architecture

### Nayax Style Guide Implementation

The application follows the official Nayax Style Guide with:

```css
/* Color Palette */
--nayax-taxi-yellow: #FFCD00
--nayax-dark: #262626
--nayax-white: #FFFFFF
--nayax-primary: var(--nayax-taxi-yellow)
--nayax-text-primary: var(--nayax-dark)
--nayax-text-secondary: #666666
--nayax-border: #E0E0E0
--nayax-error: #FF4444
--nayax-success: #00C851
```

### Typography

- **Primary Font**: Hurme Geometric Sans 3
- **Fallback Stack**: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto'

### Component Organization

- Each component has its own folder with `.tsx`, `.css`, and `index.ts`
- Component-specific styles are modular and scoped
- Global styles and variables in `nayax.css`
- CSS Custom Properties for consistent theming

## Development

### Prerequisites

- Node.js 18+
- npm or yarn

### Installation

```bash
cd Frontend
npm install
```

### Development Server

```bash
npm run dev
# Application runs on http://localhost:5173
```

### Build

```bash
npm run build
# Output in dist/ folder
```

### Preview Production Build

```bash
npm run preview
```

## API Integration

### Endpoints

- `POST /authenticate` - User authentication
- `POST /dex` - DEX file upload and parsing

### Authentication Flow

1. User enters credentials in login form
2. Frontend sends HTTP Basic Auth request to `/authenticate`
3. On success, user is redirected to upload page
4. Credentials are stored in React state for subsequent requests

### DEX Upload Flow

1. User selects DEX file through upload component
2. File is validated client-side (file type, size)
3. File is sent to `/dex` endpoint with stored credentials
4. Parse results are displayed to user

## Configuration

### Environment Variables

Create `.env` file in Frontend directory:

```env
VITE_API_BASE_URL=https://localhost:7297
```

### Vite Configuration

```typescript
// vite.config.ts
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': 'https://localhost:7297'
    }
  }
})
```

## Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## Performance Features

- **Vite HMR**: Hot Module Replacement for fast development
- **Tree Shaking**: Optimized bundle size
- **Code Splitting**: Route-based lazy loading ready
- **CSS Optimization**: Minimal CSS with custom properties

## Accessibility

- Semantic HTML structure
- ARIA labels for interactive elements
- Keyboard navigation support
- High contrast color ratios following WCAG guidelines

## Internationalization Ready

- Language selector component implemented
- Structure prepared for i18n integration
- Multiple language support in dropdown

## Testing Strategy

- Component testing with React Testing Library (when implemented)
- E2E testing with Playwright (when implemented)
- Visual regression testing for Nayax Style Guide compliance

## Deployment

- Build artifacts in `dist/` folder
- Static hosting compatible (Netlify, Vercel, S3)
- Docker container ready
- CDN optimization supported

## Development Guidelines

### Code Style

- TypeScript strict mode enabled
- ESLint configuration for React
- Prettier for code formatting
- Functional components with hooks

### Component Patterns

- Feature-based folder structure
- Barrel exports with index.ts files
- Props interface definitions
- CSS Modules for component styling

### State Management

- React useState for simple state
- Context API for global state (if needed)
- No external state management library (keeps it simple)

## Troubleshooting

### Common Issues

**CORS Errors**

- Ensure backend CORS is configured for frontend URL
- Check `VITE_API_BASE_URL` environment variable

**Authentication Failed**

- Verify backend is running on correct port
- Check network tab for 401/403 responses
- Confirm credentials match backend test data

**Styling Issues**

- Ensure `nayax.css` is imported in `app.tsx`
- Check CSS custom properties are defined
- Verify component CSS files are imported correctly

**Build Errors**

- Run `npm install` to ensure dependencies
- Check TypeScript errors with `npm run type-check`
- Verify all imports have correct file extensions

## Contributing

1. Follow existing component structure
2. Use TypeScript for type safety
3. Maintain Nayax Style Guide compliance
4. Test on multiple browsers
5. Update README for new features

---

**Built following Nayax Style Guide specifications**
