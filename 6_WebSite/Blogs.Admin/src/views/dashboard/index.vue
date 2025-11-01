<template>
  <div class="dashboard">
    <el-row :gutter="20">
      <el-col :xs="24" :sm="12" :md="6">
        <el-card class="stat-card">
          <div class="stat-content">
            <el-icon class="stat-icon" color="#409eff">
              <Document />
            </el-icon>
            <div class="stat-info">
              <div class="stat-value">{{ statistics.articleCount }}</div>
              <div class="stat-label">文章总数</div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="12" :md="6">
        <el-card class="stat-card">
          <div class="stat-content">
            <el-icon class="stat-icon" color="#67c23a">
              <User />
            </el-icon>
            <div class="stat-info">
              <div class="stat-value">{{ statistics.userCount }}</div>
              <div class="stat-label">用户数量</div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="12" :md="6">
        <el-card class="stat-card">
          <div class="stat-content">
            <el-icon class="stat-icon" color="#e6a23c">
              <ChatDotRound />
            </el-icon>
            <div class="stat-info">
              <div class="stat-value">{{ statistics.articleCommentCount }}</div>
              <div class="stat-label">评论数量</div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :xs="24" :sm="12" :md="6">
        <el-card class="stat-card">
          <div class="stat-content">
            <el-icon class="stat-icon" color="#f56c6c">
              <View />
            </el-icon>
            <div class="stat-info">
              <div class="stat-value">{{ statistics.articleViewCount }}</div>
              <div class="stat-label">总访问量</div>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <el-row :gutter="20" class="mt-20">
      <el-col :span="24">
        <el-card>
          <template #header>
            <div class="card-header">
              <span>最新文章</span>
              <el-button link type="primary" :icon="Refresh" @click="loadArticles">刷新</el-button>
            </div>
          </template>
          <el-table :data="recentArticles" stripe :loading="articleLoading">
            <el-table-column type="index" label="序号" width="80" />
            <el-table-column prop="title" label="标题" min-width="200" />
            <el-table-column prop="categoryName" label="分类" width="120" />
            <el-table-column prop="tags" label="标签" width="150">
              <template #default="{ row }">
                <el-tag v-if="row.tags" size="small" type="info">{{ row.tags }}</el-tag>
                <span v-else>-</span>
              </template>
            </el-table-column>
            <el-table-column prop="createdBy" label="作者" width="120" />
            <el-table-column prop="viewCount" label="阅读" width="80" />
            <el-table-column prop="likeCount" label="点赞" width="80" />
            <el-table-column prop="commentCount" label="评论" width="80" />
            <el-table-column prop="createdAt" label="发布时间" width="180">
              <template #default="{ row }">
                {{ formatDateTime(row.createdAt) }}
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { Document, User, ChatDotRound, View, Refresh } from '@element-plus/icons-vue'
import { getHomeStatistics, getHomeArticleList, type HomeStatistics, type ArticleItem } from '@/api/home'

const statistics = ref<HomeStatistics>({
  userCount: 0,
  articleCount: 0,
  articleCommentCount: 0,
  articleViewCount: 0,
})

const recentArticles = ref<ArticleItem[]>([])
const articleLoading = ref(false)

const formatDateTime = (dateTime: string): string => {
  if (!dateTime) return '-'
  return dateTime.replace('T', ' ')
}

const loadStatistics = async () => {
  try {
    const data = await getHomeStatistics()
    statistics.value = data
  } catch (error) {
    console.error('加载统计数据失败:', error)
  }
}

const loadArticles = async () => {
  articleLoading.value = true
  try {
    const data = await getHomeArticleList(10)
    recentArticles.value = data?.items || []
  } catch (error) {
    console.error('加载文章列表失败:', error)
    recentArticles.value = []
  } finally {
    articleLoading.value = false
  }
}

onMounted(() => {
  loadStatistics()
  loadArticles()
})
</script>

<style scoped lang="scss">
.dashboard {
  .stat-card {
    margin-bottom: 20px;

    .stat-content {
      display: flex;
      align-items: center;
      gap: 20px;

      .stat-icon {
        font-size: 48px;
      }

      .stat-info {
        .stat-value {
          font-size: 28px;
          font-weight: bold;
          color: #303133;
        }

        .stat-label {
          font-size: 14px;
          color: #909399;
          margin-top: 5px;
        }
      }
    }
  }

  .mt-20 {
    margin-top: 20px;
  }

  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
