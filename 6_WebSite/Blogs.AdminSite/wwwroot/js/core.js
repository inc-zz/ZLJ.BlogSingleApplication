/**
 * 核心工具类 - 专注于通用功能
 * HTTP请求、Token处理、状态码处理等
 */
layui.define(['jquery', 'layer'], function (exports) {
    "use strict";

    var $ = layui.$;
    var layer = layui.layer;

    var Core = {
        api: {
            baseUrl: 'http://localhost:5234/api',
            timeout: 10000,
            refreshTokenUrl: '/admin/account/refresh'
        },
        _refreshingToken: null,  // 当前正在刷新令牌的请求Promise
        _requestQueue: [],       // 等待刷新令牌的请求队列

        // ==================== HTTP 请求核心 ====================
        http: {
            get: function (url, data, options) { return Core._request('GET', url, data, options); },
            post: function (url, data, options) { return Core._request('POST', url, data, options); },
            put: function (url, data, options) { return Core._request('PUT', url, data, options); },
            delete: function (url, data, options) { return Core._request('DELETE', url, data, options); }
        },

        /**
         * 统一的请求方法
         */
        _request: function (method, url, data, options) {
            var config = $.extend({ loading: true, timeout: Core.api.timeout, headers: {} }, options);
            
            return new Promise(function (resolve, reject) {
                var loadingIndex = config.loading ? Core.msg.loading() : null;
                var token = localStorage.getItem('auth_token');
                
                if (token) {
                    config.headers['Authorization'] = 'Bearer ' + token;
                }

                $.ajax({
                    url: Core.api.baseUrl + url,
                    type: method,
                    data: method === 'GET' ? data : JSON.stringify(data),
                    contentType: 'application/json',
                    dataType: 'json',
                    timeout: config.timeout,
                    headers: config.headers,
                    beforeSend: function (xhr) {
                        // 保存原始请求信息，用于刷新令牌后重试
                        var originalRequestInfo = JSON.stringify({ method, url, data, options });
                        xhr.setRequestHeader('X-Original-Request', encodeURIComponent(originalRequestInfo));
                        config.beforeSend && config.beforeSend(xhr);
                    },
                    success: function (response) {
                        Core.msg.close(loadingIndex);
                        Core._handleResponse(response, resolve, reject);
                    },
                    error: function (xhr, status, error) {
                        Core.msg.close(loadingIndex);
                        Core._handleHttpError(xhr, status, error, reject, { method, url, data, options });
                    }
                });
            });
        },

        /**
         * 处理响应
         */
        _handleResponse: function (response, resolve, reject) {
            if (response.code === 200) {
                resolve(response);
            } else if (response.code === 401) {
                Core._handleUnauthorized();
                reject(new Error('未授权访问'));
            } else if (response.code === 403) {
                Core.msg.error('权限不足');
                reject(new Error('权限不足'));
            } else {
                var errorMsg = response.message || '请求失败';
                Core.msg.error(errorMsg);
                reject(new Error(errorMsg));
            }
        },

        /**
         * 处理HTTP错误
         */
        _handleHttpError: function (xhr, status, error, reject, requestContext) {
            var errorMap = { 400: '请求参数错误', 401: '未授权访问', 403: '权限不足', 404: '请求地址不存在', 500: '服务器内部错误', 502: '网关错误', 503: '服务不可用' };
            var message = errorMap[xhr.status] || `请求失败: ${error}`;
            if (xhr.status === 0) message = '网络连接失败';

            // 特殊处理401错误，尝试刷新令牌
            if (xhr.status === 401) {
                // 获取原始请求配置信息
                var originalRequestData;
                try {
                    var originalRequestConfig = xhr.getResponseHeader('X-Original-Request') || null;
                    originalRequestData = originalRequestConfig ? JSON.parse(decodeURIComponent(originalRequestConfig)) : requestContext;
                } catch (e) {
                    originalRequestData = requestContext || null;
                }
                
                // 检查是否已经在刷新令牌
                if (this._refreshingToken) {
                    // 如果已经在刷新，则将当前请求加入队列等待重试
                    return new Promise((resolve, reject) => {
                        this._requestQueue.push({ resolve, reject, originalRequest: originalRequestData });
                    });
                }
                
                // 尝试刷新令牌并处理结果
                return this.refreshToken()
                    .then((newToken) => {
                        if (newToken && originalRequestData) {
                            // 刷新成功，重新执行原始请求
                            return this._request(originalRequestData.method, originalRequestData.url, originalRequestData.data, originalRequestData.options);
                        }
                        return Promise.reject(new Error('令牌刷新失败'));
                    })
                    .catch(() => {
                        // 刷新失败的情况已经在refreshToken中处理，这里只需拒绝Promise
                        return Promise.reject(new Error('令牌刷新失败'));
                    });
            }

            Core.msg.error(message);
            reject(new Error(message));
        },

        /**
         * 刷新令牌 - 简洁版本
         */
      async refreshToken(refreshToken) {
        try {
            // 使用超时保护
            const timeoutPromise = new Promise((_, reject) => {
                setTimeout(() => reject(new Error('刷新令牌超时')), 10000);
            });

            const fetchPromise = Core.http.post(this.api.refreshTokenUrl, { refreshToken }, {
                loading: false,
                headers: {}
            });

            const response = await Promise.race([fetchPromise, timeoutPromise]);
            
            if (!response.success || !response.data?.accessToken) {
                throw new Error(response.message || '刷新令牌失败');
            }

            // 保存令牌
            localStorage.setItem('auth_token', response.data.accessToken);
            if (response.data.refreshToken) {
                localStorage.setItem('refresh_token', response.data.refreshToken);
            }

            // 处理等待队列
            this._processRequestQueue();
            
            return response.data.accessToken;
        } catch (error) {
            console.error('刷新令牌失败:', error);
            this._handleUnauthorized();
            throw error;
        } finally {
            this._refreshingToken = null;
        }
    },

        /**
         * 处理未授权 - 简洁版本
         */
        _handleUnauthorized: function () {
            Core.msg.error('登录已过期', {
                time: 500,
                end: function () {
                    // 移除所有相关存储
                    localStorage.removeItem('refresh_token');
                    localStorage.removeItem('auth_token');
                    window.location.href = '/account/login';
                }
            });
        },
        // ==================== 消息提示 ====================
        msg: {
            success: function (message,options) { return layer.msg(message, { icon: 1, time: 2000,...options }); },
            error: function (message,options) { return layer.msg(message, { icon: 2, time: 3000,...options }); },
            warning: function (message,options) { return layer.msg(message, { icon: 0, time: 2500,...options }); },
            loading: function (message,options) { return layer.msg(message || '加载中...', { icon: 16, shade: 0.01, time: 0,...options }); },
            close: function (index) { layer.close(index); }
        },

        // ==================== 确认对话框 ====================
        confirm: function (message, callback) {
            layer.confirm(message, { icon: 3, title: '操作确认' }, function (index) { callback && callback(); layer.close(index); });
        },

        // ==================== 模态框管理 ====================
        modal: {
            show: function (selector) { return new bootstrap.Modal(document.querySelector(selector)).show(); },
            hide: function (selector) { var modal = bootstrap.Modal.getInstance(document.querySelector(selector)); modal && modal.hide(); }
        },

        // ==================== 模板工具 ====================
        template: {
            render: function (templateId, data) {
                var template = document.getElementById(templateId);
                if (!template) { console.error('Template not found:', templateId); return ''; }
                var html = template.innerHTML;
                for (var key in data) { if (data.hasOwnProperty(key)) { html = html.replace(new RegExp('{{' + key + '}}', 'g'), data[key]); } }
                return html;
            },
            load: function (templateUrl) {
                return new Promise(function (resolve, reject) {
                    $.get(templateUrl)
                        .done(function (html) { $('body').append(html); resolve(); })
                        .fail(function (xhr, status, error) { console.error('Failed to load template:', templateUrl); reject(error); });
                });
            }
        }
    };

    // 导出模块
    exports('core', Core);
});