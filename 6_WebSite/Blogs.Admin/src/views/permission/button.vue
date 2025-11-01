<template>
  <div class="button-page">
    <el-card>
      <div class="page-title">按钮管理</div>

      <!-- 搜索栏 -->
      <el-form :inline="true" :model="searchForm" class="search-form">
        <el-form-item>
          <el-input
            v-model="searchForm.Name"
            placeholder="按钮名称"
            clearable
            style="width: 200px"
          >
            <template #prefix>
              <el-icon><Search /></el-icon>
            </template>
          </el-input>
        </el-form-item>
        <el-form-item>
          <el-select
            v-model="searchForm.Position"
            placeholder="按钮位置"
            clearable
            style="width: 150px"
          >
            <el-option label="工具栏" value="toolbar" />
            <el-option label="行操作" value="row" />
            <el-option label="两者" value="both" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" :icon="Search" @click="handleSearch">搜索</el-button>
          <el-button :icon="Refresh" @click="handleRefresh">刷新</el-button>
        </el-form-item>
      </el-form>

      <!-- 操作栏 -->
      <div class="toolbar">
        <el-button type="primary" :icon="Plus" @click="handleCreate">添加按钮</el-button>
      </div>

      <!-- 表格 -->
      <el-table
        :data="tableData"
        stripe
        border
        :loading="loading"
        row-key="id"
      >
        <el-table-column type="index" label="序号" width="80" :index="(index) => (pagination.pageIndex - 1) * pagination.pageSize + index + 1" />
        <el-table-column prop="name" label="按钮名称" width="120" />
        <el-table-column prop="code" label="按钮编码" width="150" />
        <el-table-column prop="description" label="描述" min-width="150">
          <template #default="{ row }">
            {{ row.description || '-' }}
          </template>
        </el-table-column>
        <el-table-column prop="icon" label="图标" width="100">
          <template #default="{ row }">
            {{ row.icon || '-' }}
          </template>
        </el-table-column>
        <el-table-column prop="buttonType" label="类型" width="120">
          <template #default="{ row }">
            <el-tag v-if="row.buttonType === 'primary'" type="primary">主要</el-tag>
            <el-tag v-else-if="row.buttonType === 'success'" type="success">成功</el-tag>
            <el-tag v-else-if="row.buttonType === 'warning'" type="warning">警告</el-tag>
            <el-tag v-else-if="row.buttonType === 'danger'" type="danger">危险</el-tag>
            <el-tag v-else type="info">默认</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="position" label="位置" width="120">
          <template #default="{ row }">
            <el-tag v-if="row.position === 'toolbar'">工具栏</el-tag>
            <el-tag v-else-if="row.position === 'row'" type="success">行操作</el-tag>
            <el-tag v-else type="warning">两者</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="status" label="状态" width="100">
          <template #default="{ row }">
            <el-switch
              v-model="row.status"
              :active-value="1"
              :inactive-value="0"
              @change="handleStatusChange(row)"
            />
          </template>
        </el-table-column>
        <el-table-column prop="createdAt" label="创建时间" width="180">
          <template #default="{ row }">
            {{ formatDateTime(row.createdAt) }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="220" fixed="right">
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

    <!-- 按钮详情/编辑对话框 -->
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
        <el-form-item label="按钮名称" prop="name">
          <el-input 
            v-model="formData.name" 
            :disabled="isView"
            placeholder="请输入按钮名称" 
          />
        </el-form-item>
        <el-form-item label="按钮编码" prop="code">
          <el-input 
            v-model="formData.code" 
            :disabled="isView || isEdit"
            placeholder="请输入按钮编码（唯一标识）" 
          />
        </el-form-item>
        <el-form-item label="按钮位置" prop="position">
          <el-radio-group v-model="formData.position" :disabled="isView">
            <el-radio value="toolbar">工具栏</el-radio>
            <el-radio value="row">行操作</el-radio>
            <el-radio value="both">两者</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="按钮类型" prop="buttonType">
          <el-select 
            v-model="formData.buttonType" 
            :disabled="isView"
            placeholder="请选择按钮类型"
            style="width: 100%"
          >
            <el-option label="默认" value="default" />
            <el-option label="主要" value="primary" />
            <el-option label="成功" value="success" />
            <el-option label="警告" value="warning" />
            <el-option label="危险" value="danger" />
            <el-option label="信息" value="info" />
          </el-select>
        </el-form-item>
        <el-form-item label="图标" prop="icon">
          <el-input 
            v-model="formData.icon" 
            :disabled="isView"
            placeholder="请输入图标名称（可选）" 
          />
        </el-form-item>
        <el-form-item label="描述" prop="description">
          <el-input
            v-model="formData.description"
            type="textarea"
            :rows="3"
            :disabled="isView"
            placeholder="请输入按钮描述"
          />
        </el-form-item>
        <el-form-item label="关联菜单" v-if="isView && formData.relatedMenus">
          <el-tag 
            v-for="menu in formData.relatedMenus" 
            :key="menu.id"
            style="margin-right: 8px; margin-bottom: 8px"
          >
            {{ menu.name }}
          </el-tag>
          <span v-if="!formData.relatedMenus || formData.relatedMenus.length === 0">
            暂无关联菜单
          </span>
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
import {
  getButtonList,
  getButtonDetail,
  createButton,
  updateButton,
  deleteButton,
  updateButtonStatus,
  type SysButton,
} from '@/api/button'

const loading = ref(false)
const submitLoading = ref(false)
const tableData = ref<SysButton[]>([])
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
  Name: '',
  Position: '',
})

const formData = reactive<any>({
  id: undefined,
  name: '',
  code: '',
  position: 'toolbar',
  buttonType: 'default',
  icon: '',
  description: '',
  relatedMenus: [],
})

const formRules: FormRules = {
  name: [{ required: true, message: '请输入按钮名称', trigger: 'blur' }],
  code: [
    { required: true, message: '请输入按钮编码', trigger: 'blur' },
    { pattern: /^[A-Z][a-zA-Z]*$/, message: '编码必须以大写字母开头，只能包含字母', trigger: 'blur' },
  ],
  position: [{ required: true, message: '请选择按钮位置', trigger: 'change' }],
}

const handleSearch = () => {
  pagination.pageIndex = 1
  loadData()
}

const handleRefresh = () => {
  searchForm.Name = ''
  searchForm.Position = ''
  pagination.pageIndex = 1
  loadData()
}

const handleCreate = () => {
  dialogTitle.value = '添加按钮'
  isView.value = false
  isEdit.value = false
  resetForm()
  dialogVisible.value = true
}

const handleView = async (row: SysButton) => {
  dialogTitle.value = '查看按钮'
  isView.value = true
  isEdit.value = false
  try {
    const data = await getButtonDetail(row.id)
    Object.assign(formData, data)
    dialogVisible.value = true
  } catch (error) {
    console.error('获取详情失败:', error)
  }
}

const handleEdit = async (row: SysButton) => {
  dialogTitle.value = '编辑按钮'
  isView.value = false
  isEdit.value = true
  try {
    const data = await getButtonDetail(row.id)
    Object.assign(formData, data)
    dialogVisible.value = true
  } catch (error) {
    console.error('获取详情失败:', error)
  }
}

const handleDelete = (row: SysButton) => {
  ElMessageBox.confirm(`确定要删除按钮【${row.name}】吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    try {
      await deleteButton(row.id)
      ElMessage.success('删除成功')
      loadData()
    } catch (error) {
      console.error('删除失败:', error)
    }
  })
}

const handleStatusChange = async (row: SysButton) => {
  try {
    await updateButtonStatus({ id: row.id, status: row.status })
    ElMessage.success('状态更新成功')
  } catch (error) {
    // 恢复原状态
    row.status = row.status === 1 ? 0 : 1
    console.error('状态更新失败:', error)
  }
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
        const submitData = {
          name: formData.name,
          code: formData.code,
          position: formData.position,
          buttonType: formData.buttonType,
          icon: formData.icon,
          description: formData.description,
        }

        if (isEdit.value) {
          await updateButton({
            ...submitData,
            id: formData.id,
          })
          ElMessage.success('更新成功')
        } else {
          await createButton(submitData)
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
    position: 'toolbar',
    buttonType: 'default',
    icon: '',
    description: '',
    relatedMenus: [],
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
    
    if (searchForm.Name) {
      params.Name = searchForm.Name
    }
    if (searchForm.Position) {
      params.Position = searchForm.Position
    }

    const data = await getButtonList(params)
    tableData.value = data.items || []
    pagination.total = data.total || 0
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
.button-page {
  .page-title {
    font-size: 18px;
    font-weight: bold;
    margin-bottom: 20px;
  }

  .search-form {
    margin-bottom: 0;
  }

  .toolbar {
    margin: 16px 0;
  }

  .pagination {
    margin-top: 20px;
    display: flex;
    justify-content: flex-end;
  }
}
</style>
