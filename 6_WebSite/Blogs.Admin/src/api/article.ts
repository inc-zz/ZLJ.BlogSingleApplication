import { request } from '@/utils/request'
import type { ApiResponse, Article, PageParams, PageData } from '@/types'

// 获取文章列表
export const getArticleList = (params: PageParams & { keyword?: string; status?: string }) => {
  return request.get<ApiResponse<PageData<Article>>>('/articles', { params })
}

// 获取文章详情
export const getArticleDetail = (id: number) => {
  return request.get<ApiResponse<Article>>(`/articles/${id}`)
}

// 创建文章
export const createArticle = (data: Partial<Article>) => {
  return request.post<ApiResponse<Article>>('/articles', data)
}

// 更新文章
export const updateArticle = (id: number, data: Partial<Article>) => {
  return request.put<ApiResponse<Article>>(`/articles/${id}`, data)
}

// 删除文章
export const deleteArticle = (id: number) => {
  return request.delete<ApiResponse>(`/articles/${id}`)
}

// 批量删除文章
export const batchDeleteArticles = (ids: number[]) => {
  return request.post<ApiResponse>('/articles/batch-delete', { ids })
}
