<template>
  <div class="settings-page">
    <el-card header="系统设置">
      <el-form :model="form" label-width="120px">
        <el-form-item label="网站名称">
          <el-input v-model="form.siteName" />
        </el-form-item>
        <el-form-item label="网站描述">
          <el-input v-model="form.siteDescription" type="textarea" :rows="3" />
        </el-form-item>
        <el-form-item label="每页显示数量">
          <el-input-number v-model="form.pageSize" :min="10" :max="100" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSave">保存设置</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import { ElMessage } from 'element-plus'

const form = reactive({
  siteName: '博客管理系统',
  siteDescription: '这是一个博客管理系统',
  pageSize: 10,
})

const handleSave = () => {
  ElMessage.success('保存成功')
}
</script>
