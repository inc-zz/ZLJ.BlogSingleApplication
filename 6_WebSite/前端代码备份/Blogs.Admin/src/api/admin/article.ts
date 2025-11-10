import axios from 'axios';
import type { AxiosResponse } from 'axios';

type Category = {
  id: number;
  name: string;
  description: string;
  sort: number;
  status: number;
  createdAt: string;
  createdBy: string;
  modifiedAt: string | null;
  modifiedBy: string | null;
  statusName: string;
};

type Comment = {
  id: number;
  articleId: number;
  content: string;
  createdAt: string;
  createdBy: string;
  likeCount: number;
  replies: any[] | null;
};

type PaginatedResponse<T> = {
  pageIndex: number;
  pageSize: number;
  total: number;
  items: T[];
  success: boolean;
  message: string;
  code: number;
  traceId: string | null;
};

type BaseResponse = {
  success: boolean;
  message: string;
  code: number;
  traceId: string | null;
};

// 分类管理接口
export const fetchCategories = (
  pageIndex: number = 1,
  pageSize: number = 30,
  searchTerm: string = ''
): Promise<AxiosResponse<PaginatedResponse<Category>>> => {
  return axios.get('/api/admin/Article/category', {
    params: { PageIndex: pageIndex, PageSize: pageSize, SearchTerm: searchTerm },
  });
};

export const createOrUpdateCategory = (
  data: {
    id?: number;
    name: string;
    description: string;
    sort: number;
    icon: string;
  }
): Promise<AxiosResponse<BaseResponse>> => {
  return axios.post('/api/admin/Article/category', data);
};

export const deleteCategory = (id: number): Promise<AxiosResponse<BaseResponse>> => {
  return axios.delete('/api/admin/Article/category', { data: { id } });
};

export const fetchCategoryDetail = (id: number): Promise<AxiosResponse<{ data: Category } & BaseResponse>> => {
  return axios.get(`/api/admin/Article/categories/${id}`);
};

// 评论管理接口
export const fetchComments = (
  pageIndex: number = 1,
  pageSize: number = 30,
  searchTerm: string = ''
): Promise<AxiosResponse<PaginatedResponse<Comment>>> => {
  return axios.get('/api/admin/Article/comment', {
    params: { PageIndex: pageIndex, PageSize: pageSize, SearchTerm: searchTerm },
  });
};

export const deleteComment = (id: number): Promise<AxiosResponse<BaseResponse>> => {
  return axios.delete('/api/admin/Article/comment', { data: { id } });
};