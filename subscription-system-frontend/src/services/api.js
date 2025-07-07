import axios from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5001/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const authAPI = {
  registerAdmin: async (adminData) => {
    try {
      const response = await api.post('/auth/register-admin', adminData);
      return response.data;
    } catch (error) {
      if (error.response?.data?.error === 'TOKEN_ALREADY_USED') {
        throw new Error('This admin token has already been used. Only one administrator can be registered with this token. Please contact system administrator for a new token.');
      }
      throw new Error(error.response?.data?.message || 'Failed to register admin');
    }
  },

  login: async (credentials) => {
    const response = await api.post('/auth', credentials);
    return response.data;
  },

  confirmInvitation: async (invitationData) => {
    const response = await api.post('/invite/confirm', invitationData);
    return response.data;
  },

  updateFiscalCode: async (fiscalCodeData) => {
    const response = await api.put('/admin/fiscal-code', fiscalCodeData);
    return response.data;
  }
};

export const adminAPI = {
  getUsers: async () => {
    const response = await api.get('/admin/users');
    return response.data;
  },

  getInvitations: async () => {
    const response = await api.get('/admin/invitations');
    return response.data;
  },

  getDashboardData: async () => {
    const response = await api.get('/admin/dashboard');
    return response.data;
  },

  sendInvitations: async (invitationData) => {
    const response = await api.post('/admin/invite-users', invitationData);
    return response.data;
  },

  updateFiscalCode: async (fiscalCodeData) => {
    const response = await api.put('/admin/fiscal-code', fiscalCodeData);
    return response.data;
  },

  getFiscalCodeStatus: async () => {
    const response = await api.get('/admin/fiscal-code-status');
    return response.data;
  }
};

export default api; 