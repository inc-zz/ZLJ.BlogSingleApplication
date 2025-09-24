/**
 * 优化版 HTTP 请求封装
 * 基于 Layui + jQuery Ajax 实现
 */
layui.define(['layer', 'jquery'], function (exports) {
    'use strict';

    var $ = layui.jquery,
        layer = layui.layer,
        setter = layui.setter || {};

    // 默认配置
    var defaultConfig = {
        baseURL: setter.apiUrl || '',
        timeout: 10000,
        contentType: 'application/json; charset=utf-8',
        responseType: 'json'
    };

    // HTTP 状态码映射
    var HTTP_STATUS = {
        UNAUTHORIZED: 401,
        NOT_FOUND: 404,
        SERVICE_UNAVAILABLE: 503
    };

    // 响应状态码映射
    var RESPONSE_CODE = {
        LOGOUT: setter.response ? setter.response.statusCode.logout : 401
    };

    /**
     * HTTP 请求类
     */
    var HttpRequest = {
        /**
         * 发送请求
         * @param {Object} config 请求配置
         * @returns {Promise}
         */
        request: function (config) {
            var self = this;

            // 合并配置
            config = $.extend({}, defaultConfig, config);

            // 处理 URL
            if (!config.url.startsWith('http') && config.baseURL) {
                config.url = config.baseURL + config.url;
            }

            // 设置请求头
            config.headers = config.headers || {};
            if (!config.headers['Authorization']) {
                var token = this.getToken();
                if (token) {
                    config.headers['Authorization'] = token;
                }
            }

            // 处理数据
            if (config.data && typeof config.data === 'object') {
                if (config.contentType && config.contentType.indexOf('application/json') !== -1) {
                    config.data = JSON.stringify(config.data);
                } else if (config.contentType && config.contentType.indexOf('application/x-www-form-urlencoded') !== -1) {
                    config.data = $.param(config.data);
                }
            }

            return new Promise(function (resolve, reject) {
                var loadingIndex = null;

                // 显示加载提示
                if (config.showLoading !== false) {
                    loadingIndex = layer.msg('加载中...', {
                        icon: 16,
                        shade: 0.01,
                        time: 0
                    });
                }

                $.ajax({
                    url: config.url,
                    type: config.method || 'GET',
                    data: config.data,
                    dataType: config.responseType,
                    contentType: config.contentType,
                    headers: config.headers,
                    timeout: config.timeout,
                    beforeSend: config.beforeSend,

                    success: function (response, status, xhr) {
                        layer.close(loadingIndex);

                        // 处理成功回调
                        if (typeof config.success === 'function') {
                            config.success(response);
                        }

                        // 检查是否需要退出登录
                        if (response && response.code === RESPONSE_CODE.LOGOUT) {
                            self.handleLogout();
                            return;
                        }

                        resolve({
                            data: response,
                            status: xhr.status,
                            headers: xhr.getAllResponseHeaders(),
                            config: config
                        });
                    },

                    error: function (xhr, status, error) {
                        layer.close(loadingIndex);

                        var errorInfo = {
                            status: xhr.status,
                            statusText: xhr.statusText,
                            message: error,
                            config: config
                        };

                        // 处理错误回调
                        if (typeof config.error === 'function') {
                            config.error(errorInfo);
                        } else {
                            // 默认错误处理
                            self.handleError(errorInfo);
                        }

                        reject(errorInfo);
                    },

                    complete: function (xhr, status) {
                        layer.close(loadingIndex);

                        if (typeof config.complete === 'function') {
                            config.complete(xhr, status);
                        }
                    }
                });
            });
        },

        /**
         * GET 请求
         * @param {string} url 请求地址
         * @param {Object} params 查询参数
         * @param {Object} config 额外配置
         * @returns {Promise}
         */
        get: function (url, params, config) {
            config = config || {};
            config.url = url;
            config.method = 'GET';
            config.data = params;

            return this.request(config);
        },

        /**
         * POST 请求
         * @param {string} url 请求地址
         * @param {Object} data 请求数据
         * @param {Object} config 额外配置
         * @returns {Promise}
         */
        post: function (url, data, config) {
            config = config || {};
            config.url = url;
            config.method = 'POST';
            config.data = data;

            return this.request(config);
        },

        /**
         * PUT 请求
         * @param {string} url 请求地址
         * @param {Object} data 请求数据
         * @param {Object} config 额外配置
         * @returns {Promise}
         */
        put: function (url, data, config) {
            config = config || {};
            config.url = url;
            config.method = 'PUT';
            config.data = data;

            return this.request(config);
        },

        /**
         * DELETE 请求
         * @param {string} url 请求地址
         * @param {Object} config 额外配置
         * @returns {Promise}
         */
        delete: function (url, config) {
            config = config || {};
            config.url = url;
            config.method = 'DELETE';

            return this.request(config);
        },

        /**
         * 表单提交（文件上传）
         * @param {string} url 请求地址
         * @param {FormData} formData 表单数据
         * @param {Object} config 额外配置
         * @returns {Promise}
         */
        upload: function (url, formData, config) {
            config = config || {};
            config.url = url;
            config.method = 'POST';
            config.data = formData;
            config.contentType = false;
            config.processData = false;

            return this.request(config);
        },

        /**
         * JSONP 请求
         * @param {string} url 请求地址
         * @param {Object} params 查询参数
         * @param {Object} config 额外配置
         * @returns {Promise}
         */
        jsonp: function (url, params, config) {
            config = config || {};
            config.url = url;
            config.method = 'GET';
            config.data = params;
            config.dataType = 'jsonp';
            config.jsonpCallback = 'jsonp' + (new Date()).valueOf().toString().substr(-4);

            return this.request(config);
        },

        /**
         * 获取 Token
         * @returns {string}
         */
        getToken: function () {
            var userToken = '';

            // 尝试从不同来源获取 token
            if (typeof jUtils !== 'undefined' && jUtils.userToken) {
                userToken = jUtils.userToken();
            } else if (window.localStorage && window.localStorage.getItem) {
                userToken = window.localStorage.getItem('userToken') ||
                    window.localStorage.getItem('auth_token');
            }

            return userToken ? 'Bearer ' + userToken : '';
        },

        /**
         * 处理错误
         * @param {Object} error 错误信息
         */
        handleError: function (error) {
            var message = '请求失败，请稍后重试';

            switch (error.status) {
                case HTTP_STATUS.UNAUTHORIZED:
                    this.handleLogout();
                    return;

                case HTTP_STATUS.NOT_FOUND:
                    message = '请求的资源不存在';
                    break;

                case HTTP_STATUS.SERVICE_UNAVAILABLE:
                    message = '服务暂时不可用，请稍后重试';
                    break;

                default:
                    if (error.message) {
                        message = error.message;
                    }
            }

            layer.msg(message, { icon: 5, time: 3000 });
        },

        /**
         * 处理登出
         */
        handleLogout: function () {
            // 清除 token
            if (window.localStorage) {
                window.localStorage.removeItem('userToken');
                window.localStorage.removeItem('auth_token');
            }

            // 跳转到登录页
            var loginPage = '/Indecor/login-register.html';
            if (typeof admin !== 'undefined' && admin.exit) {
                admin.exit();
            } else {
                setTimeout(function () {
                    window.location.href = loginPage;
                }, 1500);
            }

            layer.msg('登录已过期，请重新登录', { icon: 5, time: 1500 });
        },

        /**
         * 设置基础配置
         * @param {Object} config 配置对象
         */
        setConfig: function (config) {
            $.extend(defaultConfig, config);
        }
    };

    /**
     * 工具函数集合
     */
    var Utils = {
        /**
         * 字符串转数组
         * @param {string} str 字符串
         * @param {string} separator 分隔符
         * @returns {Array}
         */
        stringToArray: function (str, separator) {
            separator = separator || ',';
            if (!str || typeof str !== 'string') return [];
            return str.split(separator);
        },

        /**
         * 获取数组长度
         * @param {string} str 字符串
         * @param {string} separator 分隔符
         * @returns {number}
         */
        stringToArrayLength: function (str, separator) {
            return this.stringToArray(str, separator).length;
        },

        /**
         * 生成随机编码
         * @param {string} prefix 前缀
         * @returns {string}
         */
        generateSN: function (prefix) {
            prefix = prefix || '';
            var timestamp = new Date().getTime();
            var random = Math.floor(Math.random() * 10000);
            return prefix + timestamp + random;
        },

        /**
         * 时间格式化
         * @param {Date} date 日期对象
         * @returns {string}
         */
        formatDate: function (date) {
            if (!(date instanceof Date)) {
                date = new Date(date);
            }

            var year = date.getFullYear(),
                month = (date.getMonth() + 1).toString().padStart(2, '0'),
                day = date.getDate().toString().padStart(2, '0'),
                hour = date.getHours().toString().padStart(2, '0'),
                minute = date.getMinutes().toString().padStart(2, '0'),
                second = date.getSeconds().toString().padStart(2, '0');

            return year + '-' + month + '-' + day + ' ' + hour + ':' + minute + ':' + second;
        },

        /**
         * 获取带样式的标签
         * @param {string} style 样式名称
         * @returns {string}
         */
        getLabelClass: function (style) {
            var classes = {
                'red': '',
                'green': 'layui-bg-green',
                'orange': 'layui-bg-orange',
                'blue': 'layui-bg-blue',
                'cyan': 'layui-bg-cyan',
                'black': 'layui-bg-black'
            };

            return classes[style] || '';
        },

        /**
         * 生成标签 HTML
         * @param {Array|Object} labels 标签数据
         * @returns {string}
         */
        generateLabels: function (labels) {
            if (!labels) return '';

            var html = '';

            if (Array.isArray(labels)) {
                labels.forEach(function (label) {
                    var styleClass = this.getLabelClass(label.style);
                    html += '<span class="layui-badge ' + styleClass + '">' + label.name + '</span>&nbsp;';
                }.bind(this));
            } else if (typeof labels === 'object') {
                var styleClass = this.getLabelClass(labels.style);
                html = '<span class="layui-badge ' + styleClass + '">' + labels.name + '</span>';
            }

            return html;
        },

        /**
         * 获取 URL 查询参数
         * @returns {Object}
         */
        getQueryParams: function () {
            var queryString = window.location.search.substring(1);
            var params = {};

            if (queryString) {
                var pairs = queryString.split('&');
                pairs.forEach(function (pair) {
                    var keyValue = pair.split('=');
                    if (keyValue.length === 2) {
                        params[decodeURIComponent(keyValue[0])] = decodeURIComponent(keyValue[1]);
                    }
                });
            }

            return params;
        },

        /**
         * 获取 URL hash 参数
         * @returns {string}
         */
        getHashParam: function () {
            return window.location.hash.substring(1);
        },

        /**
         * 设置 Cookie
         * @param {string} name 名称
         * @param {string} value 值
         * @param {number} days 有效期（天）
         */
        setCookie: function (name, value, days) {
            days = days || 30;
            var expires = '';

            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                expires = '; expires=' + date.toUTCString();
            }

            document.cookie = name + '=' + encodeURIComponent(value) + expires + '; path=/';
        },

        /**
         * 获取 Cookie
         * @param {string} name 名称
         * @returns {string|null}
         */
        getCookie: function (name) {
            var nameEQ = name + '=';
            var cookies = document.cookie.split(';');

            for (var i = 0; i < cookies.length; i++) {
                var cookie = cookies[i].trim();
                if (cookie.indexOf(nameEQ) === 0) {
                    return decodeURIComponent(cookie.substring(nameEQ.length));
                }
            }

            return null;
        },

        /**
         * 删除 Cookie
         * @param {string} name 名称
         */
        removeCookie: function (name) {
            this.setCookie(name, '', -1);
        },

        /**
         * 预览图片
         * @param {string} imgUrl 图片地址
         */
        previewImage: function (imgUrl) {
            layer.open({
                type: 1,
                title: false,
                closeBtn: 0,
                scrollbar: false,
                skin: 'layui-layer-nobg',
                shadeClose: true,
                content: '<img style="max-width: 350px;max-height: 350px;" src="' + imgUrl + '">'
            });
        }
    };

    // 合并导出对象
    var coreHelper = $.extend({}, HttpRequest, Utils, {
        /**
         * 日志输出
         * @param {*} message 日志内容
         */
        log: function (message) {
            if (console && console.log) {
                console.log('CoreHelper: ', message);
            }
        }
    });

    // 输出接口
    exports('coreHelper', coreHelper);
});