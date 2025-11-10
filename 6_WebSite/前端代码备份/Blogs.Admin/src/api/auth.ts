import { request } from '@/utils/request'
import type { ApiResponse, LoginForm, LoginResponse } from '@/types'

// 登录
export const login = (data: LoginForm) => {
  return request.post<LoginResponse>('/api/admin/Account/login', data)
}

// 登出
export const logout = () => {
  return request.post<ApiResponse>('/auth/logout')
}

// 获取用户信息
export const getUserInfo = () => {
  return request.get('/api/admin/Account/userinfo')
}
