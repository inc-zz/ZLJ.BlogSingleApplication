import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { UserInfo } from '@/types'
import { storage, STORAGE_KEYS } from '@/utils/storage'

export const useUserStore = defineStore('user', () => {
  // 状态
  const token = ref<string>(storage.get(STORAGE_KEYS.TOKEN) || '')
  const refreshToken = ref<string>(storage.get('admin_refresh_token') || '')
  const userInfo = ref<UserInfo | null>(storage.get(STORAGE_KEYS.USER_INFO))

  // 设置token
  const setToken = (newToken: string, newRefreshToken?: string) => {
    token.value = newToken
    storage.set(STORAGE_KEYS.TOKEN, newToken)
    
    if (newRefreshToken) {
      refreshToken.value = newRefreshToken
      storage.set('admin_refresh_token', newRefreshToken)
    }
  }

  // 设置用户信息
  const setUserInfo = (info: UserInfo) => {
    userInfo.value = info
    storage.set(STORAGE_KEYS.USER_INFO, info)
  }

  // 登出
  const logout = () => {
    token.value = ''
    refreshToken.value = ''
    userInfo.value = null
    storage.remove(STORAGE_KEYS.TOKEN)
    storage.remove('admin_refresh_token')
    storage.remove(STORAGE_KEYS.USER_INFO)
  }

  return {
    token,
    refreshToken,
    userInfo,
    setToken,
    setUserInfo,
    logout,
  }
})
