import React, { useState, useEffect } from 'react';
import { adminAPI } from '../services/api';

const InvitationsList = () => {
  const [invitations, setInvitations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadInvitations();
  }, []);

  const loadInvitations = async () => {
    try {
      setLoading(true);
      const data = await adminAPI.getInvitations();
      setInvitations(data);
      setError(null);
    } catch (err) {
      setError('Failed to load invitations: ' + (err.response?.data?.message || err.message));
    } finally {
      setLoading(false);
    }
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleString();
  };

  const getStatusBadge = (isUsed) => {
    return isUsed ? (
      <span className="badge bg-success">Used</span>
    ) : (
      <span className="badge bg-warning">Pending</span>
    );
  };

  if (loading) {
    return (
      <div className="d-flex justify-content-center">
        <div className="spinner-border" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="alert alert-danger" role="alert">
        {error}
        <button className="btn btn-outline-danger ms-3" onClick={loadInvitations}>
          Retry
        </button>
      </div>
    );
  }

  return (
    <div className="card">
      <div className="card-header d-flex justify-content-between align-items-center">
        <h5 className="mb-0">Sent Invitations</h5>
        <button className="btn btn-outline-primary btn-sm" onClick={loadInvitations}>
          <i className="bi bi-arrow-clockwise"></i> Refresh
        </button>
      </div>
      <div className="card-body">
        {invitations.length === 0 ? (
          <div className="text-center text-muted py-4">
            <i className="bi bi-envelope-x fs-1"></i>
            <p className="mt-2">No invitations sent yet</p>
          </div>
        ) : (
          <div className="table-responsive">
            <table className="table table-hover">
              <thead>
                <tr>
                  <th>Email</th>
                  <th>Status</th>
                  <th>Sent Date</th>
                  <th>Used Date</th>
                  <th>Token</th>
                </tr>
              </thead>
              <tbody>
                {invitations.map((invitation) => (
                  <tr key={invitation.id}>
                    <td>
                      <strong>{invitation.email}</strong>
                    </td>
                    <td>{getStatusBadge(invitation.isUsed)}</td>
                    <td>{formatDate(invitation.createdAt)}</td>
                    <td>
                      {invitation.usedAt ? formatDate(invitation.usedAt) : '-'}
                    </td>
                    <td>
                      <code className="small">{invitation.invitationToken}</code>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
};

export default InvitationsList; 