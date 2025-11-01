import Mock from 'mockjs'
import type { MockMethod } from 'vite-plugin-mock'

const mockData: MockMethod[] = [
  // 登录接口
  {
    url: '/api/auth/login',
    method: 'post',
    response: ({ body }) => {
      const { username, password } = body
      if (username === 'admin' && password === '123456') {
        return {
          code: 200,
          message: '登录成功',
          data: {
            token: Mock.Random.guid(),
            userInfo: {
              id: 1,
              username: 'admin',
              email: 'admin@example.com',
              avatar: 'https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png',
              role: 'admin',
              createTime: Mock.Random.datetime(),
            },
          },
        }
      } else {
        return {
          code: 400,
          message: '用户名或密码错误',
          data: null,
        }
      }
    },
  },

  // 获取用户信息
  {
    url: '/api/auth/userinfo',
    method: 'get',
    response: () => {
      return {
        code: 200,
        message: '成功',
        data: {
          id: 1,
          username: 'admin',
          email: 'admin@example.com',
          avatar: 'https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png',
          role: 'admin',
          createTime: Mock.Random.datetime(),
        },
      }
    },
  },

  // 获取文章列表
  {
    url: '/api/articles',
    method: 'get',
    response: ({ query }) => {
      const { page = 1, pageSize = 10 } = query
      const list = Mock.mock({
        [`list|${pageSize}`]: [
          {
            'id|+1': 1,
            title: '@ctitle(10, 30)',
            content: '@cparagraph(5, 10)',
            summary: '@cparagraph(1, 3)',
            coverImage: '@image(400x300)',
            author: '@cname',
            'categoryId|1-5': 1,
            'tags|1-3': ['@word'],
            'status|1': ['draft', 'published', 'archived'],
            'views|100-10000': 1,
            createTime: '@datetime',
            updateTime: '@datetime',
          },
        ],
      })

      return {
        code: 200,
        message: '成功',
        data: {
          list: list.list,
          total: 100,
          page: Number(page),
          pageSize: Number(pageSize),
        },
      }
    },
  },

  // 获取文章详情
  {
    url: '/api/articles/:id',
    method: 'get',
    response: ({ query }) => {
      return {
        code: 200,
        message: '成功',
        data: Mock.mock({
          id: query.id,
          title: '@ctitle(10, 30)',
          content: '@cparagraph(10, 20)',
          summary: '@cparagraph(1, 3)',
          coverImage: '@image(400x300)',
          author: '@cname',
          'categoryId|1-5': 1,
          'tags|1-3': ['vue', 'typescript', 'javascript'],
          'status|1': ['draft', 'published'],
          'views|100-10000': 1,
          createTime: '@datetime',
          updateTime: '@datetime',
        }),
      }
    },
  },

  // 创建文章
  {
    url: '/api/articles',
    method: 'post',
    response: ({ body }) => {
      return {
        code: 200,
        message: '创建成功',
        data: {
          id: Mock.Random.id(),
          ...body,
          createTime: Mock.Random.datetime(),
          updateTime: Mock.Random.datetime(),
        },
      }
    },
  },

  // 更新文章
  {
    url: '/api/articles/:id',
    method: 'put',
    response: ({ body }) => {
      return {
        code: 200,
        message: '更新成功',
        data: body,
      }
    },
  },

  // 删除文章
  {
    url: '/api/articles/:id',
    method: 'delete',
    response: () => {
      return {
        code: 200,
        message: '删除成功',
        data: null,
      }
    },
  },

  // 获取分类列表
  {
    url: '/api/categories',
    method: 'get',
    response: ({ query }) => {
      const { page = 1, pageSize = 10 } = query
      const list = Mock.mock({
        [`list|${pageSize}`]: [
          {
            'id|+1': 1,
            name: '@cword(2, 6)',
            description: '@cparagraph(1, 2)',
            'sort|0-100': 1,
            createTime: '@datetime',
          },
        ],
      })

      return {
        code: 200,
        message: '成功',
        data: {
          list: list.list,
          total: 50,
          page: Number(page),
          pageSize: Number(pageSize),
        },
      }
    },
  },

  // 获取标签列表
  {
    url: '/api/tags',
    method: 'get',
    response: ({ query }) => {
      const { page = 1, pageSize = 10 } = query
      const list = Mock.mock({
        [`list|${pageSize}`]: [
          {
            'id|+1': 1,
            name: '@word(3, 8)',
            'color|1': ['#409eff', '#67c23a', '#e6a23c', '#f56c6c', '#909399'],
            createTime: '@datetime',
          },
        ],
      })

      return {
        code: 200,
        message: '成功',
        data: {
          list: list.list,
          total: 30,
          page: Number(page),
          pageSize: Number(pageSize),
        },
      }
    },
  },

  // 创建分类
  {
    url: '/api/categories',
    method: 'post',
    response: ({ body }) => {
      return {
        code: 200,
        message: '创建成功',
        data: {
          id: Mock.Random.id(),
          ...body,
          createTime: Mock.Random.datetime(),
        },
      }
    },
  },

  // 更新分类
  {
    url: '/api/categories/:id',
    method: 'put',
    response: ({ body }) => {
      return {
        code: 200,
        message: '更新成功',
        data: body,
      }
    },
  },

  // 删除分类
  {
    url: '/api/categories/:id',
    method: 'delete',
    response: () => {
      return {
        code: 200,
        message: '删除成功',
        data: null,
      }
    },
  },

  // 创建标签
  {
    url: '/api/tags',
    method: 'post',
    response: ({ body }) => {
      return {
        code: 200,
        message: '创建成功',
        data: {
          id: Mock.Random.id(),
          ...body,
          createTime: Mock.Random.datetime(),
        },
      }
    },
  },

  // 更新标签
  {
    url: '/api/tags/:id',
    method: 'put',
    response: ({ body }) => {
      return {
        code: 200,
        message: '更新成功',
        data: body,
      }
    },
  },

  // 删除标签
  {
    url: '/api/tags/:id',
    method: 'delete',
    response: () => {
      return {
        code: 200,
        message: '删除成功',
        data: null,
      }
    },
  },
]

export default mockData
