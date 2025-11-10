<template>
  <div class="categories-page">
    <el-card>
      <div class="toolbar">
        <el-button type="primary" @click="handleCreate">新建分类</el-button>
      </div>

      <CommonTable
        :data="tableData"
        :total="total"
        :loading="loading"
        @page-change="handlePageChange"
      >
        <el-table-column prop="name" label="名称" />
        <el-table-column prop="description" label="描述" />
        <el-table-column prop="sort" label="排序" width="100" />
        <el-table-column prop="createTime" label="创建时间" width="180" />
        <el-table-column label="操作" width="200" fixed="right">
          <template #default="{ row }">
            <el-button link type="primary" size="small" @click="handleEdit(row)">
              编辑
            </el-button>
            <el-button link type="danger" size="small" @click="handleDelete(row)">
              删除
            </el-button>
          </template>
        </el-table-column>
      </CommonTable>
    </el-card>

    <!-- 编辑弹窗 -->
    <CommonDialog
      v-model="dialogVisible"
      :title="dialogTitle"
      @confirm="handleConfirm"
      @cancel="handleDialogCancel"
    >
      <el-form ref="formRef" :model="form" :rules="rules" label-width="80px">
        <el-form-item label="名称" prop="name">
          <el-input v-model="form.name" placeholder="请输入分类名称" />
        </el-form-item>
        <el-form-item label="描述" prop="description">
          <el-input
            v-model="form.description"
            type="textarea"
            :rows="3"
            placeholder="请输入描述"
          />
        </el-form-item>
        <el-form-item label="排序" prop="sort">
          <el-input-number v-model="form.sort" :min="0" />
        </el-form-item>
      </el-form>
    </CommonDialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox, type FormInstance, type FormRules } from 'element-plus'
import CommonTable from '@/components/Table/index.vue'
import CommonDialog from '@/components/Dialog/index.vue'
import { getCategoryList, createCategory, updateCategory, deleteCategory } from '@/api/category'
import type { Category } from '@/types'

const loading = ref(false)
const tableData = ref([])
const total = ref(0)
const dialogVisible = ref(false)
const dialogTitle = ref('新建分类')
const formRef = ref<FormInstance>()
const isEdit = ref(false)

const form = reactive<Partial<Category>>({
  name: '',
  description: '',
  sort: 0,
})

const rules: FormRules = {
  name: [{ required: true, message: '请输入分类名称', trigger: 'blur' }],
}

const handleCreate = () => {
  isEdit.value = false
  dialogTitle.value = '新建分类'
  Object.assign(form, { name: '', description: '', sort: 0 })
  dialogVisible.value = true
}

const handleEdit = (row: Category) => {
  isEdit.value = true
  dialogTitle.value = '编辑分类'
  Object.assign(form, row)
  dialogVisible.value = true
}

const handleDelete = (row: Category) => {
  ElMessageBox.confirm('确定要删除这个分类吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    await deleteCategory(row.id)
    ElMessage.success('删除成功')
    loadData()
  })
}

const handleConfirm = async () => {
  if (!formRef.value) return

  await formRef.value.validate(async (valid) => {
    if (valid) {
      try {
        if (isEdit.value) {
          await updateCategory(form.id!, form)
          ElMessage.success('更新成功')
        } else {
          await createCategory(form)
          ElMessage.success('创建成功')
        }
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

const handlePageChange = (page: number, pageSize: number) => {
  loadData(page, pageSize)
}

const loadData = async (page = 1, pageSize = 10) => {
  loading.value = true
  try {
    const res = await getCategoryList({ page, pageSize })
    tableData.value = res.list
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
.categories-page {
  .toolbar {
    margin-bottom: 20px;
  }
}
</style>
