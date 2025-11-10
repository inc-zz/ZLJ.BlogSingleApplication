<template>
  <div class="articles-page">
    <el-card>
      <!-- 搜索栏 -->
      <el-form :inline="true" :model="searchForm" class="search-form">
        <el-form-item label="文章分类">
          <el-select 
            v-model="searchForm.CategoryId" 
            placeholder="请选择分类" 
            clearable
            style="width: 200px"
          >
            <el-option
              v-for="category in categories"
              :key="category.id"
              :label="category.name"
              :value="category.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="文章标题/简介">
          <el-input 
            v-model="searchForm.Where" 
            placeholder="请输入标题或简介" 
            clearable 
            style="width: 250px"
          />
        </el-form-item>
        <el-form-item label="创建时间">
          <el-date-picker
            v-model="dateRange"
            type="daterange"
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
            value-format="YYYY-MM-DD"
            style="width: 300px"
          />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <!-- 操作栏 -->
      <div class="toolbar">
        <div class="toolbar-actions">
          <el-button type="primary" @click="handleCreate">新建文章</el-button>
          <el-button
            type="danger"
            :disabled="selectedRows.length === 0"
            @click="handleBatchDelete"
          >
            批量删除
          </el-button>
        </div>
      </div>

      <!-- 表格 -->
      <el-table
        v-loading="loading"
        :data="tableData"
        stripe
        border
        @selection-change="handleSelectionChange"
      >
        <el-table-column type="selection" width="55" align="center" />
        <el-table-column type="index" label="序号" width="70" align="center" />
        <el-table-column prop="title" label="文章标题" min-width="200" show-overflow-tooltip />
        <el-table-column prop="tags" label="标签" width="150" show-overflow-tooltip />
        <el-table-column prop="summary" label="简介" width="200">
          <template #default="{ row }">
            <el-tooltip :content="row.summary" placement="top" :disabled="!row.summary || row.summary.length <= 20">
              <span>{{ row.summary ? (row.summary.length > 20 ? row.summary.substring(0, 20) + '...' : row.summary) : '-' }}</span>
            </el-tooltip>
          </template>
        </el-table-column>
        <el-table-column prop="categoryName" label="分类" width="120" align="center" />
        <el-table-column prop="viewCount" label="浏览量" width="100" align="center" />
        <el-table-column prop="likeCount" label="点赞数" width="100" align="center" />
        <el-table-column prop="commentCount" label="评论数" width="100" align="center" />
        <el-table-column prop="createdAt" label="创建时间" width="180" align="center" />
        <el-table-column prop="createdBy" label="创建人" width="120" align="center" />
        <el-table-column label="操作" width="150" fixed="right" align="center">
          <template #default="{ row }">
            <el-button link type="primary" @click="handleView(row)">
              <el-icon><View /></el-icon>
            </el-button>
            <el-button link type="primary" @click="handleEdit(row)">
              <el-icon><Edit /></el-icon>
            </el-button>
            <el-button link type="danger" @click="handleDelete(row)">
              <el-icon><Delete /></el-icon>
            </el-button>
          </template>
        </el-table-column>
      </el-table>

      <!-- 分页 -->
      <div class="pagination">
        <el-pagination
          v-model:current-page="pagination.pageIndex"
          v-model:page-size="pagination.pageSize"
          :page-sizes="[10, 20, 30, 50, 100]"
          :total="pagination.total"
          layout="total, sizes, prev, pager, next, jumper"
          @size-change="handleSizeChange"
          @current-change="handlePageChange"
        />
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage, ElMessageBox } from 'element-plus'
import { View, Edit, Delete } from '@element-plus/icons-vue'
import { 
  getArticleList, 
  getArticleCategories, 
  deleteArticle, 
  batchDeleteArticles,
  type ArticleListItem,
  type ArticleCategory 
} from '@/api/article'

const router = useRouter()

const loading = ref(false)
const tableData = ref<ArticleListItem[]>([])
const selectedRows = ref<ArticleListItem[]>([])
const categories = ref<ArticleCategory[]>([])
const dateRange = ref<[string, string] | null>(null)

const searchForm = reactive({
  CategoryId: undefined as number | undefined,
  Where: '',
})

const pagination = reactive({
  pageIndex: 1,
  pageSize: 30,
  total: 0,
})

// 加载文章分类
const loadCategories = async () => {
  try {
    categories.value = await getArticleCategories(10)
  } catch (error) {
    console.error('加载分类失败:', error)
  }
}

// 搜索
const handleSearch = () => {
  pagination.pageIndex = 1
  loadData()
}

// 重置
const handleReset = () => {
  searchForm.CategoryId = undefined
  searchForm.Where = ''
  dateRange.value = null
  pagination.pageIndex = 1
  loadData()
}

// 新建文章
const handleCreate = () => {
  router.push('/content/articles/create')
}

// 查看文章
const handleView = (row: ArticleListItem) => {
  router.push(`/content/articles/view/${row.id}`)
}

// 编辑文章
const handleEdit = (row: ArticleListItem) => {
  router.push(`/content/articles/edit/${row.id}`)
}

// 删除文章
const handleDelete = (row: ArticleListItem) => {
  ElMessageBox.confirm('确定要删除这篇文章吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    try {
      await deleteArticle(row.id)
      ElMessage.success('删除成功')
      loadData()
    } catch (error: any) {
      ElMessage.error(error.message || '删除失败')
    }
  })
}

// 批量删除
const handleBatchDelete = () => {
  ElMessageBox.confirm(`确定要删除选中的 ${selectedRows.value.length} 篇文章吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    try {
      const ids = selectedRows.value.map(item => item.id)
      await batchDeleteArticles(ids)
      ElMessage.success('删除成功')
      selectedRows.value = []
      loadData()
    } catch (error: any) {
      ElMessage.error(error.message || '删除失败')
    }
  })
}

// 分页大小变化
const handleSizeChange = (pageSize: number) => {
  pagination.pageSize = pageSize
  pagination.pageIndex = 1
  loadData()
}

// 分页变化
const handlePageChange = (page: number) => {
  pagination.pageIndex = page
  loadData()
}

// 选择变化
const handleSelectionChange = (selection: ArticleListItem[]) => {
  selectedRows.value = selection
}

// 加载数据
const loadData = async () => {
  loading.value = true
  try {
    const params: any = {
      PageIndex: pagination.pageIndex,
      PageSize: pagination.pageSize,
    }

    if (searchForm.CategoryId) {
      params.CategoryId = searchForm.CategoryId
    }
    if (searchForm.Where) {
      params.Where = searchForm.Where
    }
    if (dateRange.value && dateRange.value.length === 2) {
      params.StartDate = dateRange.value[0]
      params.EndDate = dateRange.value[1]
    }

    const res: any = await getArticleList(params)
    
    // 根据内存中的API响应格式，数据可能直接在items中
    if (res.items) {
      tableData.value = res.items
      pagination.total = res.total || 0
    } else {
      tableData.value = res || []
      pagination.total = 0
    }
  } catch (error) {
    console.error('加载数据失败:', error)
    ElMessage.error('加载数据失败')
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadCategories()
  loadData()
})
</script>

<style scoped lang="scss">
.articles-page {
  .search-form {
    margin-bottom: 20px;
    
    :deep(.el-form-item) {
      margin-bottom: 12px;
    }
  }

  .toolbar {
    margin-bottom: 20px;
    display: flex;
    justify-content: flex-end;
    
    .toolbar-actions {
      display: flex;
      gap: 12px;
    }
  }

  .pagination {
    margin-top: 20px;
    display: flex;
    justify-content: flex-end;
  }

  :deep(.el-table) {
    .el-button + .el-button {
      margin-left: 8px;
    }
  }
}
</style>
