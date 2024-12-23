import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Landing, Welcome, Home, Train } from './pages';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Landing />} />
        <Route path="/welcome" element={<Welcome />} />
        <Route path="/home" element={<Home />} />
        <Route path="/train/:sessionId" element={<Train />} />
      </Routes>
    </BrowserRouter>
  );
}
