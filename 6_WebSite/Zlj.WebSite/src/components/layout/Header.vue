<template>
  <header class="header">
    <div class="container">
      <div class="header-content">
        <div class="logo">
          <h1>{{ t('website.title') }}</h1>
        </div>
        <nav class="nav-menu" :class="{ 'mobile-open': isMobileMenuOpen }">
          <ul>
            <li><router-link to="/" @click="closeMobileMenu">{{ t('nav.home') }}</router-link></li>
            <li><router-link to="/about" @click="closeMobileMenu">{{ t('nav.about') }}</router-link></li>
            <li><router-link to="/skills" @click="closeMobileMenu">{{ t('nav.skills') }}</router-link></li>
            <li><router-link to="/projects" @click="closeMobileMenu">{{ t('nav.projects') }}</router-link></li>
            <li><router-link to="/contact" @click="closeMobileMenu">{{ t('nav.contact') }}</router-link></li>
          </ul>
        </nav>
        <div class="language-switch">
          <button @click="toggleLanguage" class="lang-btn">
            {{ currentLanguage === 'zh' ? 'EN' : '中文' }}
          </button>
        </div>
        <div class="mobile-menu-toggle" :class="{ 'active': isMobileMenuOpen }" @click="toggleMobileMenu">
          <span></span>
          <span></span>
          <span></span>
        </div>
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { useLanguageStore } from '@/stores/language'

const { t, locale } = useI18n()
const languageStore = useLanguageStore()
const isMobileMenuOpen = ref(false)

const currentLanguage = computed(() => languageStore.currentLanguage)

const toggleLanguage = () => {
  languageStore.toggleLanguage()
  locale.value = languageStore.currentLanguage
}

const toggleMobileMenu = () => {
  isMobileMenuOpen.value = !isMobileMenuOpen.value
}

const closeMobileMenu = () => {
  isMobileMenuOpen.value = false
}
</script>

<style scoped lang="scss">
.header {
  // background: linear-gradient(90deg, #006b3d 0%, #004d8c 50%, #5c2d91 100%);
  // background: linear-gradient(135deg, #9966ff 0%, #ff6b35 50%, #ffd700 100%);
  background: linear-gradient(135deg, #ff69b4 0%, #4169e1 50%, #c1c8c5 100%);
  color: white;
  padding: 1rem 0;
  position: fixed;
  width: 100%;
  top: 0;
  z-index: 1000;
  box-shadow: 0 2px 5px rgba(0,0,0,0.1);

  .container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 1rem;

    .header-content {
      display: flex;
      justify-content: space-between;
      align-items: center;

      .logo h1 {
        margin: 0;
        font-size: 1.5rem;
      }

      .nav-menu {
        ul {
          display: flex;
          list-style: none;
          margin: 0;
          padding: 0;

          li {
            margin-left: 1.5rem;

            a {
              color: white;
              text-decoration: none;
              font-weight: 500;
              transition: color 0.3s;

              &.router-link-active {
                color: #aaffff;
              }

              &:hover {
                color: #aaffff;
              }
            }
          }
        }
      }

      .language-switch {
        .lang-btn {
          background: transparent;
          border: 1px solid white;
          color: white;
          padding: 0.3rem 0.8rem;
          border-radius: 4px;
          cursor: pointer;
          font-weight: 500;
          transition: all 0.3s;

          &:hover {
            background: white;
            color: #1a5fb4;
          }
        }
      }

      .mobile-menu-toggle {
        display: none;
        flex-direction: column;
        cursor: pointer;

        span {
          width: 25px;
          height: 3px;
          background: white;
          margin: 3px 0;
          transition: 0.3s;
        }
      }
    }
  }
}

// 响应式设计
@media (max-width: 768px) {
  .header {
    .container {
      .header-content {
        flex-wrap: wrap;
        position: relative;

        .nav-menu {
          position: absolute;
          top: 100%;
          left: 0;
          right: 0;
          background: linear-gradient(135deg, #ff69b4 0%, #4169e1 50%, #c1c8c5 100%);
          max-height: 0;
          overflow: hidden;
          transition: max-height 0.3s ease;
          box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);

          &.mobile-open {
            max-height: 400px;
          }

          ul {
            flex-direction: column;
            padding: 1rem 0;

            li {
              margin: 0;
              padding: 0.75rem 1rem;
              border-bottom: 1px solid rgba(255, 255, 255, 0.1);

              &:last-child {
                border-bottom: none;
              }

              a {
                display: block;
                padding: 0.5rem 0;
              }
            }
          }
        }

        .mobile-menu-toggle {
          display: flex;

          &.active {
            span:nth-child(1) {
              transform: rotate(45deg) translate(5px, 5px);
            }
            span:nth-child(2) {
              opacity: 0;
            }
            span:nth-child(3) {
              transform: rotate(-45deg) translate(7px, -6px);
            }
          }
        }
      }
    }
  }
}
</style>