// 通用响应接口
export interface ApiResponse<T = any> {
  code: number
  message: string
  data: T
  success: boolean
}

// 分页请求参数
export interface PageParams {
  pageIndex: number
  pageSize: number
}

// 分页响应数据
export interface PageData<T = any> {
  pageIndex: number
  pageSize: number
  total: number
  items: T[]
  success: boolean
  message: string
  code: number
}

// 用户信息
export interface UserInfo {
  userName: string
  realName: string
  phoneNumber: string
  email: string
  avatar?: string
}

// 登录表单
export interface LoginForm {
  account: string
  password: string
  captcha: string
  remember?: boolean
}

// 登录响应数据
export interface LoginResponse {
  userInfo: UserInfo
  accessToken: string
  refreshToken: string
  expiresIn: string
  tokenType: string
}

// 菜单项
export interface MenuItem {
  id: number
  name: string
  path: string
  icon?: string
  component?: string
  children?: MenuItem[]
  meta?: {
    title: string
    hidden?: boolean
    requiresAuth?: boolean
  }
}

// 博客文章
export interface Article {
  id: number
  title: string
  content: string
  summary: string
  coverImage?: string
  author: string
  categoryId: number
  tags: string[]
  status: 'draft' | 'published' | 'archived'
  views: number
  createTime: string
  updateTime: string
}

// 文章分类
export interface Category {
  id: number
  name: string
  description?: string
  sort: number
  createTime: string
}

// 标签
export interface Tag {
  id: number
  name: string
  color?: string
  createTime: string
}

// 评论
export interface Comment {
  id: number
  articleId: number
  content: string
  author: string
  email: string
  createTime: string
  status: 'pending' | 'approved' | 'rejected'
}

// 角色信息
export interface RoleInfo {
  userId: number
  roleId: number
  name: string
  code: string
}

// 管理员
export interface Admin {
  id: number
  userName: string
  realName: string
  phoneNumber: string | null
  email: string
  departmentId: number | null
  departmentName: string | null
  roles: RoleInfo[]
  status: number
  statusName: string
  lastLoginTime: string
  createdAt: string
  description: string
}

// 部门
export interface Department {
  id: number
  name: string
  description: string
  createTime: string
  updateTime: string
  status: number
}

// 菜单
export interface Menu {
  id: number
  name: string
  type: string
  path: string
  icon: string
  sort: number
  status: number
}

// 角色
export interface Role {
  id: number
  name: string
  description: string
  permissionCount: number
  userCount: number
  status: number
  createTime: string
}
