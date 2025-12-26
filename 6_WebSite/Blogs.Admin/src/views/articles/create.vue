<template>
  <div class="article-form-container">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>创建文章</span>
          <el-button 
            v-if="!showPreview" 
            type="info" 
            @click="showPreview = true"
          >
            预览
          </el-button>
          <el-button 
            v-else 
            type="primary" 
            @click="showPreview = false"
          >
            编辑
          </el-button>
        </div>
      </template>
      
      <!-- 预览模式 -->
      <div v-if="showPreview" class="preview-container">
        <h1>{{ form.title || '未设置标题' }}</h1>
        <div class="meta-info">
          <el-tag v-if="form.categoryId" type="primary">
            {{ categories.find(c => c.id === form.categoryId)?.name }}
          </el-tag>
          <el-tag 
            v-for="tag in form.tags.split(',').filter(t => t.trim())"
            :key="tag"
            style="margin-left: 8px"
          >
            {{ tag.trim() }}
          </el-tag>
        </div>
        <p class="summary">{{ form.summary }}</p>
        <div class="content" v-html="form.content"></div>
      </div>
      
      <!-- 编辑模式 -->
      <el-form v-else ref="formRef" :model="form" :rules="rules" label-width="100px">
        <el-form-item label="文章标题" prop="title">
          <el-input v-model="form.title" placeholder="请输入文章标题" maxlength="100" show-word-limit />
        </el-form-item>
        
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="文章分类" prop="categoryId">
              <el-select 
                v-model="form.categoryId" 
                placeholder="请选择分类"
                style="width: 100%"
              >
                <el-option
                  v-for="category in categories"
                  :key="category.id"
                  :label="category.name"
                  :value="category.id"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="标签" prop="tags">
              <el-select
                v-model="selectedTags"
                multiple
                filterable
                allow-create
                default-first-option
                placeholder="请选择或输入标签"
                style="width: 100%"
                @change="handleTagsChange"
              >
                <el-option
                  v-for="tag in tagOptions"
                  :key="tag"
                  :label="tag"
                  :value="tag"
                />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        
        <el-form-item label="文章简介" prop="summary">
          <el-input
            v-model="form.summary"
            type="textarea"
            :rows="3"
            placeholder="请输入文章简介，建议不超过200字"
            maxlength="200"
            show-word-limit
          />
        </el-form-item>
        
        <el-form-item label="文章内容" prop="content">
          <div class="editor-wrapper">
            <QuillEditor
              ref="quillEditorRef"
              v-model:content="form.content"
              content-type="text"
              theme="snow"
              :toolbar="toolbarOptions"
              @ready="onEditorReady"
            />
          </div>
        </el-form-item>
        
        <el-form-item label="发布状态" prop="isPublish" style="margin-top: 60px">
          <el-radio-group v-model="form.isPublish">
            <el-radio :value="false">保存为草稿</el-radio>
            <el-radio :value="true">立即发布</el-radio>
          </el-radio-group>
        </el-form-item>
        
        <el-form-item>
          <div class="form-actions">
            <el-button type="primary" :loading="loading" @click="handleSubmit">
              {{ form.isPublish ? '发布文章' : '保存草稿' }}
            </el-button>
            <el-button @click="handleCancel">取消</el-button>
          </div>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage, type FormInstance, type FormRules } from 'element-plus'
import { QuillEditor } from '@vueup/vue-quill'
import '@vueup/vue-quill/dist/vue-quill.snow.css'
import { saveArticle, getArticleCategories, uploadArticleImage, type ArticleFormData, type ArticleCategory } from '@/api/article'

const router = useRouter()
const formRef = ref<FormInstance>()
const quillEditorRef = ref()
const loading = ref(false)
const showPreview = ref(false)
const categories = ref<ArticleCategory[]>([])
const selectedTags = ref<string[]>([])

// 预设标签选项
const tagOptions = ['NetCore', 'Vue', 'React', 'JavaScript', 'Java', 'Go', '其他']

// 富文本编辑器工具栏配置
const toolbarOptions = [
  ['bold', 'italic', 'underline', 'strike'],
  ['blockquote', 'code-block'],
  [{ 'header': 1 }, { 'header': 2 }],
  [{ 'list': 'ordered'}, { 'list': 'bullet' }],
  [{ 'indent': '-1'}, { 'indent': '+1' }],
  [{ 'size': ['small', false, 'large', 'huge'] }],
  [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
  [{ 'color': [] }, { 'background': [] }],
  [{ 'align': [] }],
  ['link', 'image'],
  ['clean']
]

const form = reactive<ArticleFormData>({
  title: '',
  summary: '',
  content: '',
  categoryId: 0,
  tags: '',
  isPublish: false,
})

const rules: FormRules = {
  title: [{ required: true, message: '请输入文章标题', trigger: 'blur' }],
  summary: [{ required: true, message: '请输入文章摘要', trigger: 'blur' }],
  content: [{ required: true, message: '请输入文章内容', trigger: 'blur' }],
  categoryId: [{ required: true, message: '请选择分类', trigger: 'change' }],
}

// 加载分类数据
const loadCategories = async () => {
  try {
    categories.value = await getArticleCategories(10)
  } catch (error) {
    console.error('加载分类失败:', error)
    ElMessage.error('加载分类失败')
  }
}

// 处理标签变化
const handleTagsChange = (tags: string[]) => {
  form.tags = tags.join(',')
}

// 编辑器准备完成
const onEditorReady = (quill: any) => {
  // 监听粘贴事件
  const editor = quill.root
  editor.addEventListener('paste', async (e: ClipboardEvent) => {
    const items = e.clipboardData?.items
    if (!items) return

    // 查找图片项
    for (let i = 0; i < items.length; i++) {
      const item = items[i]
      if (item && item.type.indexOf('image') !== -1) {
        e.preventDefault() // 阻止默认粘贴行为
        
        const file = item.getAsFile()
        if (file) {
          await handleImageUpload(file, quill)
        }
        break
      }
    }
  })
}

// 处理图片上传
const handleImageUpload = async (file: File, quill: any) => {
  try {
    // 显示上传提示
    ElMessage.info('图片上传中...')
    
    // 调用上传接口
    const response = await uploadArticleImage(file)
    
    if (response.success && response.fileUrl) {
      // 获取当前光标位置
      const range = quill.getSelection(true)
      const index = range ? range.index : quill.getLength()
      
      // 在光标位置插入图片 Markdown 格式
      const imageMarkdown = `![image](${response.fileUrl})`
      quill.insertText(index, imageMarkdown, 'user')
      
      // 移动光标到图片后面
      quill.setSelection(index + imageMarkdown.length)
      
      ElMessage.success('图片上传成功')
    } else {
      ElMessage.error('图片上传失败')
    }
  } catch (error) {
    console.error('图片上传失败:', error)
    ElMessage.error('图片上传失败')
  }
}

const handleSubmit = async () => {
  if (!formRef.value) return

  await formRef.value.validate(async (valid) => {
    if (valid) {
      loading.value = true
      try {
        await saveArticle(form)
        ElMessage.success(form.isPublish ? '文章发布成功' : '草稿保存成功')
        router.push('/content/articles')
      } catch (error) {
        console.error('保存失败:', error)
        ElMessage.error('保存失败')
      } finally {
        loading.value = false
      }
    }
  })
}

const handleCancel = () => {
  router.back()
}

onMounted(() => {
  loadCategories()
})
</script>

<style scoped lang="scss">
.article-form-container {
  width: 100%;
  padding: 20px;
  
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
  
  .preview-container {
    padding: 20px;
    
    h1 {
      font-size: 28px;
      margin-bottom: 16px;
      color: #303133;
    }
    
    .meta-info {
      margin-bottom: 20px;
    }
    
    .summary {
      color: #606266;
      font-size: 16px;
      line-height: 1.8;
      margin-bottom: 30px;
      padding: 16px;
      background: #f5f7fa;
      border-left: 4px solid #409eff;
    }
    
    .content {
      font-size: 15px;
      line-height: 1.8;
      color: #303133;
      
      :deep(img) {
        max-width: 100%;
        height: auto;
      }
      
      :deep(pre) {
        background: #f6f8fa;
        padding: 16px;
        border-radius: 4px;
        overflow-x: auto;
      }
      
      :deep(blockquote) {
        border-left: 4px solid #dfe2e5;
        padding-left: 16px;
        color: #6a737d;
      }
    }
  }
  
  :deep(.el-form) {
    max-width: 100%;
  }
  
  .form-actions {
    display: flex;
    justify-content: flex-end;
    gap: 12px;
  }
  
  // 富文本编辑器样式修复
  .editor-wrapper {

    :deep(.el-form-item__content) {
      display: block !important;
    }

    border: 1px solid #dcdfe6;
    border-radius: 4px;
    
    :deep(.ql-toolbar) {
      border: none;
      border-bottom: 1px solid #dcdfe6;
      background: #fafafa;
    }
    
    :deep(.ql-container) {
      border: none;
      min-height: 400px;
      font-size: 14px;
      
      .ql-editor {
        min-height: 400px;
        padding: 12px 15px;
        
        &.ql-blank::before {
          color: #c0c4cc;
          font-style: normal;
        }
      }
    }
  }
}
</style>
