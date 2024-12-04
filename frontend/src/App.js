import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Landing, Welcome } from './pages';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Landing />} />
        <Route path="/welcome" element={<Welcome />} />
      </Routes>
    </BrowserRouter>
  );
}
