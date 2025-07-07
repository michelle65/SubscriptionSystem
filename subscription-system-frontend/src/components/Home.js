import React from 'react';
import { Link } from 'react-router-dom';

const Home = () => {
  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-8 text-center">
          <div className="card shadow">
            <div className="card-body">
              <h1 className="display-4 text-primary mb-4">Subscription System</h1>
              <p className="lead mb-4">
                Welcome to the Subscription System Platform. 
              </p>
              
              <div className="row mt-5">
                <div className="col-md-6">
                  <div className="card h-100">
                    <div className="card-body">
                      <h5 className="card-title">Administrator</h5>
                      <p className="card-text">
                        Register as an administrator to manage users and send invitations.
                      </p>
                      <Link to="/register" className="btn btn-primary">
                        Register as Admin
                      </Link>
                    </div>
                  </div>
                </div>
                
                <div className="col-md-6">
                  <div className="card h-100">
                    <div className="card-body">
                      <h5 className="card-title">Login</h5>
                      <p className="card-text">
                        Already have an account? Login to access your dashboard.
                      </p>
                      <Link to="/login" className="btn btn-success">
                        Login
                      </Link>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home; 