<template>
  <div class="menu-permission-page">
    <el-card>
      <div class="page-title">权限配置</div>

      <div class="permission-container">
        <!-- 左侧角色树 -->
        <div class="role-tree-panel">
          <div class="tree-header">
            <span class="tree-title">角色列表</span>
            <el-button link type="primary" :icon="Refresh" @click="loadRoles">
              刷新
            </el-button>
          </div>
          <el-tree
            ref="roleTreeRef"
            :data="roleTreeData"
            :props="{ children: 'children', label: 'name' }"
            node-key="id"
            :expand-on-click-node="false"
            default-expand-all
            highlight-current
            @node-click="handleRoleClick"
          >
            <template #default="{ node, data }">
              <span class="custom-tree-node">
                <el-icon><UserFilled /></el-icon>
                <span>{{ node.label }}</span>
              </span>
            </template>
          </el-tree>
        </div>

        <!-- 右侧菜单权限配置 -->
        <div class="menu-permission-panel">
          <div v-if="!currentRole" class="empty-state">
            <el-empty description="请从左侧选择角色" :image-size="120" />
          </div>

          <div v-else class="permission-content" v-loading="loading">
            <!-- 全选控制 -->
            <div class="toolbar">
              <el-checkbox
                v-model="selectAll"
                :indeterminate="isIndeterminate"
                @change="handleSelectAllChange"
              >
                全选所有权限
              </el-checkbox>
              <div class="action-buttons">
                <el-button @click="handleCancel">取消</el-button>
                <el-button type="primary" :loading="saving" @click="handleSave">
                  保存权限配置
                </el-button>
              </div>
            </div>

            <!-- 菜单权限树 -->
            <div class="menu-tree-list">
              <template v-for="(menu, index) in menuPermissionTree" :key="menu.menuId">
                <MenuPermissionItem
                  :menu="menu"
                  :menu-selections="menuSelections"
                  :button-selections="buttonSelections"
                  :is-last-child="index === menuPermissionTree.length - 1"
                  @menu-change="handleMenuSelectChange"
                  @button-change="handleButtonChange"
                />
              </template>

              <el-empty v-if="menuPermissionTree.length === 0" description="暂无菜单权限数据" />
            </div>
          </div>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, defineComponent, h } from 'vue'
import { ElMessage, ElCheckbox, ElCheckboxGroup, ElDivider } from 'element-plus'
import {
  Refresh,
  UserFilled,
} from '@element-plus/icons-vue'
import { getAllRoles, getMenuPermissionTree, setRoleMenuAuth, type Role, type MenuPermissionNode, type MenuActionButton } from '@/api/role'

// 菜单权限项组件
const MenuPermissionItem = defineComponent({
  name: 'MenuPermissionItem',
  props: {
    menu: {
      type: Object as () => MenuPermissionNode,
      required: true
    },
    menuSelections: {
      type: Object as () => Record<number, boolean>,
      required: true
    },
    buttonSelections: {
      type: Object as () => Record<number, string[]>,
      required: true
    },
    level: {
      type: Number,
      default: 0
    },
    isLastChild: {
      type: Boolean,
      default: false
    }
  },
  emits: ['menu-change', 'button-change'],
  setup(props, { emit }) {
    // 判断菜单是否为半选状态
    const isMenuIndeterminate = () => {
      if (!props.buttonSelections[props.menu.menuId]) return false
      const selectedCount = props.buttonSelections[props.menu.menuId].length
      const totalCount = props.menu.menuButtons.length
      return selectedCount > 0 && selectedCount < totalCount
    }

    const handleMenuChange = () => {
      emit('menu-change', props.menu)
    }

    const handleButtonChange = () => {
      emit('button-change', props.menu)
    }

    return () => {
      const { menu, level, isLastChild } = props
      const hasButtons = menu.menuButtons && menu.menuButtons.length > 0
      const hasChildren = menu.children && menu.children.length > 0

      return h('div', { class: 'menu-permission-item' }, [
        // 菜单项（包含按钮的菜单）
        hasButtons ? h('div', { 
          class: 'menu-item',
          style: { marginLeft: `${level * 24}px` }
        }, [
          // 菜单头部 - 只显示菜单名称和checkbox
          h('div', { class: 'menu-header' }, [
            h(ElCheckbox, {
              modelValue: props.menuSelections[menu.menuId],
              indeterminate: isMenuIndeterminate(),
              'onUpdate:modelValue': (val: boolean) => {
                props.menuSelections[menu.menuId] = val
                handleMenuChange()
              }
            }, {
              default: () => h('span', { class: 'menu-name' }, menu.name)
            })
          ]),
          // 按钮权限 - 直接显示在菜单下方，添加左边距
          h('div', { 
            class: 'button-permissions',
            style: { marginLeft: '24px' }
          }, [
            h(ElCheckboxGroup, {
              modelValue: props.buttonSelections[menu.menuId] || [],
              class: 'button-checkbox-group',
              'onUpdate:modelValue': (val: string[]) => {
                props.buttonSelections[menu.menuId] = val
                handleButtonChange()
              }
            }, {
              default: () => menu.menuButtons.map(btn =>
                h(ElCheckbox, {
                  key: btn.buttonCode,
                  value: btn.buttonCode
                }, {
                  default: () => `${btn.buttonName}(${btn.buttonCode})`
                })
              )
            })
          ])
        ]) : (
          // 父级菜单（没有按钮只有子菜单）
          hasChildren ? h('div', { 
            class: 'parent-menu',
            style: { marginLeft: `${level * 24}px` }
          }, [
            h('div', { class: 'parent-menu-header' }, [
              h(ElCheckbox, {
                modelValue: props.menuSelections[menu.menuId],
                indeterminate: isMenuIndeterminate(),
                'onUpdate:modelValue': (val: boolean) => {
                  props.menuSelections[menu.menuId] = val
                  handleMenuChange()
                }
              }, {
                default: () => h('span', { class: 'parent-menu-name' }, menu.name)
              })
            ])
          ]) : null
        ),
        // 子菜单
        hasChildren ? menu.children.map((child, index) =>
          h(MenuPermissionItem, {
            key: child.menuId,
            menu: child,
            menuSelections: props.menuSelections,
            buttonSelections: props.buttonSelections,
            level: level + 1,
            isLastChild: index === menu.children.length - 1,
            onMenuChange: (menu: MenuPermissionNode) => emit('menu-change', menu),
            onButtonChange: (menu: MenuPermissionNode) => emit('button-change', menu)
          })
        ) : null
      ])
    }
  }
})

const loading = ref(false)
const saving = ref(false)
const roleTreeRef = ref()
const roles = ref<Role[]>([])
const currentRole = ref<Role | null>(null)
const menuPermissionTree = ref<MenuPermissionNode[]>([])

// 角色树数据（将角色列表转为树结构）
const roleTreeData = ref<any[]>([])

// 菜单选择状态（menuId -> boolean）
const menuSelections = reactive<Record<number, boolean>>({})

// 按钮选择状态（menuId -> buttonCode[]）
const buttonSelections = reactive<Record<number, string[]>>({})

// 全选状态
const selectAll = ref(false)
const isIndeterminate = ref(false)

// 加载角色列表
const loadRoles = async () => {
  try {
    roles.value = await getAllRoles()
    // 将角色列表转为树结构（单层）
    roleTreeData.value = roles.value.map(role => ({
      id: role.id,
      name: role.name,
      code: role.code,
      children: []
    }))
  } catch (error) {
    console.error('加载角色列表失败:', error)
    ElMessage.error('加载角色列表失败')
  }
}

// 递归初始化菜单选择状态
const initMenuSelections = (menus: MenuPermissionNode[]) => {
  menus.forEach(menu => {
    if (menu.menuButtons && menu.menuButtons.length > 0) {
      // 初始化按钮选择
      const selectedButtons = menu.menuButtons
        .filter(btn => btn.hasPermission)
        .map(btn => btn.buttonCode)
      buttonSelections[menu.menuId] = selectedButtons
      
      // 初始化菜单选择状态
      menuSelections[menu.menuId] = selectedButtons.length === menu.menuButtons.length && menu.menuButtons.length > 0
    }
    
    // 递归处理子菜单
    if (menu.children && menu.children.length > 0) {
      initMenuSelections(menu.children)
    }
  })
}

// 加载菜单权限数据
const loadMenuPermissions = async (roleId: number) => {
  loading.value = true
  try {
    console.log('=== 加载菜单权限 ===');
    console.log('roleId:', roleId);
    
    menuPermissionTree.value = await getMenuPermissionTree(roleId)
    
    console.log('API返回的menuPermissionTree:', menuPermissionTree.value);
    
    // 初始化选择状态
    initMenuSelections(menuPermissionTree.value)
    
    console.log('initMenuSelections后的buttonSelections:', buttonSelections);
    console.log('initMenuSelections后的menuSelections:', menuSelections);
    console.log('==================');
    
    // 更新全选状态
    updateSelectAllState()
  } catch (error) {
    console.error('加载菜单权限失败:', error)
    ElMessage.error('加载菜单权限失败')
  } finally {
    loading.value = false
  }
}

// 角色点击事件
const handleRoleClick = (data: any) => {
  const role = roles.value.find(r => r.id === data.id)
  if (role) {
    currentRole.value = role
    loadMenuPermissions(role.id)
  }
}

// 菜单选择改变
const handleMenuSelectChange = (menu: MenuPermissionNode) => {
  const isSelected = menuSelections[menu.menuId]
  
  // 如果菜单有按钮，则切换按钮选择状态
  if (menu.menuButtons && menu.menuButtons.length > 0) {
    if (isSelected) {
      // 全选该菜单下所有按钮
      buttonSelections[menu.menuId] = menu.menuButtons.map(btn => btn.buttonCode)
    } else {
      // 取消全选
      buttonSelections[menu.menuId] = []
    }
  }
  
  // 如果菜单有子菜单，则递归切换子菜单的选择状态
  if (menu.children && menu.children.length > 0) {
    const toggleChildren = (children: MenuPermissionNode[], selected: boolean) => {
      children.forEach(child => {
        if (child.menuButtons && child.menuButtons.length > 0) {
          menuSelections[child.menuId] = selected
          buttonSelections[child.menuId] = selected ? child.menuButtons.map(btn => btn.buttonCode) : []
        }
        if (child.children && child.children.length > 0) {
          toggleChildren(child.children, selected)
        }
      })
    }
    toggleChildren(menu.children, isSelected)
  }
  
  updateSelectAllState()
}

// 按钮选择改变
const handleButtonChange = (menu: MenuPermissionNode) => {
  const selectedCount = buttonSelections[menu.menuId]?.length || 0
  const totalCount = menu.menuButtons.length
  
  // 更新当前菜单选择状态
  menuSelections[menu.menuId] = selectedCount === totalCount && totalCount > 0
  
  // 更新父菜单的选择状态
  updateParentMenuState(menu.menuId)
  
  updateSelectAllState()
}

// 更新父菜单状态（向上递归）
const updateParentMenuState = (childMenuId: number) => {
  // 在整个树中查找父菜单
  const findParentAndUpdate = (menus: MenuPermissionNode[], targetChildId: number): boolean => {
    for (const menu of menus) {
      // 检查当前菜单的子菜单中是否包含目标菜单
      if (menu.children && menu.children.length > 0) {
        const hasChild = menu.children.some(child => child.menuId === targetChildId)
        
        if (hasChild) {
          // 找到了父菜单，检查所有子菜单的选择状态
          const allChildrenSelected = menu.children.every(child => {
            if (child.menuButtons && child.menuButtons.length > 0) {
              const selectedCount = buttonSelections[child.menuId]?.length || 0
              return selectedCount > 0 || menuSelections[child.menuId]
            }
            return false
          })
          
          const anyChildSelected = menu.children.some(child => {
            if (child.menuButtons && child.menuButtons.length > 0) {
              const selectedCount = buttonSelections[child.menuId]?.length || 0
              return selectedCount > 0 || menuSelections[child.menuId]
            }
            return false
          })
          
          // 如果任何子菜单被选中，父菜单也应该被选中
          if (anyChildSelected) {
            menuSelections[menu.menuId] = allChildrenSelected
          } else {
            menuSelections[menu.menuId] = false
          }
          
          // 继续向上更新父菜单的父菜单
          updateParentMenuState(menu.menuId)
          return true
        }
        
        // 递归查找
        if (findParentAndUpdate(menu.children, targetChildId)) {
          return true
        }
      }
    }
    return false
  }
  
  findParentAndUpdate(menuPermissionTree.value, childMenuId)
}

// 递归全选/取消全选菜单
const toggleAllMenus = (menus: MenuPermissionNode[], selected: boolean) => {
  menus.forEach(menu => {
    if (menu.menuButtons && menu.menuButtons.length > 0) {
      menuSelections[menu.menuId] = selected && menu.menuButtons.length > 0
      buttonSelections[menu.menuId] = selected ? menu.menuButtons.map(btn => btn.buttonCode) : []
    }
    if (menu.children && menu.children.length > 0) {
      toggleAllMenus(menu.children, selected)
    }
  })
}

// 全选改变
const handleSelectAllChange = (value: boolean) => {
  toggleAllMenus(menuPermissionTree.value, value)
  isIndeterminate.value = false
}

// 递归计算总按钮数
const countTotalButtons = (menus: MenuPermissionNode[]): number => {
  return menus.reduce((sum, menu) => {
    let count = menu.menuButtons?.length || 0
    if (menu.children && menu.children.length > 0) {
      count += countTotalButtons(menu.children)
    }
    return sum + count
  }, 0)
}

// 更新全选状态
const updateSelectAllState = () => {
  const totalButtons = countTotalButtons(menuPermissionTree.value)
  const selectedButtons = Object.values(buttonSelections).reduce((sum, buttons) => sum + buttons.length, 0)
  
  if (selectedButtons === 0) {
    selectAll.value = false
    isIndeterminate.value = false
  } else if (selectedButtons === totalButtons) {
    selectAll.value = true
    isIndeterminate.value = false
  } else {
    selectAll.value = false
    isIndeterminate.value = true
  }
}

// 取消
const handleCancel = () => {
  if (currentRole.value) {
    loadMenuPermissions(currentRole.value.id)
  }
}

// 保存
const handleSave = async () => {
  if (!currentRole.value) {
    ElMessage.warning('请先选择角色')
    return
  }

  // 递归收集所有选中的权限
  const collectPermissions = (menus: MenuPermissionNode[]): any[] => {
    const result: any[] = []
    menus.forEach(menu => {
      // 收集当前菜单的按钮权限
      if (menu.menuButtons && menu.menuButtons.length > 0) {
        const selected = buttonSelections[menu.menuId] || []
        if (selected.length > 0) {
          // 将按钮Code转换为按钮ID
          // buttonCode格式: "100001" 或 "Create"
          const buttonIds = selected.map(code => {
            const btn = menu.menuButtons.find(b => b.buttonCode === code)
            
            if (!btn) {
              console.warn(`找不到按钮: ${code} in menu ${menu.menuId}`);
              return 0;
            }
            
            // 尝试从buttonId获取，如果没有则从buttonCode解析数字ID
            if (btn.buttonId) {
              return btn.buttonId;
            }
            
            // buttonCode可能是纯数字字符串（如"100001"）或文本（如"Create"）
            // 如果是纯数字字符串，直接转换
            const numericId = parseInt(btn.buttonCode, 10);
            if (!isNaN(numericId) && numericId > 0) {
              return numericId;
            }
            
            console.error(`无法获取按钮ID: buttonCode=${btn.buttonCode}, buttonId=${btn.buttonId}`, btn);
            return 0;
          }).filter(id => id > 0)
          
          if (buttonIds.length > 0) {
            result.push({
              menuId: menu.menuId,
              buttonIds,
            })
          }
        }
      }
      
      // 递归收集子菜单的权限
      if (menu.children && menu.children.length > 0) {
        const childPermissions = collectPermissions(menu.children)
        result.push(...childPermissions)
        
        // 如果有子菜单选中了权限，且父菜单没有按钮，也要包含父菜单
        if (childPermissions.length > 0 && (!menu.menuButtons || menu.menuButtons.length === 0)) {
          // 父菜单没有按钮，但需要在请求中标识（用空数组）
          const alreadyAdded = result.some(r => r.menuId === menu.menuId)
          if (!alreadyAdded) {
            result.push({
              menuId: menu.menuId,
              buttonIds: []
            })
          }
        }
      }
    })
    return result
  }

  const roleMenus = collectPermissions(menuPermissionTree.value)
  debugger
  console.log('=== 保存权限配置 DEBUG ===');
  console.log('buttonSelections:', buttonSelections);
  console.log('menuPermissionTree:', menuPermissionTree.value);
  console.log('collected roleMenus:', roleMenus);
  console.log('========================');

  // 检查是否有选中的按钮（而不是检查roleMenus数组）
  const totalSelectedButtons = Object.values(buttonSelections).reduce((sum, buttons) => sum + buttons.length, 0)
  
  if (totalSelectedButtons === 0) {
    ElMessage.warning('请至少选择一个按钮权限')
    return
  }

  console.log('保存权限配置:', {
    roleId: currentRole.value.id,
    roleMenus,
    totalSelectedButtons,
  })

  saving.value = true
  try {
    await setRoleMenuAuth({
      roleId: currentRole.value.id,
      roleMenus,
    })
    
    ElMessage.success(`已为角色【${currentRole.value.name}】保存权限配置`)
    
    // 重新加载权限数据
    await loadMenuPermissions(currentRole.value.id)
  } catch (error: any) {
    console.error('保存失败:', error)
    ElMessage.error(error.message || '保存失败')
  } finally {
    saving.value = false
  }
}

onMounted(() => {
  loadRoles()
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

    // 左侧角色树 - 与菜单管理样式一致
    .role-tree-panel {
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

    // 右侧菜单权限配置
    .menu-permission-panel {
      flex: 1;
      display: flex;
      flex-direction: column;
      overflow: hidden;

      .empty-state {
        height: 100%;
        display: flex;
        align-items: center;
        justify-content: center;
      }

      .permission-content {
        flex: 1;
        display: flex;
        flex-direction: column;
        overflow: hidden;

        // 工具栏
        .toolbar {
          display: flex;
          justify-content: space-between;
          align-items: center;
          margin-bottom: 16px;
          padding-bottom: 12px;
          border-bottom: 1px solid #e4e7ed;

          :deep(.el-checkbox) {
            font-size: 15px;
            font-weight: 500;

            .el-checkbox__label {
              color: #303133;
            }
          }

          .action-buttons {
            display: flex;
            gap: 12px;
          }
        }

        // 菜单树列表
        .menu-tree-list {
          flex: 1;
          overflow-y: auto;
          overflow-x: hidden;
          padding-right: 4px;

          // 菜单权限项
          .menu-permission-item {
            // 父级菜单（没有按钮只有子菜单）
            .parent-menu {
              margin-bottom: 8px;

              .parent-menu-header {
                :deep(.el-checkbox) {
                  font-weight: 600;
                  font-size: 15px;
                  color: #303133;

                  .el-checkbox__label {
                    .parent-menu-name {
                      color: #303133;
                    }
                  }
                }
              }
            }

            // 菜单项（包含按钮）
            .menu-item {
              margin-bottom: 12px;
              padding: 0;
              background-color: transparent;

              // 菜单标题
              .menu-header {
                margin-bottom: 8px;

                :deep(.el-checkbox) {
                  font-weight: 500;
                  font-size: 14px;

                  .el-checkbox__label {
                    .menu-name {
                      color: #303133;
                    }
                  }
                }
              }

              // 按钮权限区域
              .button-permissions {
                .button-checkbox-group {
                  display: grid;
                  grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
                  gap: 8px;
                  margin-bottom: 8px;

                  :deep(.el-checkbox) {
                    margin-right: 0;
                    padding: 6px 12px;
                    background-color: #f0f9ff;
                    border: 1px solid #bfdbfe;
                    border-radius: 4px;
                    transition: all 0.3s;

                    &:hover {
                      border-color: #409eff;
                      background-color: #dbeafe;
                    }

                    &.is-checked {
                      border-color: #409eff;
                      background-color: #dbeafe;
                      font-weight: 500;
                    }

                    .el-checkbox__label {
                      font-size: 13px;
                      color: #1e40af;
                    }
                  }
                }

                .no-buttons {
                  text-align: center;
                  padding: 12px;
                  color: #909399;
                  font-size: 13px;
                  background-color: #f5f7fa;
                  border-radius: 4px;
                }
              }
            }

            // 分割线样式
            :deep(.el-divider) {
              margin: 16px 0;
              border-color: #e5e7eb;
            }
          }
        }
      }
    }
  }
}
</style>
