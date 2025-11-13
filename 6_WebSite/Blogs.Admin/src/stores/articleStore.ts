import { defineStore } from 'pinia';
import {
  fetchCategories,
} from '@/api/admin/article';
import type { Category, Comment } from '@/api/admin/article';

export const useArticleStore = defineStore('article', {
  state: () => ({
    categories: [] as Category[],
    categoryDetail: null as Category | null,
    comments: [] as Comment[],
    pagination: {
      pageIndex: 1,
      pageSize: 30,
      total: 0,
    },
  }),

  actions: {
    // 分类管理
    async loadCategories(pageIndex: number = 1, pageSize: number = 30, searchTerm: string = '') {
      const res = await fetchCategories(pageIndex, pageSize, searchTerm);
      this.categories = res.data.items;
      this.pagination = {
        pageIndex: res.data.pageIndex,
        pageSize: res.data.pageSize,
        total: res.data.total,
      };
    },
  },
});