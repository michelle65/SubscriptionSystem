import React from 'react';
import { useAuth } from '../contexts/AuthContext';

const UserDashboard = () => {
  const { user, logout } = useAuth();

  const handleLogout = () => {
    logout();
  };

  return (
    <>
      <nav className="navbar navbar-expand-lg navbar-dark bg-success">
        <div className="container-fluid">
          <a className="navbar-brand" href="#!">Subscription System - User</a>
          <div className="navbar-nav ms-auto">
            <span className="navbar-text me-3">
              Welcome, {user?.email}
            </span>
            <button 
              className="btn btn-outline-light" 
              onClick={handleLogout}
            >
              Logout
            </button>
          </div>
        </div>
      </nav>

      <div className="container-fluid mt-5">
        <div className="row justify-content-center">
          <div className="col-md-8 text-center">
            <div className="card shadow">
              <div className="card-body">
                <h1 className="display-4 text-primary mb-4">Hello!</h1>
                <p className="lead">
                  Welcome to the Subscription System. You have limited access as a regular user.
                </p>
                <p className="text-muted">
                  Hello! How are you ?
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default UserDashboard; 