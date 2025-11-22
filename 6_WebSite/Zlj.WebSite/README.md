# My Website

这是一个使用 Vue 3 + TypeScript + Vite 构建的个人网站项目。

## 项目结构

```
my-website/
├── public/
├── src/
│   ├── assets/
│   ├── components/
│   │   ├── layout/
│   │   │   ├── Header.vue
│   │   │   ├── Footer.vue
│   │   │   └── Navigation.vue
│   │   ├── ui/
│   │   │   ├── Card.vue
│   │   │   ├── Button.vue
│   │   │   └── SectionTitle.vue
│   │   └── home/
│   ├── views/
│   │   ├── Home.vue
│   │   ├── About.vue
│   │   ├── Skills.vue
│   │   ├── Projects.vue
│   │   └── Contact.vue
│   ├── router/
│   ├── stores/
│   ├── types/
│   ├── styles/
│   ├── composables/
│   └── utils/
├── package.json
├── vite.config.ts
├── tsconfig.json
└── index.html
```

## 功能特性

1. 响应式布局设计，支持移动端和桌面端
2. 护眼蓝色主题配色方案
3. 多语言支持（中英文切换）
4. 使用 SCSS Grid 和 Flexbox 实现响应式布局
5. Vue 3 Composition API
6. TypeScript 类型安全
7. Pinia 状态管理
8. Vue Router 路由管理
9. MockJS 数据模拟

## 安装依赖

```bash
npm install
```

## 开发运行

```bash
npm run dev
```

## 构建生产版本

```bash
npm run build
```

## 技术栈

- Vue 3 + TypeScript
- Vite
- Vue Router
- Pinia
- SCSS
- MockJS

## 页面说明

1. **首页 (Home)** - 个人简介与核心优势展示
2. **关于我 (About)** - 详细个人介绍和工作经历时间线
3. **技术栈 (Skills)** - 分类展示技术能力和熟练程度可视化
4. **项目展示 (Projects)** - 个人项目作品集和开源项目展示
5. **联系我 (Contact)** - 多种联系方式和服务咨询表单