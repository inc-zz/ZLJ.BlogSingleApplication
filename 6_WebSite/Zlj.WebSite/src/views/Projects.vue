<template>
  <div class="projects">
    <!--顶部描述-->
    <!-- <section class="projects-hero">
      <div class="container">
        <SectionTitle 
          :title="t('projects.title')" 
          :subtitle="t('projects.subtitle')" 
          :description="t('projects.description')"
          :center="true"
        />
      </div>
    </section> -->

    <section class="personal-projects">
      <div class="container">
        <SectionTitle 
          :title="t('projects.personal.title')" 
          :description="t('projects.personal.description')"
        />
        <div class="projects-grid">
          <Card 
            v-for="project in tm('projects.personal.list')" 
            :key="project.id"
            :title="project.name"
            :content="project.description"
            :image="project.image"
            class="project-card"
          >
            <template #default>
              <div class="project-details">
                <div class="project-meta">
                  <span class="project-tech">{{ project.technologies.join(', ') }}</span>
                  <span class="project-date">{{ project.date }}</span>
                </div>
                <div class="project-actions">
                  <Button 
                    v-if="project.liveUrl"
                    variant="primary" 
                    size="small" 
                    @click="openUrl(project.liveUrl)"
                  >
                    {{ t('projects.viewLive') }}
                  </Button>
                  <Button 
                    v-if="project.githubUrl"
                    variant="outline" 
                    size="small" 
                    @click="openUrl(project.githubUrl)"
                  >
                    {{ t('projects.viewCode') }}
                  </Button>
                </div>
              </div>
            </template>
          </Card>
        </div>
      </div>
    </section>

    <section class="open-source-projects">
      <div class="container">
        <SectionTitle 
          :title="t('projects.details.title')" 
          :description="t('projects.details.description')"
        />
        
        <!-- 项目详情展示 -->
        <div class="project-details-grid">
          <!-- 业务痛点 -->
          <div class="detail-card pain-points">
            <div class="detail-title">
              <div class="detail-icon">💡</div>
              <h3>{{ t('projects.details.painPoints.title') }}</h3>
            </div>
            <ul>
              <li v-for="(point, index) in tm('projects.details.painPoints.list')" :key="index">{{ point }}</li>
            </ul>
          </div>

          <!-- 技术栈 -->
          <div class="detail-card tech-stack">
            <div class="detail-title">
              <div class="detail-icon">🛠️</div>
              <h3>{{ t('projects.details.techStack.title') }}</h3>
            </div>
            <div class="tech-categories">
              <div class="tech-category" v-for="(category, key) in tm('projects.details.techStack.datas')" :key="key">
                <h4>{{ category.name }}</h4>
                <div class="tech-tags">
                  <span v-for="tech in category.items" :key="tech" class="tech-tag">{{ tech }}</span>
                </div>
              </div>
            </div>
          </div>

          <!-- 未来升级方向 -->
          <div class="detail-card future-plans">
            <div class="detail-title">
              <div class="detail-icon">🚀</div>
              <h3>{{ t('projects.details.futurePlans.title') }}</h3>
            </div>
            <ul>
              <li v-for="(plan, index) in tm('projects.details.futurePlans.list')" :key="index">{{ plan }}</li>
            </ul>
          </div>

          <!-- 部署方案 -->
          <div class="detail-card deployment">
            <div class="detail-title">
              <div class="detail-icon">⚙️</div>
              <h3>{{ t('projects.details.deployment.title') }}</h3>
            </div>
            <div class="deployment-steps">
              <div v-for="(step, index) in deploymentSteps" :key="index" class="deployment-step">
                <div class="step-number">{{ index + 1 }}</div>
                <div class="step-content">
                  <h4>{{ step.title }}</h4>
                  <p>{{ step.description }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useI18n } from 'vue-i18n'
import SectionTitle from '@/components/ui/SectionTitle.vue'
import Card from '@/components/ui/Card.vue'
import Button from '@/components/ui/Button.vue'

const { t,tm } = useI18n()
 
// 部署方案
const deploymentSteps = ref([
  {
    title: '容器化构建',
    description: '使用Docker将前后端应用容器化，确保开发、测试、生产环境一致性'
  },
  {
    title: 'CI/CD自动化',
    description: '通过Jenkins实现自动化构建、测试和部署流程，提高发布效率'
  },
  {
    title: '负载均衡',
    description: '使用Nginx作为反向代理服务器，实现负载均衡和高可用性'
  },
  {
    title: '云服务部署',
    description: '部署在AWS云平台，利用ECS、RDS等服务实现弹性伸缩和数据备份'
  },
  {
    title: '监控与日志',
    description: '集成Prometheus和ELK栈，实现系统监控、日志收集和性能分析'
  }
])

const openUrl = (url: string) => {
  window.open(url, '_blank')
}
</script>

<style scoped lang="scss">
.projects {
  .projects-hero {
    padding: 5rem 0;
    background: linear-gradient(135deg, #f0f8ff 0%, #e6f7ff 100%);
  }

  .personal-projects {
    padding: 3rem 0;

    .projects-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
      gap: 1.5rem;
      margin-top: 2rem;

      .project-card {
        .project-details {
          .project-meta {
            display: flex;
            justify-content: space-between;
            margin-bottom: 1rem;
            font-size: 0.9rem;
            color: #666;

            .project-tech {
              font-weight: 500;
            }

            .project-date {
              font-style: italic;
            }
          }

          .project-actions {
            display: flex;
            gap: 0.5rem;
          }
        }
      }
    }
  }

  .open-source-projects {
    padding: 4rem 0;
    background-color: #f8f9fa;

    .project-details-grid {
      display: grid;
      grid-template-columns: 1fr;
      gap: 2.5rem;
      margin-top: 3rem;

      .detail-card {
        background: white;
        border-radius: 12px;
        padding: 2.5rem;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
        transition: all 0.3s ease;

        &:hover {
          transform: translateY(-5px);
          box-shadow: 0 8px 25px rgba(0, 0, 0, 0.12);
        }
        .detail-title{
          .detail-icon {
            font-size: 3rem;
            width:60px;
            display: inline-block;
            margin-bottom: 1rem;
          }
          h3 {
            display: inline-block;
          }
        }

        h3 {
          color: #0d3b66;
          margin: 0 0 1.5rem 0;
          font-size: 1.8rem;
        }

        h4 {
          color: #333;
          margin: 0 0 0.75rem 0;
          font-size: 1.1rem;
        }

        ul {
          list-style: none;
          padding: 0;
          margin: 0;

          li {
            padding: 0.75rem 0;
            padding-left: 1.5rem;
            position: relative;
            color: #555;
            line-height: 1.6;
            font-size: 1rem;

            &::before {
              content: '•';
              position: absolute;
              left: 0;
              color: #1a5fb4;
              font-weight: bold;
              font-size: 1.2rem;
            }
          }
        }

        // 业务痛点 - 强调样式
        &.pain-points {
          background: linear-gradient(135deg, #fff5f5 0%, #ffffff 100%);
          border-left: 4px solid #ef4444;

          h3 {
            color: #ef4444;
            font-size: 2rem;
          }

          ul li {
            font-size: 1.05rem;
            padding: 10px 0 0 0;
            padding-left: 1.5rem;
          }
        }

        // 未来升级方向 - 强调样式
        &.future-plans {
          background: linear-gradient(135deg, #f0f9ff 0%, #ffffff 100%);
          border-left: 4px solid #3b82f6;

          h3 {
            color: #3b82f6;
            font-size: 2rem;
          }

          ul li {
            font-size: 1.05rem;
            padding: 10px 0;
            padding-left: 1.5rem;
          }
        }

        // 技术栈 - 弱化样式
        &.tech-stack {
          padding: 1.5rem 2rem;
          h3 {
            font-size: 1.3rem;
            margin-bottom: 1rem;
          }

          .tech-categories {
            display: flex;
            flex-wrap: wrap;
            gap: 1.5rem;

            .tech-category {
              flex: 1;
              min-width: 200px;

              h4 {
                font-size: 0.95rem;
                margin-bottom: 0.5rem;
                color: #666;
              }

              .tech-tags {
                display: flex;
                flex-wrap: wrap;
                gap: 0.4rem;

                .tech-tag {
                  background: #e5e7eb;
                  color: #374151;
                  padding: 0.3rem 0.6rem;
                  border-radius: 4px;
                  font-size: 0.85rem;
                  font-weight: 400;
                }
              }
            }
          }
        }

        // 部署方案 - 弱化样式
        &.deployment {
          padding: 1.5rem 2rem;

          h3 {
            font-size: 1.3rem;
            margin-bottom: 1rem;
          }

          .deployment-steps {
            display: flex;
            flex-wrap: wrap;
            gap: 1rem;

            .deployment-step {
              flex: 1;
              min-width: 180px;
              display: flex;
              flex-direction: column;
              align-items: center;
              text-align: center;

              .step-number {
                flex-shrink: 0;
                width: 32px;
                height: 32px;
                background: #6b7280;
                color: white;
                border-radius: 50%;
                display: flex;
                align-items: center;
                justify-content: center;
                font-weight: bold;
                font-size: 0.9rem;
                margin-bottom: 0.5rem;
              }

              .step-content {
                flex: 1;

                h4 {
                  margin-bottom: 0.3rem;
                  font-size: 0.9rem;
                  color: #374151;
                }

                p {
                  margin: 0;
                  color: #9ca3af;
                  line-height: 1.4;
                  font-size: 0.8rem;
                }
              }
            }
          }
        }
      }
    }
  }
}

// 响应式设计
@media (max-width: 768px) {
  .projects {
    .personal-projects {
      .projects-grid {
        grid-template-columns: 1fr;
      }
    }

    .open-source-projects {
      .project-details-grid {
        grid-template-columns: 1fr;
      }
    }
  }
}
</style>