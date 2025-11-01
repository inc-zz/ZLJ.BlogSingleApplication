import { ref } from 'vue'
import type { PageParams, PageData } from '@/types'

export function usePagination<T = any>() {
  const loading = ref(false)
  const data = ref<T[]>([])
  const total = ref(0)
  const page = ref(1)
  const pageSize = ref(10)

  const loadData = async (
    fetchFn: (params: PageParams) => Promise<PageData<T>>,
    params?: any
  ) => {
    loading.value = true
    try {
      const result = await fetchFn({
        page: page.value,
        pageSize: pageSize.value,
        ...params,
      })
      data.value = result.list
      total.value = result.total
    } catch (error) {
      console.error('加载数据失败:', error)
    } finally {
      loading.value = false
    }
  }

  const handlePageChange = (newPage: number, newPageSize: number) => {
    page.value = newPage
    pageSize.value = newPageSize
  }

  const reset = () => {
    page.value = 1
    pageSize.value = 10
    data.value = []
    total.value = 0
  }

  return {
    loading,
    data,
    total,
    page,
    pageSize,
    loadData,
    handlePageChange,
    reset,
  }
}
