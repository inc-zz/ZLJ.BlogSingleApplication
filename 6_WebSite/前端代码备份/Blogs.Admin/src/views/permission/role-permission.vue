<template>
  <div class="role-permission-page">
    <el-card>
      <div class="page-title">角色授权</div>

      <div class="permission-container">
        <!-- 左侧角色列表 -->
        <div class="role-list">
          <div class="list-title">系统角色</div>
          <el-radio-group v-model="selectedRole" class="role-group">
            <el-radio
              v-for="role in roleList"
              :key="role.id"
              :value="role.id"
              class="role-item"
            >
              {{ role.name }}
            </el-radio>
          </el-radio-group>
        </div>

        <!-- 右侧权限配置 -->
        <div class="permission-config">
          <div class="config-title">权限配置</div>

          <div class="permission-content">
            <div
              v-for="module in permissionModules"
              :key="module.name"
              class="permission-module"
            >
              <div class="module-title">{{ module.name }}</div>
              <div class="permission-actions">
                <el-checkbox
                  v-for="action in module.actions"
                  :key="action.key"
                  :label="action.label"
                  v-model="action.checked"
                />
              </div>
            </div>
          </div>

          <div class="permission-footer">
            <el-button type="primary" :icon="Check" @click="handleSave">
              保存设置
            </el-button>
            <el-button @click="handleCancel">取消</el-button>
          </div>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, watch } from 'vue'
import { ElMessage } from 'element-plus'
import { Check } from '@element-plus/icons-vue'

const selectedRole = ref(1)

const roleList = ref([
  { id: 1, name: '超级管理员' },
  { id: 2, name: '操作员' },
  { id: 3, name: '审核员' },
  { id: 4, name: '站点管理员' },
])

const permissionModules = reactive([
  {
    name: '用户管理',
    actions: [
      { key: 'delete', label: '删除 (Delete)', checked: false },
      { key: 'create', label: '创建 (Create)', checked: false },
      { key: 'edit', label: '编辑 (Edit)', checked: false },
      { key: 'auth', label: '授权 (Auth)', checked: false },
      { key: 'setRole', label: '分配角色 (SetRole)', checked: false },
      { key: 'view', label: '查看 (View)', checked: false },
    ],
  },
  {
    name: '内容管理',
    actions: [
      { key: 'delete', label: '删除 (Delete)', checked: false },
      { key: 'create', label: '创建 (Create)', checked: false },
      { key: 'edit', label: '编辑 (Edit)', checked: false },
      { key: 'publish', label: '发布 (Publish)', checked: false },
      { key: 'view', label: '查看 (View)', checked: false },
    ],
  },
])

watch(selectedRole, () => {
  // 加载角色权限
  loadRolePermissions()
})

const loadRolePermissions = () => {
  // TODO: 调用API加载角色权限
  console.log('加载角色权限:', selectedRole.value)
}

const handleSave = () => {
  ElMessage.success('保存成功')
}

const handleCancel = () => {
  // 重置表单
}
</script>

<style scoped lang="scss">
.role-permission-page {
  .page-title {
    font-size: 18px;
    font-weight: bold;
    margin-bottom: 20px;
  }

  .permission-container {
    display: flex;
    gap: 20px;
    min-height: 500px;

    .role-list {
      width: 250px;
      border: 1px solid #dcdfe6;
      border-radius: 4px;
      padding: 15px;

      .list-title {
        font-weight: bold;
        margin-bottom: 15px;
        padding-bottom: 10px;
        border-bottom: 1px solid #dcdfe6;
      }

      .role-group {
        display: flex;
        flex-direction: column;
        gap: 10px;

        .role-item {
          margin: 0;
          padding: 10px;
          border-radius: 4px;
          transition: background-color 0.3s;

          &:hover {
            background-color: #f5f7fa;
          }
        }
      }
    }

    .permission-config {
      flex: 1;
      border: 1px solid #dcdfe6;
      border-radius: 4px;
      padding: 15px;

      .config-title {
        font-weight: bold;
        margin-bottom: 20px;
      }

      .permission-content {
        max-height: 500px;
        overflow-y: auto;

        .permission-module {
          margin-bottom: 30px;

          .module-title {
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 15px;
            color: #303133;
          }

          .permission-actions {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
            gap: 15px;
            padding: 15px;
            background-color: #f5f7fa;
            border-radius: 4px;
          }
        }
      }

      .permission-footer {
        margin-top: 30px;
        padding-top: 20px;
        border-top: 1px solid #dcdfe6;
        text-align: center;
      }
    }
  }
}
</style>
