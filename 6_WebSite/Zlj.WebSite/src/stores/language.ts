import { defineStore } from 'pinia'

export const useLanguageStore = defineStore('language', {
  state: () => ({
    currentLanguage: 'zh' as 'zh' | 'en'
  }),
  
  actions: {
    toggleLanguage() {
      this.currentLanguage = this.currentLanguage === 'zh' ? 'en' : 'zh'
    },
    
    setLanguage(language: 'zh' | 'en') {
      this.currentLanguage = language
    }
  }
})