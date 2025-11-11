import { useEffect } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { ConfigProvider, App as AntApp } from 'antd';
import { useAuth } from './context/AuthContext';
import { setMessageInstance } from './utils/http';
import Header from './components/Header';
import BottomNav from './components/BottomNav';
import Footer from './components/Footer';
import Login from './pages/Login';
import Register from './pages/Register';
import Home from './pages/Home';
import ArticleDetail from './pages/ArticleDetail';
import Editor from './pages/Editor';
import Profile from './pages/Profile';
import UserProfile from './pages/UserProfile';
import NotFound from './pages/NotFound';

// 受保护的路由组件
const ProtectedRoute = ({ children }) => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? children : <Navigate to="/login" replace />;
};

function App() {
  const { isAuthenticated } = useAuth();
  const { message } = AntApp.useApp();

  // 初始化 message 实例到 http.js
  useEffect(() => {
    setMessageInstance(message);
  }, [message]);

  return (
    <ConfigProvider
      theme={{
        token: {
          colorPrimary: '#ff6b35',
          borderRadius: 6,
        },
      }}
    >
      <AntApp>
        <div className="app">
          <Header />
          
          <main className="main-content">
            <Routes>
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/" element={<Home />} />
              <Route path="/article/:id" element={<ArticleDetail />} />
              {/* Public user profile - visitor view */}
              <Route path="/user/:username" element={<UserProfile />} />
              <Route 
                path="/editor" 
                element={
                  <ProtectedRoute>
                    <Editor />
                  </ProtectedRoute>
                } 
              />
              <Route 
                path="/editor/:id" 
                element={
                  <ProtectedRoute>
                    <Editor />
                  </ProtectedRoute>
                } 
              />
              <Route 
                path="/profile" 
                element={
                  <ProtectedRoute>
                    <Profile />
                  </ProtectedRoute>
                } 
              />
              {/* 404 Page */}
              <Route path="/404" element={<NotFound />} />
              {/* Catch all unmatched routes */}
              <Route path="*" element={<NotFound />} />
            </Routes>
          </main>

          <Footer />

          {isAuthenticated && <BottomNav />}
        </div>
      </AntApp>
    </ConfigProvider>
  );
}

export default App;
