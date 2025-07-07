import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';
import Home from './components/Home';
import AdminRegistration from './components/AdminRegistration';
import Login from './components/Login';
import AdminDashboard from './components/AdminDashboard';
import UserDashboard from './components/UserDashboard';
import InvitationConfirmation from './components/InvitationConfirmation';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import './App.css';

function App() {
  return (
    <AuthProvider>
      <Router>
        <div className="App">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/register" element={<AdminRegistration />} />
            <Route path="/login" element={<Login />} />
            <Route path="/invite/confirm" element={<InvitationConfirmation />} />
            
            <Route 
              path="/admin/dashboard" 
              element={
                <ProtectedRoute requiredRole="Admin">
                  <AdminDashboard />
                </ProtectedRoute>
              } 
            />
            <Route 
              path="/user/dashboard" 
              element={
                <ProtectedRoute requiredRole="User">
                  <UserDashboard />
                </ProtectedRoute>
              } 
            />
            
            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </div>
      </Router>
    </AuthProvider>
  );
}

export default App;
