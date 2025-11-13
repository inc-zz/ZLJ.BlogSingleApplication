import { request } from '@/utils/request'
import type { PageParams } from '@/types'

// 文章列表项
export interface ArticleListItem {
  id: number
  title: string
  createdAt: string
  createdBy: string
  tags: string
  summary: string
  coverImage: string | null
  categoryName: string
  viewCount: number
  likeCount: number
  commentCount: number
}

// 文章详情
export interface ArticleDetail {
  id: number
  title: string
  categoryId: number
  categoryName: string
  createdAt: string
  createdBy: string
  summary: string
  tags: string
  content: string
  coverImage: string | null
  viewCount: number
  likeCount: number
  commentCount: number
  shareCount: number
}

// 文章编辑请求参数
export interface ArticleFormData {
  id?: number
  title: string
  summary: string
  categoryId: number
  tags: string
  content: string
  isPublish: boolean
}

// 文章列表请求参数
export interface ArticleListParams extends PageParams {
  CategoryId?: number
  TagId?: number
  SortBy?: string
  Where?: string  // 文章标题or简介
  CreatedBy?: string  // 创建人
  StartDate?: string  // 开始时间
  EndDate?: string  // 结束时间
}

// 文章分类
export interface ArticleCategory {
  id: number
  name: string
  articleCount: number
  description: string
  slug: string
}

// 文件上传响应
export interface UploadResponse {
  success: boolean
  message: string
  fileRecord: any
  fileUrl: string
}

// 评论列表项
export interface CommentListItem {
  id: number
  articleId: number
  articleTitle: string
  content: string
  createdAt: string
  createdBy: string
  likeCount: number
  replies: any
}

// 评论列表请求参数
export interface CommentListParams extends PageParams {
  SearchTerm?: string
}

// 获取文章列表
export const getArticleList = (params: ArticleListParams) => {
  return request.get<ArticleListItem[]>('/api/admin/Article/list', { params })
}

// 获取文章分类列表
export const getArticleCategories = (topCount?: number) => {
  const params = topCount ? { TopCount: topCount } : {}
  return request.get<ArticleCategory[]>('/api/admin/Article/category/ddl', { params })
}

// 获取文章详情
export const getArticleDetail = (articleId: number) => {
  return request.get<ArticleDetail>('/api/admin/Article/detail', { 
    params: { ArticleId: articleId } 
  })
}

// 创建/编辑文章
export const saveArticle = (data: ArticleFormData) => {
  return request.put('/api/admin/Article', data)
}

// 删除文章
export const deleteArticle = (id: number) => {
  return request.delete('/api/admin/Article', { data: { id } })
}

// 批量删除文章
export const batchDeleteArticles = (ids: number[]) => {
  return request.delete('/api/admin/Article/batch', { data: ids })
}

// 上传文章图片
export const uploadArticleImage = (file: File) => {
  const formData = new FormData()
  formData.append('File', file)
  formData.append('BusinessType', 'ArticleFiles')
  
  return request.post<UploadResponse>('/api/admin/Article/upload', formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  })
}

// 获取评论列表
export const getCommentList = (params: CommentListParams) => {
  return request.get('/api/admin/Article/comment', { params })
}

// 删除评论
export const deleteComment = (id: number) => {
  return request.delete('/api/admin/Article/comment', { data: { id } })
}
