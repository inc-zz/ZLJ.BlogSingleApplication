import request from '@/utils/request'
import type { ApiResponse, PageParams, PageData } from '@/types'

// 配置类型选项
export const busTypeOptions = [
  { name: '文件存储', value: 'FileStorage' },
  { name: '开源项目', value: 'OpenSourceProject' },
  { name: '开源书籍', value: 'OpenSourceBook' },
  { name: '存储目录', value: 'FileStoreDictionary' },
] as const

// 系统配置项
export interface SysConfig {
  id: number
  title: string
  summary?: string
  url?: string
  tags?: string
  busType: string
  content?: string
  status: number
  statusName?: string
}

// 查询参数
export interface SysConfigListParams extends PageParams {
  Name?: string
  Where?: string
}

// 创建/更新配置参数
export interface SysConfigFormData {
  id?: number
  title: string
  summary?: string
  url?: string
  tags?: string
  busType: string
  content?: string
}

// 获取配置列表
export const getSysConfigList = (params: SysConfigListParams) => {
  return request.get<PageData<SysConfig>>('/api/admin/SysManager/list', { params })
}

// 添加或修改配置
export const saveSysConfig = (data: SysConfigFormData) => {
  return request.post<ApiResponse<void>>('/api/admin/SysManager/set', data)
}

// 删除配置
export const deleteSysConfig = (id: number) => {
  return request.delete<ApiResponse<void>>('/api/admin/SysManager', { data: { id } })
}
