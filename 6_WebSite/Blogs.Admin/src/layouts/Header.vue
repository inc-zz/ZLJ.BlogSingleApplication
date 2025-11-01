<template>
  <div class="header">
    <div class="header-left">
      <!-- 折叠按钮 -->
      <el-icon class="collapse-icon" @click="toggleSidebar">
        <Expand v-if="appStore.sidebarCollapsed" />
        <Fold v-else />
      </el-icon>

      <!-- 面包屑 -->
      <el-breadcrumb separator="/" class="breadcrumb">
        <el-breadcrumb-item :to="{ path: '/' }">
          <el-icon><HomeFilled /></el-icon>
          <span>首页</span>
        </el-breadcrumb-item>
        <el-breadcrumb-item
          v-for="(item, index) in breadcrumbs"
          :key="index"
          :to="item.path ? { path: item.path } : undefined"
        >
          {{ item.title }}
          <el-icon v-if="index < breadcrumbs.length - 1" class="close-icon" @click.stop="closeBreadcrumb(index)">
            <Close />
          </el-icon>
        </el-breadcrumb-item>
      </el-breadcrumb>
    </div>

    <div class="header-right">
      <!-- 用户信息 -->
      <el-dropdown @command="handleCommand">
        <div class="user-info">
          <span class="username">{{ userStore.userInfo?.realName || userStore.userInfo?.userName }}</span>
          <el-icon class="arrow-icon"><ArrowDown /></el-icon>
        </div>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item command="profile">个人中心</el-dropdown-item>
            <el-dropdown-item command="settings">设置</el-dropdown-item>
            <el-dropdown-item divided command="logout">退出登录</el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAppStore } from '@/stores/app'
import { useUserStore } from '@/stores/user'
import { ElMessageBox } from 'element-plus'
import { Expand, Fold, HomeFilled, Close, ArrowDown } from '@element-plus/icons-vue'

const route = useRoute()
const router = useRouter()
const appStore = useAppStore()
const userStore = useUserStore()

const toggleSidebar = () => {
  appStore.toggleSidebar()
}

// 面包屑导航
const breadcrumbs = computed(() => {
  const matched = route.matched.filter((item) => item.meta && item.meta.title)
  const crumbs: Array<{ title: string; path?: string }> = []

  matched.forEach((item, index) => {
    if (index > 0) {
      // 跳过根路由
      crumbs.push({
        title: item.meta?.title as string,
        path: index < matched.length - 1 ? item.path : undefined,
      })
    }
  })

  return crumbs
})

const closeBreadcrumb = (index: number) => {
  // 关闭面包屑标签，返回上一级
  if (index > 0) {
    const targetRoute = route.matched[index]
    if (targetRoute) {
      router.push(targetRoute.path)
    }
  }
}

const handleCommand = (command: string) => {
  switch (command) {
    case 'profile':
      router.push('/profile')
      break
    case 'settings':
      router.push('/system/basic')
      break
    case 'logout':
      ElMessageBox.confirm('确定要退出登录吗？', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
      }).then(() => {
        userStore.logout()
        router.push('/login')
      })
      break
  }
}
</script>

<style scoped lang="scss">
.header {
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 20px;

  .header-left {
    display: flex;
    align-items: center;
    gap: 20px;
    flex: 1;

    .collapse-icon {
      font-size: 20px;
      cursor: pointer;
      transition: color 0.3s;

      &:hover {
        color: #409eff;
      }
    }

    .breadcrumb {
      flex: 1;

      :deep(.el-breadcrumb__item) {
        .el-breadcrumb__inner {
          display: flex;
          align-items: center;
          gap: 5px;

          .close-icon {
            font-size: 14px;
            margin-left: 5px;
            cursor: pointer;
            opacity: 0.6;
            transition: opacity 0.3s;

            &:hover {
              opacity: 1;
              color: #f56c6c;
            }
          }
        }
      }
    }
  }

  .header-right {
    .user-info {
      display: flex;
      align-items: center;
      gap: 8px;
      cursor: pointer;
      padding: 5px 10px;
      border-radius: 4px;
      transition: background-color 0.3s;

      &:hover {
        background-color: #f5f7fa;
      }

      .username {
        font-size: 14px;
        color: #303133;
      }

      .arrow-icon {
        font-size: 12px;
        color: #909399;
      }
    }
  }
}
</style>
