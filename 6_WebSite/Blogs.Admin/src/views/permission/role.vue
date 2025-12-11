<template>
  <div class="role-page">
    <el-card>
      <div class="page-title">角色管理</div>

      <!-- 搜索栏 -->
      <el-form :inline="true" :model="searchForm" class="search-form">
        <el-form-item>
          <el-input
            v-model="searchForm.SearchTerm"
            placeholder="角色名称/编码"
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
          <el-button :icon="Refresh" @click="handleRefresh">刷新</el-button>
        </el-form-item>
      </el-form>

      <!-- 操作栏 -->
      <div class="toolbar">
        <el-button type="primary" :icon="Plus" @click="handleCreate">添加角色</el-button>
      </div>

      <!-- 表格 -->
      <el-table
        :data="tableData"
        stripe
        border
        :loading="loading"
        row-key="id"
      >
        <el-table-column type="index" label="序号" width="80" :index="(index: number) => (pagination.pageIndex - 1) * pagination.pageSize + index + 1" />
        <el-table-column prop="name" label="角色名称" width="150" />
        <el-table-column prop="code" label="角色编码" width="150" />
        <el-table-column prop="isSystem" label="类型" width="100">
          <template #default="{ row }">
            <el-tag v-if="row.isSystem === 1" type="danger">系统角色</el-tag>
            <el-tag v-else type="primary">普通角色</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="remark" label="备注" min-width="200">
          <template #default="{ row }">
            {{ row.remark || '-' }}
          </template>
        </el-table-column>
        <el-table-column prop="status" label="状态" width="100">
          <template #default="{ row }">
            <el-tag v-if="row.status === 1" type="success">{{ row.statusName }}</el-tag>
            <el-tag v-else type="info">{{ row.statusName }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createdBy" label="创建人" width="120" />
        <el-table-column prop="createdAt" label="创建时间" width="180">
          <template #default="{ row }">
            {{ formatDateTime(row.createdAt) }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="280" fixed="right">
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
              type="success"
              size="small"
              :icon="Key"
              @click="handleAuth(row)"
            >
              权限配置
            </el-button>
            <el-button
              link
              type="danger"
              size="small"
              :icon="Delete"
              @click="handleDelete(row)"
              :disabled="row.isSystem === 1"
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

    <!-- 角色详情/编辑对话框 -->
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
        <el-form-item label="角色名称" prop="name">
          <el-input 
            v-model="formData.name" 
            :disabled="isView" 
            placeholder="请输入角色名称" 
          />
        </el-form-item>
        <el-form-item label="角色编码" prop="code">
          <el-input 
            v-model="formData.code" 
            :disabled="isView || isEdit" 
            placeholder="请输入角色编码（唯一标识）" 
          />
        </el-form-item>
        <el-form-item label="角色类型" prop="isSystem">
          <el-radio-group v-model="formData.isSystem" :disabled="isView">
            <el-radio :value="0">普通角色</el-radio>
            <el-radio :value="1">系统角色</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="排序" prop="sort" v-if="isEdit">
          <el-input-number 
            v-model="formData.sort" 
            :disabled="isView" 
            :min="0" 
            :max="999" 
          />
        </el-form-item>
        <el-form-item label="备注" prop="remark">
          <el-input
            v-model="formData.remark"
            type="textarea"
            :rows="3"
            :disabled="isView"
            placeholder="请输入备注信息"
          />
        </el-form-item>
        <el-form-item label="状态" v-if="isView">
          <el-tag v-if="formData.status === 1" type="success">{{ formData.statusName }}</el-tag>
          <el-tag v-else type="info">{{ formData.statusName }}</el-tag>
        </el-form-item>
        <el-form-item label="创建人" v-if="isView">
          <span>{{ formData.createdBy }}</span>
        </el-form-item>
        <el-form-item label="创建时间" v-if="isView">
          <span>{{ formatDateTime(formData.createdAt) }}</span>
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
import { useRouter } from 'vue-router'
import { ElMessage, ElMessageBox, type FormInstance, type FormRules } from 'element-plus'
import {
  Search,
  Delete,
  Plus,
  Refresh,
  View,
  Edit,
  Key,
} from '@element-plus/icons-vue'
import {
  getRoleList,
  getRoleDetail,
  createRole,
  updateRole,
  deleteRole,
  type Role,
} from '@/api/role'

const router = useRouter()

const loading = ref(false)
const submitLoading = ref(false)
const tableData = ref<Role[]>([])
const dialogVisible = ref(false)
const dialogTitle = ref('')
const isView = ref(false)
const isEdit = ref(false)
const formRef = ref<FormInstance>()

const pagination = reactive({
  pageIndex: 1,
  pageSize: 10,
  total: 0,
})

const searchForm = reactive({
  SearchTerm: '',
  Status: undefined as number | undefined,
})

const formData = reactive<any>({
  id: undefined,
  name: '',
  code: '',
  isSystem: 0,
  sort: 0,
  remark: '',
  status: 1,
  statusName: '',
  createdBy: '',
  createdAt: '',
})

const formRules: FormRules = {
  name: [{ required: true, message: '请输入角色名称', trigger: 'blur' }],
  code: [
    { required: true, message: '请输入角色编码', trigger: 'blur' },
    { pattern: /^[a-zA-Z][a-zA-Z0-9_]*$/, message: '编码必须以字母开头，只能包含字母、数字和下划线', trigger: 'blur' },
  ],
  isSystem: [{ required: true, message: '请选择角色类型', trigger: 'change' }],
}

const handleSearch = () => {
  pagination.pageIndex = 1
  loadData()
}

const handleRefresh = () => {
  searchForm.SearchTerm = ''
  searchForm.Status = undefined
  pagination.pageIndex = 1
  loadData()
}

const handleCreate = () => {
  dialogTitle.value = '添加角色'
  isView.value = false
  isEdit.value = false
  resetForm()
  dialogVisible.value = true
}

const handleView = async (row: Role) => {
  dialogTitle.value = '查看角色'
  isView.value = true
  isEdit.value = false
  try {
    const data = await getRoleDetail(row.id)
    Object.assign(formData, data)
    dialogVisible.value = true
  } catch (error) {
    console.error('获取详情失败:', error)
  }
}

const handleEdit = async (row: Role) => {
  dialogTitle.value = '编辑角色'
  isView.value = false
  isEdit.value = true
  try {
    const data = await getRoleDetail(row.id)
    Object.assign(formData, data)
    dialogVisible.value = true
  } catch (error) {
    console.error('获取详情失败:', error)
  }
}

const handleDelete = (row: Role) => {
  if (row.isSystem === 1) {
    ElMessage.warning('系统角色不能删除')
    return
  }
  
  ElMessageBox.confirm(`确定要删除角色【${row.name}】吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    try {
      await deleteRole(row.id)
      ElMessage.success('删除成功')
      loadData()
    } catch (error) {
      console.error('删除失败:', error)
    }
  })
}

const handleAuth = (row: Role) => {
  // 跳转到权限配置页面，传入角色ID
  router.push({
    path: '/permission/menu-permission',
    query: {
      roleId: row.id.toString()
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
          await updateRole({
            id: formData.id,
            name: formData.name,
            code: formData.code,
            isSystem: formData.isSystem,
            sort: formData.sort || 0,
            remark: formData.remark,
          })
          ElMessage.success('更新成功')
        } else {
          await createRole({
            name: formData.name,
            code: formData.code,
            isSystem: formData.isSystem,
            remark: formData.remark,
          })
          ElMessage.success('创建成功')
        }
        dialogVisible.value = false
        loadData()
      } catch (error: any) {
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
    id: undefined,
    name: '',
    code: '',
    isSystem: 0,
    sort: 0,
    remark: '',
    status: 1,
    statusName: '',
    createdBy: '',
    createdAt: '',
  })
}

const formatDateTime = (dateTime: string): string => {
  if (!dateTime) return '-'
  return dateTime.replace('T', ' ')
}

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
    if (searchForm.Status !== undefined) {
      params.Status = searchForm.Status
    }

    const data = await getRoleList(params)
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
  loadData()
})
</script>

<style scoped lang="scss">
.role-page {
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

  .auth-header {
    margin-bottom: 20px;
  }

  .auth-content {
    max-height: 600px;
    overflow-y: auto;
    padding: 10px;

    .permission-modules {
      display: flex;
      flex-direction: column;
      gap: 24px;

      .module-item {
        border: 1px solid #e4e7ed;
        border-radius: 8px;
        padding: 16px;
        background-color: #fafafa;

        .module-header {
          margin-bottom: 16px;
          padding-bottom: 12px;
          border-bottom: 2px solid #409eff;

          .module-name {
            font-size: 16px;
            font-weight: 600;
            color: #303133;
          }
        }

        .module-content {
          display: flex;
          flex-direction: column;
          gap: 16px;

          .submenu-item {
            background-color: #ffffff;
            border: 1px solid #ebeef5;
            border-radius: 6px;
            padding: 12px 16px;

            .submenu-header {
              margin-bottom: 12px;

              .submenu-name {
                font-size: 14px;
                font-weight: 500;
                color: #606266;
                margin-right: 12px;
              }

              .submenu-url {
                font-size: 12px;
                color: #909399;
                font-style: italic;
              }
            }

            .button-permissions {
              padding: 12px;
              background-color: #f5f7fa;
              border-radius: 4px;
              margin-top: 8px;

              :deep(.el-checkbox-group) {
                display: grid;
                grid-template-columns: repeat(auto-fill, minmax(100px, 1fr));
                gap: 12px;

                .el-checkbox {
                  margin-right: 0;

                  .el-checkbox__label {
                    font-size: 13px;
                    color: #606266;
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}
</style>
