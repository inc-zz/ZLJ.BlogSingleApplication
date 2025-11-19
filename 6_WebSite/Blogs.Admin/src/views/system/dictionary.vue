<template>
  <div class="dictionary-page">
    <el-card class="search-card">
      <el-form :inline="true" :model="searchForm">
        <el-form-item label="配置项">
          <el-input v-model="searchForm.Name" placeholder="请输入配置项" clearable />
        </el-form-item>
        <el-form-item label="配置模块">
          <el-select v-model="searchForm.Where" placeholder="请选择配置模块" clearable>
            <el-option
              v-for="item in busTypeOptions"
              :key="item.value"
              :label="item.name"
              :value="item.value"
            />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="table-card">
      <div class="toolbar">
        <el-button type="primary" @click="handleCreate">新建配置</el-button>
      </div>

      <el-table :data="tableData" v-loading="loading" border stripe>
        <el-table-column type="index" label="序号" width="80" align="center" />
        <el-table-column prop="title" label="标题" min-width="150" show-overflow-tooltip />
        <el-table-column prop="busType" label="类型" width="120" align="center">
          <template #default="{ row }">
            {{ getBusTypeName(row.busType) }}
          </template>
        </el-table-column>
        <el-table-column prop="tags" label="标签" width="120" align="center" />
        <el-table-column prop="summary" label="简介" min-width="150" show-overflow-tooltip />
        <el-table-column prop="url" label="外部链接" min-width="200" show-overflow-tooltip />
        <el-table-column prop="statusName" label="状态" width="100" align="center">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'danger'">
              {{ row.statusName }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="180" align="center" fixed="right">
          <template #default="{ row }">
            <el-button type="primary" size="small" link @click="handleEdit(row)">编辑</el-button>
            <el-button type="danger" size="small" link @click="handleDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <el-pagination
        v-model:current-page="pagination.pageIndex"
        v-model:page-size="pagination.pageSize"
        :total="total"
        :page-sizes="[10, 20, 50, 100]"
        layout="total, sizes, prev, pager, next, jumper"
        @size-change="handlePageChange"
        @current-change="handlePageChange"
        style="margin-top: 20px; justify-content: flex-end"
      />
    </el-card>

    <!-- 创建/编辑对话框 -->
    <CommonDialog
      v-model="dialogVisible"
      :title="dialogTitle"
      @confirm="handleConfirm"
      @cancel="handleDialogCancel"
    >
      <el-form ref="formRef" :model="form" :rules="rules" label-width="100px">
        <el-form-item label="标题" prop="title">
          <el-input v-model="form.title" placeholder="请输入标题" />
        </el-form-item>
        <el-form-item label="类型" prop="busType">
          <el-select v-model="form.busType" placeholder="请选择类型">
            <el-option
              v-for="item in busTypeOptions"
              :key="item.value"
              :label="item.name"
              :value="item.value"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="标签" prop="tags">
          <el-input v-model="form.tags" placeholder="请输入标签" />
        </el-form-item>
        <el-form-item label="简介" prop="summary">
          <el-input
            v-model="form.summary"
            type="textarea"
            :rows="2"
            placeholder="请输入简介"
          />
        </el-form-item>
        <el-form-item label="外部链接" prop="url">
          <el-input v-model="form.url" placeholder="请输入外部链接" />
        </el-form-item>
        <el-form-item label="内容" prop="content">
          <el-input
            v-model="form.content"
            type="textarea"
            :rows="4"
            placeholder="请输入内容"
          />
        </el-form-item>
      </el-form>
    </CommonDialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox, type FormInstance, type FormRules } from 'element-plus'
import CommonDialog from '@/components/Dialog/index.vue'
import type { PageData } from '@/types'
import {
  getSysConfigList,
  saveSysConfig,
  deleteSysConfig,
  busTypeOptions,
  type SysConfig,
  type SysConfigFormData,
} from '@/api/system'

const loading = ref(false)
const tableData = ref<SysConfig[]>([])
const total = ref(0)
const dialogVisible = ref(false)
const dialogTitle = ref('新建配置')
const formRef = ref<FormInstance>()
const isEdit = ref(false)

const searchForm = reactive({
  Name: '',
  Where: '',
})

const pagination = reactive({
  pageIndex: 1,
  pageSize: 10,
})

const form = reactive<SysConfigFormData>({
  title: '',
  busType: '',
  tags: '',
  summary: '',
  url: '',
  content: '',
})

const rules: FormRules = {
  title: [{ required: true, message: '请输入标题', trigger: 'blur' }],
  busType: [{ required: true, message: '请选择类型', trigger: 'change' }],
}

const getBusTypeName = (value: string) => {
  const item = busTypeOptions.find((opt) => opt.value === value)
  return item?.name || value
}

const handleSearch = () => {
  pagination.pageIndex = 1
  loadData()
}

const handleReset = () => {
  searchForm.Name = ''
  searchForm.Where = ''
  pagination.pageIndex = 1
  loadData()
}

const handleCreate = () => {
  isEdit.value = false
  dialogTitle.value = '新建配置'
  Object.assign(form, {
    title: '',
    busType: '',
    tags: '',
    summary: '',
    url: '',
    content: '',
  })
  dialogVisible.value = true
}

const handleEdit = (row: SysConfig) => {
  isEdit.value = true
  dialogTitle.value = '编辑配置'
  Object.assign(form, {
    id: row.id,
    title: row.title,
    busType: row.busType,
    tags: row.tags,
    summary: row.summary,
    url: row.url,
    content: row.content,
  })
  dialogVisible.value = true
}

const handleDelete = (row: SysConfig) => {
  ElMessageBox.confirm('确定要删除这个配置吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    await deleteSysConfig(row.id)
    ElMessage.success('删除成功')
    loadData()
  })
}

const handleConfirm = async () => {
  if (!formRef.value) return

  await formRef.value.validate(async (valid) => {
    if (valid) {
      try {
        await saveSysConfig(form)
        ElMessage.success(isEdit.value ? '更新成功' : '创建成功')
        dialogVisible.value = false
        loadData()
      } catch (error) {
        console.error('操作失败:', error)
      }
    }
  })
}

const handleDialogCancel = () => {
  formRef.value?.resetFields()
}

const handlePageChange = () => {
  loadData()
}

const loadData = async () => {
  loading.value = true
  try {
    const res = (await getSysConfigList({
      pageIndex: pagination.pageIndex,
      pageSize: pagination.pageSize,
      Name: searchForm.Name,
      Where: searchForm.Where,
    })) as any as PageData<SysConfig>
    tableData.value = res.items
    total.value = res.total
  } catch (error) {
    console.error('加载数据失败:', error)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadData()
})
</script>

<style scoped lang="scss">
.dictionary-page {
  .search-card {
    margin-bottom: 20px;
  }

  .toolbar {
    margin-bottom: 20px;
  }
}
</style>
