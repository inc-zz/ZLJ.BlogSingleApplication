<template>
  <div class="home">
    <!-- 个人简介部分 -->
    <section class="hero-section">
      <div class="container">
        <div class="hero-content">
          <div class="hero-main">
            <div class="hero-avatar">
              <div class="avatar-circle">
                <img src="/src/assets/zhenglijun.jpg" :alt="t('home.hero.imageAlt')" />
              </div>
            </div>
            <div class="hero-info">
              <h1 class="hero-title">{{ t('home.hero.title') }} </h1>
              <p class="hero-subtitle">{{ t('home.hero.subtitle') }}</p>
              <p class="hero-description">{{ t('home.hero.description') }}</p>
            </div>
          </div>
          
          <!-- 核心能力卡片 -->
          <div class="abilities-grid">
            <div class="ability-card">
              <div class="ability-icon">{{ t('home.hero.capability.fullStack.icon') }}</div>
              <h3 class="ability-title">{{ t('home.hero.capability.fullStack.title')  }}</h3>
              <p class="ability-desc">{{ t('home.hero.capability.fullStack.content')  }}</p>
            </div> 
             <div class="ability-card">
              <div class="ability-icon">{{ t('home.hero.capability.uidesign.icon') }}</div>
              <h3 class="ability-title">{{ t('home.hero.capability.uidesign.title')  }}</h3>
              <p class="ability-desc">{{ t('home.hero.capability.uidesign.content')  }}</p>
            </div> 
             <div class="ability-card">
              <div class="ability-icon">{{ t('home.hero.capability.optimization.icon') }}</div>
              <h3 class="ability-title">{{ t('home.hero.capability.optimization.title')  }}</h3>
              <p class="ability-desc">{{ t('home.hero.capability.optimization.content')  }}</p>
            </div> 
             <div class="ability-card">
              <div class="ability-icon">{{ t('home.hero.capability.mobileDevelopment.icon') }}</div>
              <h3 class="ability-title">{{ t('home.hero.capability.mobileDevelopment.title')  }}</h3>
              <p class="ability-desc">{{ t('home.hero.capability.mobileDevelopment.content')  }}</p>
            </div> 
          </div>

          <div class="hero-actions">
            <Button variant="primary" size="large" @click="goToContact">
              {{ t('home.hero.contactBtn') }}
            </Button>
            <Button variant="outline" size="large" @click="goToProjects">
              {{ t('home.hero.projectsBtn') }}
            </Button>
          </div>
        </div>
      </div>
    </section>

    <!-- 最新项目展示 -->
    <section class="projects-section">
      <div class="container">
        <SectionTitle 
          :title="t('home.projects.title')" 
          :description="t('home.projects.description')"
          :center="true"
        />
        <div class="projects-grid">
          <Card 
            v-for="project in tm('home.projects.list').slice(0, 3)" 
            :key="project.id"
            :title="project.name"
            :content="project.description"
            :image="project.image"
            class="project-card"
          />
        </div>
        <!--查看更多项目-后期开放-->
        <!-- <div class="projects-actions">
          <Button variant="primary" @click="goToProjects">
            {{ t('home.projects.viewAllBtn') }}
          </Button>
        </div> -->
      </div>
    </section>

    <!-- 合作客户 -->
    <section class="clients-section">
      <div class="container">
        <SectionTitle 
          :title="t('home.clients.title')" 
          :description="t('home.clients.description')"
          :center="true"
        />
        <div class="clients-grid">
          <Card 
            v-for="client in tm('home.clients.list') " 
            :key="client.id"
            :title="client.name"
            :content="client.description"
            :image="client.logo"
            class="client-card"
          />
        </div>
      </div>
    </section>

    <!-- 置顶按钮 -->
    <div class="scroll-to-top" :class="{ 'visible': showScrollTop }" @click="scrollToTop">
      <span>↑</span>
    </div>

    <!-- 联系浮动组件 -->
    <ContactFloat />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import Button from '@/components/ui/Button.vue'
import SectionTitle from '@/components/ui/SectionTitle.vue'
import Card from '@/components/ui/Card.vue'
import ContactFloat from '@/components/home/ContactFloat.vue'

//对象数据使用t，集合数据使用tm
const { t, tm } = useI18n()
const router = useRouter()

// 滚动到顶部功能
const showScrollTop = ref(false)

const handleScroll = () => {
  showScrollTop.value = window.scrollY > 300
}

const scrollToTop = () => {
  window.scrollTo({
    top: 0,
    behavior: 'smooth'
  })
}

onMounted(() => {
  window.addEventListener('scroll', handleScroll)
})

onUnmounted(() => {
  window.removeEventListener('scroll', handleScroll)
})

const goToContact = () => {
  router.push('/contact')
}

const goToProjects = () => {
  router.push('/projects')
}
</script>

<style scoped lang="scss">
.home {
  .hero-section {
    background: linear-gradient(135deg, 
      #00ff88 0%,
      #00ccff 33%,
      #0099ff 66%,
      #9966ff 100%);
    color: white;
    padding: 6rem 0 4rem;
    min-height: 100vh;
    display: flex;
    align-items: center;
    position: relative;
    overflow: hidden;

    &::before {
      content: '';
      position: absolute;
      top: -50%;
      left: -50%;
      width: 200%;
      height: 200%;
      background: radial-gradient(circle at 20% 20%, rgba(0, 255, 136, 0.3) 0%, transparent 50%),
                  radial-gradient(circle at 80% 80%, rgba(153, 102, 255, 0.3) 0%, transparent 50%);
      animation: gradientShift 15s ease-in-out infinite;
    }

    @keyframes gradientShift {
      0%, 100% {
        transform: translate(0, 0);
      }
      50% {
        transform: translate(-5%, -5%);
      }
    }

    .container {
      position: relative;
      z-index: 1;
    }

    .hero-content {
      .hero-main {
        display: flex;
        align-items: center;
        gap: 3rem;
        margin-bottom: 3rem;

        .hero-avatar {
          flex-shrink: 0;

          .avatar-circle {
            width: 180px;
            height: 180px;
            border-radius: 50%;
            overflow: hidden;
            border: 5px solid rgba(255, 255, 255, 0.3);
            box-shadow: 0 15px 35px rgba(0, 0, 0, 0.2);

            img {
              width: 100%;
              height: 100%;
              object-fit: cover;
            }
          }
        }

        .hero-info {
          flex: 1;

          .hero-title {
            font-size: 3.5rem;
            margin: 0 0 1rem 0;
            line-height: 1.2;
            font-weight: 700;
            text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
          }

          .hero-subtitle {
            font-size: 1.8rem;
            margin: 0 0 1.5rem 0;
            opacity: 0.95;
            font-weight: 500;
          }

          .hero-description {
            font-size: 1.2rem;
            margin: 0;
            line-height: 1.8;
            opacity: 0.9;
          }
        }
      }

      .abilities-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
        gap: 1.5rem;
        margin-bottom: 3rem;

        .ability-card {
          background: rgba(255, 255, 255, 0.15);
          backdrop-filter: blur(10px);
          padding: 2rem 1.5rem;
          border-radius: 12px;
          border: 1px solid rgba(255, 255, 255, 0.2);
          transition: all 0.3s ease;

          &:hover {
            transform: translateY(-5px);
            background: rgba(255, 255, 255, 0.2);
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.2);
          }

          .ability-icon {
            font-size: 3rem;
            margin-bottom: 1rem;
          }

          .ability-title {
            font-size: 1.3rem;
            margin: 0 0 0.5rem 0;
            font-weight: 600;
          }

          .ability-desc {
            margin: 0;
            opacity: 0.9;
            font-size: 0.95rem;
            line-height: 1.5;
          }
        }
      }

      .hero-actions {
        display: flex;
        gap: 1.5rem;
        justify-content: center;

        .btn {
          min-width: 160px;
          font-size: 1.1rem;
          box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
        }
      }
    }
  }

  .projects-section {
    padding: 2rem 0;
    // background: linear-gradient(135deg, #9966ff 0%, #ff6b35 50%, #ffd700 100%);

    .projects-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
      gap: 2rem;
      margin: 3rem 0;
    }

    .projects-actions {
      text-align: center;
    }
  }

  .clients-section {
    // padding: 5rem 0;s
    // background: linear-gradient(135deg, #ff69b4 0%, #4169e1 50%, #00fa9a 100%);

    .clients-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 2rem;
      margin-top: 3rem;

      .client-card {
        transition: transform 0.3s ease;
        background: rgba(255, 255, 255, 0.95);
        backdrop-filter: blur(10px);

        &:hover {
          transform: translateY(-8px);
        }
      }
    }
  }

  .scroll-to-top {
    position: fixed;
    bottom: 30px;
    right: 30px;
    width: 50px;
    height: 50px;
    background: linear-gradient(135deg, #00ccff, #9966ff);
    color: white;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    font-size: 1.5rem;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
    opacity: 0;
    visibility: hidden;
    transition: all 0.3s ease;
    z-index: 1000;

    &.visible {
      opacity: 1;
      visibility: visible;
    }

    &:hover {
      transform: translateY(-5px);
      box-shadow: 0 8px 20px rgba(0, 0, 0, 0.4);
    }

    span {
      font-weight: bold;
    }
  }
}

// 响应式设计
@media (max-width: 768px) {
  .home {
    .hero-section {
      padding: 4rem 0 3rem;

      .hero-content {
        .hero-main {
          flex-direction: column;
          text-align: center;
          gap: 2rem;

          .hero-avatar {
            .avatar-circle {
              width: 150px;
              height: 150px;
            }
          }

          .hero-info {
            .hero-title {
              font-size: 2.5rem;
            }

            .hero-subtitle {
              font-size: 1.4rem;
            }

            .hero-description {
              font-size: 1.05rem;
            }
          }
        }

        .abilities-grid {
          grid-template-columns: 1fr;
        }

        .hero-actions {
          flex-direction: column;
          align-items: stretch;

          .btn {
            width: 100%;
          }
        }
      }
    }

    .projects-section,
    .clients-section {
      padding: 0rem !important;

      .projects-grid,
      .clients-grid {
        grid-template-columns: 1fr;
      }
    }
    .scroll-to-top {
      bottom: 20px;
      right: 20px;
      width: 45px;
      height: 45px;
    }
  }
}
</style>