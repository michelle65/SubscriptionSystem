import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { adminAPI } from '../services/api';
import InvitationsList from './InvitationsList';

const AdminDashboard = () => {
  const { user, logout } = useAuth();
  const [fiscalCode, setFiscalCode] = useState('');
  const [emails, setEmails] = useState('');
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');
  const [hasFiscalCode, setHasFiscalCode] = useState(false);
  const [dashboardData, setDashboardData] = useState(null);

  useEffect(() => {
    checkFiscalCodeStatus();
    loadDashboardData();
  }, []);

  const checkFiscalCodeStatus = async () => {
    try {
      const response = await adminAPI.getFiscalCodeStatus();
      setHasFiscalCode(response.hasFiscalCode);
    } catch (error) {
      console.error('Error checking fiscal code status:', error);
    }
  };

  const loadDashboardData = async () => {
    try {
      const data = await adminAPI.getDashboardData();
      setDashboardData(data);
    } catch (error) {
      console.error('Error loading dashboard data:', error);
    }
  };

  const handleFiscalCodeSubmit = async (e) => {
    e.preventDefault();
    if (!fiscalCode.trim() || fiscalCode.length !== 16) {
      setMessage('Fiscal code must be exactly 16 characters');
      return;
    }

    setLoading(true);
    try {
      await adminAPI.updateFiscalCode({ fiscalCode });
      setMessage('Fiscal code updated successfully!');
      setHasFiscalCode(true);
      setFiscalCode('');
    } catch (error) {
      setMessage(error.response?.data?.message || 'Failed to update fiscal code');
    } finally {
      setLoading(false);
    }
  };

  const handleSendInvitations = async (e) => {
    e.preventDefault();
    if (!emails.trim()) {
      setMessage('Please enter at least one email address');
      return;
    }

    const emailList = emails.split(',').map(email => email.trim()).filter(email => email);
    if (emailList.length === 0) {
      setMessage('Please enter valid email addresses');
      return;
    }

    setLoading(true);
    try {
      await adminAPI.sendInvitations({ emails: emailList });
      setMessage(`Invitations sent successfully to ${emailList.length} user(s)!`);
      setEmails('');
    } catch (error) {
      setMessage(error.response?.data?.message || 'Failed to send invitations');
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    logout();
  };

  return (
    <>
      <nav className="navbar navbar-expand-lg navbar-dark bg-primary">
        <div className="container-fluid">
          <a className="navbar-brand" href="#!">Subscription System - Admin</a>
          <div className="navbar-nav ms-auto">
            <span className="navbar-text me-3">
              Welcome, {user?.firstName} {user?.lastName}
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

      <div className="container-fluid mt-4">
        <div className="row">
          <div className="col-12">
            <h2>Admin Dashboard</h2>
            <p className="text-muted">Extended access for administrators</p>
          </div>
        </div>

        {message && (
          <div className={`alert ${message.includes('successfully') ? 'alert-success' : 'alert-danger'}`}>
            {message}
          </div>
        )}

        {!hasFiscalCode && (
          <div className="row mt-4">
            <div className="col-md-6">
              <div className="card">
                <div className="card-header">
                  <h5 className="mb-0">Enter Fiscal Code</h5>
                </div>
                <div className="card-body">
                  <p className="text-muted">Please enter your fiscal code to continue with admin functions.</p>
                  <form onSubmit={handleFiscalCodeSubmit}>
                    <div className="mb-3">
                      <label htmlFor="fiscalCode" className="form-label">Fiscal Code *</label>
                      <input
                        type="text"
                        className="form-control"
                        id="fiscalCode"
                        value={fiscalCode}
                        onChange={(e) => setFiscalCode(e.target.value)}
                        placeholder="Enter 16-character fiscal code"
                        maxLength={16}
                        required
                      />
                      <div className="form-text">Fiscal code must be exactly 16 characters</div>
                    </div>
                    <button
                      type="submit"
                      className="btn btn-primary"
                      disabled={loading}
                    >
                      {loading ? 'Updating...' : 'Update Fiscal Code'}
                    </button>
                  </form>
                </div>
              </div>
            </div>
          </div>
        )}

        {hasFiscalCode && (
          <div className="row mt-4">
            <div className="col-md-6">
              <div className="card">
                <div className="card-header">
                  <h5 className="mb-0">Send User Invitations</h5>
                </div>
                <div className="card-body">
                  <p className="text-muted">Send invitations to new users. Enter email addresses separated by commas.</p>
                  <form onSubmit={handleSendInvitations}>
                    <div className="mb-3">
                      <label htmlFor="emails" className="form-label">Email Addresses *</label>
                      <textarea
                        className="form-control"
                        id="emails"
                        rows="4"
                        value={emails}
                        onChange={(e) => setEmails(e.target.value)}
                        placeholder="test@proton.com, test@gmail.com"
                        required
                      />
                      <div className="form-text">Separate multiple email addresses with commas</div>
                    </div>
                    <button
                      type="submit"
                      className="btn btn-success"
                      disabled={loading}
                    >
                      {loading ? 'Sending...' : 'Send Invitations'}
                    </button>
                  </form>
                </div>
              </div>
            </div>
          </div>
        )}

        {hasFiscalCode && (
          <div className="row mt-4">
            <div className="col-12">
              <InvitationsList />
            </div>
          </div>
        )}

        <div className="row mt-4">
          <div className="col-md-3">
            <div className="card text-center">
              <div className="card-body">
                <h5 className="card-title text-primary">{dashboardData?.totalUsers || 0}</h5>
                <p className="card-text">Total Users</p>
              </div>
            </div>
          </div>
          <div className="col-md-3">
            <div className="card text-center">
              <div className="card-body">
                <h5 className="card-title text-success">{dashboardData?.regularUsers || 0}</h5>
                <p className="card-text">Regular Users</p>
              </div>
            </div>
          </div>
          <div className="col-md-3">
            <div className="card text-center">
              <div className="card-body">
                <h5 className="card-title text-warning">{dashboardData?.adminUsers || 0}</h5>
                <p className="card-text">Administrators</p>
              </div>
            </div>
          </div>
          <div className="col-md-3">
            <div className="card text-center">
              <div className="card-body">
                <h5 className="card-title text-info">{dashboardData?.confirmedUsers || 0}</h5>
                <p className="card-text">Confirmed Users</p>
              </div>
            </div>
          </div>
        </div>


      </div>
    </>
  );
};

export default AdminDashboard; 