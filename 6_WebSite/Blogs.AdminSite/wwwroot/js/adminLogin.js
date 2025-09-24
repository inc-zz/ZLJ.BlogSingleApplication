// login.js - 登录业务逻辑模块
define(['http', 'layer'], function (http, layer) {
    'use strict';

    // 登录类
    class Login {
        constructor(options = {}) {
            this.options = options;
            this.http = options.httpClient || http.default;
            this.form = options.form || '#loginForm';
            this.captchaElement = options.captchaElement || '#captchaImage';

            this.init();
        }

        init() {
            this.bindEvents();
            this.refreshCaptcha();
        }

        // 生成验证码（实际项目中应由后端生成）
        generateCaptcha() {
            const chars = 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789';
            let captcha = '';
            for (let i = 0; i < 4; i++) {
                captcha += chars.charAt(Math.floor(Math.random() * chars.length));
            }
            return captcha;
        }

        // 刷新验证码
        refreshCaptcha() {
            this.currentCaptcha = this.generateCaptcha();
            $(this.captchaElement).find('#captchaText').text(this.currentCaptcha);
            $(this.form).find('#captcha').val('');
        }

        // 绑定事件
        bindEvents() {
            const self = this;

            // 验证码点击刷新
            $(this.captchaElement).click(function () {
                self.refreshCaptcha();
            });

            // 表单提交
            layui.form.on('submit(loginSubmit)', function (data) {
                self.handleLogin(data.field);
                return false; // 阻止表单默认提交
            });

            // 忘记密码
            $('#forgotPassword').click(function () {
                layer.msg('请联系系统管理员重置密码', { icon: 5 });
            });
        }

        // 处理登录
        async handleLogin(formData) {
            // 验证验证码
            if (formData.captcha.toUpperCase() !== this.currentCaptcha) {
                layer.msg('验证码错误，请重新输入', { icon: 5 });
                this.refreshCaptcha();
                return;
            }

            // 显示加载中
            const loadIndex = layer.load(1, {
                shade: [0.1, '#fff']
            });

            try {
                // 调用登录API
                const response = await this.http.post('/admin/login', {
                    username: formData.username,
                    password: formData.password,
                    captcha: formData.captcha,
                    rememberMe: formData.rememberMe === 'on'
                });

                layer.close(loadIndex);

                if (response.data && response.data.success) {
                    layer.msg('登录成功！', { icon: 1 });

                    // 存储token
                    if (response.data.data && response.data.data.token) {
                        localStorage.setItem('auth_token', response.data.data.token);

                        if (formData.rememberMe === 'on') {
                            localStorage.setItem('rememberMe', 'true');
                        }
                    }

                    // 跳转到首页
                    setTimeout(() => {
                        window.location.href = this.options.redirectUrl || '/';
                    }, 1000);

                } else {
                    layer.msg(response.data.message || '登录失败，请检查用户名和密码', { icon: 5 });
                    this.refreshCaptcha();
                }

            } catch (error) {
                layer.close(loadIndex);

                let errorMsg = '网络错误，请稍后重试';
                if (error.status === 401) {
                    errorMsg = '用户名或密码错误';
                } else if (error.status === 500) {
                    errorMsg = '服务器内部错误';
                } else if (error.message) {
                    errorMsg = error.message;
                }

                layer.msg(errorMsg, { icon: 5 });
                this.refreshCaptcha();
            }
        }
    }

    return Login;
});