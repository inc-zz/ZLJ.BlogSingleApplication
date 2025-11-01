<template>
  <div class="articles-page">
    <el-card>
      <!-- 搜索栏 -->
      <el-form :inline="true" :model="searchForm" class="search-form">
        <el-form-item label="关键词">
          <el-input v-model="searchForm.keyword" placeholder="请输入标题关键词" clearable />
        </el-form-item>
        <el-form-item label="状态">
          <el-select v-model="searchForm.status" placeholder="请选择状态" clearable>
            <el-option label="草稿" value="draft" />
            <el-option label="已发布" value="published" />
            <el-option label="已归档" value="archived" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <!-- 操作栏 -->
      <div class="toolbar">
        <el-button type="primary" @click="handleCreate">新建文章</el-button>
        <el-button
          type="danger"
          :disabled="selectedRows.length === 0"
          @click="handleBatchDelete"
        >
          批量删除
        </el-button>
      </div>

      <!-- 表格 -->
      <CommonTable
        :data="tableData"
        :total="total"
        :loading="loading"
        selection
        @page-change="handlePageChange"
        @selection-change="handleSelectionChange"
      >
        <el-table-column prop="title" label="标题" min-width="200" />
        <el-table-column prop="author" label="作者" width="120" />
        <el-table-column prop="categoryId" label="分类" width="120" />
        <el-table-column prop="status" label="状态" width="100">
          <template #default="{ row }">
            <el-tag v-if="row.status === 'published'" type="success">已发布</el-tag>
            <el-tag v-else-if="row.status === 'draft'" type="info">草稿</el-tag>
            <el-tag v-else type="warning">已归档</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="views" label="阅读量" width="100" />
        <el-table-column prop="createTime" label="创建时间" width="180" />
        <el-table-column label="操作" width="200" fixed="right">
          <template #default="{ row }">
            <el-button link type="primary" size="small" @click="handleEdit(row)">
              编辑
            </el-button>
            <el-button link type="danger" size="small" @click="handleDelete(row)">
              删除
            </el-button>
          </template>
        </el-table-column>
      </CommonTable>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage, ElMessageBox } from 'element-plus'
import CommonTable from '@/components/Table/index.vue'
import { getArticleList, deleteArticle, batchDeleteArticles } from '@/api/article'

const router = useRouter()

const loading = ref(false)
const tableData = ref([])
const total = ref(0)
const selectedRows = ref([])

const searchForm = reactive({
  keyword: '',
  status: '',
})

const handleSearch = () => {
  loadData()
}

const handleReset = () => {
  searchForm.keyword = ''
  searchForm.status = ''
  loadData()
}

const handleCreate = () => {
  router.push('/articles/create')
}

const handleEdit = (row: any) => {
  router.push(`/articles/edit/${row.id}`)
}

const handleDelete = (row: any) => {
  ElMessageBox.confirm('确定要删除这篇文章吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    await deleteArticle(row.id)
    ElMessage.success('删除成功')
    loadData()
  })
}

const handleBatchDelete = () => {
  ElMessageBox.confirm(`确定要删除选中的 ${selectedRows.value.length} 篇文章吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    const ids = selectedRows.value.map((item: any) => item.id)
    await batchDeleteArticles(ids)
    ElMessage.success('删除成功')
    loadData()
  })
}

const handlePageChange = (page: number, pageSize: number) => {
  loadData(page, pageSize)
}

const handleSelectionChange = (selection: any[]) => {
  selectedRows.value = selection
}

const loadData = async (page = 1, pageSize = 10) => {
  loading.value = true
  try {
    const res = await getArticleList({
      page,
      pageSize,
      ...searchForm,
    })
    tableData.value = res.list
    total.value = res.total
  } catch (error) {
    console.error('加载数据失败:', error)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadData()
})
</script>

<style scoped lang="scss">
.articles-page {
  .search-form {
    margin-bottom: 20px;
  }

  .toolbar {
    margin-bottom: 20px;
  }
}
</style>
