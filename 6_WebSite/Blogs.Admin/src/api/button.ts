import { request } from '@/utils/request'
import type { PageParams, PageData } from '@/types'

// 按钮信息
export interface SysButton {
  id: number
  code: string
  name: string
  description: string
  icon: string
  buttonType: string
  position: string
  status: number
  statusName: string
  createdAt: string
  createdBy: string | null
}

// 按钮详情（包含关联菜单）
export interface SysButtonDetail extends SysButton {
  relatedMenus: any[]
}

// 按钮列表请求参数
export interface ButtonListParams extends PageParams {
  Name?: string
  Position?: string
}

// 获取按钮列表
export const getButtonList = (params: ButtonListParams) => {
  return request.get<PageData<SysButton>>('/api/admin/SysButton/list', { params })
}

// 获取按钮详情
export const getButtonDetail = (id: number) => {
  return request.get<SysButtonDetail>('/api/admin/SysButton/info', { params: { id } })
}

// 创建按钮
export const createButton = (data: {
  name: string
  code: string
  position: string
  description?: string
  icon?: string
  buttonType?: string
}) => {
  return request.post('/api/admin/SysButton', data)
}

// 更新按钮
export const updateButton = (data: {
  id: number
  name: string
  code: string
  position: string
  description?: string
  icon?: string
  buttonType?: string
}) => {
  return request.put('/api/admin/SysButton', data)
}

// 删除按钮
export const deleteButton = (id: number) => {
  return request.delete('/api/admin/SysButton', { data: { id } })
}

// 更新按钮状态
export const updateButtonStatus = (data: { id: number; status: number }) => {
  return request.put('/api/admin/SysButton/status', data)
}
