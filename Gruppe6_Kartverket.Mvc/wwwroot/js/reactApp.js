import React from 'react';
import ReactDOM from 'react-dom';
import { Logo } from "@kvib/react";

// Render the Logo component into a specific DOM element
function App() {
    return (
        <Logo label="Logo" variant="horizontal" />
    );
}

// Export a function to render the React component
export function renderLogoComponent() {
    const logoContainer = document.getElementById('logoContainer');
    if (logoContainer) {
        const root = ReactDOM.createRoot(logoContainer);
        root.render(<App />);
    }
};
