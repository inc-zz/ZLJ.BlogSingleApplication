# 博客管理后台系统

一个基于 Vue 3 + TypeScript + Vite 构建的现代化博客管理后台系统。

## ✨ 特性

- 🚀 **Vue 3.3+** - 使用最新的 Vue 3 Composition API
- 💪 **TypeScript 5+** - 完整的类型支持
- ⚡️ **Vite 5+** - 快速的开发构建工具
- 📦 **Pinia** - 新一代状态管理
- 🎨 **Element Plus** - 企业级UI组件库
- 🛣️ **Vue Router 4** - 官方路由管理
- 🎯 **Axios** - HTTP请求封装
- 🔧 **Mock.js** - 数据模拟
- 🧪 **Vitest** - 单元测试框架
- 📏 **ESLint + Prettier** - 代码规范
- 📱 **响应式布局** - 支持移动端适配

## 📁 项目结构

```
src/
├── api/                # API接口管理
│   ├── auth.ts        # 认证相关接口
│   ├── article.ts     # 文章接口
│   ├── category.ts    # 分类接口
│   └── tag.ts         # 标签接口
├── assets/            # 静态资源
├── components/        # 公共组件
│   ├── Dialog/        # 弹窗组件
│   └── Table/         # 表格组件
├── composables/       # Vue组合式函数
│   ├── useResponsive.ts   # 响应式布局
│   └── usePagination.ts   # 分页功能
├── layouts/           # 布局组件
│   ├── MainLayout.vue # 主布局
│   ├── Header.vue     # 顶部导航
│   └── Sidebar.vue    # 侧边栏
├── router/            # 路由配置
│   └── index.ts       # 路由定义
├── stores/            # Pinia状态管理
│   ├── user.ts        # 用户状态
│   └── app.ts         # 应用状态
├── styles/            # 全局样式
│   ├── variables.scss # 变量
│   ├── reset.scss     # 重置样式
│   ├── common.scss    # 通用样式
│   └── index.scss     # 样式入口
├── types/             # TypeScript类型定义
│   ├── index.ts       # 公共类型
│   └── env.d.ts       # 环境变量类型
├── utils/             # 工具函数
│   ├── request.ts     # Axios封装
│   ├── storage.ts     # 本地存储
│   ├── validate.ts    # 表单验证
│   └── format.ts      # 格式化工具
├── views/             # 页面组件
│   ├── login/         # 登录页
│   ├── dashboard/     # 仪表盘
│   ├── articles/      # 文章管理
│   ├── categories/    # 分类管理
│   ├── tags/          # 标签管理
│   ├── comments/      # 评论管理
│   ├── users/         # 用户管理
│   ├── settings/      # 系统设置
│   └── error/         # 错误页面
└── main.ts            # 入口文件
```

## 🚀 快速开始

### 环境要求

- Node.js >= 18.0.0
- npm >= 9.0.0

### 安装依赖

```bash
npm install
```

### 开发运行

```bash
npm run dev
```

访问 http://localhost:3000

### 构建生产

```bash
npm run build
```

### 预览构建结果

```bash
npm run preview
```

### 运行测试

```bash
# 运行单元测试
npm run test

# 测试UI界面
npm run test:ui
```

### 代码检查

```bash
# ESLint检查
npm run lint

# 代码格式化
npm run format
```

## 🔑 默认账号

```
用户名: admin
密码: 123456
验证码: 输入图形显示的4位验证码
```

## 🔌 后端接口配置

后端 API 地址: `https://localhost:7235`

### 登录接口
- **URL**: `/api/admin/Account/login`
- **Method**: POST
- **请求参数**:
  ```json
  {
    "account": "admin",
    "password": "123456",
    "captcha": "se32"
  }
  ```
- **响应数据**:
  ```json
  {
    "data": {
      "userInfo": {
        "userName": "admin",
        "realName": "admin",
        "phoneNumber": "15816814415",
        "email": "admin@sing.com"
      },
      "accessToken": "...",
      "refreshToken": "...",
      "expiresIn": "2025-10-30T10:11:41.9943325+08:00",
      "tokenType": "Bearer"
    },
    "success": true,
    "message": "登录成功",
    "code": 200
  }
  ```

## 📝 功能模块

### 已实现功能

- ✅ 用户登录/登出
- ✅ 仪表盘数据展示
- ✅ 文章管理（增删改查）
- ✅ 分类管理
- ✅ 标签管理
- ✅ 评论管理
- ✅ 用户管理
- ✅ 系统设置
- ✅ 响应式布局
- ✅ 路由权限控制
- ✅ Mock数据模拟

### 核心组件

#### CommonTable 通用表格组件

```vue
<CommonTable
  :data="tableData"
  :total="total"
  :loading="loading"
  selection
  @page-change="handlePageChange"
  @selection-change="handleSelectionChange"
>
  <el-table-column prop="name" label="名称" />
  <!-- 其他列定义 -->
</CommonTable>
```

#### CommonDialog 通用弹窗组件

```vue
<CommonDialog
  v-model="dialogVisible"
  title="标题"
  @confirm="handleConfirm"
>
  <!-- 弹窗内容 -->
</CommonDialog>
```

## 🛠️ 技术栈

- **核心框架**: Vue 3.3+
- **开发语言**: TypeScript 5+
- **构建工具**: Vite 5+
- **状态管理**: Pinia
- **路由管理**: Vue Router 4
- **UI组件库**: Element Plus
- **CSS预处理**: Sass/SCSS
- **HTTP库**: Axios
- **数据模拟**: Mock.js
- **测试框架**: Vitest
- **代码规范**: ESLint + Prettier

## 📄 环境变量

项目使用环境变量进行配置，支持以下环境：

- `.env.development` - 开发环境
- `.env.production` - 生产环境

### 环境变量说明

```bash
# 应用标题
VITE_APP_TITLE=博客管理后台

# API地址
VITE_API_BASE_URL=http://localhost:8080

# 环境
VITE_APP_ENV=development
```

## 🔧 配置说明

### Vite 配置

- 自动导入 Vue API 和 Element Plus 组件
- 配置路径别名 `@` 指向 `src` 目录
- 开发服务器端口: 3000
- API代理配置
- Mock数据支持

### TypeScript 配置

- 严格模式
- 路径映射
- 类型检查

### ESLint 配置

- Vue 3 推荐规则
- TypeScript 支持
- Prettier 集成

## 📚 开发规范

### 命名规范

- **组件**: PascalCase (如: `UserList.vue`)
- **工具函数**: camelCase (如: `formatDate`)
- **常量**: UPPER_SNAKE_CASE (如: `API_BASE_URL`)
- **CSS类名**: kebab-case (如: `user-list`)

### 文件组织

- 每个模块独立目录
- 相关文件就近放置
- 公共资源统一管理

### 代码风格

- 使用 Composition API
- TypeScript 类型定义
- 单一职责原则
- 代码注释清晰

## 🤝 贡献指南

1. Fork 项目
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 开启 Pull Request

## 📄 License

MIT License

## 👨‍💻 作者

Your Name

## 🙏 鸣谢

- [Vue.js](https://vuejs.org/)
- [Element Plus](https://element-plus.org/)
- [Vite](https://vitejs.dev/)
- [Pinia](https://pinia.vuejs.org/)
