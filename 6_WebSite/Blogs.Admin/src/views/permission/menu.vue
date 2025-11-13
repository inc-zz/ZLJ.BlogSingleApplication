<template>
  <div class="menu-page">
    <el-card>
      <div class="page-title">菜单管理</div>

      <div class="menu-container">
        <!-- 左侧菜单树 -->
        <div class="menu-tree-panel">
          <div class="tree-header">
            <span class="tree-title">菜单结构</span>
            <el-button link type="primary" :icon="Refresh" @click="loadMenuTree">
              刷新
            </el-button>
          </div>
          <el-tree
            ref="menuTreeRef"
            :data="menuTreeData"
            :props="{ children: 'children', label: 'name' }"
            node-key="id"
            :expand-on-click-node="false"
            default-expand-all
            highlight-current
            @node-click="handleTreeNodeClick"
          >
            <template #default="{ node, data }">
              <span class="custom-tree-node">
                <el-icon v-if="data.icon"><component :is="data.icon" /></el-icon>
                <span>{{ node.label }}</span>
              </span>
            </template>
          </el-tree>
        </div>

        <!-- 右侧菜单列表 -->
        <div class="menu-content-panel">
          <!-- 搜索栏 -->
          <el-form :inline="true" :model="searchForm" class="search-form">
            <el-form-item>
              <el-input
                v-model="searchForm.MenuName"
                placeholder="菜单名称"
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
            <el-button type="primary" :icon="Plus" @click="handleCreate">添加菜单</el-button>
            <el-button type="success" :icon="FolderAdd" @click="handleCreateChild" :disabled="!currentParentId && currentParentId !== 0">
              添加子菜单
            </el-button>
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
            <el-table-column prop="name" label="菜单名称" min-width="150" />
            <el-table-column prop="url" label="路径" min-width="200" />
            <el-table-column prop="icon" label="图标" width="150">
              <template #default="{ row }">
                <div v-if="row.icon" style="display: flex; align-items: center; gap: 8px;">
                  <el-icon><component :is="row.icon" /></el-icon>
                  <span>{{ row.icon }}</span>
                </div>
                <span v-else>-</span>
              </template>
            </el-table-column>
            <el-table-column prop="sort" label="排序" width="100" />
            <el-table-column prop="type" label="类型" width="100">
              <template #default="{ row }">
                <el-tag v-if="row.type === 1" type="info">目录</el-tag>
                <el-tag v-else-if="row.type === 2" type="success">页面</el-tag>
                <el-tag v-else-if="row.type === 3" type="warning">外部链接</el-tag>
                <span v-else>-</span>
              </template>
            </el-table-column>
            <el-table-column label="操作" width="260" fixed="right">
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
        </div>
      </div>
    </el-card>

    <!-- 菜单编辑对话框 -->
    <el-dialog
      v-model="dialogVisible"
      :title="dialogTitle"
      width="900px"
      @closed="handleDialogClosed"
    >
      <el-form
        ref="formRef"
        :model="formData"
        :rules="formRules"
        label-width="100px"
      >
        <el-form-item label="父级菜单" prop="parentId">
          <el-tree-select
            v-model="formData.parentId"
            :data="parentMenuOptions"
            :props="{ children: 'children', label: 'name', value: 'id' }"
            check-strictly
            :render-after-expand="false"
            :disabled="isView"
            placeholder="请选择父级菜单（不选则为顶级菜单）"
            style="width: 100%"
            clearable
          />
        </el-form-item>
        <el-form-item label="菜单名称" prop="name">
          <el-input 
            v-model="formData.name"
            :disabled="isView"
            placeholder="请输入菜单名称" 
          />
        </el-form-item>
        <el-form-item label="菜单类型" prop="type">
          <el-radio-group v-model="formData.type" :disabled="isView">
            <el-radio :value="1">目录</el-radio>
            <el-radio :value="2">页面</el-radio>
            <el-radio :value="3">外部链接</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="菜单路径" prop="url">
          <el-input 
            v-model="formData.url"
            :disabled="isView"
            placeholder="请输入菜单路径，如：/users" 
          />
        </el-form-item>
        <el-form-item label="图标" prop="icon">
          <el-input 
            v-model="formData.icon"
            :disabled="isView"
            placeholder="请输入图标名称（可选）" 
          />
        </el-form-item>
        <el-form-item label="排序" prop="sort">
          <el-input-number 
            v-model="formData.sort" 
            :min="0" 
            :max="9999"
            :disabled="isView"
            placeholder="数字越小越靠前"
            style="width: 150px"
          />
        </el-form-item>
        <el-form-item label="功能按钮" v-if="formData.type === 2">
          <el-checkbox-group v-model="formData.selectedButtons" class="button-checkbox-group" :disabled="isView">
            <el-checkbox
              v-for="btn in availableButtons"
              :key="btn.code"
              :value="btn.code"
            >
              {{ btn.name }}({{ btn.code }})
            </el-checkbox>
          </el-checkbox-group>
          <div v-if="availableButtons.length === 0" class="empty-hint">
            暂无可用按钮，请先在按钮管理中添加
          </div>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button v-if="!isView" type="primary" :loading="submitLoading" @click="handleSubmit">
          确定
        </el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, computed, watch } from 'vue'
import { ElMessage, ElMessageBox, type FormInstance, type FormRules } from 'element-plus'
import {
  Search,
  Delete,
  Plus,
  Refresh,
  Edit,
  View,
  FolderAdd,
} from '@element-plus/icons-vue'
import {
  getMenuList,
  getMenuTree,
  getMenuDetail,
  createMenu,
  updateMenu,
  deleteMenu,
  type Menu,
  type MenuTreeNode,
} from '@/api/menu'
import { getButtonList, type SysButton } from '@/api/button'

const loading = ref(false)
const submitLoading = ref(false)
const tableData = ref<Menu[]>([])
const menuTreeData = ref<MenuTreeNode[]>([])
const availableButtons = ref<SysButton[]>([])
const dialogVisible = ref(false)
const dialogTitle = ref('')
const isEdit = ref(false)
const isView = ref(false)
const formRef = ref<FormInstance>()
const menuTreeRef = ref()
const currentParentId = ref<number | null>(null)

const pagination = reactive({
  pageIndex: 1,
  pageSize: 10,
  total: 0,
})

const searchForm = reactive({
  MenuName: '',
})

const formData = reactive<any>({
  id: undefined,
  parentId: 0,
  name: '',
  url: '',
  iCon: '',
  type: 1,
  sort: 0,
  selectedButtons: [],
})

const formRules: FormRules = {
  name: [{ required: true, message: '请输入菜单名称', trigger: 'blur' }],
  url: [{ required: true, message: '请输入菜单路径', trigger: 'blur' }],
  type: [{ required: true, message: '请选择菜单类型', trigger: 'change' }],
}

// 父级菜单选项（用于下拉选择）
const parentMenuOptions = computed(() => {
  // 添加顶级选项
  const options: any[] = [
    { id: 0, name: '顶级菜单', children: [] }
  ]
  
  // 递归构建树形选项
  const buildOptions = (nodes: MenuTreeNode[]): any[] => {
    return nodes.map(node => ({
      id: node.id,
      name: node.name,
      children: node.children ? buildOptions(node.children) : []
    }))
  }
  
  options[0].children = buildOptions(menuTreeData.value)
  return options
})

// 监听对话框打开，加载按钮列表
watch(dialogVisible, (newVal) => {
  if (newVal) {
    loadAvailableButtons()
  }
})

// 加载可用按钮列表
const loadAvailableButtons = async () => {
  try {
    const data = await getButtonList({ pageIndex: 1, pageSize: 100 })
    availableButtons.value = data?.items || []
  } catch (error) {
    console.error('加载按钮列表失败:', error)
    availableButtons.value = []
  }
}

const handleSearch = () => {
  pagination.pageIndex = 1
  loadData()
}

const handleRefresh = () => {
  searchForm.MenuName = ''
  currentParentId.value = null
  pagination.pageIndex = 1
  loadData()
}

const handleCreate = () => {
  dialogTitle.value = '添加菜单'
  isEdit.value = false
  isView.value = false
  resetForm()
  formData.parentId = 0
  formData.url = '/'
  formData.type = 1
  dialogVisible.value = true
}

const handleCreateChild = () => {
  if (currentParentId.value === null) return
  
  dialogTitle.value = '添加子菜单'
  isEdit.value = false
  isView.value = false
  resetForm()
  formData.parentId = currentParentId.value
  formData.type = 2 // 默认为页面类型
  dialogVisible.value = true
}

const handleView = async (row: Menu) => {
  dialogTitle.value = '查看菜单'
  isEdit.value = false
  isView.value = true
  
  try {
    const data = await getMenuDetail(row.id)
    
    // 解析buttons字段
    let selectedButtons: string[] = []
    if (data.buttons) {
      if (Array.isArray(data.buttons)) {
        selectedButtons = data.buttons.map((btnId: number) => {
          const btn = availableButtons.value.find(b => b.id === btnId)
          return btn?.code
        }).filter((code): code is string => code !== undefined)
      } else if (typeof data.buttons === 'string') {
        try {
          const buttons = JSON.parse(data.buttons)
          if (Array.isArray(buttons)) {
            selectedButtons = buttons.map((btn: any) => {
              if (typeof btn === 'number') {
                const found = availableButtons.value.find(b => b.id === btn)
                return found?.code
              }
              const found = availableButtons.value.find(b => b.id === btn.Id)
              return found?.code
            }).filter((code): code is string => code !== undefined)
          }
        } catch (e) {
          console.error('解析buttons失败:', e)
        }
      }
    }
    
    Object.assign(formData, {
      id: data.id,
      parentId: data.parentId,
      name: data.name,
      url: data.url,
      icon: data.icon || '',
      type: data.type || 1,
      sort: data.sort,
      selectedButtons,
    })
    dialogVisible.value = true
  } catch (error) {
    console.error('获取菜单详情失败:', error)
  }
}

const handleEdit = async (row: Menu) => {
  dialogTitle.value = '编辑菜单'
  isEdit.value = true
  isView.value = false
  
  try {
    const data = await getMenuDetail(row.id)
    
    // 解析buttons字段 - 现在返回的是ID数组
    let selectedButtons: string[] = []
    if (data.buttons) {
      // 如果是数组，直接使用
      if (Array.isArray(data.buttons)) {
        // 将按钮ID映射为code
        selectedButtons = data.buttons.map((btnId: number) => {
          const btn = availableButtons.value.find(b => b.id === btnId)
          return btn?.code
        }).filter((code): code is string => code !== undefined)
      } else if (typeof data.buttons === 'string') {
        // 如果是JSON字符串，解析它
        try {
          const buttons = JSON.parse(data.buttons)
          if (Array.isArray(buttons)) {
            selectedButtons = buttons.map((btn: any) => {
              // 如果是数字，查找对应的code
              if (typeof btn === 'number') {
                const found = availableButtons.value.find(b => b.id === btn)
                return found?.code
              }
              // 如果是对象，使用Id字段
              const found = availableButtons.value.find(b => b.id === btn.Id)
              return found?.code
            }).filter((code): code is string => code !== undefined)
          }
        } catch (e) {
          console.error('解析buttons失败:', e)
        }
      }
    }
    
    Object.assign(formData, {
      id: data.id,
      parentId: data.parentId,
      name: data.name,
      url: data.url,
      icon: data.icon || '',
      type: data.type || 1,
      sort: data.sort,
      selectedButtons,
    })
    dialogVisible.value = true
  } catch (error) {
    console.error('获取菜单详情失败:', error)
  }
}

const handleDelete = (row: Menu) => {
  ElMessageBox.confirm(`确定要删除菜单【${row.name}】吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    try {
      await deleteMenu(row.id)
      ElMessage.success('删除成功')
      loadData()
      loadMenuTree()
    } catch (error) {
      console.error('删除失败:', error)
    }
  })
}

const handleTreeNodeClick = (data: MenuTreeNode) => {
  currentParentId.value = data.id
  pagination.pageIndex = 1
  loadData()
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
        // 构建按钮ID数组
        const buttonIds: number[] = formData.selectedButtons.map((code: string) => {
          const btn = availableButtons.value.find(b => b.code === code)
          return btn?.id
        }).filter((id: number | undefined) => id !== undefined)
        
        const submitData = {
          parentId: formData.parentId || 0,
          name: formData.name,
          url: formData.url,
          icon: formData.icon || '',
          buttons: buttonIds,
          type: formData.type,
          sort: formData.sort,
        }

        if (isEdit.value) {
          await updateMenu({
            ...submitData,
            id: formData.id,
          })
          ElMessage.success('更新成功')
        } else {
          await createMenu(submitData)
          ElMessage.success('创建成功')
        }
        
        dialogVisible.value = false
        loadData()
        loadMenuTree()
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
    parentId: 0,
    name: '',
    url: '',
    icon: '',
    type: 1,
    sort: 0,
    selectedButtons: [],
  })
}

const loadData = async () => {
  loading.value = true
  try {
    const params: any = {
      PageIndex: pagination.pageIndex,
      PageSize: pagination.pageSize,
      ParentId: currentParentId.value !== null ? currentParentId.value : 0,
    }
    
    if (searchForm.MenuName) {
      params.MenuName = searchForm.MenuName
    }

    const data = await getMenuList(params)
    tableData.value = data.items || []
    pagination.total = data.total || 0
  } catch (error) {
    console.error('加载数据失败:', error)
  } finally {
    loading.value = false
  }
}

const loadMenuTree = async () => {
  try {
    menuTreeData.value = await getMenuTree()
  } catch (error) {
    console.error('加载菜单树失败:', error)
  }
}

onMounted(() => {
  loadMenuTree()
  loadData()
})
</script>

<style scoped lang="scss">
.menu-page {
  .page-title {
    font-size: 18px;
    font-weight: bold;
    margin-bottom: 20px;
  }

  .menu-container {
    display: flex;
    gap: 20px;
    height: calc(100vh - 200px);

    .menu-tree-panel {
      width: 280px;
      flex-shrink: 0;
      border: 1px solid #e4e7ed;
      border-radius: 4px;
      padding: 16px;
      background-color: #fafafa;
      overflow-y: auto;

      .tree-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 16px;
        padding-bottom: 12px;
        border-bottom: 2px solid #409eff;

        .tree-title {
          font-size: 16px;
          font-weight: 600;
          color: #303133;
        }
      }

      :deep(.el-tree) {
        background-color: transparent;

        .custom-tree-node {
          display: flex;
          align-items: center;
          gap: 8px;
          font-size: 14px;

          .el-icon {
            color: #409eff;
          }
        }
      }
    }

    .menu-content-panel {
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

  .form-tip {
    font-size: 12px;
    color: #909399;
    margin-top: 4px;
  }

  .empty-hint {
    font-size: 13px;
    color: #909399;
    padding: 12px;
    background-color: #f5f7fa;
    border-radius: 4px;
    text-align: center;
  }

  :deep(.el-dialog__body) {
    max-height: 700px;
    overflow-y: auto;

    .button-checkbox-group {
      display: grid;
      grid-template-columns: repeat(3, minmax(0, 1fr));
      gap: 12px;
      padding: 16px;
      background-color: #f5f7fa;
      border-radius: 4px;
      max-height: 400px;
      overflow-y: auto;

      .el-checkbox {
        margin-right: 0;
        width: 100%;

        &.is-disabled {
          opacity: 0.5;
        }
      }
    }
  }
}
</style>
