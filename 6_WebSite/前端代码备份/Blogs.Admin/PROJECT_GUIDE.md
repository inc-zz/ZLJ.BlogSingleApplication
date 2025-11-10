# 博客管理后台系统 - 项目说明文档

## 项目概述

这是一个使用 Vue 3 + TypeScript + Vite 构建的现代化博客管理后台系统，采用了最新的前端技术栈和最佳实践。

## 核心技术栈

### 前端框架
- **Vue 3.5+**: 使用 Composition API 和 `<script setup>` 语法
- **TypeScript 5+**: 提供完整的类型支持和类型检查

### 构建工具
- **Vite 5+**: 快速的开发服务器和构建工具
- **Rolldown-Vite 7.1.14**: 高性能构建引擎

### UI 组件库
- **Element Plus 2.11+**: 完整的企业级 UI 组件
- **@element-plus/icons-vue**: Element Plus 图标库

### 状态管理与路由
- **Pinia 3+**: Vue 官方推荐的状态管理库
- **Vue Router 4**: 官方路由管理器

### 样式处理
- **Sass 1.93+**: CSS 预处理器

### HTTP 请求
- **Axios 1.13+**: Promise 基础的 HTTP 客户端

### 开发工具
- **Mock.js**: 数据模拟
- **Vitest**: 单元测试框架
- **ESLint**: 代码质量检查
- **Prettier**: 代码格式化

## 项目特性

### 1. 完整的功能模块

#### 用户认证
- 登录/登出功能
- Token 存储和管理
- 路由权限控制

#### 内容管理
- **文章管理**: 创建、编辑、删除、查询文章
- **分类管理**: 文章分类的增删改查
- **标签管理**: 标签的管理和配置
- **评论管理**: 评论审核和管理

#### 系统管理
- **用户管理**: 用户账号管理
- **系统设置**: 系统配置项管理
- **仪表盘**: 数据统计展示

### 2. 通用组件封装

#### CommonTable 表格组件
```typescript
特性:
- 自动分页
- 支持多选
- 加载状态
- 自定义列配置
- 响应式设计
```

#### CommonDialog 弹窗组件
```typescript
特性:
- 统一样式
- 确认/取消回调
- 加载状态
- 自定义内容插槽
```

### 3. 工具函数库

#### 网络请求 (request.ts)
- Axios 拦截器配置
- 统一错误处理
- Token 自动携带
- 响应数据解包

#### 本地存储 (storage.ts)
- LocalStorage 封装
- SessionStorage 封装
- 类型安全的存取

#### 表单验证 (validate.ts)
- 邮箱验证
- 手机号验证
- 密码强度验证
- URL 验证

#### 数据格式化 (format.ts)
- 日期时间格式化
- 文件大小格式化
- 数字千分位格式化
- 相对时间格式化

### 4. Composables 组合函数

#### useResponsive
- 响应式布局检测
- 设备类型判断
- 自动监听窗口变化

#### usePagination
- 分页逻辑封装
- 数据加载管理
- 页码控制

### 5. Mock 数据系统

完整的 Mock API 配置，包括：
- 用户认证接口
- 文章 CRUD 接口
- 分类和标签接口
- 自动生成模拟数据

## 项目结构详解

```
Blogs.Admin/
├── src/
│   ├── api/              # API 接口层
│   │   ├── auth.ts       # 认证接口
│   │   ├── article.ts    # 文章接口
│   │   ├── category.ts   # 分类接口
│   │   └── tag.ts        # 标签接口
│   │
│   ├── assets/           # 静态资源
│   │
│   ├── components/       # 公共组件
│   │   ├── Dialog/       # 通用弹窗
│   │   └── Table/        # 通用表格
│   │
│   ├── composables/      # 组合式函数
│   │   ├── useResponsive.ts   # 响应式
│   │   └── usePagination.ts   # 分页
│   │
│   ├── layouts/          # 布局组件
│   │   ├── MainLayout.vue     # 主布局
│   │   ├── Header.vue         # 头部
│   │   └── Sidebar.vue        # 侧边栏
│   │
│   ├── router/           # 路由配置
│   │   └── index.ts      # 路由定义
│   │
│   ├── stores/           # 状态管理
│   │   ├── index.ts      # Pinia 实例
│   │   ├── user.ts       # 用户状态
│   │   └── app.ts        # 应用状态
│   │
│   ├── styles/           # 全局样式
│   │   ├── reset.scss    # 样式重置
│   │   ├── variables.scss# 变量定义
│   │   ├── common.scss   # 通用样式
│   │   └── index.scss    # 入口文件
│   │
│   ├── types/            # 类型定义
│   │   ├── index.ts      # 通用类型
│   │   └── env.d.ts      # 环境变量类型
│   │
│   ├── utils/            # 工具函数
│   │   ├── request.ts    # HTTP 请求
│   │   ├── storage.ts    # 本地存储
│   │   ├── validate.ts   # 表单验证
│   │   └── format.ts     # 数据格式化
│   │
│   ├── views/            # 页面组件
│   │   ├── login/        # 登录页
│   │   ├── dashboard/    # 仪表盘
│   │   ├── articles/     # 文章管理
│   │   ├── categories/   # 分类管理
│   │   ├── tags/         # 标签管理
│   │   ├── comments/     # 评论管理
│   │   ├── users/        # 用户管理
│   │   ├── settings/     # 系统设置
│   │   └── error/        # 错误页面
│   │
│   ├── App.vue           # 根组件
│   └── main.ts           # 入口文件
│
├── mock/                 # Mock 数据
│   └── index.ts          # Mock 配置
│
├── public/               # 公共资源
│
├── .env.development      # 开发环境变量
├── .env.production       # 生产环境变量
├── .eslintrc.cjs         # ESLint 配置
├── .prettierrc.json      # Prettier 配置
├── tsconfig.json         # TypeScript 配置
├── tsconfig.app.json     # 应用 TS 配置
├── vite.config.ts        # Vite 配置
├── vitest.config.ts      # Vitest 配置
├── package.json          # 项目依赖
└── README.md             # 项目说明
```

## 快速开始

### 1. 安装依赖
```bash
npm install
```

### 2. 启动开发服务器
```bash
npm run dev
```

访问: http://localhost:3000

### 3. 登录系统
```
用户名: admin
密码: 123456
```

## 开发指南

### 添加新页面

1. 在 `src/views/` 下创建页面组件
2. 在 `src/router/index.ts` 中添加路由配置
3. 如需要，在侧边栏菜单中添加入口

### 添加新 API

1. 在 `src/api/` 下创建或编辑 API 文件
2. 定义接口函数
3. 在 `mock/index.ts` 中添加对应的 Mock 数据

### 添加新的状态

1. 在 `src/stores/` 下创建新的 store 文件
2. 使用 `defineStore` 定义状态
3. 在组件中通过 `useXxxStore()` 使用

### 添加新组件

1. 在 `src/components/` 下创建组件目录
2. 创建组件文件和必要的样式
3. 导出组件供其他地方使用

## 构建和部署

### 构建生产版本
```bash
npm run build
```

构建产物在 `dist/` 目录

### 预览构建结果
```bash
npm run preview
```

### 代码质量检查
```bash
# ESLint 检查
npm run lint

# 代码格式化
npm run format
```

### 运行测试
```bash
# 单元测试
npm run test

# 测试 UI
npm run test:ui
```

## 环境配置

### 开发环境 (.env.development)
```bash
VITE_APP_TITLE=博客管理后台
VITE_API_BASE_URL=http://localhost:8080
VITE_APP_ENV=development
```

### 生产环境 (.env.production)
```bash
VITE_APP_TITLE=博客管理后台
VITE_API_BASE_URL=https://api.yourdomain.com
VITE_APP_ENV=production
```

## 最佳实践

### 代码风格
- 使用 TypeScript 进行类型定义
- 使用 Composition API
- 组件使用 `<script setup>` 语法
- 遵循 ESLint 和 Prettier 规则

### 命名规范
- 组件: PascalCase (`UserList.vue`)
- 方法/变量: camelCase (`getUserList`)
- 常量: UPPER_SNAKE_CASE (`API_BASE_URL`)
- CSS 类: kebab-case (`user-list`)

### 文件组织
- 相关文件就近原则
- 公共代码抽取复用
- 保持目录结构清晰

## 注意事项

1. 当前使用 Mock 数据，实际项目需要对接真实后端 API
2. 需要根据实际需求调整环境变量配置
3. 生产环境需要配置正确的 API 地址
4. 建议配置 HTTPS 和其他安全措施

## 扩展功能建议

### 已实现
- ✅ 用户认证和权限控制
- ✅ 内容管理基础功能
- ✅ 响应式布局
- ✅ Mock 数据支持

### 可以扩展
- 🔲 富文本编辑器集成
- 🔲 图片上传功能
- 🔲 数据导出功能
- 🔲 更详细的权限管理
- 🔲 主题切换功能
- 🔲 国际化支持
- 🔲 WebSocket 实时通知
- 🔲 数据可视化图表

## 技术支持

如有问题，请通过以下方式获取帮助：
- 查看项目 README
- 查看相关技术文档
- 提交 Issue

## 更新日志

### v0.0.0 (2025-10-29)
- 🎉 项目初始化
- ✨ 完成基础架构搭建
- ✨ 实现核心功能模块
- ✨ 添加 Mock 数据支持
- 📝 完善项目文档
