<template>
  <div>
    <el-card>
      <div class="flex justify-between mb-4">
        <el-input
          v-model="searchTerm"
          placeholder="评论内容查询"
          style="width: 300px"
          @keyup.enter="loadComments"
        />
      </div>

      <el-table :data="comments" border>
        <el-table-column prop="content" label="评论内容" />
        <el-table-column prop="createdBy" label="评论人" />
        <el-table-column prop="createdAt" label="评论时间" />
        <el-table-column prop="likeCount" label="点赞数" />
        <el-table-column label="操作" width="120">
          <template #default="scope">
            <el-button size="small" type="danger" @click="handleDelete(scope.row.id)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>

      <el-pagination
        v-model:current-page="pagination.pageIndex"
        v-model:page-size="pagination.pageSize"
        :total="pagination.total"
        @current-change="loadComments"
        @size-change="loadComments"
      />
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useArticleStore } from '@/stores/articleStore';

const articleStore = useArticleStore();
const { comments, pagination } = storeToRefs(articleStore);

const searchTerm = ref('');

const loadComments = () => {
  articleStore.loadComments(pagination.value.pageIndex, pagination.value.pageSize, searchTerm.value);
};

const handleDelete = (id: number) => {
  ElMessageBox.confirm('确认删除该评论吗？', '提示', {
    type: 'warning',
  }).then(() => {
    articleStore.removeComment(id);
  });
};

onMounted(() => {
  loadComments();
});
</script>