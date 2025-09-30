// 使用 Layui 模块
layui.use(['form', 'layer'], function () {
    var form = layui.form;
    var layer = layui.layer;
    var $ = layui.$;

    // 登录功能封装
    class LoginManager {
        constructor() {
            this.apiBaseUrl = 'http://localhost:5234/api/admin';
            this.loginEndpoint = '/Account/login';
            this.tokenKey = 'auth_token';
            this.refreshTokenKey = 'refresh_token';
            this.userInfoKey = 'user_info';
            this.tokenExpiryKey = 'token_expiry';

            this.init();
        }

        init() {
            // 设置自定义验证规则
            this.setupFormValidation();

            // 渲染 Layui 表单组件
            form.render();

            // 检查是否已登录
            this.checkLoginStatus();

            // 绑定事件
            this.bindEvents();

            // 初始生成验证码
            this.refreshCaptcha();
        }

        // 设置表单验证规则
        setupFormValidation() {
            form.verify({
                // 密码验证规则
                password: function (value, item) {
                    if (value.length < 6) {
                        return '密码长度不能小于6位';
                    }
                    if (!/^[\S]{6,}$/.test(value)) {
                        return '密码不能包含空格';
                    }
                },
                // 验证码验证规则
                captcha: function (value, item) {
                    var currentCaptcha = $('#captchaImage').text();
                    if (value.toUpperCase() !== currentCaptcha) {
                        return '验证码错误，请重新输入';
                    }
                }
            });
        }

        // 检查登录状态
        checkLoginStatus() {
            const token = this.getToken();
            if (token && this.isTokenValid()) {
                // 如果已登录且token有效，直接跳转到首页
                console.log('用户已登录，跳转到首页');
                this.redirectToHome();
            }
        }

        // 绑定事件
        bindEvents() {
            var self = this;

            // 验证码点击刷新
            $('#captchaImage').on('click', function () {
                self.refreshCaptcha();
            });

            // 忘记密码
            $('.forgot-password').on('click', function (e) {
                e.preventDefault();
                self.handleForgotPassword();
            });

            // 监听表单提交
            form.on('submit(loginSubmit)', function (data) {
                self.handleLogin(data.field);
                return false; // 阻止表单默认提交
            });

            // 添加回车键提交支持
            $('#loginForm input').on('keypress', function (e) {
                if (e.which === 13) {
                    $('.btn-login').click();
                    return false;
                }
            });
        }

        // 刷新验证码
        refreshCaptcha() {
            const chars = 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789';
            let captcha = '';
            for (let i = 0; i < 4; i++) {
                captcha += chars.charAt(Math.floor(Math.random() * chars.length));
            }
            $('#captchaImage').text(captcha);

            // 清空验证码输入框
            $('#CaptchaCode').val('');
        }

        // 处理登录
        async handleLogin(formData) {
            var self = this;

            // 显示加载状态
            this.setButtonLoading(true);

            try {

                // 调用登录API
                const response = await this.callLoginApi(
                    formData.Username,
                    formData.Password,
                    formData.CaptchaCode,
                    formData.RememberMe === 'on'
                );

                if (response.success) {
                    // 登录成功
                    layer.msg('登录成功，正在跳转...', { icon: 1, time: 1000 });

                    // 存储Token和用户信息
                    this.saveloginResult(response.data, formData.RememberMe === 'on');

                    // 跳转到首页
                    setTimeout(() => {
                        this.redirectToHome();
                    }, 1000);
                } else {
                    // 登录失败
                    layer.msg(response.message || '登录失败，请检查用户名和密码', { icon: 2 });
                    this.refreshCaptcha();
                }
            } catch (error) {
                console.error('登录错误:', error);
                layer.msg('网络错误，请稍后重试: ' + error.message, { icon: 5 });
                this.refreshCaptcha();
            } finally {
                // 恢复按钮状态
                this.setButtonLoading(false);
            }
        }

        // 调用登录API
        async callLoginApi(username, password, captcha, rememberMe) {

            const requestData = {
                account: username,
                password: password,
                captcha: captcha,
                rememberMe: rememberMe
            };

            const response = await fetch(this.apiBaseUrl + this.loginEndpoint, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(requestData)
            });

            if (!response.ok) {
                throw new Error(`HTTP错误! 状态码: ${response.status}`);
            }

            return await response.json();
        }

        // 保存认证数据
        saveloginResult(loginResult, rememberMe) {

            /**
             * 后端登录接口返还数据格式：
               {
                  "data": {
                    "userInfo": {
                      "id": 100001,
                      "userName": "admin",
                      "email": "admin@sing.com",
                      "lastLoginDate": "2025-09-24T15:03:30"
                    },
                    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMDAwMDEiLCJuYW1lIjoiYWRtaW4iLCJlbWFpbCI6ImFkbWluQHNpbmcuY29tIiwianRpIjoiZTU0NTcyMTgtY2ZlMC00ODEwLThmMjEtMzE3MDMwMTFiMGJmIiwiaWF0IjoxNzU4Njk4NjMxLCJyb2xlIjoiYWRtaW4iLCJuYmYiOjE3NTg2OTg2MzEsImV4cCI6MTc1ODcwMjIzMSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MjM0IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MjM0In0.tsmnvQRc0tLXTlZogIfxKe4Nznw8bNb30KaxBakmxHg",
                    "refreshToken": "ffdb9b3a18eb40ae9ff1290e6ac8b794",
                    "expiresIn": "2025-09-24T08:23:51.9036243Z",
                    "tokenType": "Bearer"
                  },
                  "success": true,
                  "message": "登录成功",
                  "code": 200
                }
             */
            if (loginResult && loginResult.accessToken) {
                // 存储Token
                localStorage.setItem(this.tokenKey, loginResult.accessToken);

                // 存储用户信息
                if (loginResult.userInfo) {
                    localStorage.setItem(this.userInfoKey, JSON.stringify(loginResult.userInfo));
                }

                // 配置Tsoken过期时间（1天-保持与后端Token过期时间一致）
                const expiryTime = new Date().getTime() + (24 * 60 * 60 * 1000);
                localStorage.setItem(this.tokenExpiryKey, expiryTime.toString());

                // 存储刷新令牌，过期时间与后端一致，如果刷新令牌请求返还401则清空
                if (loginResult.refreshToken) {
                    localStorage.setItem(this.refreshTokenKey, loginResult.refreshToken);
                    const refreshExpiryTime = new Date().getTime() + (7 * 24 * 60 * 60 * 1000); // 7天
                }

                // 如果选择"记住我"，也存储在sessionStorage中作为备份
                if (rememberMe) {
                    sessionStorage.setItem(this.tokenKey, loginResult.accessToken);
                }

                console.log('认证数据保存成功');
            } else {
                console.error('认证数据格式错误，无法保存');
            }
        }

        // 获取Token
        getToken() {
            return localStorage.getItem(this.tokenKey) || sessionStorage.getItem(this.tokenKey);
        }

        // 检查Token是否有效
        isTokenValid() {
            const expiryTime = localStorage.getItem(this.tokenExpiryKey);
            if (!expiryTime) return false;

            return new Date().getTime() < parseInt(expiryTime);
        }

        // 跳转到首页
        redirectToHome() {
            // 这里根据实际路由配置修改
            window.location.href = '/';
        }

        // 处理忘记密码
        handleForgotPassword() {
            layer.msg('请联系系统管理员重置密码', { icon: 0 });
        }

        // 设置按钮加载状态
        setButtonLoading(loading) {
            const button = $('.btn-login');
            if (loading) {
                button.prop('disabled', true);
                button.html('<i class="layui-icon layui-icon-loading layui-anim layui-anim-rotate"></i> 登录中...');
            } else {
                button.prop('disabled', false);
                button.text('登录');
            }
        }
    }

    // 页面加载完成后初始化登录管理器
    window.loginManager = new LoginManager();
});