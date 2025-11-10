import { request } from '@/utils/request'
import type { ApiResponse, Tag, PageParams, PageData } from '@/types'

// 获取标签列表
export const getTagList = (params?: PageParams) => {
  return request.get<PageData<Tag>>('/api/admin/Article/tags', { params })
}

// 获取所有标签（不分页）
export const getAllTags = () => {
  return request.get<ApiResponse<Tag[]>>('/tags/all')
}

// 创建标签
export const createTag = (data: Partial<Tag>) => {
  return request.post<ApiResponse<Tag>>('/tags', data)
}

// 更新标签
export const updateTag = (id: number, data: Partial<Tag>) => {
  return request.put<ApiResponse<Tag>>(`/tags/${id}`, data)
}

// 删除标签
export const deleteTag = (id: number) => {
  return request.delete<ApiResponse>(`/tags/${id}`)
}
