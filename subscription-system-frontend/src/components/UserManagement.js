import React, { useState, useEffect } from 'react';
import { adminAPI } from '../services/api';

const UserManagement = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadUsers();
  }, []);

  const loadUsers = async () => {
    try {
      setLoading(true);
      const response = await adminAPI.getUsers();
      setUsers(response);
    } catch (error) {
      setError('Failed to load users. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="card shadow">
        <div className="card-body text-center">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
          <p className="mt-2">Loading users...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="card shadow">
        <div className="card-body">
          <div className="alert alert-danger">{error}</div>
          <button className="btn btn-primary" onClick={loadUsers}>
            Try Again
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="card shadow">
      <div className="card-header bg-primary text-white d-flex justify-content-between align-items-center">
        <h5 className="mb-0">User Management</h5>
        <span className="badge bg-light text-dark">{users.length} Users</span>
      </div>
      <div className="card-body">
        {users.length === 0 ? (
          <p className="text-muted text-center">No users found.</p>
        ) : (
          <div className="table-responsive">
            <table className="table table-striped">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Email</th>
                  <th>Role</th>
                  <th>Status</th>
                  <th>Fiscal Code</th>
                </tr>
              </thead>
              <tbody>
                {users.map((user) => (
                  <tr key={user.id}>
                    <td>{user.firstName} {user.lastName}</td>
                    <td>{user.email}</td>
                    <td>
                      <span className={`badge ${user.role === 'Admin' ? 'bg-danger' : 'bg-success'}`}>
                        {user.role}
                      </span>
                    </td>
                    <td>
                      <span className={`badge ${user.isConfirmed ? 'bg-success' : 'bg-warning'}`}>
                        {user.isConfirmed ? 'Confirmed' : 'Pending'}
                      </span>
                    </td>
                    <td>
                      {user.fiscalCode ? (
                        <code>{user.fiscalCode}</code>
                      ) : (
                        <span className="text-muted">-</span>
                      )}
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

export default UserManagement; 