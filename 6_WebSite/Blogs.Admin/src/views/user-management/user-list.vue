<template>
  <div class="user-list-page">
    <el-card>
      <!-- 搜索栏 -->
      <el-form :inline="true" :model="searchForm" class="search-form">
        <el-form-item>
          <el-input
            v-model="searchForm.keyword"
            placeholder="用户名/姓名/邮箱"
            clearable
            style="width: 250px"
          >
            <template #prefix>
              <el-icon><Search /></el-icon>
            </template>
          </el-input>
        </el-form-item>
        <el-form-item>
          <el-select
            v-model="searchForm.role"
            placeholder="全部角色"
            clearable
            style="width: 150px"
          >
            <el-option label="普通用户" value="user" />
            <el-option label="VIP用户" value="vip" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-select
            v-model="searchForm.status"
            placeholder="全部状态"
            clearable
            style="width: 150px"
          >
            <el-option label="启用" value="1" />
            <el-option label="禁用" value="0" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" :icon="Search" @click="handleSearch">搜索</el-button>
          <el-button :icon="Delete" @click="handleBatchDelete" :disabled="selectedRows.length === 0">
            批量删除
          </el-button>
        </el-form-item>
      </el-form>

      <!-- 操作栏 -->
      <div class="toolbar">
        <el-button type="primary" :icon="Plus" @click="handleCreate">添加用户</el-button>
        <el-button :icon="Refresh" @click="handleRefresh">刷新</el-button>
      </div>

      <!-- 表格 -->
      <el-table
        :data="tableData"
        stripe
        border
        :loading="loading"
        @selection-change="handleSelectionChange"
      >
        <el-table-column type="selection" width="55" />
        <el-table-column prop="id" label="ID" width="100" />
        <el-table-column prop="userName" label="用户名" width="150" />
        <el-table-column prop="realName" label="姓名" width="120" />
        <el-table-column prop="phoneNumber" label="手机号" width="150" />
        <el-table-column prop="email" label="邮箱" min-width="200" />
        <el-table-column prop="role" label="角色" width="120">
          <template #default="{ row }">
            <el-tag v-if="row.role === 'admin'" type="danger">管理员</el-tag>
            <el-tag v-else>{{ row.role || '普通用户' }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="status" label="状态" width="100">
          <template #default="{ row }">
            <el-tag v-if="row.status === 1" type="success">启用</el-tag>
            <el-tag v-else type="info">禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="lastLoginTime" label="最近登录" width="180" />
        <el-table-column prop="createTime" label="注册时间" width="180" />
        <el-table-column label="操作" width="200" fixed="right">
          <template #default="{ row }">
            <el-button
              link
              type="primary"
              size="small"
              :icon="View"
              @click="handleView(row)"
            >
              查看
            </el-button>
            <el-button
              link
              type="primary"
              size="small"
              :icon="Edit"
              @click="handleEdit(row)"
            >
              编辑
            </el-button>
            <el-button
              link
              type="danger"
              size="small"
              :icon="Delete"
              @click="handleDelete(row)"
            >
              删除
            </el-button>
          </template>
        </el-table-column>
      </el-table>

      <!-- 分页 -->
      <el-pagination
        v-model:current-page="pagination.page"
        v-model:page-size="pagination.pageSize"
        :total="pagination.total"
        :page-sizes="[10, 20, 50, 100]"
        layout="total, sizes, prev, pager, next, jumper"
        background
        class="pagination"
        @size-change="handlePageChange"
        @current-change="handlePageChange"
      />
    </el-card>

    <!-- 用户详情/编辑对话框 -->
    <el-dialog
      v-model="dialogVisible"
      :title="dialogTitle"
      width="600px"
      @closed="handleDialogClosed"
    >
      <el-form
        ref="formRef"
        :model="formData"
        :rules="formRules"
        label-width="100px"
      >
        <el-form-item label="用户名" prop="userName">
          <el-input v-model="formData.userName" :disabled="isView" />
        </el-form-item>
        <el-form-item label="真实姓名" prop="realName">
          <el-input v-model="formData.realName" :disabled="isView" />
        </el-form-item>
        <el-form-item label="手机号" prop="phoneNumber">
          <el-input v-model="formData.phoneNumber" :disabled="isView" />
        </el-form-item>
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="formData.email" :disabled="isView" />
        </el-form-item>
        <el-form-item label="角色" prop="role">
          <el-select v-model="formData.role" :disabled="isView" style="width: 100%">
            <el-option label="普通用户" value="user" />
            <el-option label="VIP用户" value="vip" />
          </el-select>
        </el-form-item>
        <el-form-item label="状态" prop="status">
          <el-radio-group v-model="formData.status" :disabled="isView">
            <el-radio :value="1">启用</el-radio>
            <el-radio :value="0">禁用</el-radio>
          </el-radio-group>
        </el-form-item>
      </el-form>
      <template #footer v-if="!isView">
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" :loading="submitLoading" @click="handleSubmit">
          确定
        </el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox, type FormInstance, type FormRules } from 'element-plus'
import {
  Search,
  Delete,
  Plus,
  Refresh,
  View,
  Edit,
} from '@element-plus/icons-vue'

const loading = ref(false)
const submitLoading = ref(false)
const tableData = ref([])
const selectedRows = ref([])
const dialogVisible = ref(false)
const dialogTitle = ref('')
const isView = ref(false)
const formRef = ref<FormInstance>()

const pagination = reactive({
  page: 1,
  pageSize: 10,
  total: 0,
})

const searchForm = reactive({
  keyword: '',
  role: '',
  status: '',
})

const formData = reactive({
  id: '',
  userName: '',
  realName: '',
  phoneNumber: '',
  email: '',
  role: 'user',
  status: 1,
})

const formRules: FormRules = {
  userName: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  realName: [{ required: true, message: '请输入真实姓名', trigger: 'blur' }],
  phoneNumber: [
    { required: true, message: '请输入手机号', trigger: 'blur' },
    { pattern: /^1[3-9]\d{9}$/, message: '手机号格式不正确', trigger: 'blur' },
  ],
  email: [
    { required: true, message: '请输入邮箱', trigger: 'blur' },
    { type: 'email', message: '邮箱格式不正确', trigger: 'blur' },
  ],
}

const handleSearch = () => {
  pagination.page = 1
  loadData()
}

const handleRefresh = () => {
  loadData()
}

const handleCreate = () => {
  dialogTitle.value = '添加用户'
  isView.value = false
  resetForm()
  dialogVisible.value = true
}

const handleView = (row: any) => {
  dialogTitle.value = '查看用户'
  isView.value = true
  Object.assign(formData, row)
  dialogVisible.value = true
}

const handleEdit = (row: any) => {
  dialogTitle.value = '编辑用户'
  isView.value = false
  Object.assign(formData, row)
  dialogVisible.value = true
}

const handleDelete = (row: any) => {
  ElMessageBox.confirm(`确定要删除用户 ${row.userName} 吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(() => {
    ElMessage.success('删除成功')
    loadData()
  })
}

const handleBatchDelete = () => {
  ElMessageBox.confirm(`确定要删除选中的 ${selectedRows.value.length} 个用户吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(() => {
    ElMessage.success('删除成功')
    loadData()
  })
}

const handleSelectionChange = (selection: any[]) => {
  selectedRows.value = selection
}

const handlePageChange = () => {
  loadData()
}

const handleSubmit = async () => {
  if (!formRef.value) return

  await formRef.value.validate(async (valid) => {
    if (valid) {
      submitLoading.value = true
      try {
        // TODO: 调用API
        await new Promise((resolve) => setTimeout(resolve, 1000))
        ElMessage.success(formData.id ? '更新成功' : '创建成功')
        dialogVisible.value = false
        loadData()
      } catch (error) {
        console.error('操作失败:', error)
      } finally {
        submitLoading.value = false
      }
    }
  })
}

const handleDialogClosed = () => {
  formRef.value?.resetFields()
  resetForm()
}

const resetForm = () => {
  Object.assign(formData, {
    id: '',
    userName: '',
    realName: '',
    phoneNumber: '',
    email: '',
    role: 'user',
    status: 1,
  })
}

const loadData = async () => {
  loading.value = true
  try {
    // TODO: 调用真实API
    // 模拟数据
    await new Promise((resolve) => setTimeout(resolve, 500))
    tableData.value = [
      {
        id: '29219497415184',
        userName: 'zhangsan',
        realName: '张三',
        phoneNumber: '13800138000',
        email: 'zhangsan@example.com',
        role: 'user',
        status: 1,
        lastLoginTime: '2025/10/29 10:20:35',
        createTime: '2025/10/22 20:35:31',
      },
      {
        id: '29219840909840',
        userName: 'lisi',
        realName: '李四',
        phoneNumber: '13800138001',
        email: 'lisi@example.com',
        role: 'vip',
        status: 1,
        lastLoginTime: '2025/10/28 15:30:00',
        createTime: '2025/10/22 20:46:42',
      },
      {
        id: '35514340842008',
        userName: 'wangwu',
        realName: '王五',
        phoneNumber: '13800138002',
        email: 'wangwu@example.com',
        role: 'user',
        status: 0,
        lastLoginTime: '',
        createTime: '2025/10/22 20:22:33',
      },
    ]
    pagination.total = 3
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
.user-list-page {
  .search-form {
    margin-bottom: 0;
  }

  .toolbar {
    margin-bottom: 20px;
  }

  .pagination {
    margin-top: 20px;
    display: flex;
    justify-content: flex-end;
  }
}
</style>
