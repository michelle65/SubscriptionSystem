import React, { useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { authAPI } from '../services/api';

const FiscalCodeManager = ({ onFiscalCodeUpdated, isRequired = false }) => {
  const { user } = useAuth();
  const [fiscalCode, setFiscalCode] = useState('');
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage('');
    setError('');

    if (!fiscalCode.trim()) {
      setError('Fiscal code is required');
      return;
    }

    if (fiscalCode.length !== 16) {
      setError('Fiscal code must be exactly 16 characters long');
      return;
    }

    setLoading(true);
    try {
      await authAPI.updateFiscalCode({ fiscalCode });
      setMessage('Fiscal code updated successfully!');
      setFiscalCode('');
      
      if (onFiscalCodeUpdated) {
        setTimeout(() => {
          onFiscalCodeUpdated();
        }, 1500);
      }
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to update fiscal code');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card shadow">
      <div className={`card-header ${isRequired ? 'bg-warning text-dark' : 'bg-info text-white'}`}>
        <h5 className="mb-0">
          <i className={`fas ${isRequired ? 'fa-exclamation-triangle' : 'fa-id-card'} me-2`}></i>
          {isRequired ? 'Set Fiscal Code (Required)' : 'Manage Fiscal Code'}
        </h5>
      </div>
      <div className="card-body">
        {isRequired && (
          <div className="alert alert-warning">
            <strong>Important:</strong> You must set your fiscal code before accessing the admin dashboard. 
            This is required for administrative and legal purposes.
          </div>
        )}
        
        {message && (
          <div className="alert alert-success alert-dismissible fade show" role="alert">
            {message}
            <button type="button" className="btn-close" onClick={() => setMessage('')}></button>
          </div>
        )}
        
        {error && (
          <div className="alert alert-danger alert-dismissible fade show" role="alert">
            {error}
            <button type="button" className="btn-close" onClick={() => setError('')}></button>
          </div>
        )}

        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label htmlFor="fiscalCode" className="form-label">
              Fiscal Code {isRequired && <span className="text-danger">*</span>}
            </label>
            <input
              type="text"
              className="form-control"
              id="fiscalCode"
              value={fiscalCode}
              onChange={(e) => setFiscalCode(e.target.value)}
              placeholder="Enter 16-character fiscal code"
              maxLength={16}
              required={isRequired}
            />
            <div className="form-text">
              Enter your 16-character fiscal code. This is required for administrative purposes.
            </div>
          </div>

          <div className="d-grid">
            <button
              type="submit"
              className={`btn ${isRequired ? 'btn-warning' : 'btn-info'}`}
              disabled={loading}
            >
              {loading ? (
                <>
                  <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                  Updating...
                </>
              ) : (
                isRequired ? 'Set Fiscal Code & Continue' : 'Update Fiscal Code'
              )}
            </button>
          </div>
        </form>

        <div className="mt-3">
          <small className="text-muted">
            <strong>Note:</strong> Fiscal code is used for administrative and legal purposes. 
            Make sure to enter the correct 16-character code.
          </small>
        </div>
      </div>
    </div>
  );
};

export default FiscalCodeManager; 