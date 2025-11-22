<template>
  <div class="contact-float">
    <!-- 联系按钮 -->
    <div class="contact-btn" @click="toggleContact">
      <span class="contact-icon">📞</span>
    </div>
    
    <!-- 联系信息卡片 -->
    <transition name="slide-fade">
      <div v-if="showContact" class="contact-card">
        <div class="contact-header">
          <h3>联系我</h3>
          <button class="close-btn" @click="toggleContact">×</button>
        </div>
        
        <div class="contact-info">
          <div class="info-item">
            <span class="icon">📞</span>
            <div class="info-text">
              <label>电话</label>
              <a href="tel:17302602302">17302602302</a>
            </div>
          </div>
          
          <div class="info-item">
            <span class="icon">✉️</span>
            <div class="info-text">
              <label>邮箱</label>
              <a href="mailto:392090057@qq.com">392090057@qq.com</a>
            </div>
          </div>
          
          <div class="info-item">
            <span class="icon">💬</span>
            <div class="info-text">
              <label>QQ</label>
              <span>392090057</span>
            </div>
          </div>
          
          <div class="info-item">
            <span class="icon">📱</span>
            <div class="info-text">
              <label>微信</label>
              <span>zlj392090057</span>
            </div>
          </div>
        </div>
        
        <div class="contact-form">
          <h4>快速留言</h4>
          <form @submit.prevent="submitMessage">
            <input 
              v-model="message.name" 
              type="text" 
              placeholder="您的姓名" 
              required
            />
            <input 
              v-model="message.contact" 
              type="text" 
              placeholder="联系方式" 
              required
            />
            <textarea 
              v-model="message.content" 
              placeholder="留言内容" 
              rows="3"
              required
            ></textarea>
            <button type="submit" class="submit-btn" :disabled="isSubmitting">
              {{ isSubmitting ? '发送中...' : '发送留言' }}
            </button>
          </form>
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const showContact = ref(false)
const isSubmitting = ref(false)

const message = reactive({
  name: '',
  contact: '',
  content: ''
})

const toggleContact = () => {
  showContact.value = !showContact.value
}

const submitMessage = async () => {
  isSubmitting.value = true
  
  try {
    const response = await fetch('http://localhost:5234/api/app/OpenApi/suggest', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        userName: message.name,
        contact: message.contact,
        content: message.content
      })
    })

    const result = await response.json()
    
    if (result.code === 200) {
      alert(t('contact.form.suggestionSuccess'))
      
      // 重置表单
      message.name = ''
      message.contact = ''
      message.content = ''
      
      showContact.value = false
    } else {
      alert(result.message || '提交失败，请稍后重试')
    }
  } catch (error) {
    console.error('提交失败:', error)
    alert('提交失败，请检查网络连接')
  } finally {
    isSubmitting.value = false
  }
}
</script>

<style scoped lang="scss">
.contact-float {
  position: fixed;
  right: 30px;
  top: 50%;
  transform: translateY(-50%);
  z-index: 999;
  
  .contact-btn {
    width: 60px;
    height: 60px;
    background: linear-gradient(135deg, #00ccff, #9966ff);
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    box-shadow: 0 5px 20px rgba(0, 0, 0, 0.3);
    transition: all 0.3s ease;
    
    &:hover {
      transform: scale(1.1);
      box-shadow: 0 8px 25px rgba(0, 0, 0, 0.4);
    }
    
    .contact-icon {
      font-size: 28px;
      animation: ring 2s ease-in-out infinite;
    }
  }
  
  .contact-card {
    position: absolute;
    right: 80px;
    top: 50%;
    transform: translateY(-50%);
    width: 320px;
    background: white;
    border-radius: 12px;
    box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
    overflow: hidden;
    
    .contact-header {
      background: linear-gradient(135deg, #00ccff, #9966ff);
      color: white;
      padding: 1rem 1.5rem;
      display: flex;
      justify-content: space-between;
      align-items: center;
      
      h3 {
        margin: 0;
        font-size: 1.2rem;
      }
      
      .close-btn {
        background: none;
        border: none;
        color: white;
        font-size: 2rem;
        cursor: pointer;
        line-height: 1;
        padding: 0;
        width: 30px;
        height: 30px;
        display: flex;
        align-items: center;
        justify-content: center;
        
        &:hover {
          opacity: 0.8;
        }
      }
    }
    
    .contact-info {
      padding: 1.5rem;
      border-bottom: 1px solid #eee;
      
      .info-item {
        display: flex;
        align-items: flex-start;
        margin-bottom: 1rem;
        
        &:last-child {
          margin-bottom: 0;
        }
        
        .icon {
          font-size: 20px;
          margin-right: 10px;
        }
        
        .info-text {
          flex: 1;
          
          label {
            display: block;
            font-size: 0.85rem;
            color: #666;
            margin-bottom: 2px;
          }
          
          a, span {
            color: #333;
            text-decoration: none;
            font-weight: 500;
            
            &:hover {
              color: #1a5fb4;
            }
          }
        }
      }
    }
    
    .contact-form {
      padding: 1.5rem;
      
      h4 {
        margin: 0 0 1rem 0;
        color: #333;
        font-size: 1rem;
      }
      
      form {
        display: flex;
        flex-direction: column;
        gap: 0.75rem;
        
        input,
        textarea {
          padding: 0.75rem;
          border: 1px solid #ddd;
          border-radius: 6px;
          font-family: inherit;
          font-size: 0.9rem;
          transition: border-color 0.3s;
          
          &:focus {
            outline: none;
            border-color: #1a5fb4;
          }
        }
        
        textarea {
          resize: vertical;
        }
        
        .submit-btn {
          background: linear-gradient(135deg, #00ccff, #9966ff);
          color: white;
          border: none;
          padding: 0.75rem;
          border-radius: 6px;
          font-weight: 500;
          cursor: pointer;
          transition: all 0.3s;
          
          &:disabled {
            opacity: 0.6;
            cursor: not-allowed;
          }
          
          &:hover:not(:disabled) {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
          }
        }
      }
    }
  }
}

// 动画
@keyframes ring {
  0%, 100% {
    transform: rotate(0deg);
  }
  10%, 30% {
    transform: rotate(-15deg);
  }
  20%, 40% {
    transform: rotate(15deg);
  }
  50% {
    transform: rotate(0deg);
  }
}

.slide-fade-enter-active,
.slide-fade-leave-active {
  transition: all 0.3s ease;
}

.slide-fade-enter-from {
  transform: translateY(-50%) translateX(20px);
  opacity: 0;
}

.slide-fade-leave-to {
  transform: translateY(-50%) translateX(20px);
  opacity: 0;
}

// 响应式
@media (max-width: 768px) {
  .contact-float {
    right: 15px;
    bottom: 20px;
    top: auto;
    transform: none;
    
    .contact-btn {
      width: 55px;
      height: 55px;
      
      .contact-icon {
        font-size: 26px;
      }
    }
    
    .contact-card {
      position: fixed;
      right: 0;
      left: 0;
      bottom: 0;
      top: auto;
      transform: none;
      width: 100%;
      max-height: 85vh;
      overflow-y: auto;
      border-radius: 20px 20px 0 0;
      box-shadow: 0 -5px 30px rgba(0, 0, 0, 0.3);
      
      .contact-header {
        padding: 1rem 1.5rem;
        position: sticky;
        top: 0;
        z-index: 1;
        
        h3 {
          font-size: 1.2rem;
        }
      }
      
      .contact-info,
      .contact-form {
        padding: 1.25rem 1.5rem;
      }

      .contact-info {
        .info-item {
          margin-bottom: 1.25rem;
          
          .icon {
            font-size: 20px;
          }
          
          .info-text {
            label {
              font-size: 0.85rem;
            }
            
            a, span {
              font-size: 0.95rem;
            }
          }
        }
      }
    }
  }

  // 移动端底部弹出动画
  .slide-fade-enter-from {
    transform: translateY(100%);
    opacity: 1;
  }

  .slide-fade-leave-to {
    transform: translateY(100%);
    opacity: 1;
  }

  .slide-fade-enter-active,
  .slide-fade-leave-active {
    transition: transform 0.3s ease-out;
  }
}

@media (max-width: 480px) {
  .contact-float {
    right: 10px;
    bottom: 15px;
    
    .contact-btn {
      width: 50px;
      height: 50px;
      
      .contact-icon {
        font-size: 24px;
      }
    }
    
    .contact-card {
      max-height: 90vh;
      
      .contact-header {
        padding: 0.875rem 1.25rem;
        
        h3 {
          font-size: 1.1rem;
        }
      }
      
      .contact-info,
      .contact-form {
        padding: 1rem 1.25rem;
      }

      .contact-form {
        form {
          gap: 0.65rem;
          
          input,
          textarea {
            padding: 0.7rem;
            font-size: 0.9rem;
          }
          
          .submit-btn {
            padding: 0.875rem;
            font-size: 0.95rem;
          }
        }
      }
    }
  }
}
</style>