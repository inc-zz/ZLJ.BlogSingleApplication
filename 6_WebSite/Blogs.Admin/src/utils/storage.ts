// 存储键名常量
export const STORAGE_KEYS = {
  TOKEN: 'admin_token',
  USER_INFO: 'admin_user_info',
  SETTINGS: 'admin_settings',
}

// LocalStorage 封装
export const storage = {
  set(key: string, value: any): void {
    try {
      localStorage.setItem(key, JSON.stringify(value))
    } catch (error) {
      console.error('LocalStorage set error:', error)
    }
  },

  get<T = any>(key: string): T | null {
    try {
      const value = localStorage.getItem(key)
      return value ? JSON.parse(value) : null
    } catch (error) {
      console.error('LocalStorage get error:', error)
      return null
    }
  },

  remove(key: string): void {
    try {
      localStorage.removeItem(key)
    } catch (error) {
      console.error('LocalStorage remove error:', error)
    }
  },

  clear(): void {
    try {
      localStorage.clear()
    } catch (error) {
      console.error('LocalStorage clear error:', error)
    }
  },
}

// SessionStorage 封装
export const sessionStorage = {
  set(key: string, value: any): void {
    try {
      window.sessionStorage.setItem(key, JSON.stringify(value))
    } catch (error) {
      console.error('SessionStorage set error:', error)
    }
  },

  get<T = any>(key: string): T | null {
    try {
      const value = window.sessionStorage.getItem(key)
      return value ? JSON.parse(value) : null
    } catch (error) {
      console.error('SessionStorage get error:', error)
      return null
    }
  },

  remove(key: string): void {
    try {
      window.sessionStorage.removeItem(key)
    } catch (error) {
      console.error('SessionStorage remove error:', error)
    }
  },

  clear(): void {
    try {
      window.sessionStorage.clear()
    } catch (error) {
      console.error('SessionStorage clear error:', error)
    }
  },
}
