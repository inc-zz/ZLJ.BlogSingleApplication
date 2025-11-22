<template>
  <div class="contact">
    <section class="contact-hero">
      <div class="container container-top" id="containerTop">
       <div class="section-title" :class="'center'">
        <h2 class="title">合作共赢</h2>
        <div class="description">可以通过下面的方式联系我，期待与您的合作！</div>
      </div>
      </div>
    </section>
    <section class="contact-content">
      <div class="container">
        <div class="contact-grid">
          <div class="contact-info">
            <h3>{{ t('contact.info.title') }}</h3>
            <div class="contact-methods">
              <div class="contact-item">
                <i class="contact-icon">📞</i>
                <div class="contact-text">
                  <h4>{{ t('contact.info.phone') }}</h4>
                  <p>17302602302</p>
                </div>
              </div>
              <div class="contact-item">
                <i class="contact-icon">✉️</i>
                <div class="contact-text">
                  <h4>{{ t('contact.info.email') }}</h4>
                  <p>392090057@qq.com</p>
                </div>
              </div>
              <div class="contact-item">
                <i class="contact-icon">💬</i>
                <div class="contact-text">
                  <h4>WhatsApp</h4>
                  <p>+86 17302602302</p>
                </div>
              </div>
              <div class="contact-item">
                <i class="contact-icon">📱</i>
                <div class="contact-text">
                  <h4>WeChat</h4>
                  <p>zlj392090057</p>
                </div>
              </div>
            </div>

            <h3>{{ t('contact.social.title') }}</h3>
            <div class="social-links">
              <a href="https://github.com/inc-zz" target="_blank" rel="noopener" class="social-link">
                <i class="social-icon">🐙</i>
                <span>GitHub</span>
              </a>
              <a href="https://gitee.com/inc-zz" target="_blank" rel="noopener" class="social-link">
                <i class="social-icon">🧱</i>
                <span>Gitee</span>
              </a>
            </div>
          </div>

          <div class="contact-form">
            <h3>{{ t('contact.form.title') }}</h3>
            <form @submit.prevent="submitForm">
              <div class="form-group">
                <label for="name">{{ t('contact.form.name') }}</label>
                <input 
                  type="text" 
                  id="name" 
                  v-model="form.name" 
                  :placeholder="t('contact.form.namePlaceholder')"
                  required
                />
              </div>
              <div class="form-group">
                <label for="email">{{ t('contact.form.email') }}</label>
                <input 
                  type="email" 
                  id="email" 
                  v-model="form.email" 
                  :placeholder="t('contact.form.emailPlaceholder')"
                  required
                />
              </div>
              <div class="form-group">
                <label for="subject">{{ t('contact.form.subject') }}</label>
                <input 
                  type="text" 
                  id="subject" 
                  v-model="form.subject" 
                  :placeholder="t('contact.form.subjectPlaceholder')"
                  required
                />
              </div>
              <div class="form-group">
                <label for="message">{{ t('contact.form.message') }}</label>
                <textarea 
                  id="message" 
                  v-model="form.message" 
                  :placeholder="t('contact.form.messagePlaceholder')"
                  rows="5"
                  required
                ></textarea>
              </div>
              <Button type="submit" variant="primary" :disabled="isSubmitting">
                {{ isSubmitting ? t('contact.form.sending') : t('contact.form.send') }}
              </Button>
            </form>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useI18n } from 'vue-i18n'
import Button from '@/components/ui/Button.vue'

const { t } = useI18n()

// 表单数据
const form = reactive({
  name: '',
  email: '',
  subject: '',
  message: ''
})

const isSubmitting = ref(false)

const submitForm = async () => {
  isSubmitting.value = true
  
  // 模拟提交过程
  await new Promise(resolve => setTimeout(resolve, 1000))
  
  // 重置表单
  form.name = ''
  form.email = ''
  form.subject = ''
  form.message = ''
  
  isSubmitting.value = false
  alert(t('contact.form.success'))
}
</script>

<style scoped lang="scss">
#containerTop {
  text-align: center;
  h2{
    font-size: 4rem;
    padding: 3rem;
    color:white;
  }
  .description{
    color:white;
  }
}
.contact {
  
  .contact-hero {
    padding: 5rem 0rem;
    margin-top: -100px;
    background-image: url(/src/assets/contact-top-bg.jpg);
    background-size: 100% 100%;
  }
  .container-top {
    margin-top:120px; 
    font-size:1.25rem !important;
  }

  .contact-content {
    .contact-grid {
      display: grid; 
      padding:50px 0px;
      grid-template-columns: 1fr 1fr;
      gap: 3rem;

      .contact-info {
        h3 {
          color: white;
          margin: 0 0 1.5rem 0;
        }

        .contact-methods {
          margin-bottom: 2rem;

          .contact-item {
            display: flex;
            align-items: flex-start;
            margin-bottom: 1.5rem;

            .contact-icon {
              font-size: 1.5rem;
              margin-right: 1rem;
              min-width: 40px;
            }

            .contact-text {
              h4 {
                margin: 0 0 0.25rem 0;
                color: white;
              }

              p {
                margin: 0;
                color: #666;
              }
            }
          }
        }

        .social-links {
          display: flex;
          gap: 1rem;

          .social-link {
            display: flex;
            align-items: center;
            text-decoration: none;
            color: #1a5fb4;
            padding: 0.5rem 1rem;
            border: 1px solid #1a5fb4;
            border-radius: 4px;
            transition: all 0.3s;

            &:hover {
              background: #1a5fb4;
              color: white;
            }

            .social-icon {
              margin-right: 0.5rem;
              font-size: 1.2rem;
            }
          }
        }
      }

      .contact-form {
        h3 {
          color: #0d3b66;
          margin: 0 0 1.5rem 0;
        }

        form {
          .form-group {
            margin-bottom: 1.5rem;

            label {
              display: block;
              margin-bottom: 0.5rem;
              font-weight: 500;
              color: #333;
            }

            input,
            textarea {
              width: 100%;
              padding: 0.75rem;
              border: 1px solid #ddd;
              border-radius: 4px;
              font-family: inherit;
              font-size: 1rem;
              transition: border-color 0.3s;

              &:focus {
                outline: none;
                border-color: #1a5fb4;
                box-shadow: 0 0 0 2px rgba(26, 95, 180, 0.2);
              }
            }

            textarea {
              resize: vertical;
              min-height: 120px;
            }
          }

          .btn {
            width: 100%;
          }
        }
      }
    }
  }
}

// 响应式设计
@media (max-width: 768px) {
  .contact {
    .contact-content {
      .contact-grid {
        grid-template-columns: 1fr;

        .contact-info,
        .contact-form {
          h3 {
            text-align: center;
          }
        }
      }
    }
  }
} 
</style>