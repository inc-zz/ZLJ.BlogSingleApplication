import React, { createContext, useContext, useState, useEffect } from 'react';
import { isAuthenticated as checkAuth } from '../utils/authApi';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  // 从localStorage加载用户信息
  useEffect(() => {
    const hasValidToken = checkAuth(); // 检查token是否有效
    
    if (hasValidToken) {
      // Token有效，检查用户数据
      const savedUser = localStorage.getItem('blogs_user');
      if (savedUser) {
        try {
          const userData = JSON.parse(savedUser);
          setUser(userData);
          setIsAuthenticated(true);
        } catch (error) {
          console.error('Failed to parse user data:', error);
          localStorage.removeItem('blogs_user');
        }
      }
    } else {
      // Token无效或不存在，清除用户状态
      setUser(null);
      setIsAuthenticated(false);
    }
    setIsLoading(false);
  }, []);

  // 登录
  const login = (userData) => {
    setUser(userData);
    setIsAuthenticated(true);
    localStorage.setItem('blogs_user', JSON.stringify(userData));
  };

  // 登出
  const logout = () => {
    setUser(null);
    setIsAuthenticated(false);
    localStorage.removeItem('blogs_user');
    localStorage.removeItem('blogs_token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('tokenExpiry');
  };

  // 更新用户信息
  const updateUser = (newUserData) => {
    const updatedUser = { ...user, ...newUserData };
    setUser(updatedUser);
    localStorage.setItem('blogs_user', JSON.stringify(updatedUser));
  };

  // 检查认证状态
  const checkAuthentication = () => {
    return checkAuth();
  };

  // 获取用户信息
  const getUser = () => {
    const savedUser = localStorage.getItem('blogs_user');
    if (savedUser) {
      try {
        return JSON.parse(savedUser);
      } catch (error) {
        console.error('Failed to parse user data:', error);
        localStorage.removeItem('blogs_user');
        return null;
      }
    }
    return null;
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated,
        isLoading,
        login,
        logout,
        updateUser,
        checkAuthentication,
        getUser,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

// 自定义Hook，方便使用
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

export default AuthContext;
