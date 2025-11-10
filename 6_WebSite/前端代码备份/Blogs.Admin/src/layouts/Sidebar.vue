<template>
  <div class="sidebar">
    <!-- Logo -->
    <div class="logo">
      <el-icon><Connection /></el-icon>
      <h2 v-if="!appStore.sidebarCollapsed">博客后台</h2>
    </div>

    <!-- 菜单 -->
    <el-menu
      :default-active="currentRoute"
      :collapse="appStore.sidebarCollapsed"
      :unique-opened="true"
      background-color="#304156"
      text-color="#bfcbd9"
      active-text-color="#409eff"
      router
    >
      <template v-for="route in menuRoutes" :key="route.path">
        <!-- 有子菜单的项 -->
        <el-sub-menu
          v-if="route.children && route.children.length > 0 && !route.meta?.hidden"
          :index="route.path"
        >
          <template #title>
            <el-icon v-if="route.meta?.icon">
              <component :is="route.meta.icon" />
            </el-icon>
            <span>{{ route.meta?.title }}</span>
          </template>
          <el-menu-item
            v-for="child in route.children"
            :key="child.path"
            :index="getFullPath(route.path, child.path)"
            v-show="!child.meta?.hidden"
          >
            {{ child.meta?.title }}
          </el-menu-item>
        </el-sub-menu>

        <!-- 没有子菜单的项 -->
        <el-menu-item
          v-else-if="!route.meta?.hidden"
          :index="route.path"
        >
          <el-icon v-if="route.meta?.icon">
            <component :is="route.meta.icon" />
          </el-icon>
          <template #title>{{ route.meta?.title }}</template>
        </el-menu-item>
      </template>
    </el-menu>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAppStore } from '@/stores/app'
import { Connection } from '@element-plus/icons-vue'

const route = useRoute()
const router = useRouter()
const appStore = useAppStore()

const currentRoute = computed(() => route.path)

const menuRoutes = computed(() => {
  const mainRoute = router.getRoutes().find((r) => r.path === '/')
  return mainRoute?.children || []
})

// 获取完整路径
const getFullPath = (parentPath: string, childPath: string) => {
  // 如果子路径已经是完整路径（以/开头），直接返回
  if (childPath.startsWith('/')) {
    return childPath
  }
  // 否则拼接父路径
  const fullPath = `${parentPath}/${childPath}`.replace(/\/+/g, '/')
  return fullPath
}
</script>

<style scoped lang="scss">
.sidebar {
  height: 100%;
  display: flex;
  flex-direction: column;

  .logo {
    height: 60px;
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #2b3a4a;
    color: #fff;

    h2 {
      font-size: 20px;
      font-weight: bold;
    }
  }

  .el-menu {
    border-right: none;
    flex: 1;
    overflow-y: auto;
  }
}
</style>
