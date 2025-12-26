import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  base: '/' //部署配置，当前部署在容器中并且通过域名zhenglijun.com/website来访问，如果使用子域名则不需要配置
})

 