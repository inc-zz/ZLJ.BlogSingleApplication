<template>
  <div>
    <el-card>
      <div class="flex justify-between mb-4">
        <el-input
          v-model="searchTerm"
          placeholder="分类名称查询"
          style="width: 300px"
          @keyup.enter="loadCategories"
        />
        <el-button type="primary" @click="handleCreate">新增分类</el-button>
      </div>

      <el-table :data="categories" border>
        <el-table-column prop="name" label="分类名称" />
        <el-table-column prop="description" label="描述" />
        <el-table-column prop="sort" label="排序" />
        <el-table-column prop="statusName" label="状态" />
        <el-table-column label="操作" width="180">
          <template #default="scope">
            <el-button size="small" @click="handleEdit(scope.row)">编辑</el-button>
            <el-button size="small" type="danger" @click="handleDelete(scope.row.id)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <el-pagination
        v-model:current-page="pagination.pageIndex"
        v-model:page-size="pagination.pageSize"
        :total="pagination.total"
        @current-change="loadCategories"
        @size-change="loadCategories"
      />
    </el-card>

    <el-dialog v-model="dialogVisible" :title="dialogTitle">
      <el-form :model="form">
        <el-form-item label="分类名称" required>
          <el-input v-model="form.name" />
        </el-form-item>
        <el-form-item label="描述">
          <el-input v-model="form.description" type="textarea" />
        </el-form-item>
        <el-form-item label="排序">
          <el-input-number v-model="form.sort" :min="1" />
        </el-form-item>
        <el-form-item label="图标">
          <el-input v-model="form.icon" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="submitForm">确认</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useArticleStore } from '@/stores/articleStore';

const articleStore = useArticleStore();
const { categories, pagination } = storeToRefs(articleStore);

const searchTerm = ref('');
const dialogVisible = ref(false);
const dialogTitle = ref('新增分类');
const form = ref({
  id: undefined,
  name: '',
  description: '',
  sort: 1,
  icon: '',
});

const loadCategories = () => {
  articleStore.loadCategories(pagination.value.pageIndex, pagination.value.pageSize, searchTerm.value);
};

const handleCreate = () => {
  dialogTitle.value = '新增分类';
  form.value = {
    id: undefined,
    name: '',
    description: '',
    sort: 1,
    icon: '',
  };
  dialogVisible.value = true;
};

const handleEdit = (category: any) => {
  dialogTitle.value = '编辑分类';
  form.value = { ...category };
  dialogVisible.value = true;
};

const handleDelete = (id: number) => {
  ElMessageBox.confirm('确认删除该分类吗？', '提示', {
    type: 'warning',
  }).then(() => {
    articleStore.removeCategory(id);
  });
};

const submitForm = () => {
  articleStore.createOrUpdateCategory(form.value).then(() => {
    dialogVisible.value = false;
    loadCategories();
  });
};

onMounted(() => {
  loadCategories();
});
</script>