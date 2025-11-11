/**
 * HTTP请求封装类
 * 统一处理请求和响应，支持拦截器
 */
// 基础配置
const BASE_URL = import.meta.env.VITE_API_BASE_URL;
const TIMEOUT = 60000; // 1分钟超时

// Message 全局实例（将由 App 组件初始化）
let messageInstance = null;

export const setMessageInstance = (instance) => {
  messageInstance = instance;
};

const showMessage = (type, content) => {
  if (messageInstance && messageInstance[type]) {
    messageInstance[type](content);
  } else {
    console.warn(`Message instance not available. ${type}: ${content}`);
  }
};

class Http {
  constructor() {
    this.baseURL = BASE_URL;
    this.timeout = TIMEOUT;
    this.requestInterceptors = [];
    this.responseInterceptors = [];
    
    // 添加默认拦截器
    this.setupDefaultInterceptors();
  }

  /**
   * 设置默认拦截器
   */
  setupDefaultInterceptors() {
    // 请求拦截器 - 添加Token
    this.addRequestInterceptor((config) => {
      const token = localStorage.getItem('blogs_token');
      if (token) {
        config.headers = {
          ...config.headers,
          'Authorization': `Bearer ${token}`,
        };
      }
      
      // 添加默认headers（但不包括 FormData）
      // FormData 需要浏览器自动设置 Content-Type 和 boundary
      if (!(config.body instanceof FormData)) {
        config.headers = {
          'Content-Type': 'application/json',
          ...config.headers,
        };
      }
      
      console.log('📤 Request:', config.method?.toUpperCase(), config.url, config);
      return config;
    });

    // 响应拦截器 - 统一错误处理
    this.addResponseInterceptor(
      (response) => {
        console.log('📥 Response:', response.status, response.data);
        return response;
      },
      (error) => {
        console.error('❌ Request Error:', error);
        
        if (error.response) {
          const { status, data } = error.response;
          
          switch (status) {
            case 401:
              // 未授权，不自动跳转，由组件处理
              console.warn('⚠️ 401 Unauthorized');
              // 不在这里清除 token，由组件决定是否清除
              break;
              
            case 403:
              console.warn('⚠️ 403 Forbidden - Access denied');
              showMessage('error', '访问被拒绝，您没有权限执行此操作');
              break;
              
            case 404:
              console.warn('⚠️ 404 Not Found');
              showMessage('error', '请求的资源不存在');
              break;
              
            case 500:
              console.error('❌ 500 Server Error');
              showMessage('error', '服务器出现异常，请稍后重试');
              break;
              
            case 502:
            case 503:
            case 504:
              console.error('❌ Server Unavailable');
              showMessage('error', '服务器暂时无法访问，请稍后重试');
              break;
              
            default:
              showMessage('error', data?.message || '请求失败，请重试');
          }
        } else if (error.request) {
          // 请求发送但没有收到响应
          console.error('❌ No response received');
          showMessage('error', '网络连接失败，请检查您的网络');
        } else {
          // 请求配置出错
          console.error('❌ Request setup error:', error.message);
          showMessage('error', '请求配置错误');
        }
        
        return Promise.reject(error);
      }
    );
  }

  /**
   * 添加请求拦截器
   */
  addRequestInterceptor(onFulfilled, onRejected) {
    this.requestInterceptors.push({ onFulfilled, onRejected });
  }

  /**
   * 添加响应拦截器
   */
  addResponseInterceptor(onFulfilled, onRejected) {
    this.responseInterceptors.push({ onFulfilled, onRejected });
  }

  /**
   * 执行请求拦截器
   */
  async executeRequestInterceptors(config) {
    let processedConfig = config;
    
    for (const interceptor of this.requestInterceptors) {
      try {
        if (interceptor.onFulfilled) {
          processedConfig = await interceptor.onFulfilled(processedConfig);
        }
      } catch (error) {
        if (interceptor.onRejected) {
          return interceptor.onRejected(error);
        }
        throw error;
      }
    }
    
    return processedConfig;
  }

  /**
   * 执行响应拦截器
   */
  async executeResponseInterceptors(response) {
    let processedResponse = response;
    
    for (const interceptor of this.responseInterceptors) {
      try {
        if (interceptor.onFulfilled) {
          processedResponse = await interceptor.onFulfilled(processedResponse);
        }
      } catch (error) {
        if (interceptor.onRejected) {
          return interceptor.onRejected(error);
        }
        throw error;
      }
    }
    
    return processedResponse;
  }

  /**
   * 核心请求方法
   */
  async request(config) {
    try {
      // 执行请求拦截器
      const processedConfig = await this.executeRequestInterceptors(config);
      
      // 构建完整URL
      const url = processedConfig.url.startsWith('http') 
        ? processedConfig.url 
        : `${this.baseURL}${processedConfig.url}`;
      
      // 设置超时
      const controller = new AbortController();
      const timeoutId = setTimeout(() => controller.abort(), this.timeout);
      
      // 发送请求
      const response = await fetch(url, {
        method: processedConfig.method || 'GET',
        headers: processedConfig.headers,
        body: processedConfig.body instanceof FormData 
          ? processedConfig.body  // FormData 不需要 JSON.stringify
          : (processedConfig.body ? JSON.stringify(processedConfig.body) : undefined),
        signal: controller.signal,
        ...processedConfig.options,
      });
      
      clearTimeout(timeoutId);
      
      // 解析响应
      const data = await response.json().catch(() => null);
      
      const result = {
        status: response.status,
        statusText: response.statusText,
        headers: response.headers,
        data,
        config: processedConfig,
      };
      
      // 检查HTTP状态码
      if (!response.ok) {
        const error = new Error(response.statusText);
        error.response = result;
        throw error;
      }
      
      // 执行响应拦截器
      return await this.executeResponseInterceptors(result);
      
    } catch (error) {
      // 执行响应拦截器的错误处理
      for (const interceptor of this.responseInterceptors) {
        if (interceptor.onRejected) {
          try {
            return await interceptor.onRejected(error);
          } catch (e) {
            // 继续抛出错误
          }
        }
      }
      throw error;
    }
  }

  /**
   * GET请求
   */
  get(url, params = {}, config = {}) {
    // 构建查询字符串
    const queryString = Object.keys(params)
      .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(params[key])}`)
      .join('&');
    
    const fullUrl = queryString ? `${url}?${queryString}` : url;
    
    return this.request({
      url: fullUrl,
      method: 'GET',
      ...config,
    });
  }

  /**
   * POST请求
   */
  post(url, data = {}, config = {}) {
    return this.request({
      url,
      method: 'POST',
      body: data,
      ...config,
    });
  }

  /**
   * PUT请求
   */
  put(url, data = {}, config = {}) {
    return this.request({
      url,
      method: 'PUT',
      body: data,
      ...config,
    });
  }

  /**
   * DELETE请求
   */
  delete(url, config = {}) {
    return this.request({
      url,
      method: 'DELETE',
      ...config,
    });
  }

  /**
   * PATCH请求
   */
  patch(url, data = {}, config = {}) {
    return this.request({
      url,
      method: 'PATCH',
      body: data,
      ...config,
    });
  }
}

// 创建实例
const http = new Http();

// 导出实例和类
export default http;
export { Http };
