import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authAPI } from '../services/api';

const AdminRegistration = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    token: ''
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');

  const validateForm = () => {
    const newErrors = {};

    if (!formData.firstName.trim()) {
      newErrors.firstName = 'First name is required';
    }

    if (!formData.lastName.trim()) {
      newErrors.lastName = 'Last name is required';
    }

    if (!formData.email.trim()) {
      newErrors.email = 'Email is required';
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = 'Email is invalid';
    }

    if (!formData.password) {
      newErrors.password = 'Password is required';
    } else if (formData.password.length < 6) {
      newErrors.password = 'Password must be at least 6 characters';
    }

    if (!formData.confirmPassword) {
      newErrors.confirmPassword = 'Please confirm your password';
    } else if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match';
    }

    if (!formData.token.trim()) {
      newErrors.token = 'Admin token is required';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    if (errors[name]) {
      setErrors(prev => ({
        ...prev,
        [name]: ''
      }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage('');

    if (!validateForm()) {
      return;
    }

    setLoading(true);
    try {
      await authAPI.registerAdmin(formData);
      setMessage('Admin registration successful! Please login.');
      setTimeout(() => {
        navigate('/login');
      }, 2000);
    } catch (error) {
      setMessage(error.response?.data?.message || 'Registration failed. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container-fluid d-flex align-items-center justify-content-center" style={{ minHeight: '100vh' }}>
      <div className="row justify-content-center w-100">
        <div className="col-md-6 col-lg-5">
          <div className="card shadow">
            <div className="card-header bg-primary text-white">
              <h3 className="mb-0">Admin Registration</h3>
            </div>
            <div className="card-body">
              {message && (
                <div className={`alert ${message.includes('successful') ? 'alert-success' : 'alert-danger'}`}>
                  {message}
                </div>
              )}
              
              <form onSubmit={handleSubmit}>
                <div className="row">
                  <div className="col-md-6 mb-3">
                    <label htmlFor="firstName" className="form-label">First Name *</label>
                    <input
                      type="text"
                      className={`form-control ${errors.firstName ? 'is-invalid' : ''}`}
                      id="firstName"
                      name="firstName"
                      value={formData.firstName}
                      onChange={handleChange}
                      placeholder="Enter first name"
                    />
                    {errors.firstName && <div className="invalid-feedback">{errors.firstName}</div>}
                  </div>
                  
                  <div className="col-md-6 mb-3">
                    <label htmlFor="lastName" className="form-label">Last Name *</label>
                    <input
                      type="text"
                      className={`form-control ${errors.lastName ? 'is-invalid' : ''}`}
                      id="lastName"
                      name="lastName"
                      value={formData.lastName}
                      onChange={handleChange}
                      placeholder="Enter last name"
                    />
                    {errors.lastName && <div className="invalid-feedback">{errors.lastName}</div>}
                  </div>
                </div>

                <div className="mb-3">
                  <label htmlFor="email" className="form-label">Email Address *</label>
                  <input
                    type="email"
                    className={`form-control ${errors.email ? 'is-invalid' : ''}`}
                    id="email"
                    name="email"
                    value={formData.email}
                    onChange={handleChange}
                    placeholder="Enter email address"
                  />
                  {errors.email && <div className="invalid-feedback">{errors.email}</div>}
                </div>

                <div className="mb-3">
                  <label htmlFor="token" className="form-label">Admin Token *</label>
                  <input
                    type="text"
                    className={`form-control ${errors.token ? 'is-invalid' : ''}`}
                    id="token"
                    name="token"
                    value={formData.token}
                    onChange={handleChange}
                    placeholder="Enter admin token"
                  />
                  {errors.token && <div className="invalid-feedback">{errors.token}</div>}
                  <div className="form-text">Enter the mock admin token for registration</div>
                </div>



                <div className="mb-3">
                  <label htmlFor="password" className="form-label">Password *</label>
                  <input
                    type="password"
                    className={`form-control ${errors.password ? 'is-invalid' : ''}`}
                    id="password"
                    name="password"
                    value={formData.password}
                    onChange={handleChange}
                    placeholder="Enter password"
                  />
                  {errors.password && <div className="invalid-feedback">{errors.password}</div>}
                </div>

                <div className="mb-3">
                  <label htmlFor="confirmPassword" className="form-label">Confirm Password *</label>
                  <input
                    type="password"
                    className={`form-control ${errors.confirmPassword ? 'is-invalid' : ''}`}
                    id="confirmPassword"
                    name="confirmPassword"
                    value={formData.confirmPassword}
                    onChange={handleChange}
                    placeholder="Confirm your password"
                  />
                  {errors.confirmPassword && <div className="invalid-feedback">{errors.confirmPassword}</div>}
                </div>

                <div className="d-grid gap-2">
                  <button
                    type="submit"
                    className="btn btn-primary"
                    disabled={loading}
                  >
                    {loading ? (
                      <>
                        <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                        Registering...
                      </>
                    ) : (
                      'Register Admin'
                    )}
                  </button>
                </div>
              </form>

              <div className="text-center mt-3">
                <p className="mb-0">
                  Already have an account?{' '}
                  <a href="/login" className="text-decoration-none">Login here</a>
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminRegistration; 