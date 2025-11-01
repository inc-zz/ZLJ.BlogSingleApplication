import { request } from '@/utils/request'
import type { PageParams, PageData, Admin } from '@/types'

// 角色信息接口
export interface RoleOption {
  id: number
  code: string
  name: string
}

// 获取所有角色列表
export const getAllRoles = () => {
  return request.get<RoleOption[]>('/api/admin/Role/all')
}

// 获取管理员列表
export const getAdminList = (params: PageParams & { where?: string }) => {
  return request.get<PageData<Admin>>('/api/admin/User/list', { params })
}

// 获取管理员详情
export const getAdminDetail = (id: number) => {
  return request.get<Admin>(`/api/admin/User/info`, { params: { id } })
}

// 添加管理员
export const createAdmin = (data: {
  userName: string
  password: string
  departmentJson?: string
  userRoleJson?: string
  description?: string
  email: string
  phoneNumber: string
  realName: string
}) => {
  return request.post('/api/admin/user', data)
}

// 编辑管理员
export const updateAdmin = (data: {
  Id: number
  departmentJson?: string
  userRoleJson?: string
  description?: string
  email?: string
  phoneNumber?: string
  realName?: string
  sex?: number
}) => {
  return request.put('/api/admin/user', data)
}

// 删除管理员
export const deleteAdmin = (id: number) => {
  return request.delete('/api/admin/user', { data: { id } })
}

// 批量删除管理员
export const batchDeleteAdmin = (ids: number[]) => {
  return request.delete('/api/admin/User/batch', { data: ids })
}

// 启用/禁用管理员
export const updateAdminStatus = (data: { id: number; status: number }) => {
  return request.put('/api/admin/user/status', data)
}

// 重置管理员密码
export const resetAdminPassword = (data: {
  userId: number
  password: string
  oldPassword: string
}) => {
  return request.put('/api/admin/User/resetPassword', data)
}

// 设置用户角色
export const setUserRoles = (data: { userId: number; userRoleJson: string }) => {
  return request.put('/api/admin/User/setRoles', data)
}
