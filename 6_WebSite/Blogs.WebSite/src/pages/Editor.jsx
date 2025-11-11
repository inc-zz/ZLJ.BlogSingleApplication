import React, { useState, useEffect, useRef } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { FaBold, FaItalic, FaUnderline, FaListUl, FaListOl, FaCode, FaImage, FaLink, FaEye } from 'react-icons/fa';
import { Modal, Spin, App, Upload } from 'antd';
import { publishArticle, getArticleInfo, updateArticle, getCategories } from '../utils/api';
import http from '../utils/http';

// Markdown 示例模板
const MARKDOWN_EXAMPLE = `# 技术文档示例

## 简介

这是一篇技术文章的示例模板，展示了 **Markdown** 的常用语法和排版效果。您可以参考这个模板来编写自己的文章。

### 文本格式

- **加粗文本**：使用 \`**文本**\`
- *斜体文本*：使用 \`*文本*\`
- <u>下划线</u>：使用 \`<u>文本</u>\`
- ~~删除线~~：使用 \`~~文本~~\`

### 列表

**无序列表：**
- 第一项
- 第二项
  - 子项 1
  - 子项 2
- 第三项

**有序列表：**
1. 第一步
2. 第二步
3. 第三步

## 代码示例

### JavaScript 代码块

\`\`\`javascript
// React 函数组件示例
function HelloWorld({ name }) {
  const [count, setCount] = useState(0);
  
  return (
    <div className="hello">
      <h1>Hello, {name}!</h1>
      <button onClick={() => setCount(count + 1)}>
        Clicked {count} times
      </button>
    </div>
  );
}
\`\`\`

### Python 代码块

\`\`\`python
def fibonacci(n):
    """\u8ba1\u7b97\u6590\u6ce2\u90a3\u5951\u6570\u5217"""
    if n <= 1:
        return n
    return fibonacci(n-1) + fibonacci(n-2)

# 输出前 10 \u4e2a\u6570
for i in range(10):
    print(f"F({i}) = {fibonacci(i)}")
\`\`\`

### 行内代码

在文本中使用 \`const x = 10;\` 这样的行内代码。

## 图片展示

![React Logo](https://upload.wikimedia.org/wikipedia/commons/a/a7/React-icon.svg)

*图片说明：React 官方 Logo*

## 链接

- [React 官方文档](https://react.dev)
- [MDN Web Docs](https://developer.mozilla.org)
- [GitHub](https://github.com)

## 表格

| 特性 | React | Vue | Svelte |
|------|-------|-----|--------|
| 虚拟DOM | ✓ | ✓ | ✗ |
| TypeScript | ✓ | ✓ | ✓ |
| 生态系统 | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐ |
| 学习曲线 | 中 | 低 | 低 |

## 引用

> “代码质量不仅仅是关于功能，更是关于可读性和可维护性。”
> 
> —— Robert C. Martin

## 注意事项

⚠️ **重要：**请确保代码的安全性和性能。

✅ **提示：**使用 ESLint 和 Prettier 保持代码风格统一。

## 总结

本文展示了 Markdown 的常用语法，包括：

1. 标题和段落
2. 文本格式化
3. 列表和代码块
4. 图片和链接
5. 表格和引用

希望这个示例能帮助您快速上手 Markdown 编写！
`;

const Editor = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const { isAuthenticated, isLoading } = useAuth();
  const textareaRef = useRef(null);
  const { message } = App.useApp();
  
  const [title, setTitle] = useState('');
  const [summary, setSummary] = useState('');
  const [content, setContent] = useState(MARKDOWN_EXAMPLE);
  const [selectedTechStack, setSelectedTechStack] = useState('');
  const [tags, setTags] = useState([]);
  const [tagInput, setTagInput] = useState('');
  const [isSaving, setIsSaving] = useState(false);
  const [isPublishing, setIsPublishing] = useState(false);
  const [preview, setPreview] = useState(false);
  const [showExample, setShowExample] = useState(true);
  const [isEditMode, setIsEditMode] = useState(false);
  const [articleId, setArticleId] = useState(null);
  const [isImageModalOpen, setIsImageModalOpen] = useState(false);
  const [isLinkModalOpen, setIsLinkModalOpen] = useState(false);
  const [uploadFile, setUploadFile] = useState(null);
  const [imageUrl, setImageUrl] = useState('');
  const [linkUrl, setLinkUrl] = useState('');
  const [linkText, setLinkText] = useState('');
  const [categories, setCategories] = useState([]);
  const [isLoadingCategories, setIsLoadingCategories] = useState(true);

  useEffect(() => {
    // 等待认证状态加载完成
    if (isLoading) return;
    
    if (!isAuthenticated) {
      message.warning('请先登录');
      navigate('/login');
    }
  }, [isAuthenticated, isLoading, navigate]); // 移除 message 依赖

  // 加载分类数据
  useEffect(() => {
    let isCancelled = false;
    
    const fetchCategories = async () => {
      setIsLoadingCategories(true);
      try {
        const response = await getCategories(10);
        if (isCancelled) return;
        
        if (response.success && response.data) {
          setCategories(response.data);
        } else {
          console.error('获取分类失败:', response.message);
          message.error('获取分类失败');
        }
      } catch (error) {
        if (isCancelled) return;
        console.error('获取分类异常:', error);
        message.error('获取分类失败');
      } finally {
        if (!isCancelled) {
          setIsLoadingCategories(false);
        }
      }
    };

    fetchCategories();
    
    return () => {
      isCancelled = true;
    };
  }, []);

  // 加载文章数据（编辑模式）
  useEffect(() => {
    const loadArticle = async () => {
      if (id) {
        setIsEditMode(true);
        setArticleId(id);
        setShowExample(false);
        
        try {
          const response = await getArticleInfo(id);
          if (response.success && response.data) {
            const articleData = response.data;
            setTitle(articleData.title || '');
            setSummary(articleData.summary || '');
            setContent(articleData.content || '');
            
            // 设置技术栈（直接使用 categoryId）
            if (articleData.categoryId) {
              setSelectedTechStack(articleData.categoryId);
            }
            
            // 设置标签
            if (articleData.tags) {
              const tagsArray = articleData.tags.split('，');
              setTags(tagsArray);
            }
            
            message.success('文章加载成功');
          } else {
            message.error('加载文章失败');
          }
        } catch (error) {
          console.error('加载文章失败:', error);
          message.error('加载文章失败');
        }
      }
    };
    
    loadArticle();
  }, [id, navigate, message]);

  // 文本编辑工具
  const insertText = (before, after = '') => {
    const textarea = textareaRef.current;
    if (!textarea) return;

    const start = textarea.selectionStart;
    const end = textarea.selectionEnd;
    const selectedText = content.substring(start, end);
    const newText = content.substring(0, start) + before + selectedText + after + content.substring(end);
    
    setContent(newText);
    
    // 设置光标位置
    setTimeout(() => {
      textarea.focus();
      const newPosition = start + before.length + selectedText.length;
      textarea.setSelectionRange(newPosition, newPosition);
    }, 0);
  };

  const formatBold = () => insertText('**', '**');
  const formatItalic = () => insertText('*', '*');
  const formatUnderline = () => insertText('<u>', '</u>');
  const formatList = () => {
    const textarea = textareaRef.current;
    const start = textarea.selectionStart;
    const lineStart = content.lastIndexOf('\n', start - 1) + 1;
    const newText = content.substring(0, lineStart) + '- ' + content.substring(lineStart);
    setContent(newText);
  };
  const formatOrderedList = () => {
    const textarea = textareaRef.current;
    const start = textarea.selectionStart;
    const lineStart = content.lastIndexOf('\n', start - 1) + 1;
    const newText = content.substring(0, lineStart) + '1. ' + content.substring(lineStart);
    setContent(newText);
  };
  const formatCode = () => insertText('```\n', '\n```');
  
  // 图片上传函数
    const handleImageUpload = async (file) => {
    try {
        // 创建 FormData
        const formData = new FormData();
        formData.append('File', file);  // 参数名：File（首字母大写）
        formData.append('BusinessType', 'ArticleFiles');  // 参数名：BusinessType（驼峰命名）
        
        // 上传图片 - 显示加载提示
        message.loading({ content: '图片上传中...', key: 'uploadImage' });
        
        // 不要手动设置 Content-Type，让浏览器自动设置
        const response = await http.post('/AppFileStore/upload', formData);  // URL：/AppFileStore/upload（注意大小写）
        
        // 响应格式：{ data: { success, message, fileRecord, fileUrl }, success, message }
        console.log('图片上传响应:', response);
        if (response.data && response.data.success && response.data.data) {
            const fileUrl = response.data.data.fileUrl;
            
            // 在光标位置插入图片 Markdown 语法
            insertText(`![image](${fileUrl})`);
            
            message.success({ content: '图片上传成功！', key: 'uploadImage' });
            return true;
        } else {
            message.error({ content: response.data?.message || '图片上传失败', key: 'uploadImage' });
            return false;
        }
    } catch (error) {
        console.error('图片上传错误:', error);
        
        // 提供更详细的错误信息
        if (error.response) {
            console.log('错误状态:', error.response.status);
            console.log('错误数据:', error.response.data);
            
            if (error.response.status === 415) {
                message.error({ content: '服务器不支持该媒体类型，请联系管理员', key: 'uploadImage' });
            } else {
                message.error({ content: `上传失败: ${error.response.status}`, key: 'uploadImage' });
            }
        } else {
            message.error({ content: '图片上传失败，请重试', key: 'uploadImage' });
        }
        return false;
    }
};
  
  // 打开图片插入模态框
  const insertImage = () => {
    setUploadFile(null);
    setImageUrl('');
    setIsImageModalOpen(true);
  };
  
  // 处理图片插入
  const handleImageInsert = async () => {
    // 优先处理文件上传
    if (uploadFile) {
      const success = await handleImageUpload(uploadFile);
      if (success) {
        setIsImageModalOpen(false);
        setUploadFile(null);
        setImageUrl('');
      }
      return;
    }
    
    // 其次处理 URL 输入
    if (imageUrl.trim()) {
      insertText(`![image](${imageUrl.trim()})`);
      message.success('图片已插入');
      setIsImageModalOpen(false);
      setImageUrl('');
      return;
    }
    
    message.warning('请选择图片或输入URL');
  };
  
  // 处理文件选择
  const handleFileSelect = (e) => {
    const file = e.target.files?.[0];
    if (file && file.type.startsWith('image/')) {
      setUploadFile(file);
    } else if (file) {
      message.warning('请选择图片文件');
    }
  };
  
  // 打开链接插入模态框
  const insertLink = () => {
    setLinkUrl('');
    setLinkText('');
    setIsLinkModalOpen(true);
  };
  
  // 处理链接插入
  const handleLinkInsert = () => {
    if (linkUrl.trim()) {
      insertText(`[${linkText.trim() || linkUrl.trim()}](${linkUrl.trim()})`);
      message.success('链接已插入');
      setIsLinkModalOpen(false);
      setLinkUrl('');
      setLinkText('');
    } else {
      message.warning('请输入链接URL');
    }
  };

  const handleAddTag = (e) => {
    e.preventDefault();
    if (tagInput.trim() && !tags.includes(tagInput.trim()) && tags.length < 5) {
      setTags([...tags, tagInput.trim()]);
      setTagInput('');
    }
  };

  const handleRemoveTag = (tagToRemove) => {
    setTags(tags.filter(tag => tag !== tagToRemove));
  };

  const handleSaveDraft = () => {
    if (!title.trim()) {
      message.warning('请输入文章标题');
      return;
    }

    setIsSaving(true);
    
    // 模拟保存
    setTimeout(() => {
      const draft = {
        title,
        summary,
        content,
        techStack: selectedTechStack,
        tags,
        savedAt: new Date().toISOString(),
      };
      
      localStorage.setItem('articleDraft', JSON.stringify(draft));
      message.success('草稿已保存');
      setIsSaving(false);
    }, 500);
  };

  const handlePublish = async () => {
    // 验证输入
    if (!title.trim()) {
      message.warning('请输入文章标题');
      return;
    }

    if (!summary.trim()) {
      message.warning('请输入文章简介');
      return;
    }

    if (!content.trim()) {
      message.warning('请输入文章内容');
      return;
    }

    if (!selectedTechStack) {
      message.warning('请选择技术栈分类');
      return;
    }

    if (tags.length === 0) {
      message.warning('请至少添加一个标签');
      return;
    }

    setIsPublishing(true);

    try {
      // 准备提交数据
      const articleData = {
        id: articleId,
        title: title.trim(),
        summary: summary.trim(),
        categoryId: selectedTechStack, // selectedTechStack 现在存储的是 categoryId
        tags: tags.join('，'), // 使用中文逗号分隔
        content: content.trim(),
        isPublish: true
      };
      
      let response;
      
      // 如果是编辑模式，调用更新接口并携带 id
      if (isEditMode && articleId) {
        articleData.id = articleId;
        response = await updateArticle(articleId, articleData);
      } else {
        // 调用发布接口
        response = await publishArticle(articleData);
      }

      if (response.success) {
        // 发布/更新成功
        message.success(response.message || `文章${isEditMode ? '更新' : '发布'}成功！`);
        
        // 清除草稿
        localStorage.removeItem('articleDraft');
        
        // 跳转到首页
        setTimeout(() => {
          navigate('/');
        }, 1000);
      } else {
        // 发布/更新失败
        message.error(response.message || `文章${isEditMode ? '更新' : '发布'}失败`);
      }
    } catch (error) {
      console.error(`${isEditMode ? '更新' : '发布'}文章异常:`, error);
      
      // 检查是否是401错误
      
      if (error.response?.status === 401) {
        Modal.confirm({
          title: '登录已过期',
          content: '您的登录状态已过期，请重新登录',
          okText: '确定',
          cancelText: '取消',
          onOk: () => {
            localStorage.removeItem('blogs_user');
            localStorage.removeItem('blogs_token');
            // navigate('/login');
          }
        });
      } else {
        message.error(`${isEditMode ? '更新' : '发布'}文章失败，请重试`);
      }
    } finally {
      setIsPublishing(false);
    }
  };

  // 加载草稿
  useEffect(() => {
    
    const savedDraft = localStorage.getItem('articleDraft');
    if (savedDraft && !id) {
      Modal.confirm({
        title: '检测到未发布的草稿',
        content: '是否加载草稿内容？',
        okText: '加载',
        cancelText: '取消',
        onOk: () => {
          try {
            const draft = JSON.parse(savedDraft);
            setTitle(draft.title || '');
            setSummary(draft.summary || '');
            setContent(draft.content || '');
            setSelectedTechStack(draft.techStack || '');
            setTags(draft.tags || []);
            message.success('草稿加载成功');
          } catch (error) {
            console.error('加载草稿失败:', error);
            message.error('加载草稿失败');
          }
        }
      });
    }
  }, [id]);

  return (
    <div className="editor-container">
      <div className={`editor-layout ${!showExample ? 'sidebar-hidden' : ''}`}>
        {/* 主编辑区域 */}
        <div className="editor-main">
          <div className="editor-wrapper">
            <div className="editor-header">
              <h1>{isEditMode ? '编辑文章' : '发布文章'}</h1>
            </div>

        <div className="editor-content">
          {/* 标题输入 */}
          <div className="form-group">
            <input
              type="text"
              className="title-input"
              placeholder="请输入文章标题..."
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              maxLength={100}
            />
            <span className="char-count">{title.length}/100</span>
          </div>

          {/* 文章简介 */}
          <div className="form-group summary-group">
            <textarea
              className="summary-input"
              placeholder="请输入文章简介（200字以内）..."
              value={summary}
              onChange={(e) => setSummary(e.target.value)}
              maxLength={200}
              rows={3}
            />
            <span className="summary-char-count">{summary.length}/200</span>
          </div>

          {/* 技术栈选择 */}
          <div className="form-group">
            <label>技术栈分类</label>
            <div className="tech-stack-select">
              {isLoadingCategories ? (
                <div className="loading-categories">加载分类中...</div>
              ) : (
                categories.map(category => (
                  <button
                    key={category.id}
                    className={`tech-stack-btn ${selectedTechStack === category.id ? 'active' : ''}`}
                    onClick={() => setSelectedTechStack(category.id)}
                    title={category.description}
                  >
                    {category.name}
                  </button>
                ))
              )}
            </div>
          </div>

          {/* 标签输入 */}
          <div className="form-group">
            <label>文章标签（最多5个）</label>
            <div className="tags-input-container">
              <div className="tags-list">
                {tags.map((tag, index) => (
                  <span key={index} className="tag-item">
                    {tag}
                    <button 
                      className="tag-remove"
                      onClick={() => handleRemoveTag(tag)}
                    >
                      ×
                    </button>
                  </span>
                ))}
              </div>
              {tags.length < 5 && (
                <form onSubmit={handleAddTag} className="tag-input-form">
                  <input
                    type="text"
                    className="tag-input"
                    placeholder="输入标签后按回车"
                    value={tagInput}
                    onChange={(e) => setTagInput(e.target.value)}
                    maxLength={20}
                  />
                </form>
              )}
            </div>
          </div>

          {/* 富文本编辑器 */}
          <div className="form-group">
            <div className="editor-header-row">
              <label>文章内容</label>
              <div className="editor-tabs">
                <button
                  type="button"
                  className={`tab-btn ${!preview ? 'active' : ''}`}
                  onClick={() => setPreview(false)}
                >
                  编辑
                </button>
                <button
                  type="button"
                  className={`tab-btn ${preview ? 'active' : ''}`}
                  onClick={() => setPreview(true)}
                >
                  预览
                </button>
              </div>
            </div>

            {!preview ? (
              <>
                <div className="editor-toolbar">
                  <button type="button" className="toolbar-btn" onClick={formatBold} title="加粗">
                    <FaBold />
                  </button>
                  <button type="button" className="toolbar-btn" onClick={formatItalic} title="斜体">
                    <FaItalic />
                  </button>
                  <button type="button" className="toolbar-btn" onClick={formatUnderline} title="下划线">
                    <FaUnderline />
                  </button>
                  <div className="toolbar-divider"></div>
                  <button type="button" className="toolbar-btn" onClick={formatList} title="无序列表">
                    <FaListUl />
                  </button>
                  <button type="button" className="toolbar-btn" onClick={formatOrderedList} title="有序列表">
                    <FaListOl />
                  </button>
                  <div className="toolbar-divider"></div>
                  <button type="button" className="toolbar-btn" onClick={formatCode} title="代码块">
                    <FaCode />
                  </button>
                  <button type="button" className="toolbar-btn" onClick={insertImage} title="插入图片">
                    <FaImage />
                  </button>
                  <button type="button" className="toolbar-btn" onClick={insertLink} title="插入链接">
                    <FaLink />
                  </button>
                </div>
                <textarea
                  ref={textareaRef}
                  className="content-editor"
                  placeholder="开始编写你的文章...\n\n支持 Markdown 语法:\n- **加粗** *斜体*\n- # 标题\n- - 列表\n- ```代码块```\n- ![image](url) 图片\n- [text](url) 链接"
                  value={content}
                  onChange={(e) => setContent(e.target.value)}
                  rows="20"
                />
              </>
            ) : (
              <div className="content-preview" dangerouslySetInnerHTML={{ 
                __html: content
                  .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')
                  .replace(/\*(.+?)\*/g, '<em>$1</em>')
                  .replace(/<u>(.+?)<\/u>/g, '<u>$1</u>')
                  .replace(/^### (.+)$/gm, '<h3>$1</h3>')
                  .replace(/^## (.+)$/gm, '<h2>$1</h2>')
                  .replace(/^# (.+)$/gm, '<h1>$1</h1>')
                  .replace(/^- (.+)$/gm, '<li>$1</li>')
                  .replace(/^\d+\. (.+)$/gm, '<li>$1</li>')
                  .replace(/```([\s\S]*?)```/g, '<pre><code>$1</code></pre>')
                  .replace(/!\[([^\]]*)\]\(([^\)]+)\)/g, '<img src="$2" alt="$1" />')
                  .replace(/\[([^\]]+)\]\(([^\)]+)\)/g, '<a href="$2" target="_blank">$1</a>')
                  .replace(/\n/g, '<br />')
              }} />
            )}
          </div>
          <div className='editor-footer'>
            <div className="editor-actions">
                <button 
                  className="btn-draft" 
                  onClick={handleSaveDraft}
                  disabled={isSaving}
                >
                  {isSaving ? '保存中...' : '保存草稿'}
                </button>
                <button 
                  className="btn-publish" 
                  onClick={handlePublish}
                  disabled={isPublishing || isSaving}
                >
                  {isPublishing ? (
                    <>
                      <Spin size="small" style={{ marginRight: 8 }} />
                      {isEditMode ? '正在更新...' : '正在发布...'}
                    </>
                  ) : (isEditMode ? '更新文章' : '发布文章')}
                </button>
              </div>
            </div>  
        </div>
        
        </div>
        
      </div>
      
      {/* 示例侧边栏 */}
      {showExample && (
        <aside className="editor-sidebar">
          <div className="sidebar-header">
            <h3>📖 Markdown 语法示例</h3>
            <button 
              className="btn-close-sidebar"
              onClick={() => setShowExample(false)}
              title="关闭示例"
            >
              ×
            </button>
          </div>
          
          <div className="sidebar-content">
            <div className="example-section">
              <h4>标题</h4>
              <pre><code># 一级标题
## 二级标题
### 三级标题</code></pre>
            </div>
            
            <div className="example-section">
              <h4>文本格式</h4>
              <pre><code>**加粗文本**
*斜体文本*
<u>下划线</u></code></pre>
            </div>
            
            <div className="example-section">
              <h4>列表</h4>
              <pre><code>- 无序列表项 1
- 无序列表项 2

1. 有序列表项 1
2. 有序列表项 2</code></pre>
            </div>
            
            <div className="example-section">
              <h4>代码块</h4>
              <pre><code>{`\`\`\`javascript
const hello = () => {
  console.log('Hello!');
};
\`\`\``}</code></pre>
            </div>
            
            <div className="example-section">
              <h4>链接和图片</h4>
              <pre><code>[Link Text](https://example.com)
![Alt Text](image-url.jpg)</code></pre>
            </div>
            
            <div className="example-section">
              <h4>表格</h4>
              <pre><code>| 列1 | 列2 |
|------|------|
| 数据1 | 数据2 |</code></pre>
            </div>
            
            <div className="example-section">
              <h4>引用</h4>
              <pre><code>{`> 这是一段引用文本`}</code></pre>
            </div>
            
            <button 
              className="btn-use-template"
              onClick={() => {
                Modal.confirm({
                  title: '使用示例模板',
                  content: '确定要使用示例模板？当前内容将被替换。',
                  okText: '确定',
                  cancelText: '取消',
                  onOk: () => {
                    setContent(MARKDOWN_EXAMPLE);
                    message.success('模板已加载');
                  }
                });
              }}
            >
              📄 使用完整示例模板
            </button>
          </div>
        </aside>
      )}
      
      {/* 显示示例按钮 */}
      {!showExample && (
        <button 
          className="btn-show-example"
          onClick={() => setShowExample(true)}
          title="显示示例"
        >
          <FaEye /> 示例
        </button>
      )}
      </div>
      
      {/* 图片插入模态框 */}
      <Modal
        title="插入图片"
        open={isImageModalOpen}
        onOk={handleImageInsert}
        onCancel={() => {
          setIsImageModalOpen(false);
          setUploadFile(null);
          setImageUrl('');
        }}
        okText="插入"
        cancelText="取消"
        width={500}
      >
        <div style={{ marginTop: '16px' }}>
          <div style={{ marginBottom: '16px' }}>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: 500 }}>
              选择上传方式：
            </label>
            <input
              type="file"
              accept="image/*"
              onChange={handleFileSelect}
              style={{
                width: '100%',
                padding: '10px',
                border: '2px dashed #d9d9d9',
                borderRadius: '8px',
                cursor: 'pointer',
                background: '#fafafa'
              }}
            />
            {uploadFile && (
              <div style={{ marginTop: '8px', fontSize: '14px', color: '#ff9800' }}>
                已选择: {uploadFile.name}
              </div>
            )}
          </div>
          <div>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: 500 }}>
              或者输入图片链接：
            </label>
            <input
              type="text"
              placeholder="请输入图片URL"
              value={imageUrl}
              onChange={(e) => setImageUrl(e.target.value)}
              style={{
                width: '100%',
                padding: '8px 12px',
                border: '1px solid #d9d9d9',
                borderRadius: '4px',
                fontSize: '14px'
              }}
            />
          </div>
        </div>
      </Modal>
      
      {/* 链接插入模态框 */}
      <Modal
        title="插入链接"
        open={isLinkModalOpen}
        onOk={handleLinkInsert}
        onCancel={() => {
          setIsLinkModalOpen(false);
          setLinkUrl('');
          setLinkText('');
        }}
        okText="插入"
        cancelText="取消"
      >
        <div style={{ marginTop: '16px' }}>
          <div style={{ marginBottom: '16px' }}>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: 500 }}>
              链接URL：
            </label>
            <input
              type="text"
              placeholder="请输入链接URL"
              value={linkUrl}
              onChange={(e) => setLinkUrl(e.target.value)}
              style={{
                width: '100%',
                padding: '8px 12px',
                border: '1px solid #d9d9d9',
                borderRadius: '4px',
                fontSize: '14px'
              }}
            />
          </div>
          <div>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: 500 }}>
              链接文字（可选）：
            </label>
            <input
              type="text"
              placeholder="请输入链接文字"
              value={linkText}
              onChange={(e) => setLinkText(e.target.value)}
              style={{
                width: '100%',
                padding: '8px 12px',
                border: '1px solid #d9d9d9',
                borderRadius: '4px',
                fontSize: '14px'
              }}
            />
          </div>
        </div>
      </Modal>
    </div>
  );
};

export default Editor;
