import { request } from '@/utils/request'
import type { PageData } from '@/types'

// 首页统计数据
export interface HomeStatistics {
  userCount: number
  articleCount: number
  articleCommentCount: number
  articleViewCount: number
}

// 文章列表项
export interface ArticleItem {
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

// 获取首页统计数据
export const getHomeStatistics = () => {
  return request.get<HomeStatistics>('/api/admin/Home/statistics')
}

// 获取首页文章列表
export const getHomeArticleList = (topCount: number = 10) => {
  return request.get<PageData<ArticleItem>>('/api/admin/Home/articleList', {
    params: { TopCount: topCount }
  })
}
