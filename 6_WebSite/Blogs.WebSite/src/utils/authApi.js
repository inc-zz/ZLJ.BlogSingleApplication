/**
 * Authentication API Service
 * Handles user authentication endpoints
 */

import http from './http';

/**
 * User login
 * @param {Object} credentials - Login credentials
 * @param {string} credentials.account - User account (username)
 * @param {string} credentials.password - User password
 * @param {string} credentials.captcha - Captcha code
 * @returns {Promise} Response with user info and tokens
 */
export const login = async (credentials) => {
  try {
    const response = await http.post('/AppUser/login', credentials);
    
    // API response format:
    // {
    //   data: { userInfo, accessToken, refreshToken, expiresIn, tokenType },
    //   success: true,
    //   message: "登录成功",
    //   code: 200
    // }
    
    if (response.data && response.data.success) {
      const { data } = response.data;
      
      // Store tokens in localStorage
      if (data.accessToken) {
        localStorage.setItem('blogs_token', data.accessToken);
        localStorage.setItem('refreshToken', data.refreshToken);
        localStorage.setItem('tokenExpiry', data.expiresIn);
      }
      
      // Store user info
      if (data.userInfo) {
        localStorage.setItem('blogs_user', JSON.stringify(data.userInfo));
      }
      
      return {
        success: true,
        data: data,
        message: response.data.message
      };
    } else {
      return {
        success: false,
        message: response.data?.message || '登录失败'
      };
    }
  } catch (error) {
    console.error('Login API Error:', error);
    return {
      success: false,
      message: error.response?.data?.message || '登录请求失败，请检查网络连接'
    };
  }
};

/**
 * User logout
 * @returns {Promise} Logout response
 */
export const logout = async () => {
  try {
    const response = await http.post('/AppUser/logout');
    
    // Clear local storage
    localStorage.removeItem('blogs_token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('tokenExpiry');
    localStorage.removeItem('blogs_user');
    
    return {
      success: true,
      message: '登出成功'
    };
  } catch (error) {
    // Clear local storage even if API call fails
    localStorage.removeItem('blogs_token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('tokenExpiry');
    localStorage.removeItem('blogs_user');
    
    return {
      success: true,
      message: '已退出登录'
    };
  } 
};

/**
 *  刷新令牌
 * @returns {Promise} New tokens
 */
export const refreshToken = async () => {
  try {
    const refreshTokenValue = localStorage.getItem('refreshToken');
    
    if (!refreshTokenValue) {
      throw new Error('No refresh token available');
    }
    
    const response = await http.post('/AppUser/refreshToken', {
      refreshToken: refreshTokenValue
    });
    
    // API response format:
    // {
    //   data: { result, accessToken, refreshToken, expiration, message },
    //   success: true,
    //   message: "处理成功",
    //   code: 200
    // }
    
    if (response.data && response.data.success && response.data.data.result) {
      const { data } = response.data;
      
      // Update tokens
      if (data.accessToken) {
        localStorage.setItem('blogs_token', data.accessToken);
        localStorage.setItem('refreshToken', data.refreshToken);
        localStorage.setItem('tokenExpiry', data.expiration);
      }
      
      return {
        success: true,
        data: data
      };
    } else {
      return {
        success: false,
        message: response.data?.message || 'Token刷新失败'
      };
    }
  } catch (error) {
    console.error('Refresh Token Error:', error);
    return {
      success: false,
      message: 'Token刷新失败'
    };
  }
};

/**
 * Get current user info
 * @returns {Promise} User information
 */
export const getCurrentUser = async () => {
  try {
    const response = await http.get('/AppUser/current');
    
    if (response.data && response.data.success) {
      const { data } = response.data;
      
      // Update user info in localStorage
      if (data) {
        localStorage.setItem('blogs_user', JSON.stringify(data));
      }
      
      return {
        success: true,
        data: data
      };
    } else {
      return {
        success: false,
        message: response.data?.message || '获取用户信息失败'
      };
    }
  } catch (error) {
    console.error('Get Current User Error:', error);
    return {
      success: false,
      message: '获取用户信息失败'
    };
  }
};

/**
 * Check if user is authenticated
 * @returns {boolean} Authentication status
 */
export const isAuthenticated = () => {
  const token = localStorage.getItem('blogs_token');
  const tokenExpiry = localStorage.getItem('tokenExpiry');
  
  if (!token) {
    return false;
  }
  
  // Check if token is expired
  if (tokenExpiry) {
    const expiryDate = new Date(tokenExpiry);
    const now = new Date();
    
    if (now >= expiryDate) {
      // Token expired, clear storage
      localStorage.removeItem('blogs_token');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('tokenExpiry');
      localStorage.removeItem('blogs_user');
      return false;
    }
  }
  
  return true;
};

export default {
  login,
  logout,
  refreshToken,
  getCurrentUser,
  isAuthenticated
};
