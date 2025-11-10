import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import { useUserStore } from '@/stores/user'

// 路由配置
const routes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/views/login/index.vue'),
    meta: {
      title: '登录',
      hidden: true,
    },
  },
  {
    path: '/',
    component: () => import('@/layouts/MainLayout.vue'),
    redirect: '/dashboard',
    children: [
      {
        path: '/dashboard',
        name: 'Dashboard',
        component: () => import('@/views/dashboard/index.vue'),
        meta: {
          title: '仪表盘',
          icon: 'Odometer',
          requiresAuth: true,
        },
      },
      // 用户管理
      {
        path: '/user-management',
        name: 'UserManagement',
        redirect: '/user-management/user-list',
        meta: {
          title: '用户管理',
          icon: 'User',
          requiresAuth: true,
        },
        children: [
          {
            path: '/user-management/user-list',
            name: 'UserList',
            component: () => import('@/views/user-management/user-list.vue'),
            meta: {
              title: '用户列表',
              requiresAuth: true,
            },
          },
        ],
      },
      // 文章管理
      {
        path: '/admin/article',
        name: 'ArticleManagement',
        redirect: '/admin/article/categories',
        meta: {
          title: '文章管理',
          icon: 'Document',
          requiresAuth: true,
        },
        children: [
          {
            path: '/admin/article/categories',
            name: 'CategoryManagement',
            component: () => import('@/views/admin/article/CategoryManagement.vue'),
            meta: {
              title: '分类管理',
              requiresAuth: true,
            },
          },
          {
            path: '/admin/article/comments',
            name: 'CommentManagement',
            component: () => import('@/views/admin/article/CommentManagement.vue'),
            meta: {
              title: '评论管理',
              requiresAuth: true,
            },
          },
        ],
      },
      // 权限管理
      {
        path: '/permission',
        name: 'Permission',
        redirect: '/permission/admin-list',
        meta: {
          title: '权限管理',
          icon: 'Lock',
          requiresAuth: true,
        },
        children: [
          {
            path: '/permission/admin-list',
            name: 'AdminList',
            component: () => import('@/views/permission/admin-list.vue'),
            meta: {
              title: '管理员列表',
              requiresAuth: true,
            },
          },
          {
            path: '/permission/department',
            name: 'Department',
            component: () => import('@/views/permission/department.vue'),
            meta: {
              title: '部门管理',
              requiresAuth: true,
            },
          },
          {
            path: '/permission/menu',
            name: 'Menu',
            component: () => import('@/views/permission/menu.vue'),
            meta: {
              title: '菜单管理',
              requiresAuth: true,
            },
          },
          {
            path: '/permission/role',
            name: 'Role',
            component: () => import('@/views/permission/role.vue'),
            meta: {
              title: '角色管理',
              requiresAuth: true,
            },
          },
          {
            path: '/permission/button',
            name: 'Button',
            component: () => import('@/views/permission/button.vue'),
            meta: {
              title: '按钮管理',
              requiresAuth: true,
            },
          },
          {
            path: '/permission/menu-permission',
            name: 'MenuPermission',
            component: () => import('@/views/permission/menu-permission.vue'),
            meta: {
              title: '权限配置',
              requiresAuth: true,
            },
          },
        ],
      },
      // 内容管理
      {
        path: '/content',
        name: 'Content',
        redirect: '/content/articles',
        meta: {
          title: '内容管理',
          icon: 'Document',
          requiresAuth: true,
        },
        children: [
          {
            path: '/content/articles',
            name: 'Articles',
            component: () => import('@/views/articles/index.vue'),
            meta: {
              title: '文章管理',
              requiresAuth: true,
            },
          },
          {
            path: '/content/articles/create',
            name: 'ArticleCreate',
            component: () => import('@/views/articles/create.vue'),
            meta: {
              title: '写文章',
              hidden: true,
              requiresAuth: true,
            },
          },
          {
            path: '/content/articles/edit/:id',
            name: 'ArticleEdit',
            component: () => import('@/views/articles/edit.vue'),
            meta: {
              title: '编辑文章',
              hidden: true,
              requiresAuth: true,
            },
          },
          {
            path: '/content/articles/view/:id',
            name: 'ArticleView',
            component: () => import('@/views/articles/edit.vue'),
            meta: {
              title: '查看文章',
              hidden: true,
              requiresAuth: true,
            },
          },
          {
            path: '/content/categories',
            name: 'Categories',
            component: () => import('@/views/categories/index.vue'),
            meta: {
              title: '分类管理',
              requiresAuth: true,
            },
          },
          {
            path: '/content/tags',
            name: 'Tags',
            component: () => import('@/views/tags/index.vue'),
            meta: {
              title: '标签管理',
              requiresAuth: true,
            },
          },
          {
            path: '/content/comments',
            name: 'Comments',
            component: () => import('@/views/comments/index.vue'),
            meta: {
              title: '评论管理',
              requiresAuth: true,
            },
          },
        ],
      },
      // 系统设置
      {
        path: '/system',
        name: 'System',
        redirect: '/system/basic',
        meta: {
          title: '系统设置',
          icon: 'Setting',
          requiresAuth: true,
        },
        children: [
          {
            path: '/system/basic',
            name: 'BasicSettings',
            component: () => import('@/views/system/basic.vue'),
            meta: {
              title: '基本设置',
              requiresAuth: true,
            },
          },
          {
            path: '/system/seo',
            name: 'SeoSettings',
            component: () => import('@/views/system/seo.vue'),
            meta: {
              title: 'SEO设置',
              requiresAuth: true,
            },
          },
          {
            path: '/system/email',
            name: 'EmailSettings',
            component: () => import('@/views/system/email.vue'),
            meta: {
              title: '邮件设置',
              requiresAuth: true,
            },
          },
          {
            path: '/system/storage',
            name: 'StorageSettings',
            component: () => import('@/views/system/storage.vue'),
            meta: {
              title: '存储设置',
              requiresAuth: true,
            },
          },
        ],
      },
      // 数据统计
      {
        path: '/statistics',
        name: 'Statistics',
        redirect: '/statistics/overview',
        meta: {
          title: '数据统计',
          icon: 'DataAnalysis',
          requiresAuth: true,
        },
        children: [
          {
            path: '/statistics/overview',
            name: 'StatisticsOverview',
            component: () => import('@/views/statistics/overview.vue'),
            meta: {
              title: '数据概览',
              requiresAuth: true,
            },
          },
          {
            path: '/statistics/article',
            name: 'ArticleStatistics',
            component: () => import('@/views/statistics/article.vue'),
            meta: {
              title: '文章统计',
              requiresAuth: true,
            },
          },
          {
            path: '/statistics/user',
            name: 'UserStatistics',
            component: () => import('@/views/statistics/user.vue'),
            meta: {
              title: '用户统计',
              requiresAuth: true,
            },
          },
          {
            path: '/statistics/visit',
            name: 'VisitStatistics',
            component: () => import('@/views/statistics/visit.vue'),
            meta: {
              title: '访问统计',
              requiresAuth: true,
            },
          },
        ],
      },
    ],
  },
  {
    path: '/404',
    name: 'NotFound',
    component: () => import('@/views/error/404.vue'),
    meta: {
      title: '404',
      hidden: true,
    },
  },
  {
    path: '/:pathMatch(.*)*',
    redirect: '/404',
  },
]

// 创建路由实例
const router = createRouter({
  history: createWebHistory(),
  routes,
})

// 路由守卫
router.beforeEach((to, from, next) => {
  const userStore = useUserStore()
  const title = to.meta.title as string
  
  // 设置页面标题
  if (title) {
    document.title = `${title} - ${import.meta.env.VITE_APP_TITLE}`
  }

  // 检查是否需要登录
  if (to.meta.requiresAuth) {
    if (userStore.token) {
      next()
    } else {
      next('/login')
    }
  } else {
    // 已登录用户访问登录页，重定向到首页
    if (to.path === '/login' && userStore.token) {
      next('/')
    } else {
      next()
    }
  }
})

export default router
