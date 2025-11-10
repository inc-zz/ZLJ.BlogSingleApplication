<template>
  <div class="error-page">
    <div class="error-container">
      <h1 class="error-code">404</h1>
      <p class="error-message">抱歉，您访问的页面不存在</p>
      <el-button type="primary" @click="goHome">返回首页</el-button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'

const router = useRouter()

const goHome = () => {
  router.push('/')
}
</script>

<style scoped lang="scss">
.error-page {
  width: 100%;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #f5f7fa;

  .error-container {
    text-align: center;

    .error-code {
      font-size: 120px;
      font-weight: bold;
      color: #909399;
      margin: 0;
    }

    .error-message {
      font-size: 20px;
      color: #606266;
      margin: 20px 0 40px;
    }
  }
}
</style>
