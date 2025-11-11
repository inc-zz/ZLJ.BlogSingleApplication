// Mock数据 - 模拟文章数据

export const techStacks = [
  { id: 1, name: '服务器部署', key: 'server' },
  { id: 2, name: '后端开发', key: 'backend' },
  { id: 3, name: '数据库', key: 'database' },
  { id: 4, name: '单元测试', key: 'testing' },
  { id: 5, name: '前端开发', key: 'frontend' },
];

export const mockArticles = [
  // 服务器部署
  {
    id: 1,
    title: 'Docker容器化部署最佳实践',
    summary: '详细介绍Docker在生产环境中的部署方案，包括镜像优化、多阶段构建、安全配置等实战技巧，帮助你构建高效稳定的容器化应用...',
    content: `<h2>Docker容器化部署最佳实践</h2>
      <p>Docker已经成为现代应用部署的标准工具，本文将从实战角度分享生产环境中的部署经验。</p>
      
      <h3>1. 镜像优化策略</h3>
      <p>使用多阶段构建可以显著减小镜像体积。例如：</p>
      <pre><code>FROM node:18 AS builder
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html
EXPOSE 80</code></pre>
      
      <h3>2. 安全配置</h3>
      <p>• 使用非root用户运行容器<br>
      • 定期更新基础镜像<br>
      • 扫描镜像漏洞<br>
      • 限制容器资源使用</p>
      
      <h3>3. 网络配置</h3>
      <p>合理规划Docker网络，使用自定义网络隔离不同服务，配置健康检查确保服务可用性。</p>
      
      <p>通过这些实践，可以构建出稳定、高效、安全的容器化应用。</p>`,
    author: '运维小王',
    authorAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=server1',
    likes: 245,
    views: 1523,
    comments: 32,
    techStack: 'server',
    tags: ['Docker', 'DevOps', '容器化'],
    createdAt: '2025-10-15',
    isLiked: false,
    isCollected: false,
  }
];

// 推荐文章
export const recommendedArticles = mockArticles.slice(0, 5);

// 推荐开源项目
export const recommendedProjects = [
  {
    id: 1,
    name: 'React',
    description: '用于构建用户界面的JavaScript库，由Meta开发维护',
    stars: '220k',
    url: 'https://github.com/facebook/react',
    language: 'JavaScript',
  },
  {
    id: 2,
    name: 'Vue.js',
    description: '渐进式JavaScript框架，易学易用，性能出色',
    stars: '206k',
    url: 'https://github.com/vuejs/vue',
    language: 'JavaScript',
  },
  {
    id: 3,
    name: 'Spring Boot',
    description: 'Java应用开发框架，简化Spring配置',
    stars: '72k',
    url: 'https://github.com/spring-projects/spring-boot',
    language: 'Java',
  },
  {
    id: 4,
    name: 'Docker',
    description: '容器化平台，轻松构建、发布和运行应用',
    stars: '68k',
    url: 'https://github.com/docker',
    language: 'Go',
  },
  {
    id: 5,
    name: 'Kubernetes',
    description: '容器编排平台，自动化应用部署和管理',
    stars: '107k',
    url: 'https://github.com/kubernetes/kubernetes',
    language: 'Go',
  },
  {
    id: 6,
    name: 'TypeScript',
    description: 'JavaScript的超集，添加类型系统',
    stars: '98k',
    url: 'https://github.com/microsoft/TypeScript',
    language: 'TypeScript',
  },
];

// 技术标签
export const techTags = [
  'JavaScript', 'TypeScript', 'React', 'Vue', 'Angular', 'Next.js', 'Nuxt.js',
  'Node.js', 'Express', 'Koa', 'Nest.js',
  'Java', 'Spring', 'Spring Boot', 'MyBatis',
  'Python', 'Django', 'Flask', 'FastAPI',
  'Go', 'Gin', 'Echo',
  'MySQL', 'PostgreSQL', 'MongoDB', 'Redis', 'Elasticsearch',
  'Docker', 'Kubernetes', 'Jenkins', 'GitHub Actions', 'GitLab CI',
  'Git', 'Linux', 'Nginx', 'Apache',
  'Jest', 'Mocha', 'Cypress', 'Selenium', 'TDD', 'BDD',
  'Webpack', 'Vite', 'Rollup', 'esbuild',
  'CSS', 'Sass', 'Less', 'Tailwind CSS',
  'GraphQL', 'REST API', 'gRPC', 'WebSocket',
  'AWS', 'Azure', 'GCP', 'Alibaba Cloud',
  '微服务', '云原生', '服务网格', 'DevOps', 'CI/CD',
  '性能优化', '安全', '架构设计', '设计模式',
];

// 评论数据
export const mockComments = [
  {
    id: 1,
    userId: 1,
    userName: '技术爱好者',
    userAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=user1',
    content: '写得很详细，学到了很多！特别是多阶段构建这一部分，确实能显著减小镜像体积。',
    createdAt: '2025-10-18 10:30',
    likes: 12,
    replies: [
      {
        id: 11,
        userId: 2,
        userName: '编程小白',
        userAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=user2',
        content: '同感，这篇文章解决了我的困惑，现在我的镜像从800MB减到了100MB！',
        createdAt: '2025-10-18 11:00',
        likes: 3,
      },
      {
        id: 12,
        userId: 3,
        userName: '运维小王',
        userAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=server1',
        content: '感谢支持！如果有其他问题欢迎留言讨论~',
        createdAt: '2025-10-18 11:30',
        likes: 5,
      },
    ],
  },
  {
    id: 2,
    userId: 3,
    userName: '全栈开发',
    userAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=user3',
    content: '能否分享一下实际项目中的应用案例？比如如何处理容器间的数据共享问题？',
    createdAt: '2025-10-18 14:20',
    likes: 8,
    replies: [],
  },
  {
    id: 3,
    userId: 4,
    userName: 'DevOps工程师',
    userAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=user4',
    content: '关于安全配置这一块，建议补充一下 Secret 管理和环境变量的最佳实践。',
    createdAt: '2025-10-18 15:45',
    likes: 15,
    replies: [
      {
        id: 31,
        userId: 1,
        userName: '运维小王',
        userAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=server1',
        content: '好建议！我会在下一篇文章中详细介绍 Docker Secrets 和环境变量的管理。',
        createdAt: '2025-10-18 16:00',
        likes: 7,
      },
    ],
  },
  {
    id: 4,
    userId: 5,
    userName: '前端工程师',
    userAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=user5',
    content: '对于前端项目，有没有推荐的基础镜像？比如 Node 版本选择和 Nginx 配置。',
    createdAt: '2025-10-18 17:10',
    likes: 6,
    replies: [],
  },
  {
    id: 5,
    userId: 6,
    userName: '后端架构师',
    userAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=user6',
    content: '很实用的文章！已经在我们的微服务项目中应用了这些最佳实践，效果显著。期待更多相关内容！',
    createdAt: '2025-10-18 18:25',
    likes: 10,
    replies: [],
  },
];

// 模拟用户数据
export const mockUsers = {
  admin: {
    username: 'admin',
    password: '123456',
    nickname: '博客管理员',
    email: 'admin@blog.com',
    avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=admin',
    articles: [],
  },
  user: {
    username: 'user',
    password: '123456',
    nickname: '普通用户',
    email: 'user@blog.com',
    avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=user',
    articles: [],
  },
};

// 根据技术栈获取文章
export const getArticlesByTechStack = (techStack) => {
  if (!techStack || techStack === 'all') {
    return mockArticles;
  }
  return mockArticles.filter(article => article.techStack === techStack);
};

// 根据ID获取文章
export const getArticleById = (id) => {
  return mockArticles.find(article => article.id === parseInt(id));
};

// 根据标签筛选文章
export const getArticlesByTag = (tag) => {
  return mockArticles.filter(article => article.tags.includes(tag));
};
