import { useState, useEffect, useMemo } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { FaHeart, FaRegHeart, FaBookmark, FaRegBookmark, FaShare, FaArrowLeft, FaEye, FaEdit, FaTrash, FaEyeSlash, FaReply } from 'react-icons/fa';
import LoginModal from '../components/LoginModal';
import { getArticleInfo, deleteArticle, toggleArticleVisibility, getArticleComments, postComment, replyComment, deleteComment, getRelatedArticles } from '../utils/api';
import { App, Input } from 'antd';
import { marked } from 'marked';
import DOMPurify from 'dompurify';

// 配置marked
marked.setOptions({
  breaks: true,        // 支持GFM换行
  gfm: true,           // 启用GitHub Flavored Markdown
  headerIds: true,     // 为标题添加ID
  mangle: false,       // 不转义邮箱地址
});

const ArticleDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { isAuthenticated, user } = useAuth();
  const { message, modal } = App.useApp();
  
  const [article, setArticle] = useState(null);
  const [isLiked, setIsLiked] = useState(false);
  const [isCollected, setIsCollected] = useState(false);
  const [isFollowing, setIsFollowing] = useState(false);
  const [likes, setLikes] = useState(0);
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState('');
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [showSidebar, setShowSidebar] = useState(true);
  const [loading, setLoading] = useState(true);
  const [isAuthor, setIsAuthor] = useState(false);
  const [loadingComments, setLoadingComments] = useState(false);
  const [replyingTo, setReplyingTo] = useState(null);
  const [replyContent, setReplyContent] = useState('');
  const [relatedArticles, setRelatedArticles] = useState([]);
  const [loadingRelated, setLoadingRelated] = useState(false);

  // 检测内容类型
  const detectContentType = (content) => {
    if (!content) return 'plain';
    
    const trimmedContent = content.trim();
    
    // 检测是否为HTML文档（以<!DOCTYPE html>或<!doctype html>开头）
    if (/^<!DOCTYPE\s+html/i.test(trimmedContent) || /^<!doctype\s+html/i.test(trimmedContent)) {
      return 'html';
    }
    
    // 检测Markdown特征
    const markdownPatterns = [
      /^#{1,6}\s+.+/m,                    // 标题: # Heading
      /^[*\-+]\s+.+/m,                    // 无序列表: * item, - item, + item
      /^\d+\.\s+.+/m,                    // 有序列表: 1. item
      /```[\s\S]*?```/,                   // 代码块: ```code```
      /`[^`]+`/,                          // 行内代码: `code`
      /^>\s+.+/m,                         // 引用块: > quote
      /\|.+\|.+\|/,                       // 表格: | col1 | col2 |
      /\[.+\]\(.+\)/,                     // 链接: [text](url)
      /!\[.+\]\(.+\)/,                    // 图片: ![alt](url)
      /^\*{3,}$|^-{3,}$|^_{3,}$/m,       // 分隔线: ---, ***, ___
      /\*\*.+\*\*/,                       // 加粗: **bold**
      /__.+__/,                           // 加粗: __bold__
      /\*.+\*/,                           // 斜体: *italic*
      /_.+_/,                             // 斜体: _italic_
    ];
    
    // 如果匹配多个Markdown特征，认为是Markdown
    const matchCount = markdownPatterns.filter(pattern => pattern.test(trimmedContent)).length;
    if (matchCount >= 2) {
      return 'markdown';
    }
    
    // 默认为普通文本/HTML片段
    return 'plain';
  };

  // 处理内容渲染
  const renderContent = useMemo(() => {
    if (!article || !article.content) return { type: 'plain', html: '' };
    
    const contentType = detectContentType(article.content);
    
    if (contentType === 'html') {
      // 独立HTML文档，使用iframe隔离渲染
      return {
        type: 'html',
        html: article.content
      };
    } else if (contentType === 'markdown') {
      // Markdown内容，转换为HTML
      const rawHtml = marked(article.content);
      const sanitizedHtml = DOMPurify.sanitize(rawHtml);
      return {
        type: 'markdown',
        html: sanitizedHtml
      };
    } else {
      // 普通HTML内容
      const sanitizedHtml = DOMPurify.sanitize(article.content);
      return {
        type: 'plain',
        html: sanitizedHtml
      };
    }
  }, [article]);

  // 加载评论列表
  const fetchComments = async () => {
    if (!id) return;
    
    setLoadingComments(true);
    try {
      const response = await getArticleComments(id, 1, 100);
      if (response.success && response.items) {
        // 处理评论数据，添加头像
        const processedComments = response.items.map(comment => ({
          ...comment,
          userName: comment.createdBy,
          userAvatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${comment.createdBy}`,
          // 处理回复数据
          replies: comment.replies?.map(reply => ({
            ...reply,
            userName: reply.createdBy,
            userAvatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${reply.createdBy}`
          })) || []
        }));
        setComments(processedComments);
      } else {
        console.error('获取评论失败:', response.message);
      }
    } catch (error) {
      console.error('获取评论异常:', error);
    } finally {
      setLoadingComments(false);
    }
  };

  // 加载相关文章
  const fetchRelatedArticles = async () => {
    if (!id) return;
    
    setLoadingRelated(true);
    try {
      const response = await getRelatedArticles(id);
      if (response.success && response.data) {
        setRelatedArticles(response.data);
      } else {
        console.error('获取相关文章失败:', response.message);
      }
    } catch (error) {
      console.error('获取相关文章异常:', error);
    } finally {
      setLoadingRelated(false);
    }
  };

  // 正确的 useEffect 使用方式
  useEffect(() => {
    let isCancelled = false;
    
    const fetchArticle = async () => {
      try {
        setLoading(true);
        const articleResp = await getArticleInfo(id);
        
        if (isCancelled) return;
        
        console.log('API响应:', articleResp);
        
        if (articleResp && articleResp.success) {
          // 正确设置文章数据，并添加在线生成的头像
          const articleData = {
            ...articleResp.data,
            authorAvatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${articleResp.data.createdBy}`
          };
          
          setArticle(articleData);
          
          // 设置点赞数
          setLikes(articleResp.data.likeCount || 0);
          
          // 加载评论数据
          fetchComments();
          
          // 加载相关文章
          fetchRelatedArticles();
          
          // 检查是否是作者本人
          if (user && user.nickname === articleResp.data.createdBy) {
            setIsAuthor(true);
          }
        } else {
          // Article not found - redirect to 404
          if (!isCancelled) {
            navigate('/404', { replace: true });
          }
        }
      } catch (error) {
        if (isCancelled) return;
        console.error('获取文章详情失败:', error);
        navigate('/404', { replace: true });
      } finally {
        if (!isCancelled) {
          setLoading(false);
        }
      }
    };

    fetchArticle();
    
    return () => {
      isCancelled = true;
    };
  }, [id, navigate, user]);

  const handleAuthAction = (action) => {
    if (!isAuthenticated) {
      setShowLoginModal(true);
      return false;
    }
    return true;
  };

  const handleLike = () => {
    if (!handleAuthAction('like')) return;
    
    setIsLiked(!isLiked);
    setLikes(isLiked ? likes - 1 : likes + 1);
    
    // 如果需要更新文章数据中的点赞数
    setArticle(prevArticle => {
      if (!prevArticle) return prevArticle;
      return {
        ...prevArticle,
        likeCount: isLiked ? prevArticle.likeCount - 1 : prevArticle.likeCount + 1
      };
    });
  };

  const handleCollect = () => {
    if (!handleAuthAction('collect')) return;
    setIsCollected(!isCollected);
  };

  const handleFollow = () => {
    if (!handleAuthAction('follow')) return;
    setIsFollowing(!isFollowing);
  };

  const handleComment = async (e) => {
    e.preventDefault();
    
    if (!isAuthenticated) {
      setShowLoginModal(true);
      return;
    }
    
    if (!newComment.trim()) {
      message.warning('请输入评论内容');
      return;
    }

    try {
      const response = await postComment(id, newComment.trim());
      if (response.success) {
        message.success(response.message || '评论成功！');
        setNewComment('');
        // 重新加载评论列表
        fetchComments();
      } else {
        message.error(response.message || '评论失败');
      }
    } catch (error) {
      if (error.response?.status === 401) {
        message.error('登录已过期，请重新登录');
        setShowLoginModal(true);
      } else {
        message.error('评论失败，请重试');
      }
    }
  };

  // 处理回复评论
  const handleReply = async (commentId) => {
    if (!isAuthenticated) {
      setShowLoginModal(true);
      return;
    }
    
    if (!replyContent.trim()) {
      message.warning('请输入回复内容');
      return;
    }

    try {
      const response = await replyComment(id, commentId, replyContent.trim());
      if (response.success) {
        message.success(response.message || '回复成功！');
        setReplyContent('');
        setReplyingTo(null);
        // 重新加载评论列表
        fetchComments();
      } else {
        message.error(response.message || '回复失败');
      }
    } catch (error) {
      if (error.response?.status === 401) {
        message.error('登录已过期，请重新登录');
        setShowLoginModal(true);
      } else {
        message.error('回复失败，请重试');
      }
    }
  };

  // 删除评论
  const handleDeleteComment = (commentId) => {
    modal.confirm({
      title: '确认删除',
      content: '确定要删除这条评论吗？',
      okText: '确定',
      okType: 'danger',
      cancelText: '取消',
      onOk: async () => {
        try {
          const response = await deleteComment(commentId);
          if (response.success) {
            message.success(response.message || '删除成功！');
            // 重新加载评论列表
            fetchComments();
          } else {
            message.error(response.message || '删除失败');
          }
        } catch (error) {
          if (error.response?.status === 401) {
            message.error('登录已过期，请重新登录');
            setShowLoginModal(true);
          } else {
            message.error('删除失败，请重试');
          }
        }
      }
    });
  };

  const handleShare = () => {
    navigator.clipboard.writeText(window.location.href);
    message.success('链接已复制到剪贴板');
  };

  // 作者操作：编辑文章
  const handleEdit = () => {
    navigate(`/editor/${id}`);
  };

  // 作者操作：删除文章
  const handleDelete = () => {
    modal.confirm({
      title: '确认删除',
      content: '确定要删除这篇文章吗？此操作不可恢复！',
      okText: '确定',
      okType: 'danger',
      cancelText: '取消',
      onOk: async () => {
        try {
          const response = await deleteArticle(id);
          if (response.success) {
            message.success('文章已删除');
            setTimeout(() => {
              navigate('/');
            }, 1000);
          } else {
            message.error(response.message || '删除失败');
          }
        } catch (error) {
          if (error.response?.status === 401) {
            message.error('登录已过期，请重新登录');
            navigate('/login');
          } else {
            message.error('删除失败，请重试');
          }
        }
      }
    });
  };

  // 作者操作：隐藏/显示文章
  const handleToggleVisibility = () => {
    const isCurrentlyHidden = article.isHidden || false;
    const actionText = isCurrentlyHidden ? '显示' : '隐藏';
    
    modal.confirm({
      title: `确认${actionText}文章`,
      content: `确定要${actionText}这篇文章吗？`,
      okText: '确定',
      cancelText: '取消',
      onOk: async () => {
        try {
          const response = await toggleArticleVisibility(id, !isCurrentlyHidden);
          if (response.success) {
            message.success(`文章已${actionText}`);
            // 更新本地状态
            setArticle(prev => ({ ...prev, isHidden: !isCurrentlyHidden }));
          } else {
            message.error(response.message || `${actionText}失败`);
          }
        } catch (error) {
          if (error.response?.status === 401) {
            message.error('登录已过期，请重新登录');
            navigate('/login');
          } else {
            message.error(`${actionText}失败，请重试`);
          }
        }
      }
    });
  };

  if (loading) {
    return <div className="loading-container">加载中...</div>;
  }

  if (!article) {
    return <div className="error-container">文章不存在</div>;
  }

  return (
    <div className="article-detail-container">
      <div className="container">
        <div className={`article-detail-content ${!showSidebar ? 'sidebar-hidden' : ''}`}>
          {/* 文章主体 */}
          <article className="article-main">
            {/* Back button and Author Actions */}
            <div className="article-top-bar">
              <button className="btn-back" onClick={() => navigate(-1)}>
                <FaArrowLeft />
                <span>返回</span>
              </button>
              
              {/* 作者操作按钮 - 右上角图标 */}
              {isAuthor && (
                <div className="author-actions-icons">
                  <button className="icon-btn icon-edit" onClick={handleEdit} title="编辑文章">
                    <FaEdit />
                  </button>
                  <button className="icon-btn icon-hide" onClick={handleToggleVisibility} title={article.isHidden ? '显示文章' : '隐藏文章'}>
                    <FaEyeSlash />
                  </button>
                  <button className="icon-btn icon-delete" onClick={handleDelete} title="删除文章">
                    <FaTrash />
                  </button>
                </div>
              )}
            </div>
            
            <header className="article-header">
              <h1 className="article-title">{article.title}</h1>
              
              <div className="article-meta">
                <div className="author-section">
                  <img 
                    src={article.authorAvatar || '/default-avatar.png'} 
                    alt={article.createdBy} 
                    className="author-avatar"
                  />
                  <div className="author-info">
                    <span className="author-name">{article.createdBy}</span>
                    <span className="publish-info">
                      发布于 {article.createdAt}
                    </span>
                  </div>
                  <button 
                    className={`btn-follow ${isFollowing ? 'following' : ''}`}
                    onClick={handleFollow}
                  >
                    {isFollowing ? '已关注' : '+ 关注'}
                  </button>
                </div>
              </div>

              <div className="article-tags">
                <span className="article-tag">{article.categoryName}</span>
                {/* 如果有更多标签可以在这里添加 */}
              </div>
            </header>

            {/* 文章内容 - 根据类型渲染 */}
            {renderContent.type === 'html' ? (
              <iframe
                className="article-content-iframe"
                srcDoc={renderContent.html}
                title="Article Content"
                sandbox="allow-same-origin allow-scripts"
              />
            ) : (
              <div 
                className="article-content"
                dangerouslySetInnerHTML={{ __html: renderContent.html }}
              />
            )}

            {/* 文章操作栏 */}
            <div className="article-actions">
              <button 
                className={`action-btn ${isLiked ? 'active' : ''}`}
                onClick={handleLike}
              >
                {isLiked ? <FaHeart /> : <FaRegHeart />}
                <span>{likes}</span>
              </button>
              
              <button 
                className={`action-btn ${isCollected ? 'active' : ''}`}
                onClick={handleCollect}
              >
                {isCollected ? <FaBookmark /> : <FaRegBookmark />}
                <span>{isCollected ? '已收藏' : '收藏'}</span>
              </button>
              
              <button className="action-btn" onClick={handleShare}>
                <FaShare />
                <span>分享</span>
              </button>
            </div>

            {/* 文章统计信息 */}
            <div className="article-stats">
              <span>浏览: {article.viewCount || 0}</span>
              <span>点赞: {article.likeCount || 0}</span>
              <span>评论: {article.commentCount || 0}</span>
              <span>分享: {article.shareCount || 0}</span>
            </div>

            {/* 评论区 */}
            <div className="comments-section">
              <h3 className="comments-title">
                评论 ({comments.length})
              </h3>

              {/* 发表评论 */}
              <form className="comment-form" onSubmit={handleComment}>
                <textarea
                  className="comment-input"
                  placeholder={isAuthenticated ? "写下你的评论..." : "登录后发表评论"}
                  value={newComment}
                  onChange={(e) => setNewComment(e.target.value)}
                  rows="4"
                  disabled={!isAuthenticated}
                />
                <button 
                  type="submit" 
                  className="btn-submit-comment"
                  disabled={!isAuthenticated || !newComment.trim()}
                >
                  发表评论
                </button>
              </form>

              {/* 评论列表 */}
              {loadingComments ? (
                <div className="loading-comments">加载评论中...</div>
              ) : (
                <div className="comments-list">
                  {comments.map(comment => (
                    <div key={comment.id} className="comment-item">
                      <img 
                        src={comment.userAvatar} 
                        alt={comment.userName} 
                        className="comment-avatar"
                      />
                      <div className="comment-content">
                        <div className="comment-header">
                          <span className="comment-user">{comment.userName}</span>
                          <span className="comment-time">{comment.createdAt}</span>
                          {/* 删除按钮 - 只有作者或评论者本人可见 */}
                          {isAuthenticated && (user?.nickname === comment.userName || isAuthor) && (
                            <button 
                              className="btn-delete-comment"
                              onClick={() => handleDeleteComment(comment.id)}
                              title="删除评论"
                            >
                              <FaTrash />
                            </button>
                          )}
                        </div>
                        <p className="comment-text">{comment.content}</p>
                        
                        {/* 回复按钮 */}
                        {isAuthenticated && (
                          <button 
                            className="btn-reply"
                            onClick={() => setReplyingTo(replyingTo === comment.id ? null : comment.id)}
                          >
                            <FaReply /> 回复
                          </button>
                        )}
                        
                        {/* 回复输入框 */}
                        {replyingTo === comment.id && (
                          <div className="reply-input-box">
                            <Input.TextArea
                              value={replyContent}
                              onChange={(e) => setReplyContent(e.target.value)}
                              placeholder="写下你的回复..."
                              rows={3}
                              autoFocus
                            />
                            <div className="reply-actions">
                              <button 
                                className="btn-cancel-reply"
                                onClick={() => {
                                  setReplyingTo(null);
                                  setReplyContent('');
                                }}
                              >
                                取消
                              </button>
                              <button 
                                className="btn-submit-reply"
                                onClick={() => handleReply(comment.id)}
                                disabled={!replyContent.trim()}
                              >
                                提交回复
                              </button>
                            </div>
                          </div>
                        )}
                        
                        {/* 回复列表 */}
                        {comment.replies && comment.replies.length > 0 && (
                          <div className="comment-replies">
                            {comment.replies.map(reply => (
                              <div key={reply.id} className="reply-item">
                                <img 
                                  src={reply.userAvatar} 
                                  alt={reply.userName} 
                                  className="reply-avatar"
                                />
                                <div className="reply-content">
                                  <div className="reply-header">
                                    <span className="reply-user">{reply.userName}</span>
                                    <span className="reply-time">{reply.createdAt}</span>
                                    {/* 删除回复按钮 */}
                                    {isAuthenticated && (user?.nickname === reply.userName || isAuthor) && (
                                      <button 
                                        className="btn-delete-reply"
                                        onClick={() => handleDeleteComment(reply.id)}
                                        title="删除回复"
                                      >
                                        <FaTrash />
                                      </button>
                                    )}
                                  </div>
                                  <p className="reply-text">{reply.content}</p>
                                </div>
                              </div>
                            ))}
                          </div>
                        )}
                      </div>
                    </div>
                  ))}
                  {comments.length === 0 && (
                    <div className="no-comments">
                      <p>暂无评论，来发表第一条评论吧！</p>
                    </div>
                  )}
                </div>
              )}
            </div>
          </article>

          {/* 侧边栏 - 相关推荐 */}
          {showSidebar && (
            <aside className="article-sidebar">
              <div className="sidebar-header">
                <h3 className="sidebar-title">相关推荐</h3>
                <button 
                  className="btn-close-sidebar"
                  onClick={() => setShowSidebar(false)}
                  title="关闭推荐"
                >
                  ×
                </button>
              </div>
              <div className="related-articles">
                {loadingRelated ? (
                  <div className="loading-related">加载中...</div>
                ) : relatedArticles.length > 0 ? (
                  relatedArticles.slice(0, 5).map(item => (
                    <div 
                      key={item.id} 
                      className="related-item"
                      onClick={() => navigate(`/article/${item.id}`)}
                    >
                      <h4>{item.title}</h4>
                      <div className="related-meta">
                        <span>👁 {item.viewCount || 0}</span>
                        <span>❤ {item.likeCount || 0}</span>
                      </div>
                    </div>
                  ))
                ) : (
                  <div className="no-related">暂无相关文章</div>
                )}
              </div>
            </aside>
          )}
          
          {/* 显示推荐按钮 */}
          {!showSidebar && (
            <button 
              className="btn-show-sidebar"
              onClick={() => setShowSidebar(true)}
              title="显示推荐"
            >
              <FaEye /> 推荐
            </button>
          )}
        </div>
      </div>

      {/* 登录弹窗 */}
      {showLoginModal && (
        <LoginModal onClose={() => setShowLoginModal(false)} />
      )}
    </div>
  );
};

export default ArticleDetail;