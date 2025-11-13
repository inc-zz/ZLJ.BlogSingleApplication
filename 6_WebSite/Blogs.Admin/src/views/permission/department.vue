<template>
  <div class="department-page">
    <el-card>
      <div class="page-title">部门管理</div>

      <div class="department-container">
        <!-- 左侧部门树 -->
        <div class="department-tree-panel">
          <div class="tree-header">
            <span>部门列表</span>
            <el-button link type="primary" :icon="Refresh" @click="loadTree">
              刷新
            </el-button>
          </div>
          <el-tree
            ref="treeRef"
            :data="treeData"
            :props="treeProps"
            node-key="id"
            :expand-on-click-node="false"
            default-expand-all
            highlight-current
            @node-click="handleNodeClick"
          >
            <template #default="{ node }">
              <span class="custom-tree-node">
                <el-icon><OfficeBuilding /></el-icon>
                <span>{{ node.label }}</span>
              </span>
            </template>
          </el-tree>
        </div>

        <!-- 右侧部门列表 -->
        <div class="department-content-panel">
          <!-- 搜索栏 -->
          <el-form :inline="true" :model="searchForm" class="search-form">
            <el-form-item>
              <el-input
                v-model="searchForm.Where"
                placeholder="请输入部门名称"
                clearable
                style="width: 250px"
              >
                <template #prefix>
                  <el-icon><Search /></el-icon>
                </template>
              </el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" :icon="Search" @click="handleSearch">搜索</el-button>
              <el-button :icon="Refresh" @click="handleRefresh">刷新</el-button>
            </el-form-item>
          </el-form>

          <!-- 操作栏 -->
          <div class="toolbar">
            <el-button type="primary" :icon="Plus" @click="handleCreate">添加部门</el-button>
          </div>

          <!-- 树形表格 -->
          <el-table
            :data="tableData"
            stripe
            border
            :loading="loading"
            row-key="id"
            :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
            default-expand-all
          >
            <el-table-column type="index" label="序号" width="80" :index="(index) => (pagination.pageIndex - 1) * pagination.pageSize + index + 1" />
            <el-table-column prop="name" label="部门名称" min-width="200" />
            <el-table-column prop="abbreviation" label="简称" min-width="200" />
            <el-table-column prop="parentName" label="上级部门" width="150">
              <template #default="{ row }">
                {{ row.parentName || '顶级部门' }}
              </template>
            </el-table-column>
            <el-table-column prop="description" label="部门描述" min-width="180">
              <template #default="{ row }">
                {{ row.description || '-' }}
              </template>
            </el-table-column>
            <el-table-column prop="createdBy" label="创建人" width="120" />
            <el-table-column prop="createdAt" label="创建时间" width="180">
              <template #default="{ row }">
                {{ formatDateTime(row.createdAt) }}
              </template>
            </el-table-column>
            <el-table-column prop="modifiedAt" label="修改时间" width="180">
              <template #default="{ row }">
                {{ formatDateTime(row.modifiedAt) }}
              </template>
            </el-table-column>
            <el-table-column label="操作" width="180" fixed="right">
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
            :page-sizes="[50, 100, 200]"
            layout="total, sizes, prev, pager, next, jumper"
            background
            class="pagination"
            @size-change="handlePageChange"
            @current-change="handlePageChange"
          />
        </div>
      </div>
    </el-card>

    <!-- 添加/编辑部门对话框 -->
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
        <el-form-item label="部门名称" prop="name">
          <el-input
            v-model="formData.name"
            :disabled="isView"
            placeholder="请输入部门名称"
            maxlength="30"
            show-word-limit
          />
        </el-form-item>
        <el-form-item label="上级部门" prop="parentId">
          <el-tree-select
            v-model="formData.parentId"
            :data="parentTreeOptions"
            :props="{ children: 'children', label: 'name', value: 'id', disabled: 'disabled' }"
            check-strictly
            :render-after-expand="false"
            :disabled="isView"
            placeholder="选择顶级部门则作为一级部门"
            style="width: 100%"
            clearable
          />
        </el-form-item>
        <el-form-item label="排序" prop="sort">
          <el-input-number
            v-model="formData.sort"
            :min="0"
            :disabled="isView"
            placeholder="请输入排序"
            controls-position="right"
            style="width: 150px"
          />
        </el-form-item>
        <el-form-item label="简称" prop="abbreviation">
          <el-input
            v-model="formData.abbreviation"
            :disabled="isView"
            placeholder="请输入简称（可选）"
            maxlength="20"
            show-word-limit
          />
        </el-form-item>
        <el-form-item label="部门描述" prop="description">
          <el-input
            v-model="formData.description"
            type="textarea"
            :rows="3"
            :disabled="isView"
            placeholder="请输入部门描述"
            maxlength="150"
            show-word-limit
          />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button
          v-if="!isView"
          type="primary"
          :loading="submitLoading"
          @click="handleSubmit"
        >
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
  Edit,
  View,
  OfficeBuilding,
} from '@element-plus/icons-vue'
import {
  getDepartmentList,
  getDepartmentTree,
  createDepartment,
  updateDepartment,
  deleteDepartment,
  type Department,
  type DepartmentTreeNode,
} from '@/api/department'

const loading = ref(false)
const submitLoading = ref(false)
const tableData = ref<Department[]>([])
const treeData = ref<DepartmentTreeNode[]>([])
const dialogVisible = ref(false)
const dialogTitle = ref('')
const formRef = ref<FormInstance>()
const treeRef = ref()
const currentDepId = ref<number | undefined>(undefined)
const isView = ref(false)

const searchForm = reactive({
  Where: '',
})

const formData = reactive<any>({
  id: undefined,
  name: '',
  parentId: 0,
  abbreviation: '',
  sort: 0,
  description: '',
})

const pagination = reactive({
  pageIndex: 1,
  pageSize: 50,
  total: 0,
})

const treeProps = {
  label: 'name',
  children: 'children',
}

const formRules: FormRules = {
  name: [
    { required: true, message: '请输入部门名称', trigger: 'blur' },
    { max: 30, message: '部门名称最多30个字符', trigger: 'blur' }
  ],
  abbreviation: [
    { max: 20, message: '简称最多20个字符', trigger: 'blur' }
  ],
  description: [
    { max: 150, message: '部门描述最多150个字符', trigger: 'blur' }
  ],
}

// 计算父部门选项（过滤掉三级部门）
const parentTreeOptions = computed(() => {
  const filterThirdLevel = (nodes: DepartmentTreeNode[], level: number = 1): any[] => {
    return nodes.map((node) => {
      const newNode: any = {
        id: node.id,
        name: node.name,
        disabled: level >= 3, // 三级及以上不能作为父节点
      }
      
      // 如果有子节点且未达到三级，递归处理
      if (node.children && node.children.length > 0 && level < 3) {
        newNode.children = filterThirdLevel(node.children, level + 1)
      }
      
      return newNode
    })
  }

  const options = [
    {
      id: 0,
      name: '顶级部门',
      disabled: false,
      children: filterThirdLevel(treeData.value),
    },
  ]
  
  return options
})

const formatDateTime = (dateTime: string | null): string => {
  if (!dateTime) return '-'
  return dateTime.replace('T', ' ')
}

const handleSearch = () => {
  pagination.pageIndex = 1
  loadData()
}

const handleRefresh = () => {
  searchForm.Where = ''
  currentDepId.value = undefined
  pagination.pageIndex = 1
  loadData()
  loadTree()
}

const handlePageChange = () => {
  loadData()
}

const handleCreate = () => {
  dialogTitle.value = '添加部门'
  isView.value = false
  resetForm()
  // 如果左侧选择了部门，设置为父部门
  if (currentDepId.value) {
    formData.parentId = currentDepId.value
  }
  dialogVisible.value = true
}

const handleView = async (row: Department) => {
  dialogTitle.value = '查看部门'
  isView.value = true
  // 直接使用行数据，不需要再次请求详情
  Object.assign(formData, {
    id: row.id,
    name: row.name,
    parentId: getParentIdFromTree(row.id) || 0,
    abbreviation: '',
    sort: 0,
    description: row.description || '',
  })
  dialogVisible.value = true
}

const handleEdit = async (row: Department) => {
  dialogTitle.value = '编辑部门'
  isView.value = false
  // 直接使用行数据，不需要再次请求详情
  Object.assign(formData, {
    id: row.id,
    name: row.name,
    parentId: getParentIdFromTree(row.id) || 0,
    abbreviation: '',
    sort: 0,
    description: row.description || '',
  })
  dialogVisible.value = true
}

// 从树结构中查找节点的父ID
const getParentIdFromTree = (targetId: number): number | null => {
  const findParent = (nodes: DepartmentTreeNode[], parentId: number = 0): number | null => {
    for (const node of nodes) {
      if (node.id === targetId) {
        return parentId
      }
      if (node.children && node.children.length > 0) {
        const found = findParent(node.children, node.id)
        if (found !== null) {
          return found
        }
      }
    }
    return null
  }
  return findParent(treeData.value)
}

const handleDelete = (row: Department) => {
  ElMessageBox.confirm(`确定要删除部门 "${row.name}" 吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  })
    .then(async () => {
      try {
        await deleteDepartment(row.id)
        ElMessage.success('删除成功')
        loadData()
        loadTree()
      } catch (error) {
        console.error('删除失败:', error)
      }
    })
    .catch(() => {
      // 取消删除
    })
}

const handleNodeClick = (data: DepartmentTreeNode) => {
  currentDepId.value = data.id
  pagination.pageIndex = 1
  loadData()
}

const handleSubmit = async () => {
  if (!formRef.value) return

  await formRef.value.validate(async (valid) => {
    if (valid) {
      submitLoading.value = true
      try {
        if (formData.id) {
          // 编辑
          await updateDepartment({
            id: formData.id,
            parentId: formData.parentId,
            name: formData.name,
            abbreviation: formData.abbreviation || '',
            sort: formData.sort || 0,
            description: formData.description || '',
          })
          ElMessage.success('更新成功')
        } else {
          // 添加
          await createDepartment({
            parentId: formData.parentId,
            name: formData.name,
            abbreviation: formData.abbreviation || '',
            sort: formData.sort || 0,
            description: formData.description || '',
          })
          ElMessage.success('创建成功')
        }
        dialogVisible.value = false
        loadData()
        loadTree()
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
    id: undefined,
    name: '',
    parentId: 0,
    abbreviation: '',
    sort: 0,
    description: '',
  })
}

const loadTree = async () => {
  try {
    const data = await getDepartmentTree()
    treeData.value = data || []
  } catch (error) {
    console.error('加载部门树失败:', error)
    treeData.value = []
  }
}

const loadData = async () => {
  loading.value = true
  try {
    const params: any = {
      PageIndex: pagination.pageIndex,
      PageSize: pagination.pageSize,
    }

    if (currentDepId.value) {
      params.DepId = currentDepId.value
    }
    if (searchForm.Where) {
      params.Where = searchForm.Where
    }

    const data = await getDepartmentList(params)
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
  loadTree()
  loadData()
})
</script>

<style scoped lang="scss">
.department-page {
  .page-title {
    font-size: 18px;
    font-weight: bold;
    margin-bottom: 20px;
  }

  .department-container {
    display: flex;
    gap: 20px;
    height: calc(100vh - 200px);

    .department-tree-panel {
      width: 280px;
      flex-shrink: 0;
      border: 1px solid #dcdfe6;
      border-radius: 4px;
      padding: 16px;
      overflow-y: auto;
      background-color: #fafafa;

      .tree-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 12px;
        padding-bottom: 12px;
        border-bottom: 1px solid #dcdfe6;

        span {
          font-weight: bold;
          font-size: 14px;
        }
      }

      .custom-tree-node {
        display: flex;
        align-items: center;
        gap: 6px;
        font-size: 14px;
      }
    }

    .department-content-panel {
      flex: 1;
      display: flex;
      flex-direction: column;
      overflow: hidden;

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
  }
}
</style>
