import { request } from '@/utils/request'
import type { ApiResponse, Category, PageParams, PageData } from '@/types'

// 获取分类列表
export const getCategoryList = (params?: PageParams) => {
  return request.get<PageData<Category>>('/api/admin/Article/category', { params })
}

// 获取所有分类（不分页）
export const getAllCategories = () => {
  return request.get<ApiResponse<Category[]>>('/api/admin/Article/categories/all')
}

// 创建/编辑分类（使用同一个接口，通过id区分）
export const saveCategory = (data: Partial<Category>) => {
  return request.post<ApiResponse<Category>>('/api/admin/Article/category', data)
}

// 创建分类（兼容旧代码）
export const createCategory = (data: Partial<Category>) => {
  return saveCategory(data)
}

// 更新分类（兼容旧代码）
export const updateCategory = (id: number, data: Partial<Category>) => {
  data.id = id
  return saveCategory(data)
}

// 删除分类
export const deleteCategory = (id: number) => {
  const postData = {id: id}
  return request.delete<ApiResponse>(`/api/admin/Article/category`, { data: postData })
}
