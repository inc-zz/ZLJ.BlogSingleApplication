<template>
  <div class="menu-permission-page">
    <el-card>
      <div class="page-title">权限配置</div>

      <div class="permission-container">
        <!-- 左侧菜单树 -->
        <div class="menu-tree-panel">
          <div class="tree-header">
            <span class="tree-title">菜单列表</span>
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
            @node-click="handleMenuClick"
          >
            <template #default="{ node, data }">
              <span class="custom-tree-node">
                <el-icon v-if="data.icon"><component :is="data.icon" /></el-icon>
                <span>{{ node.label }}</span>
              </span>
            </template>
          </el-tree>
        </div>

        <!-- 右侧按钮权限配置 -->
        <div class="button-config-panel">
          <div class="panel-header">
            <div v-if="currentMenu" class="menu-info">
              <el-tag type="primary" size="large">{{ currentMenu.name }}</el-tag>
              <span class="menu-url">{{ currentMenu.url || '-' }}</span>
            </div>
            <div v-else class="empty-hint">
              <el-empty description="请从左侧选择菜单" />
            </div>
          </div>

          <div v-if="currentMenu" class="button-list">
            <div class="section-title">
              <el-icon><Key /></el-icon>
              <span>按钮权限配置</span>
            </div>

            <el-alert
              title="提示：勾选下方按钮为该菜单授予相应的操作权限"
              type="info"
              :closable="false"
              style="margin-bottom: 20px"
            />

            <!-- 工具栏按钮 -->
            <div class="button-section">
              <div class="section-label">
                <el-icon><Tools /></el-icon>
                <span>工具栏按钮</span>
              </div>
              <el-checkbox-group v-model="selectedToolbarButtons" class="button-checkbox-group">
                <el-checkbox 
                  v-for="button in toolbarButtons" 
                  :key="button.id" 
                  :value="button.code"
                  :label="button.code"
                  :disabled="button.status === 0"
                >
                  <el-tag :type="getButtonTypeTag(button.buttonType)" size="small">
                    {{ button.name }}
                  </el-tag>
                  <span class="button-code">({{ button.code }})</span>
                  <span v-if="button.description" class="button-desc">- {{ button.description }}</span>
                </el-checkbox>
              </el-checkbox-group>
              <el-empty v-if="toolbarButtons.length === 0" description="暂无工具栏按钮" :image-size="60" />
            </div>

            <!-- 行操作按钮 -->
            <div class="button-section">
              <div class="section-label">
                <el-icon><Menu /></el-icon>
                <span>行操作按钮</span>
              </div>
              <el-checkbox-group v-model="selectedRowButtons" class="button-checkbox-group">
                <el-checkbox 
                  v-for="button in rowButtons" 
                  :key="button.id" 
                  :value="button.code"
                  :label="button.code"
                  :disabled="button.status === 0"
                >
                  <el-tag :type="getButtonTypeTag(button.buttonType)" size="small">
                    {{ button.name }}
                  </el-tag>
                  <span class="button-code">({{ button.code }})</span>
                  <span v-if="button.description" class="button-desc">- {{ button.description }}</span>
                </el-checkbox>
              </el-checkbox-group>
              <el-empty v-if="rowButtons.length === 0" description="暂无行操作按钮" :image-size="60" />
            </div>

            <!-- 保存按钮 -->
            <div class="action-footer">
              <el-button @click="handleCancel">取消</el-button>
              <el-button type="primary" :loading="saving" @click="handleSave">
                保存权限配置
              </el-button>
            </div>
          </div>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, computed } from 'vue'
import { ElMessage } from 'element-plus'
import {
  Refresh,
  Key,
  Tools,
  Menu,
} from '@element-plus/icons-vue'
import { getMenuTree, type MenuTreeNode } from '@/api/menu'
import { getButtonList, type SysButton } from '@/api/button'

const menuTreeRef = ref()
const menuTreeData = ref<MenuTreeNode[]>([])
const currentMenu = ref<MenuTreeNode | null>(null)
const allButtons = ref<SysButton[]>([])
const selectedToolbarButtons = ref<string[]>([])
const selectedRowButtons = ref<string[]>([])
const saving = ref(false)

// 工具栏按钮（position为toolbar或both）
const toolbarButtons = computed(() => {
  return allButtons.value.filter(btn => 
    btn.position === 'toolbar' || btn.position === 'both'
  )
})

// 行操作按钮（position为row或both）
const rowButtons = computed(() => {
  return allButtons.value.filter(btn => 
    btn.position === 'row' || btn.position === 'both'
  )
})

// 获取按钮类型对应的标签类型
const getButtonTypeTag = (buttonType: string) => {
  const typeMap: Record<string, any> = {
    primary: 'primary',
    success: 'success',
    warning: 'warning',
    danger: 'danger',
    info: 'info',
    default: '',
  }
  return typeMap[buttonType] || ''
}

const handleMenuClick = (data: MenuTreeNode) => {
  currentMenu.value = data
  // TODO: 加载该菜单已配置的按钮权限
  // 临时清空选中
  selectedToolbarButtons.value = []
  selectedRowButtons.value = []
  
  ElMessage.info(`已选择菜单：${data.name}，待后端接口完成后可保存配置`)
}

const handleCancel = () => {
  currentMenu.value = null
  selectedToolbarButtons.value = []
  selectedRowButtons.value = []
}

const handleSave = async () => {
  if (!currentMenu.value) {
    ElMessage.warning('请先选择菜单')
    return
  }

  const allSelectedButtons = [
    ...selectedToolbarButtons.value,
    ...selectedRowButtons.value,
  ]

  if (allSelectedButtons.length === 0) {
    ElMessage.warning('请至少选择一个按钮权限')
    return
  }

  saving.value = true
  try {
    // TODO: 调用保存接口
    // await saveMenuButtonPermission({
    //   menuId: currentMenu.value.id,
    //   buttonCodes: allSelectedButtons,
    // })
    
    // 模拟保存
    await new Promise(resolve => setTimeout(resolve, 1000))
    
    ElMessage.success(`已为菜单【${currentMenu.value.name}】保存权限配置（待接口对接）`)
    console.log('保存配置:', {
      menuId: currentMenu.value.id,
      menuName: currentMenu.value.name,
      toolbarButtons: selectedToolbarButtons.value,
      rowButtons: selectedRowButtons.value,
      allButtons: allSelectedButtons,
    })
  } catch (error) {
    console.error('保存失败:', error)
  } finally {
    saving.value = false
  }
}

const loadMenuTree = async () => {
  try {
    menuTreeData.value = await getMenuTree()
  } catch (error) {
    console.error('加载菜单树失败:', error)
  }
}

const loadButtons = async () => {
  try {
    const result = await getButtonList({
      PageIndex: 1,
      PageSize: 1000, // 加载所有按钮
    })
    allButtons.value = result.data.items || []
  } catch (error) {
    console.error('加载按钮列表失败:', error)
  }
}

onMounted(() => {
  loadMenuTree()
  loadButtons()
})
</script>

<style scoped lang="scss">
.menu-permission-page {
  .page-title {
    font-size: 18px;
    font-weight: bold;
    margin-bottom: 20px;
  }

  .permission-container {
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

    .button-config-panel {
      flex: 1;
      display: flex;
      flex-direction: column;
      overflow: hidden;

      .panel-header {
        margin-bottom: 20px;

        .menu-info {
          display: flex;
          align-items: center;
          gap: 12px;

          .menu-url {
            color: #909399;
            font-size: 14px;
          }
        }

        .empty-hint {
          height: 200px;
          display: flex;
          align-items: center;
          justify-content: center;
        }
      }

      .button-list {
        flex: 1;
        overflow-y: auto;

        .section-title {
          display: flex;
          align-items: center;
          gap: 8px;
          font-size: 16px;
          font-weight: 600;
          color: #303133;
          margin-bottom: 16px;
          padding-bottom: 12px;
          border-bottom: 2px solid #409eff;

          .el-icon {
            color: #409eff;
          }
        }

        .button-section {
          margin-bottom: 32px;
          padding: 16px;
          background-color: #f5f7fa;
          border-radius: 8px;

          .section-label {
            display: flex;
            align-items: center;
            gap: 8px;
            font-size: 14px;
            font-weight: 500;
            color: #606266;
            margin-bottom: 12px;

            .el-icon {
              color: #67c23a;
            }
          }

          .button-checkbox-group {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
            gap: 12px;

            :deep(.el-checkbox) {
              margin-right: 0;
              padding: 8px 12px;
              background-color: #fff;
              border: 1px solid #dcdfe6;
              border-radius: 4px;
              transition: all 0.3s;

              &:hover {
                border-color: #409eff;
                background-color: #ecf5ff;
              }

              &.is-checked {
                border-color: #409eff;
                background-color: #ecf5ff;
              }

              &.is-disabled {
                opacity: 0.5;
                cursor: not-allowed;
              }

              .el-checkbox__label {
                display: flex;
                align-items: center;
                gap: 4px;
                white-space: nowrap;

                .button-code {
                  color: #909399;
                  font-size: 12px;
                }

                .button-desc {
                  color: #909399;
                  font-size: 12px;
                  margin-left: 4px;
                }
              }
            }
          }
        }

        .action-footer {
          display: flex;
          justify-content: flex-end;
          gap: 12px;
          padding: 16px 0;
          border-top: 1px solid #e4e7ed;
          margin-top: 20px;
        }
      }
    }
  }
}
</style>
