import React, { useState } from 'react'; 
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import http from '../utils/http';
import { login as loginApi } from '../utils/authApi';
import SliderCaptcha from '../components/SliderCaptcha';
import TermsModal from '../components/TermsModal';
import { App } from 'antd';

const Register = () => {
  const navigate = useNavigate();
  const { login } = useAuth();
  const { message } = App.useApp();
  
  const [formData, setFormData] = useState({
    username: '',
    nickname: '',
    email: '',
    password: '',
    confirmPassword: '',
  });
  
  const [isSliderVerified, setIsSliderVerified] = useState(false);
  const [agreedToTerms, setAgreedToTerms] = useState(false);
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [showTermsModal, setShowTermsModal] = useState(false);
  const [termsType, setTermsType] = useState('terms');
  const [showCaptcha, setShowCaptcha] = useState(false);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value,
    }));
    setError('');
  };

  const handleCaptchaSuccess = () => {
    setIsSliderVerified(true);
    setError('');
    // 验证成功后隐藏验证码区域
    setTimeout(() => {
      setShowCaptcha(false);
    }, 800);
  };

  const handleCaptchaFail = () => {
    setIsSliderVerified(false);
    setError('验证失败，请重试');
  };

  const handleTermsClick = (e, type) => {
    e.preventDefault();
    setTermsType(type);
    setShowTermsModal(true);
  };

  const validateForm = () => {
    // 验证用户名
    if (!formData.username.trim()) {
      setError('请输入用户名');
      return false;
    }
    if (formData.username.length < 3 || formData.username.length > 20) {
      setError('用户名长度应在3-20个字符之间');
      return false;
    }
    if (!/^[a-zA-Z0-9_]+$/.test(formData.username)) {
      setError('用户名只能包含字母、数字和下划线');
      return false;
    }

    // 验证昵称
    if (!formData.nickname.trim()) {
      setError('请输入昵称');
      return false;
    }
    if (formData.nickname.length > 50) {
      setError('昵称长度不能超过50个字符');
      return false;
    }

    // 验证邮箱
    if (!formData.email.trim()) {
      setError('请输入邮箱');
      return false;
    }
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.email)) {
      setError('请输入有效的邮箱地址');
      return false;
    }

    // 验证密码
    if (!formData.password.trim()) {
      setError('请输入密码');
      return false;
    }
    if (formData.password.length < 6) {
      setError('密码长度不能少于6个字符');
      return false;
    }

    // 验证确认密码
    if (formData.password !== formData.confirmPassword) {
      setError('两次输入的密码不一致');
      return false;
    }

    // 验证拼图
    if (!isSliderVerified) {
      setError('请完成拼图验证');
      return false;
    }

    // 验证协议勾选
    if (!agreedToTerms) {
      setError('请同意用户协议和隐私政策');
      return false;
    }

    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    if (!validateForm()) {
      return;
    }

    setIsLoading(true);

    try {
      // 调用注册接口
      const response = await http.post('/AppUser/register', {
        userName: formData.username,
        password: formData.password,
        email: formData.email
      });

      if (response.data && response.data.success) {
        // 注册成功提示
        message.success('注册成功！正在自动登录...');
        
        // 等待2秒后自动调用登录接口
        setTimeout(async () => {
          try {
            // 调用登录API
            const loginResult = await loginApi({
              account: formData.username,
              password: formData.password,
              captcha: '1001'
            });

            if (loginResult.success) {
              // 登录成功，更新上下文
              const { userInfo } = loginResult.data;
              
              login({
                username: userInfo.account,
                nickname: userInfo.account,
                email: userInfo.email,
                avatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${userInfo.account}`,
                phoneNumber: userInfo.phoneNumber,
                description: userInfo.description,
                lastLoginTime: userInfo.lastLoginTime
              });

              // 跳转到首页
              navigate('/');
            } else {
              setError(loginResult.message || '自动登录失败，请手动登录');
              setIsLoading(false);
            }
          } catch (loginError) {
            console.error('Auto login error:', loginError);
            setError('自动登录失败，请手动登录');
            setIsLoading(false);
            // 登录失败，跳转到登录页
            setTimeout(() => {
              navigate('/login');
            }, 2000);
          }
        }, 2000);
      } else {
        setError(response.data?.message || '注册失败，请重试');
        setIsLoading(false);
      }
    } catch (error) {
      console.error('Registration error:', error);
      setError(error.response?.data?.message || '注册请求失败，请检查网络连接');
      setIsLoading(false);
    }
  };

  return (
    <div className="register-container">
      <div className="register-card">
        <div className="register-header">
          <h1>注册账号</h1>
          <p>加入知识分享，开启学习之旅</p>
        </div>

        <form onSubmit={handleSubmit} className="register-form">
          {error && (
            <div className="error-message">
              {error}
            </div>
          )}

          <div className="form-row">
            <label htmlFor="username">用户名 *</label>
            <input
              type="text"
              id="username"
              name="username"
              maxLength={20}
              value={formData.username}
              onChange={handleInputChange}
              placeholder="3-20个字符，仅限字母数字下划线"
              autoComplete="username"
            />
          </div>

          <div className="form-row">
            <label htmlFor="nickname">昵称 *</label>
            <input
              type="text"
              id="nickname"
              name="nickname"
              maxLength={30}
              value={formData.nickname}
              onChange={handleInputChange}
              placeholder="请输入昵称"
              autoComplete="nickname"
            />
          </div>

          <div className="form-row">
            <label htmlFor="email">邮箱 *</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleInputChange}
              placeholder="请输入邮箱地址"
              autoComplete="email"
            />
          </div>

          <div className="form-row">
            <label htmlFor="password">密码 *</label>
            <input
              type="password"
              id="password"
              name="password"
              maxLength={30}
              value={formData.password}
              onChange={handleInputChange}
              placeholder="至少6个字符"
              autoComplete="new-password"
            />
          </div>

          <div className="form-row">
            <label htmlFor="confirmPassword">确认密码 *</label>
            <input
              type="password"
              id="confirmPassword"
              name="confirmPassword"
              value={formData.confirmPassword}
              onChange={handleInputChange}
              placeholder="请再次输入密码"
              autoComplete="new-password"
            />
          </div>

          <div 
            className="captcha-hover-area"
            onMouseEnter={() => !isSliderVerified && setShowCaptcha(true)}
            onMouseLeave={() => !isSliderVerified && setShowCaptcha(false)}
          >
            <div className="captcha-placeholder">
              {!isSliderVerified ? '请将鼠标移至此处进行验证' : '✓ 验证成功'}
            </div>
            {showCaptcha && !isSliderVerified && (
              <div className="captcha-dropdown">
                <SliderCaptcha 
                  onSuccess={handleCaptchaSuccess}
                  onFail={handleCaptchaFail}
                />
              </div>
            )}
          </div>

          <div className="form-group checkbox-group">
            <label className="checkbox-label">
              <input
                type="checkbox"
                checked={agreedToTerms}
                onChange={(e) => setAgreedToTerms(e.target.checked)}
              />
              <span>
                我已阅读并同意
                <a href="#terms" onClick={(e) => handleTermsClick(e, 'terms')}>
                  《用户协议》
                </a>
                和
                <a href="#privacy" onClick={(e) => handleTermsClick(e, 'privacy')}>
                  《隐私政策》
                </a>
              </span>
            </label>
          </div>

          <button
            type="submit"
            className="btn-register"
            disabled={isLoading}
          >
            {isLoading ? '注册中...' : '立即注册'}
          </button>
        </form>

        <div className="register-footer">
          <p className="login-link">
            已有账号？
            <a href="#login" onClick={(e) => { e.preventDefault(); navigate('/login'); }}>
              立即登录
            </a>
          </p>
        </div>
      </div>

      <div className="register-background">
        <div className="bg-circle circle-1"></div>
        <div className="bg-circle circle-2"></div>
        <div className="bg-circle circle-3"></div>
      </div>

      {/* 用户协议弹窗 */}
      <TermsModal 
        isOpen={showTermsModal}
        onClose={() => setShowTermsModal(false)}
        type={termsType}
      />
    </div>
  );
};

export default Register;
