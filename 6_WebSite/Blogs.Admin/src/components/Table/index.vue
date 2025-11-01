<template>
  <div class="common-table">
    <el-table
      :data="data"
      :border="border"
      :stripe="stripe"
      :loading="loading"
      :height="height"
      :max-height="maxHeight"
      v-bind="$attrs"
      @selection-change="handleSelectionChange"
    >
      <el-table-column v-if="selection" type="selection" width="55" />
      <el-table-column v-if="index" type="index" label="序号" width="60" />
      <slot></slot>
    </el-table>

    <!-- 分页 -->
    <el-pagination
      v-if="pagination"
      v-model:current-page="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      :page-sizes="pageSizes"
      :layout="layout"
      :background="true"
      class="pagination"
      @size-change="handleSizeChange"
      @current-change="handleCurrentChange"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'

interface Props {
  data: any[]
  total?: number
  pagination?: boolean
  selection?: boolean
  index?: boolean
  border?: boolean
  stripe?: boolean
  loading?: boolean
  height?: string | number
  maxHeight?: string | number
  pageSizes?: number[]
  layout?: string
}

const props = withDefaults(defineProps<Props>(), {
  total: 0,
  pagination: true,
  selection: false,
  index: true,
  border: true,
  stripe: true,
  loading: false,
  pageSizes: () => [10, 20, 50, 100],
  layout: 'total, sizes, prev, pager, next, jumper',
})

const emit = defineEmits<{
  'page-change': [page: number, pageSize: number]
  'selection-change': [selection: any[]]
}>()

const currentPage = ref(1)
const pageSize = ref(10)

watch(
  () => props.data,
  () => {
    // 数据变化时，重置到第一页
    if (currentPage.value !== 1) {
      currentPage.value = 1
    }
  }
)

const handleSizeChange = (size: number) => {
  pageSize.value = size
  currentPage.value = 1
  emit('page-change', currentPage.value, pageSize.value)
}

const handleCurrentChange = (page: number) => {
  currentPage.value = page
  emit('page-change', currentPage.value, pageSize.value)
}

const handleSelectionChange = (selection: any[]) => {
  emit('selection-change', selection)
}
</script>

<style scoped lang="scss">
.common-table {
  .pagination {
    margin-top: 20px;
    display: flex;
    justify-content: flex-end;
  }
}
</style>
