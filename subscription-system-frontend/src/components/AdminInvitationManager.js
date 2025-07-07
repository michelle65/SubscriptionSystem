import React, { useState } from 'react';
import { adminAPI } from '../services/api';

const AdminInvitationManager = () => {
  const [emails, setEmails] = useState('');
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage('');

    if (!emails.trim()) {
      setMessage('Please enter at least one email address.');
      return;
    }

    const emailList = emails.split(',').map(email => email.trim()).filter(email => email);
    
    if (emailList.length === 0) {
      setMessage('Please enter valid email addresses.');
      return;
    }

    setLoading(true);
    try {
      await adminAPI.sendInvitations({ emails: emailList });
      setMessage('Invitations sent successfully!');
      setEmails('');
    } catch (error) {
      setMessage(error.response?.data?.message || 'Failed to send invitations. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card shadow">
      <div className="card-header bg-primary text-white">
        <h5 className="mb-0">Send User Invitations</h5>
      </div>
      <div className="card-body">
        {message && (
          <div className={`alert ${message.includes('successfully') ? 'alert-success' : 'alert-danger'}`}>
            {message}
          </div>
        )}
        
        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label htmlFor="emails" className="form-label">Email Addresses *</label>
            <textarea
              className="form-control"
              id="emails"
              rows="4"
              value={emails}
              onChange={(e) => setEmails(e.target.value)}
              placeholder="Enter email addresses separated by commas (e.g., test@proton.com, test@gmail.com)"
            />
            <div className="form-text">
              Enter multiple email addresses separated by commas. Each user will receive an invitation email.
            </div>
          </div>

          <div className="d-grid">
            <button
              type="submit"
              className="btn btn-primary"
              disabled={loading}
            >
              {loading ? (
                <>
                  <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                  Sending Invitations...
                </>
              ) : (
                'Send Invitations'
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AdminInvitationManager; 