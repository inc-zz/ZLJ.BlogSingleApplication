import { request } from '@/utils/request'
import type { ApiResponse, Category, PageParams, PageData } from '@/types'

// 获取分类列表
export const getCategoryList = (params?: PageParams) => {
  return request.get<ApiResponse<PageData<Category>>>('/categories', { params })
}

// 获取所有分类（不分页）
export const getAllCategories = () => {
  return request.get<ApiResponse<Category[]>>('/categories/all')
}

// 创建分类
export const createCategory = (data: Partial<Category>) => {
  return request.post<ApiResponse<Category>>('/categories', data)
}

// 更新分类
export const updateCategory = (id: number, data: Partial<Category>) => {
  return request.put<ApiResponse<Category>>(`/categories/${id}`, data)
}

// 删除分类
export const deleteCategory = (id: number) => {
  return request.delete<ApiResponse>(`/categories/${id}`)
}
