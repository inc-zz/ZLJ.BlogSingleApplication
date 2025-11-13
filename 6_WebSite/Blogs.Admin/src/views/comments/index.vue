<template>
  <div class="comments-page">
    <el-card>
      <!-- 搜索栏 -->
      <el-form :inline="true" :model="searchForm" class="search-form">
        <el-form-item label="评论内容">
          <el-input 
            v-model="searchForm.SearchTerm" 
            placeholder="请输入评论内容" 
            clearable 
            style="width: 300px"
            @keyup.enter="handleSearch"
          />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <!-- 表格 -->
      <el-table
        v-loading="loading"
        :data="tableData"
        stripe
        border
      >
        <el-table-column type="index" label="序号" width="70" align="center" />
        <el-table-column prop="articleTitle" label="文章标题" min-width="200" show-overflow-tooltip>
          <template #default="{ row }">
            <el-link 
              type="primary" 
              :underline="false"
              @click="handleViewArticle(row)"
            >
              {{ row.articleTitle }}
            </el-link>
          </template>
        </el-table-column>
        <el-table-column prop="content" label="评论内容" min-width="300" show-overflow-tooltip />
        <el-table-column prop="createdBy" label="评论人" width="120" align="center" />
        <el-table-column prop="likeCount" label="点赞数" width="100" align="center" />
        <el-table-column prop="createdAt" label="评论时间" width="180" align="center" />
        <el-table-column label="操作" width="100" fixed="right" align="center">
          <template #default="{ row }">
            <el-button link type="danger" @click="handleDelete(row)">
              删除
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
import { getCommentList, deleteComment, type CommentListItem } from '@/api/article'

const router = useRouter()
const loading = ref(false)
const tableData = ref<CommentListItem[]>([])

const searchForm = reactive({
  SearchTerm: '',
})

const pagination = reactive({
  pageIndex: 1,
  pageSize: 30,
  total: 0,
})

// 搜索
const handleSearch = () => {
  pagination.pageIndex = 1
  loadData()
}

// 重置
const handleReset = () => {
  searchForm.SearchTerm = ''
  pagination.pageIndex = 1
  loadData()
}

// 查看文章详情
const handleViewArticle = (row: CommentListItem) => {
  router.push(`/content/articles/view/${row.articleId}`)
}

// 删除评论
const handleDelete = (row: CommentListItem) => {
  ElMessageBox.confirm('确定要删除这条评论吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    try {
      await deleteComment(row.id)
      ElMessage.success('评论删除成功')
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

// 加载数据
const loadData = async () => {
  loading.value = true
  try {
    const params: any = {
      PageIndex: pagination.pageIndex,
      PageSize: pagination.pageSize,
    }

    if (searchForm.SearchTerm) {
      params.SearchTerm = searchForm.SearchTerm
    }

    const res: any = await getCommentList(params)
    
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
  loadData()
})
</script>

<style scoped lang="scss">
.comments-page {
  .search-form {
    margin-bottom: 20px;
    
    :deep(.el-form-item) {
      margin-bottom: 12px;
    }
  }

  .pagination {
    margin-top: 20px;
    display: flex;
    justify-content: flex-end;
  }
}
</style>
