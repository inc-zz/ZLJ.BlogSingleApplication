<template>
  <div class="user-list-page">
    <el-card>
      <!-- 搜索栏 -->
      <el-form :inline="true" :model="searchForm" class="search-form">
        <el-form-item>
          <el-input
            v-model="searchForm.Where"
            placeholder="用户名/邮箱"
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
            v-model="searchForm.Status"
            placeholder="全部状态"
            clearable
            style="width: 150px"
          >
            <el-option label="启用" :value="1" />
            <el-option label="禁用" :value="0" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" :icon="Search" @click="handleSearch">搜索</el-button>
          <el-button :icon="Refresh" @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <!-- 操作栏 -->
      <div class="toolbar">
        <el-button type="primary" :icon="Plus" @click="handleCreate">添加用户</el-button>
      </div>

      <!-- 表格 -->
      <el-table
        :data="tableData"
        stripe
        border
        :loading="loading"
      >
        <el-table-column type="index" label="序号" width="80" align="center" />
        <el-table-column prop="account" label="用户名" width="150" />
        <el-table-column prop="realName" label="姓名" width="120" />
        <el-table-column prop="email" label="邮箱" min-width="200" />
        <el-table-column prop="lastLoginIp" label="最近登录IP" width="150" />
        <el-table-column prop="lastLoginTime" label="最近登录" width="180" />
        <el-table-column prop="statusName" label="状态" width="100" align="center">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'danger'">
              {{ row.statusName }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createdAt" label="注册时间" width="180" />
        <el-table-column label="操作" width="280" fixed="right" align="center">
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
              :type="row.status === 1 ? 'warning' : 'success'"
              size="small"
              @click="handleToggleStatus(row)"
            >
              {{ row.status === 1 ? '禁用' : '启用' }}
            </el-button>
            <el-button
              link
              type="danger"
              size="small"
              :icon="Key"
              @click="handleResetPassword(row)"
            >
              重置密码
            </el-button>
          </template>
        </el-table-column>
      </el-table>

      <!-- 分页 -->
      <el-pagination
        v-model:current-page="pagination.pageIndex"
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
        <el-form-item label="用户名" prop="account" v-if="!isEdit">
          <el-input v-model="formData.account" :disabled="isView" placeholder="请输入用户名" />
        </el-form-item>
        <el-form-item label="密码" prop="password" v-if="!isEdit && !isView">
          <el-input v-model="formData.password" type="password" placeholder="请输入密码" show-password />
        </el-form-item>
        <el-form-item label="姓名" prop="realName">
          <el-input v-model="formData.realName" :disabled="isView" placeholder="请输入姓名" />
        </el-form-item>
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="formData.email" :disabled="isView" placeholder="请输入邮箱" />
        </el-form-item>
        <el-form-item label="手机号" prop="phoneNumber">
          <el-input v-model="formData.phoneNumber" :disabled="isView" placeholder="请输入手机号" />
        </el-form-item>
        <el-form-item label="备注" prop="remark" v-if="isEdit || isView">
          <el-input v-model="formData.remark" type="textarea" :rows="3" :disabled="isView" placeholder="请输入备注" />
        </el-form-item>
        <el-form-item label="最近登录IP" v-if="isView">
          <el-input v-model="formData.lastLoginIp" disabled />
        </el-form-item>
        <el-form-item label="最近登录" v-if="isView">
          <el-input v-model="formData.lastLoginTime" disabled />
        </el-form-item>
        <el-form-item label="状态" v-if="isView">
          <el-tag :type="formData.status === 1 ? 'success' : 'danger'">
            {{ formData.statusName }}
          </el-tag>
        </el-form-item>
        <el-form-item label="注册时间" v-if="isView">
          <el-input v-model="formData.createdAt" disabled />
        </el-form-item>
      </el-form>
      <template #footer v-if="!isView">
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" :loading="submitLoading" @click="handleSubmit">
          确定
        </el-button>
      </template>
    </el-dialog>

    <!-- 重置密码对话框 -->
    <el-dialog
      v-model="resetPasswordVisible"
      title="重置密码"
      width="400px"
    >
      <el-form
        ref="resetPasswordFormRef"
        :model="resetPasswordForm"
        :rules="resetPasswordRules"
        label-width="80px"
      >
        <el-form-item label="新密码" prop="password">
          <el-input 
            v-model="resetPasswordForm.password" 
            type="password" 
            placeholder="请输入新密码" 
            show-password 
          />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="resetPasswordVisible = false">取消</el-button>
        <el-button type="primary" :loading="resetPasswordLoading" @click="handleResetPasswordSubmit">
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
  Plus,
  Refresh,
  View,
  Edit,
  Key,
} from '@element-plus/icons-vue'
import type { PageData } from '@/types'
import {
  getAppUserList,
  createAppUser,
  getAppUserInfo,
  toggleAppUserStatus,
  updateAppUser,
  resetAppUserPassword,
  type AppUser,
} from '@/api/appUser'

const loading = ref(false)
const submitLoading = ref(false)
const resetPasswordLoading = ref(false)
const tableData = ref<AppUser[]>([])
const dialogVisible = ref(false)
const resetPasswordVisible = ref(false)
const dialogTitle = ref('')
const isView = ref(false)
const isEdit = ref(false)
const formRef = ref<FormInstance>()
const resetPasswordFormRef = ref<FormInstance>()

const pagination = reactive({
  pageIndex: 1,
  pageSize: 10,
  total: 0,
})

const searchForm = reactive({
  Where: '',
  Status: undefined as number | undefined,
})

const formData = reactive({
  id: 0,
  account: '',
  password: '',
  realName: '',
  phoneNumber: '',
  email: '',
  remark: '',
  avatar: '',
  lastLoginIp: '',
  lastLoginTime: '',
  status: 1,
  statusName: '',
  createdAt: '',
})

const resetPasswordForm = reactive({
  id: 0,
  password: '',
})

const formRules: FormRules = {
  account: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  password: [{ required: true, message: '请输入密码', trigger: 'blur' }],
  realName: [{ required: true, message: '请输入姓名', trigger: 'blur' }],
  email: [
    { required: true, message: '请输入邮箱', trigger: 'blur' },
    { type: 'email', message: '邮箱格式不正确', trigger: 'blur' },
  ],
}

const resetPasswordRules: FormRules = {
  password: [
    { required: true, message: '请输入新密码', trigger: 'blur' },
    { min: 6, message: '密码至少 6 位', trigger: 'blur' },
  ],
}

const handleSearch = () => {
  pagination.pageIndex = 1
  loadData()
}

const handleReset = () => {
  searchForm.Where = ''
  searchForm.Status = undefined
  pagination.pageIndex = 1
  loadData()
}

const handleCreate = () => {
  dialogTitle.value = '添加用户'
  isView.value = false
  isEdit.value = false
  resetForm()
  dialogVisible.value = true
}

const handleView = async (row: AppUser) => {
  dialogTitle.value = '查看用户'
  isView.value = true
  isEdit.value = false
  
  try {
    const res = await getAppUserInfo(row.id) as any
    const userInfo = res.data || res
    Object.assign(formData, userInfo)
    dialogVisible.value = true
  } catch (error) {
    console.error('获取用户详情失败:', error)
    ElMessage.error('获取用户详情失败')
  }
}

const handleEdit = async (row: AppUser) => {
  dialogTitle.value = '编辑用户'
  isView.value = false
  isEdit.value = true
  
  try {
    const res = await getAppUserInfo(row.id) as any
    const userInfo = res.data || res
    Object.assign(formData, userInfo)
    dialogVisible.value = true
  } catch (error) {
    console.error('获取用户详情失败:', error)
    ElMessage.error('获取用户详情失败')
  }
}

const handleToggleStatus = (row: AppUser) => {
  const action = row.status === 1 ? '禁用' : '启用'
  ElMessageBox.confirm(`确定要${action}用户 ${row.account} 吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    try {
      await toggleAppUserStatus(row.id)
      ElMessage.success(`${action}成功`)
      loadData()
    } catch (error) {
      console.error(`${action}失败:`, error)
    }
  })
}

const handleResetPassword = (row: AppUser) => {
  resetPasswordForm.id = row.id
  resetPasswordForm.password = ''
  resetPasswordVisible.value = true
}

const handleResetPasswordSubmit = async () => {
  if (!resetPasswordFormRef.value) return

  await resetPasswordFormRef.value.validate(async (valid) => {
    if (valid) {
      resetPasswordLoading.value = true
      try {
        await resetAppUserPassword(resetPasswordForm)
        ElMessage.success('密码重置成功')
        resetPasswordVisible.value = false
      } catch (error) {
        console.error('重置密码失败:', error)
      } finally {
        resetPasswordLoading.value = false
      }
    }
  })
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
        if (isEdit.value) {
          // 编辑用户
          await updateAppUser({
            id: formData.id,
            email: formData.email,
            phoneNumber: formData.phoneNumber,
            remark: formData.remark,
            avatar: formData.avatar,
          })
          ElMessage.success('更新成功')
        } else {
          // 添加用户
          await createAppUser({
            account: formData.account,
            password: formData.password,
            realName: formData.realName,
            email: formData.email,
            phoneNumber: formData.phoneNumber,
            remark: formData.remark,
          })
          ElMessage.success('创建成功')
        }
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
    id: 0,
    account: '',
    password: '',
    realName: '',
    phoneNumber: '',
    email: '',
    remark: '',
    avatar: '',
    lastLoginIp: '',
    lastLoginTime: '',
    status: 1,
    statusName: '',
    createdAt: '',
  })
}

const loadData = async () => {
  loading.value = true
  try {
    const res = (await getAppUserList({
      pageIndex: pagination.pageIndex,
      pageSize: pagination.pageSize,
      Status: searchForm.Status,
      Where: searchForm.Where,
    })) as any as PageData<AppUser>
    tableData.value = res.items
    pagination.total = res.total
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
