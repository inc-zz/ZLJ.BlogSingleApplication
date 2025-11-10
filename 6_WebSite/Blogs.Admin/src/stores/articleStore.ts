import { defineStore } from 'pinia';
import {
  fetchCategories,
  createOrUpdateCategory,
  deleteCategory,
  fetchCategoryDetail,
  fetchComments,
  deleteComment,
} from '@/api/admin/article';
import type { Category, Comment, PaginatedResponse } from '@/api/admin/article';

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

    async createOrUpdateCategory(data: {
      id?: number;
      name: string;
      description: string;
      sort: number;
      icon: string;
    }) {
      await createOrUpdateCategory(data);
      await this.loadCategories();
    },

    async removeCategory(id: number) {
      await deleteCategory(id);
      await this.loadCategories();
    },

    async loadCategoryDetail(id: number) {
      const res = await fetchCategoryDetail(id);
      this.categoryDetail = res.data.data;
    },

    // 评论管理
    async loadComments(pageIndex: number = 1, pageSize: number = 30, searchTerm: string = '') {
      const res = await fetchComments(pageIndex, pageSize, searchTerm);
      this.comments = res.data.items;
      this.pagination = {
        pageIndex: res.data.pageIndex,
        pageSize: res.data.pageSize,
        total: res.data.total,
      };
    },

    async removeComment(id: number) {
      await deleteComment(id);
      await this.loadComments();
    },
  },
});