<template>
  <nav class="navigation" :class="{ 'mobile-open': isMobileMenuOpen }">
    <div class="nav-overlay" @click="closeMobileMenu"></div>
    <div class="nav-content">
      <ul>
        <li><router-link to="/" @click="closeMobileMenu">{{ t('nav.home') }}</router-link></li>
        <li><router-link to="/about" @click="closeMobileMenu">{{ t('nav.about') }}</router-link></li>
        <li><router-link to="/skills" @click="closeMobileMenu">{{ t('nav.skills') }}</router-link></li>
        <li><router-link to="/projects" @click="closeMobileMenu">{{ t('nav.projects') }}</router-link></li>
        <li><router-link to="/contact" @click="closeMobileMenu">{{ t('nav.contact') }}</router-link></li>
      </ul>
      <div class="mobile-close" @click="closeMobileMenu">
        <span></span>
        <span></span>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const isMobileMenuOpen = defineModel<boolean>('isMobileMenuOpen', { required: true })

const closeMobileMenu = () => {
  isMobileMenuOpen.value = false
}
</script>

<style scoped lang="scss">
.navigation {
  .nav-overlay {
    display: none;
  }

  .nav-content {
    ul {
      display: flex;
      list-style: none;

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

    .mobile-close {
      display: none;
    }
  }
}

// 移动端菜单样式
@media (max-width: 768px) {
  .navigation {
    &.mobile-open {
      .nav-overlay {
        display: block;
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        z-index: 999;
      }

      .nav-content {
        transform: translateX(0);
      }
    }

    .nav-content {
      position: fixed;
      top: 0;
      right: 0;
      width: 250px;
      height: 100vh;
      background: #1a5fb4;
      z-index: 1000;
      padding: 60px 20px 20px;
      transform: translateX(100%);
      transition: transform 0.3s ease;
      box-shadow: -2px 0 5px rgba(0,0,0,0.1);

      ul {
        flex-direction: column;

        li {
          margin: 0 0 1.5rem 0;

          a {
            display: block;
            padding: 0.5rem 0;
            font-size: 1.1rem;
          }
        }
      }

      .mobile-close {
        display: block;
        position: absolute;
        top: 20px;
        right: 20px;
        width: 30px;
        height: 30px;
        cursor: pointer;

        span {
          position: absolute;
          top: 50%;
          left: 50%;
          width: 100%;
          height: 3px;
          background: white;
          transform-origin: center;
          
          &:first-child {
            transform: translate(-50%, -50%) rotate(45deg);
          }
          
          &:last-child {
            transform: translate(-50%, -50%) rotate(-45deg);
          }
        }
      }
    }
  }
}
</style>