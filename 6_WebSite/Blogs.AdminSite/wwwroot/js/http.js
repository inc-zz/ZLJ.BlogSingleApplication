// http.js - HTTP请求封装模块
(function (root, factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD规范
        define(['jquery'], factory);
    } else if (typeof exports === 'object') {
        // CommonJS规范
        module.exports = factory(require('jquery'));
    } else {
        // 浏览器全局变量
        root.HttpClient = factory(root.jQuery);
    }
}(this, function ($) {
    'use strict';

    // HTTP请求类
    class HttpClient {
        constructor(options = {}) {
            this.baseURL = options.baseURL || '';
            this.timeout = options.timeout || 10000;
            this.headers = options.headers || {
                'Content-Type': 'application/json'
            };

            // 请求拦截器
            this.requestInterceptors = [];
            // 响应拦截器
            this.responseInterceptors = [];
        }

        // 添加请求拦截器
        addRequestInterceptor(interceptor) {
            console.log("===================添加请求拦截器======================");
            this.requestInterceptors.push(interceptor);
            return this;
        }

        // 添加响应拦截器
        addResponseInterceptor(interceptor) {
            console.log("===================添加响应拦截器======================");
            this.responseInterceptors.push(interceptor);
            return this;
        }

        // 执行请求拦截器
        _applyRequestInterceptors(config) {
            return this.requestInterceptors.reduce((cfg, interceptor) => {
                console.log("===================执行请求拦截器======================");
                return interceptor(cfg) || cfg;
            }, config);
        }

        // 执行响应拦截器
        _applyResponseInterceptors(response) {
            return this.responseInterceptors.reduce((resp, interceptor) => {
                console.log("===================执行响应拦截器======================");
                return interceptor(resp) || resp;
            }, response);
        }

        // 发送请求
        _request(method, url, data = null, options = {}) {
            console.log("===================执行响应拦截器======================");
            return new Promise((resolve, reject) => {
                // 构建请求配置
                let config = {
                    url: this.baseURL + url,
                    type: method,
                    timeout: this.timeout,
                    headers: { ...this.headers, ...options.headers },
                    data: data,
                    dataType: 'json',
                    contentType: 'application/json',
                    success: (response, status, xhr) => {
                        resolve(this._applyResponseInterceptors({
                            data: response,
                            status: xhr.status,
                            statusText: xhr.statusText,
                            headers: xhr.getAllResponseHeaders(),
                            config: config
                        }));
                    },
                    error: (xhr, status, error) => {
                        reject({
                            status: xhr.status,
                            statusText: xhr.statusText,
                            message: error,
                            config: config
                        });
                    }
                };

                // 处理数据
                if (data && method !== 'GET') {
                    config.data = JSON.stringify(data);
                }

                // 应用请求拦截器
                config = this._applyRequestInterceptors(config);

                // 发送请求
                $.ajax(config);
            });
        }

        // GET请求
        get(url, params = {}, options = {}) {
            return this._request('GET', url, params, options);
        }

        // POST请求
        post(url, data = {}, options = {}) {
            return this._request('POST', url, data, options);
        }

        // PUT请求
        put(url, data = {}, options = {}) {
            return this._request('PUT', url, data, options);
        }

        // DELETE请求
        delete(url, options = {}) {
            return this._request('DELETE', url, null, options);
        }

        // 上传文件
        upload(url, formData, options = {}) {
            options.headers = options.headers || {};
            // 上传文件时不设置Content-Type，让浏览器自动设置
            delete options.headers['Content-Type'];

            return this._request('POST', url, formData, options);
        }
    }

    // 创建默认实例
    const defaultInstance = new HttpClient({
        baseURL: 'http://localhost:5001/api'
    });

    // 添加认证拦截器
    defaultInstance.addRequestInterceptor(function (config) {
        // 从localStorage获取token
        const token = localStorage.getItem('auth_token');
        if (token) {
            config.headers = config.headers || {};
            config.headers['Authorization'] = 'Bearer ' + token;
        }
        return config;
    });

    // 添加响应拦截器 - 处理通用错误
    defaultInstance.addResponseInterceptor(function (response) {
        if (response.status >= 400) {
            // 可以在这里统一处理错误
            console.error('HTTP Error:', response.status, response.statusText);
        }
        return response;
    });

    // 导出
    return {
        HttpClient: HttpClient,
        create: function (options) {
            return new HttpClient(options);
        },
        // 默认实例
        default: defaultInstance
    };
}));