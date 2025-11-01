<template>
  <div class="comments-page">
    <el-card>
      <CommonTable
        :data="tableData"
        :total="total"
        :loading="loading"
        @page-change="handlePageChange"
      >
        <el-table-column prop="author" label="作者" width="120" />
        <el-table-column prop="email" label="邮箱" width="200" />
        <el-table-column prop="content" label="内容" min-width="300" />
        <el-table-column prop="status" label="状态" width="100">
          <template #default="{ row }">
            <el-tag v-if="row.status === 'approved'" type="success">已通过</el-tag>
            <el-tag v-else-if="row.status === 'pending'" type="warning">待审核</el-tag>
            <el-tag v-else type="danger">已拒绝</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createTime" label="评论时间" width="180" />
        <el-table-column label="操作" width="200" fixed="right">
          <template #default="{ row }">
            <el-button
              v-if="row.status === 'pending'"
              link
              type="success"
              size="small"
              @click="handleApprove(row)"
            >
              通过
            </el-button>
            <el-button
              v-if="row.status === 'pending'"
              link
              type="warning"
              size="small"
              @click="handleReject(row)"
            >
              拒绝
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
import { ref, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import CommonTable from '@/components/Table/index.vue'

const loading = ref(false)
const tableData = ref([])
const total = ref(0)

const handleApprove = (row: any) => {
  ElMessage.success('已通过')
  loadData()
}

const handleReject = (row: any) => {
  ElMessage.warning('已拒绝')
  loadData()
}

const handleDelete = (row: any) => {
  ElMessage.success('删除成功')
  loadData()
}

const handlePageChange = (page: number, pageSize: number) => {
  loadData(page, pageSize)
}

const loadData = async () => {
  loading.value = true
  // TODO: 调用API
  setTimeout(() => {
    loading.value = false
  }, 500)
}

onMounted(() => {
  loadData()
})
</script>

<style scoped lang="scss">
.comments-page {
  // styles
}
</style>
