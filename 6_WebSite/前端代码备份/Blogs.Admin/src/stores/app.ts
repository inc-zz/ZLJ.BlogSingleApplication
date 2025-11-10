import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useAppStore = defineStore('app', () => {
  // 侧边栏折叠状态
  const sidebarCollapsed = ref(false)
  
  // 设备类型
  const device = ref<'desktop' | 'mobile'>('desktop')

  // 切换侧边栏
  const toggleSidebar = () => {
    sidebarCollapsed.value = !sidebarCollapsed.value
  }

  // 设置设备类型
  const setDevice = (type: 'desktop' | 'mobile') => {
    device.value = type
  }

  return {
    sidebarCollapsed,
    device,
    toggleSidebar,
    setDevice,
  }
})
