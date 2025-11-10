<template>
  <div class="tags-page">
    <el-card>
      <div class="toolbar">
        <el-button type="primary" @click="handleCreate">新建标签</el-button>
      </div>

      <CommonTable
        :data="tableData"
        :total="total"
        :loading="loading"
        @page-change="handlePageChange"
      >
        <el-table-column prop="name" label="名称" />
        <el-table-column prop="color" label="颜色" width="150">
          <template #default="{ row }">
            <el-tag :color="row.color">{{ row.name }}</el-tag>
          </template>
        </el-table-column>
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

    <CommonDialog
      v-model="dialogVisible"
      :title="dialogTitle"
      @confirm="handleConfirm"
    >
      <el-form ref="formRef" :model="form" :rules="rules" label-width="80px">
        <el-form-item label="名称" prop="name">
          <el-input v-model="form.name" placeholder="请输入标签名称" />
        </el-form-item>
        <el-form-item label="颜色" prop="color">
          <el-color-picker v-model="form.color" />
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
import { getTagList, createTag, updateTag, deleteTag } from '@/api/tag'
import type { Tag } from '@/types'

const loading = ref(false)
const tableData = ref([])
const total = ref(0)
const dialogVisible = ref(false)
const dialogTitle = ref('新建标签')
const formRef = ref<FormInstance>()
const isEdit = ref(false)

const form = reactive<Partial<Tag>>({
  name: '',
  color: '#409eff',
})

const rules: FormRules = {
  name: [{ required: true, message: '请输入标签名称', trigger: 'blur' }],
}

const handleCreate = () => {
  isEdit.value = false
  dialogTitle.value = '新建标签'
  Object.assign(form, { name: '', color: '#409eff' })
  dialogVisible.value = true
}

const handleEdit = (row: Tag) => {
  isEdit.value = true
  dialogTitle.value = '编辑标签'
  Object.assign(form, row)
  dialogVisible.value = true
}

const handleDelete = (row: Tag) => {
  ElMessageBox.confirm('确定要删除这个标签吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    await deleteTag(row.id)
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
          await updateTag(form.id!, form)
          ElMessage.success('更新成功')
        } else {
          await createTag(form)
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

const handlePageChange = (page: number, pageSize: number) => {
  loadData(page, pageSize)
}

const loadData = async (page = 1, pageSize = 10) => {
  loading.value = true
  try {
    const res = await getTagList({ page, pageSize })
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
.tags-page {
  .toolbar {
    margin-bottom: 20px;
  }
}
</style>
