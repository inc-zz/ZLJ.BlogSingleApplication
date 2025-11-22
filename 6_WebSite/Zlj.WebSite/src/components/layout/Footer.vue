<template>
  <footer class="footer">
    <div class="container">
      <div class="footer-content">
        <div class="footer-section">
          <h3>{{ t('footer.contact') }}</h3>
          <p>{{ t('footer.phone') }}: 17302602302</p>
          <p>{{ t('footer.email') }}: 392090057@qq.com</p>
          <p>WhatsApp: +86 17302602302</p>
          <p>WeChat: zlj392090057</p>
        </div>
        <div class="footer-section">
          <h3>{{ t('footer.follow') }}</h3>
          <div class="social-links">
            <a href="https://github.com/inc-zz" target="_blank" rel="noopener">{{ t('footer.github') }}</a>
            <a href="https://gitee.com/inc-zz" target="_blank" rel="noopener">{{ t('footer.gitee') }}</a>
          </div>
        </div>
        <div class="footer-section">
          <p class="copyright">{{ t('footer.copyright') }}</p>
          <p class="backupNumber"  v-html="t('footer.backupNumber')">
          </p>
        </div>


      </div>
    </div>
  </footer>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
</script>

<style scoped lang="scss">
.footer {
  background-color: #0d3b66;
  color: white;
  padding: 2rem 0;
  margin-top: auto;

  .container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 1rem;

    .footer-content {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 2rem;

      .footer-section {
        h3 {
          margin-top: 0;
          margin-bottom: 1rem;
          font-size: 1.2rem;
        }

        p {
          margin: 0.5rem 0;
        }

        .social-links {
          display: flex;
          flex-direction: column;
          gap: 0.5rem;

          a {
            color: #aaffff;
            text-decoration: none;
            transition: color 0.3s;

            &:hover {
              color: white;
              text-decoration: underline;
            }
          }
        }

        .copyright {
          margin-top: 1rem;
          opacity: 0.8;
        }
        .backupNumber{
          color:white !important;
        }
      }
    }
  }
}

// 响应式设计
@media (max-width: 768px) {
  .footer {
    .container {
      .footer-content {
        grid-template-columns: 1fr;
        text-align: center;

        .social-links {
          align-items: center;
        }
      }
    }
  }
}
</style>