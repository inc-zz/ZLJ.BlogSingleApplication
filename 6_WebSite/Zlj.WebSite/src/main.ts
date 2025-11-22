import { createApp } from 'vue'
import './styles/main.scss'
import App from './App.vue'
import router from './router'
import { createPinia } from 'pinia'
import i18n from './i18n'

// 初始化 MockJS
import './mock'

const app = createApp(App)
const pinia = createPinia()

app.use(router)
app.use(pinia)
app.use(i18n)

app.mount('#app')