import { request } from '@/utils/request'
import type { ApiResponse, PageParams, PageData } from '@/types'

// App用户信息
export interface AppUser {
  id: number
  account: string
  realName: string | null
  email: string
  bio: string | null
  lastLoginIp: string
  lastLoginTime: string
  status: number
  statusName: string
  createdAt: string
  createdBy: string | null
  phoneNumber?: string
  avatar?: string
  remark?: string
}

// 用户列表查询参数
export interface AppUserListParams extends PageParams {
  Status?: number
  Where?: string
}

// 创建用户参数
export interface CreateAppUserParams {
  account: string
  password: string
  realName: string
  remark?: string
  email: string
  phoneNumber?: string
}

// 更新用户参数
export interface UpdateAppUserParams {
  id: number
  remark?: string
  email?: string
  phoneNumber?: string
  avatar?: string
}

// 重置密码参数
export interface ResetPasswordParams {
  id: number
  password: string
}

// 获取用户列表
export const getAppUserList = (params: AppUserListParams) => {
  return request.get<PageData<AppUser>>('/api/admin/AppUser/list', { params })
}

// 添加用户
export const createAppUser = (data: CreateAppUserParams) => {
  return request.post<ApiResponse<void>>('/api/admin/AppUser/create', data)
}

// 获取用户详情
export const getAppUserInfo = (id: number) => {
  return request.get<ApiResponse<AppUser>>('/api/admin/AppUser/info', { 
    params: { Id: id } 
  })
}

// 启用/禁用用户
export const toggleAppUserStatus = (id: number) => {
  return request.put<ApiResponse<void>>('/api/admin/AppUser/status', null, { 
    params: { Id: id } 
  })
}

// 修改用户
export const updateAppUser = (data: UpdateAppUserParams) => {
  return request.put<ApiResponse<void>>('/api/admin/AppUser/update', data)
}

// 重置密码
export const resetAppUserPassword = (data: ResetPasswordParams) => {
  return request.put<ApiResponse<void>>('/api/admin/AppUser/resetPassword', data)
}
