import axios from 'axios';
import type { AxiosResponse } from 'axios';

export type Category = {
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

export type Comment=[];

export type PaginatedResponse<T> = {
  pageIndex: number;
  pageSize: number;
  total: number;
  items: T[];
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

