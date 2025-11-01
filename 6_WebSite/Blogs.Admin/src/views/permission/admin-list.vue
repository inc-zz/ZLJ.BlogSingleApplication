<template>
  <div class="admin-list-page">
    <el-card>
      <div class="page-title">管理员列表</div>

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
            <el-option 
              v-for="role in roleOptions" 
              :key="role.id" 
              :label="role.name" 
              :value="role.id" 
            />
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
        <el-button type="primary" :icon="Plus" @click="handleCreate">添加管理员</el-button>
        <el-button :icon="Refresh" @click="handleRefresh">刷新</el-button>
      </div>

      <!-- 表格 -->
      <el-table
        :data="tableData"
        stripe
        border
        :loading="loading"
        row-key="id"
        @selection-change="handleSelectionChange"
      >
        <el-table-column type="selection" width="55" />
        <el-table-column type="index" label="序号" width="80" :index="(index) => (pagination.pageIndex - 1) * pagination.pageSize + index + 1" />
        <el-table-column prop="userName" label="用户名" width="150" />
        <el-table-column prop="realName" label="姓名" width="120" />
        <el-table-column prop="phoneNumber" label="手机号" width="150">
          <template #default="{ row }">
            {{ row.phoneNumber || '-' }}
          </template>
        </el-table-column>
        <el-table-column prop="email" label="邮箱" min-width="200" />
        <el-table-column prop="departmentName" label="部门" width="150">
          <template #default="{ row }">
            {{ row.departmentName || '-' }}
          </template>
        </el-table-column>
        <el-table-column prop="roles" label="角色" width="200">
          <template #default="{ row }">
            <el-tag
              v-for="role in row.roles"
              :key="role.roleId"
              type="primary"
              size="small"
              style="margin-right: 5px"
            >
              {{ role.name }}
            </el-tag>
            <span v-if="!row.roles || row.roles.length === 0">-</span>
          </template>
        </el-table-column>
        <el-table-column prop="status" label="状态" width="100">
          <template #default="{ row }">
            <el-tag v-if="row.status === 1" type="success">{{ row.statusName }}</el-tag>
            <el-tag v-else type="info">{{ row.statusName }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="lastLoginTime" label="最近登录" width="180">
          <template #default="{ row }">
            {{ formatDateTime(row.lastLoginTime) }}
          </template>
        </el-table-column>
        <el-table-column prop="createdAt" label="注册时间" width="180">
          <template #default="{ row }">
            {{ formatDateTime(row.createdAt) }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="320" fixed="right">
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
              type="info"
              size="small"
              @click="handleResetPassword(row)"
            >
              重置密码
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

    <!-- 管理员详情/编辑对话框 -->
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
          <el-input v-model="formData.userName" :disabled="isView || isEdit" />
        </el-form-item>
        <el-form-item label="密码" prop="password" v-if="!isView && !isEdit">
          <el-input v-model="formData.password" type="password" placeholder="请输入密码" />
        </el-form-item>
        <el-form-item label="真实姓名" prop="realName">
          <el-input v-model="formData.realName" :disabled="isView" placeholder="请输入真实姓名" />
        </el-form-item>
        <el-form-item label="性别" prop="sex" v-if="isEdit">
          <el-radio-group v-model="formData.sex" :disabled="isView">
            <el-radio :value="0">保密</el-radio>
            <el-radio :value="1">男</el-radio>
            <el-radio :value="2">女</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="手机号" prop="phoneNumber">
          <el-input v-model="formData.phoneNumber" :disabled="isView" placeholder="请输入手机号" />
        </el-form-item>
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="formData.email" :disabled="isView" placeholder="请输入邮箱" />
        </el-form-item>
        <el-form-item label="部门" prop="departmentId">
          <el-select 
            v-model="formData.departmentId" 
            :disabled="isView" 
            placeholder="请选择部门" 
            style="width: 100%"
            :teleported="false"
          >
            <el-option 
              v-for="dept in departmentOptions" 
              :key="dept.id" 
              :label="dept.name" 
              :value="dept.id" 
            />
          </el-select>
        </el-form-item>
        <el-form-item label="角色" prop="roleIds">
          <el-select 
            v-model="formData.roleIds" 
            :disabled="isView" 
            multiple 
            placeholder="请选择角色" 
            style="width: 100%"
            :teleported="false"
            collapse-tags
            collapse-tags-tooltip
          >
            <el-option 
              v-for="role in roleOptions" 
              :key="role.id" 
              :label="role.name" 
              :value="role.id" 
            />
          </el-select>
        </el-form-item>
        <el-form-item label="描述" prop="description">
          <el-input
            v-model="formData.description"
            type="textarea"
            :rows="3"
            :disabled="isView"
            placeholder="请输入描述"
          />
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
import { ref, reactive, onMounted, computed } from 'vue'
import { ElMessage, ElMessageBox, type FormInstance, type FormRules } from 'element-plus'
import {
  Search,
  Delete,
  Plus,
  Refresh,
  View,
  Edit,
} from '@element-plus/icons-vue'
import { 
  getAdminList, 
  createAdmin, 
  updateAdmin, 
  deleteAdmin, 
  getAdminDetail, 
  updateAdminStatus, 
  resetAdminPassword,
  batchDeleteAdmin,
  getAllRoles,
  type RoleOption
} from '@/api/admin'
import type { Admin } from '@/types'

const loading = ref(false)
const submitLoading = ref(false)
const tableData = ref<Admin[]>([])
const selectedRows = ref<Admin[]>([])
const dialogVisible = ref(false)
const dialogTitle = ref('')
const isView = ref(false)
const isEdit = ref(false)
const formRef = ref<FormInstance>()
const roleOptions = ref<RoleOption[]>([])
const departmentOptions = ref([  
  { id: 1, name: '开发部' },
  { id: 2, name: '实施部' },
  { id: 3, name: '产品部' },
])

const pagination = reactive({
  pageIndex: 1,
  pageSize: 10,
  total: 0,
})

const searchForm = reactive({
  keyword: '',
  role: '',
  status: '',
})

const formData = reactive<any>({
  id: undefined,
  userName: '',
  password: '',
  realName: '',
  phoneNumber: '',
  email: '',
  roleIds: [],
  departmentId: undefined,
  description: '',
  status: 1,
  sex: 0,
})

const formRules: FormRules = {
  userName: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  password: [{ required: true, message: '请输入密码', trigger: 'blur' }],
  realName: [{ required: true, message: '请输入真实姓名', trigger: 'blur' }],
  phoneNumber: [
    { pattern: /^1[3-9]\d{9}$/, message: '手机号格式不正确', trigger: 'blur' },
  ],
  email: [
    { required: true, message: '请输入邮箱', trigger: 'blur' },
    { type: 'email', message: '邮箱格式不正确', trigger: 'blur' },
  ],
}

const handleSearch = () => {
  pagination.pageIndex = 1
  loadData()
}

const handleRefresh = () => {
  loadData()
}

const handleCreate = () => {
  dialogTitle.value = '添加管理员'
  isView.value = false
  isEdit.value = false
  resetForm()
  dialogVisible.value = true
}

const handleView = async (row: Admin) => {
  dialogTitle.value = '查看管理员'
  isView.value = true
  isEdit.value = false
  try {
    const data = await getAdminDetail(row.id)
    Object.assign(formData, {
      ...data,
      roleIds: data.roles?.map((r: any) => r.roleId) || [],
    })
    dialogVisible.value = true
  } catch (error) {
    console.error('获取详情失败:', error)
  }
}

const handleEdit = async (row: Admin) => {
  dialogTitle.value = '编辑管理员'
  isView.value = false
  isEdit.value = true
  try {
    const data = await getAdminDetail(row.id)
    Object.assign(formData, {
      ...data,
      roleIds: data.roles?.map((r: any) => r.roleId) || [],
    })
    dialogVisible.value = true
  } catch (error) {
    console.error('获取详情失败:', error)
  }
}

const handleDelete = (row: Admin) => {
  ElMessageBox.confirm(`确定要删除管理员 ${row.userName} 吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    try {
      await deleteAdmin(row.id)
      ElMessage.success('删除成功')
      loadData()
    } catch (error) {
      console.error('删除失败:', error)
    }
  })
}

// 切换用户状态
const handleToggleStatus = (row: Admin) => {
  const newStatus = row.status === 1 ? 0 : 1
  const statusText = newStatus === 1 ? '启用' : '禁用'
  
  ElMessageBox.confirm(`确定要${statusText}管理员 ${row.userName} 吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    try {
      await updateAdminStatus({ id: row.id, status: newStatus })
      ElMessage.success(`${statusText}成功`)
      loadData()
    } catch (error) {
      console.error('状态更新失败:', error)
    }
  })
}

// 重置密码
const handleResetPassword = (row: Admin) => {
  ElMessageBox.prompt('请输入新密码', '重置密码', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    inputPattern: /.{6,}/,
    inputErrorMessage: '密码长度不能少于6位',
  }).then(async ({ value }) => {
    try {
      await resetAdminPassword({
        userId: row.id,
        password: value,
        oldPassword: '',
      })
      ElMessage.success('密码重置成功')
    } catch (error) {
      console.error('密码重置失败:', error)
    }
  }).catch(() => {
    // 用户取消
  })
}

const handleBatchDelete = () => {
  ElMessageBox.confirm(`确定要删除选中的 ${selectedRows.value.length} 个管理员吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    try {
      const ids = selectedRows.value.map(item => item.id)
      await batchDeleteAdmin(ids)
      ElMessage.success('删除成功')
      loadData()
    } catch (error) {
      console.error('批量删除失败:', error)
    }
  })
}

const handleSelectionChange = (selection: Admin[]) => {
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
        if (isEdit.value) {
          // 构造部门JSON字符串
          const departmentJson = formData.departmentId 
            ? `{id:${formData.departmentId},name:'${getDepartmentName(formData.departmentId)}'}`
            : ''
          
          // 构造角色JSON字符串
          const userRoleJson = formData.roleIds && formData.roleIds.length > 0
            ? `[${formData.roleIds.map((roleId: number) => `{RoleId:${roleId},RoleName:'${getRoleName(roleId)}'}`).join(',')}]`
            : ''

          await updateAdmin({
            Id: formData.id,
            realName: formData.realName,
            phoneNumber: formData.phoneNumber,
            email: formData.email,
            description: formData.description,
            sex: formData.sex || 0,
            departmentJson,
            userRoleJson,
          })
          ElMessage.success('更新成功')
        } else {
          // 构造部门JSON字符串
          const departmentJson = formData.departmentId 
            ? `{id:${formData.departmentId},name:'${getDepartmentName(formData.departmentId)}'}`
            : ''
          
          // 构造角色JSON字符串
          const userRoleJson = formData.roleIds && formData.roleIds.length > 0
            ? `[${formData.roleIds.map((roleId: number) => `{RoleId:${roleId},RoleName:'${getRoleName(roleId)}'}`).join(',')}]`
            : ''

          await createAdmin({
            userName: formData.userName,
            password: formData.password,
            realName: formData.realName,
            phoneNumber: formData.phoneNumber,
            email: formData.email,
            description: formData.description,
            departmentJson,
            userRoleJson,
          })
          ElMessage.success('创建成功')
        }
        dialogVisible.value = false
        loadData()
      } catch (error: any) {
        if (Array.isArray(error)) {
          ElMessage.error(error[0]?.value || '操作失败')
        } else {
          console.error('操作失败:', error)
        }
      } finally {
        submitLoading.value = false
      }
    }
  })
}

// 获取部门名称
const getDepartmentName = (departmentId: number): string => {
  const dept = departmentOptions.value.find(d => d.id === departmentId)
  return dept?.name || ''
}

// 获取角色名称
const getRoleName = (roleId: number): string => {
  const role = roleOptions.value.find(r => r.id === roleId)
  return role?.name || ''
}

// 格式化日期时间，去除T
const formatDateTime = (dateTime: string): string => {
  if (!dateTime) return '-'
  return dateTime.replace('T', ' ')
}

// 加载角色列表
const loadRoles = async () => {
  try {
    roleOptions.value = await getAllRoles()
  } catch (error) {
    console.error('加载角色列表失败:', error)
  }
}

const handleDialogClosed = () => {
  formRef.value?.resetFields()
  resetForm()
}

const resetForm = () => {
  Object.assign(formData, {
    id: undefined,
    userName: '',
    password: '',
    realName: '',
    phoneNumber: '',
    email: '',
    roleIds: [],
    departmentId: undefined,
    description: '',
    status: 1,
    sex: 0,
  })
}

const loadData = async () => {
  loading.value = true
  try {
    const params: any = {
      pageIndex: pagination.pageIndex,
      pageSize: pagination.pageSize,
      where: searchForm.keyword || '',
    }

    const data = await getAdminList(params)
    tableData.value = data?.items || []
    pagination.total = data?.total || 0
  } catch (error) {
    console.error('加载数据失败:', error)
    tableData.value = []
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadRoles()
  loadData()
})
</script>

<style scoped lang="scss">
.admin-list-page {
  .page-title {
    font-size: 18px;
    font-weight: bold;
    margin-bottom: 20px;
  }

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
