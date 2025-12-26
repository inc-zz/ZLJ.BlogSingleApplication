// API 封装层 - 使用统一的http请求类

import http from './http';
import { 
  mockArticles, 
  mockUsers, 
  mockComments, 
  getArticlesByTechStack
} from './mockData';

// 模拟网络延迟
const delay = (ms = 300) => new Promise(resolve => setTimeout(resolve, ms));

// ==================== 真实API调用 ====================

/**
 * 获取热门文章分类列表
 * @param {number} topCount - 获取前N个分类
 * @returns {Promise<ResultObject>}
 */
export const getCategories = async (topCount = 5) => {
  try {
    const response = await http.get('/Article/hot', { TopCount: topCount });
    return response.data;
  } catch (error) {
    console.error('获取分类失败:', error);
    return {
      success: false,
      message: '获取分类失败',
      code: 500
    };
  }
};

/**
 * 获取文章列表（分页）
 * @param {Object} params - 查询参数
 * @param {number} params.CategoryId - 分类ID
 * @param {number} params.TagId - 标签ID（暂未对接）
 * @param {string} params.SortBy - 排序字段（暂未对接）
 * @param {number} params.PageIndex - 当前页码
 * @param {number} params.PageSize - 每页数量
 * @param {string} params.Where - 搜索关键词
 * @returns {Promise<ResultObject>}
 */
export const getArticleList = async (params = {}) => {
  try {
    const {
      CategoryId,
      TagId = null,      // 标签ID
      SortBy = null,     // 排序字段
      PageIndex = 1,
      PageSize = 20,
      Where = ''
    } = params;

    const queryParams = {
      PageIndex,
      PageSize
    };

    // 只添加有值的参数
    if (CategoryId) queryParams.CategoryId = CategoryId;
    if (TagId) queryParams.TagId = TagId;
    if (SortBy) queryParams.SortBy = SortBy;
    if (Where) queryParams.Where = Where;

    const response = await http.get('/Article/list', queryParams);
    return response.data;
  } catch (error) {
    console.error('获取文章列表失败:', error);
    return {
      success: false,
      message: '获取文章列表失败',
      code: 500,
      pageIndex: params.PageIndex || 1,
      pageSize: params.PageSize || 20,
      total: 0,
      items: []
    };
  }
};

/**
 * 获取文章详情
 * @param {string} id - 文章ID
 * @returns {Promise<ResultObject>}
 */
export const getArticleInfo = async (id) => { 
  try {
    const response  = await http.get('/article/detail', { ArticleId: id });
    return response.data;
  }catch(error){
    console.error('获取文章详情失败:', error);
    return {
      success: false,
      message: '获取文章详情失败',
      code: 500,
      data: {}
    };
  }
};


/**
 * 获取推荐文章
 * @param {number} topCount - 获取前N篇推荐文章
 * @returns {Promise<ResultObject>}
 */
export const getRecommendedArticles = async (topCount = 10) => {
  try {
    const response = await http.get('/article/recommendations', { TopCount: topCount });
    return response.data;
  } catch (error) {
    console.error('获取推荐文章失败:', error);
    return {
      success: false,
      message: '获取推荐文章失败',
      code: 500,
      data: []
    };
  }
};

/**
 * 获取推荐开源项目
 * @param {number} topCount - 获取前N个推荐项目
 * @returns {Promise<ResultObject>}
 */
export const getOpenSourceProjects = async (topCount = 10) => {
  try {
    const response = await http.get('/article/opensourceproject', { TopCount: topCount });
    return response.data;
  } catch (error) {
    console.error('获取推荐项目失败:', error);
    return {
      success: false,
      message: '获取推荐项目失败',
      code: 500,
      data: []
    };
  }
};

/**
 * 获取技术标签列表
 * @param {number} topicCount - 获取前N个标签
 * @returns {Promise<ResultObject>}
 */
export const getTags = async (topicCount = 30) => {
  try {
    const response = await http.get('/Article/tags', { TopicCount: topicCount });
    return response.data;
  } catch (error) {
    console.error('获取标签失败:', error);
    return {
      success: false,
      message: '获取标签失败',
      code: 500,
      data: []
    };
  }
};

/**
 * 发布文章
 * @param {Object} articleData - 文章数据
 * @param {string} articleData.title - 文章标题
 * @param {string} articleData.summary - 文章简介
 * @param {number} articleData.categoryId - 分类ID
 * @param {string} articleData.tags - 标签（逗号分隔）
 * @param {string} articleData.content - 文章内容
 * @param {boolean} articleData.isPublish - 是否发布
 * @returns {Promise<ResultObject>}
 */
export const publishArticle = async (articleData) => {
  try {
    const response = await http.post('/Article/publish', articleData);
    return response.data;
  } catch (error) {
    console.error('发布文章失败:', error);
    // 如果是401错误，向上抛出
    if (error.response?.status === 401) {
      throw error;
    }
    return {
      success: false,
      message: error.response?.data?.message || '发布文章失败',
      code: 500
    };
  }
};

/**
 * 更新文章
 * @param {string} articleId - 文章ID
 * @param {Object} articleData - 文章数据
 * @returns {Promise<ResultObject>}
 */
export const updateArticle = async (articleId, articleData) => {
  try { 
    // const { id, ...articleData } = articleData;
    articleData.id = articleId;
    const response = await http.post(`/Article/publish`, articleData);
    return response.data;
  } catch (error) {
    console.error('更新文章失败:', error);
    if (error.response?.status === 401) {
      throw error;
    }
    return {
      success: false,
      message: error.response?.data?.message || '更新文章失败',
      code: 500
    };
  }
};

/**
 * 删除文章
 * @param {string} articleId - 文章ID
 * @returns {Promise<ResultObject>}
 */
export const deleteArticle = async (articleId) => {
  try {
    const response = await http.delete('/Article', { id: articleId });
    return response.data;
  } catch (error) {
    console.error('删除文章失败:', error);
    if (error.response?.status === 401) {
      throw error;
    }
    return {
      success: false,
      message: error.response?.data?.message || '删除文章失败',
      code: 500
    };
  }
};

/**
 * 隐藏/显示文章
 * @param {string} articleId - 文章ID
 * @param {boolean} isHide - 是否隐藏 (1=隐藏, 0=显示)
 * @returns {Promise<ResultObject>}
 */
export const toggleArticleVisibility = async (articleId, isHide) => {
  try {
    const response = await http.post('/Article/status', { 
      ArticleId: articleId, 
      IsHide: isHide ? 1 : 0 
    });
    return response.data;
  } catch (error) {
    console.error('更改文章可见性失败:', error);
    if (error.response?.status === 401) {
      throw error;
    }
    return {
      success: false,
      message: error.response?.data?.message || '操作失败',
      code: 500
    };
  }
};

/**
 * 获取文章评论列表
 * @param {string} articleId - 文章ID
 * @param {number} pageIndex - 页码
 * @param {number} pageSize - 每页数量
 * @returns {Promise<ResultObject>}
 */
export const getArticleComments = async (articleId, pageIndex = 1, pageSize = 10) => {
  try {
    const response = await http.get('/articleComment/list', {
      articleId,
      pageIndex,
      pageSize
    });
    return response.data;
  } catch (error) {
    console.error('获取评论列表失败:', error);
    return {
      success: false,
      message: '获取评论列表失败',
      code: 500,
      items: []
    };
  }
};

/**
 * 发表评论
 * @param {string} articleId - 文章ID
 * @param {string} content - 评论内容
 * @returns {Promise<ResultObject>}
 */
export const postComment = async (articleId, content) => {
  try {
    const response = await http.post('/articleComment', {
      articleId,
      content
    });
    return response.data;
  } catch (error) {
    console.error('发表评论失败:', error);
    if (error.response?.status === 401) {
      throw error;
    }
    return {
      success: false,
      message: error.response?.data?.message || '发表评论失败',
      code: 500
    };
  }
};

/**
 * 回复评论
 * @param {string} articleId - 文章ID
 * @param {number} parentId - 父评论ID
 * @param {string} content - 回复内容
 * @returns {Promise<ResultObject>}
 */
export const replyComment = async (articleId, parentId, content) => {
  try {
    const response = await http.post('/articleComment/reply', {
      articleId,
      parentId,
      content
    });
    return response.data;
  } catch (error) {
    console.error('回复评论失败:', error);
    if (error.response?.status === 401) {
      throw error;
    }
    return {
      success: false,
      message: error.response?.data?.message || '回复评论失败',
      code: 500
    };
  }
};

/**
 * 删除评论
 * @param {number} commentId - 评论ID
 * @returns {Promise<ResultObject>}
 */
export const deleteComment = async (commentId) => {
  try {
    var jsonData = { id: commentId };
    const response = await http.delete('/articleComment', jsonData);
    return response.data;

  } catch (error) {
    console.error('删除评论失败:', error);
    if (error.response?.status === 401) {
      throw error;
    }
    return {
      success: false,
      message: error.response?.data?.message || '删除评论失败',
      code: 500
    };
  }
};

/**
 * 获取相关文章推荐
 * @param {string} articleId - 文章ID
 * @returns {Promise<ResultObject>}
 */
export const getRelatedArticles = async (articleId) => {
  try {
    const response = await http.get('/Article/related', {
      ArticleId: articleId
    });
    return response.data;
  } catch (error) {
    console.error('获取相关文章失败:', error);
    return {
      success: false,
      message: '获取相关文章失败',
      code: 500,
      data: []
    };
  }
};

/**
 * 获取我的文章列表
 * @param {number} pageIndex - 页码
 * @param {number} pageSize - 每页数量
 * @param {string} where - 搜索关键词
 * @returns {Promise<ResultObject>}
 */
export const getMyArticles = async (pageIndex = 1, pageSize = 20, where = '') => {
  try {
    // 注意: 这里使用 /admin/Article/list 而不是 /Article/MyArticles
    // 因为 BASE_URL 已经包含 /api, 所以需要使用相对路径
    // 完整路径为: https://localhost:7235 + /list
    const baseUrl = import.meta.env.VITE_API_BASE_URL.replace('/api/app', '');
    const params = {
      page: pageIndex,
      pageSize: pageSize,
      keyword: where || '',
      status: ''
    };
    
    const queryString = Object.keys(params)
      .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(params[key])}`)
      .join('&');
    
    const fullUrl = `${baseUrl}/api/app/Article/myArticles?${queryString}`;
    
    const token = localStorage.getItem('blogs_token');
    const headers = {
      'Content-Type': 'application/json'
    };
    
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }
    
    console.log('📤 Fetching My Articles:', fullUrl);
    
    const response = await fetch(fullUrl, {
      method: 'GET',
      headers
    });
    
    const data = await response.json();
    console.log('📥 My Articles Response:', data);
    
    return data;
  } catch (error) {
    console.error('获取我的文章失败:', error);
    return {
      success: false,
      message: '获取我的文章失败',
      code: 500,
      pageIndex,
      pageSize,
      total: 0,
      items: []
    };
  }
};

// ==================== Mock API (临时使用) ====================

// 基础请求函数
const request = async (url, options = {}) => {
  await delay();
  
  // 这里可以直接替换为真实的API调用
  // return fetch(url, options).then(res => res.json());
  
  // 当前返回mock数据
  return mockRequest(url, options);
};

// 模拟请求处理
const mockRequest = async (url, options) => {
  const { method = 'GET', body } = options;
  
  // 解析URL和参数
  const urlObj = new URL(url, 'http://localhost');
  const path = urlObj.pathname;
  const params = Object.fromEntries(urlObj.searchParams);
  
  console.log(`API Request: ${method} ${path}`, params, body);
  
  // 路由处理
  if (path === '/api/auth/login') {
    return handleLogin(JSON.parse(body));
  } else if (path === '/api/articles/search') {
    return handleSearchArticles(params);
  } else if (path === '/api/articles') {
    return handleGetArticles(params);
  } else if (path.startsWith('/api/articles/')) {
    const id = path.split('/').pop();
    return handleGetArticle(id);
  } else if (path === '/api/articles/create') {
    return handleCreateArticle(JSON.parse(body));
  } else if (path === '/api/comments') {
    return handleGetComments(params);
  } else if (path === '/api/comments/create') {
    return handleCreateComment(JSON.parse(body));
  } else if (path.startsWith('/api/articles/') && path.endsWith('/like')) {
    const id = path.split('/')[3];
    return handleLikeArticle(id);
  } else if (path.startsWith('/api/articles/') && path.endsWith('/collect')) {
    const id = path.split('/')[3];
    return handleCollectArticle(id);
  }
  
  return { success: false, message: 'API not found' };
};

// API 方法

// 用户登录
export const loginUser = (username, password) => {
  return request('/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ username, password })
  });
};

const handleLogin = ({ username, password }) => {
  const user = mockUsers[username];
  
  if (!user) {
    return { success: false, message: '用户名不存在' };
  }
  
  if (user.password !== password) {
    return { success: false, message: '密码错误' };
  }
  
  return {
    success: true,
    data: {
      username: user.username,
      nickname: user.nickname,
      email: user.email,
      avatar: user.avatar,
    }
  };
};

// 获取文章列表
export const getArticles = (techStack = 'all', page = 1, limit = 10) => {
  return request(`/api/articles?techStack=${techStack}&page=${page}&limit=${limit}`);
};

// 搜索文章
export const searchArticles = (query) => {
  return request(`/api/articles/search?q=${encodeURIComponent(query)}`);
};

const handleGetArticles = ({ techStack = 'all', page = 1, limit = 10 }) => {
  const articles = getArticlesByTechStack(techStack === 'all' ? null : techStack);
  const start = (page - 1) * limit;
  const end = start + parseInt(limit);
  
  return {
    success: true,
    data: {
      articles: articles.slice(start, end),
      total: articles.length,
      page: parseInt(page),
      limit: parseInt(limit)
    }
  };
};

// 搜索文章
const handleSearchArticles = ({ q }) => {
  if (!q || !q.trim()) {
    return { success: false, message: '搜索关键词不能为空' };
  }
  
  const query = q.toLowerCase();
  const allArticles = getArticlesByTechStack();
  
  // 搜索所有字段：标题、内容、摘要、作者
  const results = allArticles.filter(article => {
    return article.title.toLowerCase().includes(query) ||
           article.content?.toLowerCase().includes(query) ||
           article.summary?.toLowerCase().includes(query) ||
           article.author.toLowerCase().includes(query);
  });
  
  return {
    success: true,
    data: {
      articles: results,
      total: results.length,
      query: q
    }
  };
};

// 获取单篇文章
export const getArticle = (id) => {
  return request(`/api/articles/${id}`);
};

const handleGetArticle = (id) => {
  const article = getArticleById(id);
  
  if (!article) {
    return { success: false, message: '文章不存在' };
  }
  
  return { success: true, data: article };
};

// 创建文章
export const createArticle = (articleData) => {
  return request('/api/articles/create', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(articleData)
  });
};

const handleCreateArticle = (articleData) => {
  const newArticle = {
    id: Date.now(),
    ...articleData,
    likes: 0,
    views: 0,
    comments: 0,
    isLiked: false,
    isCollected: false,
    createdAt: new Date().toLocaleDateString('zh-CN')
  };
  
  // 在实际应用中，这里应该保存到数据库
  mockArticles.unshift(newArticle);
  
  return { success: true, data: newArticle };
};

// 获取评论
export const getComments = (articleId) => {
  return request(`/api/comments?articleId=${articleId}`);
};

const handleGetComments = ({ articleId }) => {
  // 返回该文章的所有评论
  return { success: true, data: mockComments };
};

// 创建评论
export const createComment = (commentData) => {
  return request('/api/comments/create', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(commentData)
  });
};

const handleCreateComment = (commentData) => {
  const newComment = {
    id: Date.now(),
    ...commentData,
    likes: 0,
    replies: [],
    createdAt: new Date().toLocaleString('zh-CN')
  };
  
  mockComments.unshift(newComment);
  
  return { success: true, data: newComment };
};

// 点赞文章
export const likeArticle = (articleId) => {
  return request(`/api/articles/${articleId}/like`, { method: 'POST' });
};

const handleLikeArticle = (articleId) => {
  const article = getArticleById(articleId);
  
  if (!article) {
    return { success: false, message: '文章不存在' };
  }
  
  // 切换点赞状态
  article.isLiked = !article.isLiked;
  article.likes = article.isLiked ? article.likes + 1 : article.likes - 1;
  
  return { 
    success: true, 
    data: { 
      isLiked: article.isLiked, 
      likes: article.likes 
    } 
  };
};

// 收藏文章
export const collectArticle = (articleId) => {
  return request(`/api/articles/${articleId}/collect`, { method: 'POST' });
};

const handleCollectArticle = (articleId) => {
  const article = getArticleById(articleId);
  
  if (!article) {
    return { success: false, message: '文章不存在' };
  }
  
  // 切换收藏状态
  article.isCollected = !article.isCollected;
  
  return { 
    success: true, 
    data: { 
      isCollected: article.isCollected 
    } 
  };
};

// 上传图片（模拟）
export const uploadImage = async (file) => {
  await delay(1000);
  
  // 实际应用中，这里应该上传到服务器或OSS
  // 这里返回一个模拟的URL
  return {
    success: true,
    data: {
      url: URL.createObjectURL(file)
    }
  };
};

export default {
  loginUser,
  getArticles,
  getArticle,
  searchArticles,
  createArticle,
  getComments,
  createComment,
  likeArticle,
  collectArticle,
  uploadImage
};
