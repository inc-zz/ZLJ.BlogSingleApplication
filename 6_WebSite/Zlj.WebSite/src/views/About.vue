<template>
  <div class="about">
    <section class="about-hero">
      <div class="container">
        <SectionTitle 
          :title="t('about.title')" 
          :center="false"
        />
        <div class="about-content">
          <!-- 左侧：图片+文本（L型布局） -->
          <div class="about-left">
            <img class="about-image" src="/src/assets/mywork.jpg" :alt="t('about.imageAlt')" />
            <div class="about-text">
              <p>{{ t('about.description') }}</p>
              <p>{{ t('about.passion') }}</p>
            </div>
          </div> 

          <!-- 中间分割线 -->
          <div class="divider"></div>

          <!-- 右侧：六边形兴趣展示 -->
          <div class="about-right">
            <svg viewBox="0 0 500 500" class="hexagon-chart">
              <!-- 六边形背景 -->
              <defs>
                <linearGradient id="codeGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                  <stop offset="0%" style="stop-color:#667eea;stop-opacity:1" />
                  <stop offset="100%" style="stop-color:#764ba2;stop-opacity:1" />
                </linearGradient>
                <linearGradient id="entertainGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                  <stop offset="0%" style="stop-color:#f093fb;stop-opacity:1" />
                  <stop offset="100%" style="stop-color:#f5576c;stop-opacity:1" />
                </linearGradient>
                <linearGradient id="foodGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                  <stop offset="0%" style="stop-color:#ffecd2;stop-opacity:1" />
                  <stop offset="100%" style="stop-color:#fcb69f;stop-opacity:1" />
                </linearGradient>
                <linearGradient id="musicGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                  <stop offset="0%" style="stop-color:#4facfe;stop-opacity:1" />
                  <stop offset="100%" style="stop-color:#00f2fe;stop-opacity:1" />
                </linearGradient>
                <linearGradient id="novelGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                  <stop offset="0%" style="stop-color:#43e97b;stop-opacity:1" />
                  <stop offset="100%" style="stop-color:#38f9d7;stop-opacity:1" />
                </linearGradient>
                <linearGradient id="gameGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                  <stop offset="0%" style="stop-color:#fa709a;stop-opacity:1" />
                  <stop offset="100%" style="stop-color:#fee140;stop-opacity:1" />
                </linearGradient>
              </defs>

              <!-- 六边形外框 -->
              <polygon 
                :points="getHexagonPoints()" 
                fill="none" 
                stroke="#ddd" 
                stroke-width="2"
              />

              <!-- 六个扇形区域 -->
              <g class="hexagon-sections">
                <g 
                  v-for="(interest, index) in interests" 
                  :key="index"
                  @mouseenter="handleMouseEnter(index, $event)"
                  @mouseleave="handleMouseLeave"
                >
                  <!-- 扇形路径 -->
                  <path 
                    :d="getHexagonSectionPath(index)" 
                    :fill="interest.gradient" 
                    class="hex-section"
                    :class="{ 
                      'active': activeSection === index,
                      'hovered': hoveredSection === index 
                    }"
                  />
                  
                  <!-- 图标和文字 -->
                  <text 
                    :x="getSectionLabelPosition(index).x"
                    :y="getSectionLabelPosition(index).y - 15"
                    text-anchor="middle"
                    class="section-icon"
                    :class="{ 'active': activeSection === index }"
                  >
                    {{ interest.icon }}
                  </text>
                  <text 
                    :x="getSectionLabelPosition(index).x"
                    :y="getSectionLabelPosition(index).y + 10"
                    text-anchor="middle"
                    class="section-label"
                    :class="{ 'active': activeSection === index }"
                  >
                    {{ interest.name }}
                  </text>
                </g>
              </g>

              <!-- 中心圆 -->
              <circle cx="250" cy="250" r="70" fill="white" class="center-circle" />
              
              <!-- 中心文字 -->
              <text x="250" y="245" text-anchor="middle" class="center-text">兴趣</text>
              <text x="250" y="265" text-anchor="middle" class="center-subtext">爱好</text>
            </svg>
            
            <!-- 气泡提示框 -->
            <transition name="bubble-fade">
              <div 
                v-if="showBubble" 
                class="interest-bubble"
                :style="{
                  top: bubblePosition.y + 'px',
                  left: bubblePosition.x + 'px'
                }"
              >
                <div class="bubble-arrow"></div>
                <div class="bubble-content">
                  {{ bubbleContent }}
                </div>
              </div>
            </transition>
          </div>
        </div>
      </div>
    </section>
    <section class="experience-section">
      <div class="container">
        <SectionTitle 
          :title="t('about.experience.title')" 
          :description="t('about.experience.description')"
        />
        <div class="timeline">
          <div 
            v-for="(exp, index) in experiences" 
            :key="exp.id"
            class="timeline-item" 
            :class="{ 
              'timeline-item-left': index % 2 === 0,
              'timeline-item-right': index % 2 === 1 
            }" 
          >
            <div class="timeline-marker"></div>
            <div class="timeline-content">
              <div class="timeline-date">{{ exp.date }}</div>
              <h3 class="timeline-title">{{ exp.position }}</h3>
              <h4 class="timeline-company">{{ exp.company }}</h4>
              <p class="timeline-description">{{ exp.description }}</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <section class="education-section">
      <div class="container">
        <SectionTitle 
          :title="t('about.education.title')" 
          :description="t('about.education.description')"
        />
        <div class="education-list">
          <div 
            v-for="edu in education" 
            :key="edu.id"
            class="education-item"
          >
            <div class="education-date">{{ edu.date }}</div>
            <div class="education-content">
              <h3 class="education-degree">{{ edu.degree }}</h3>
              <h4 class="education-institution">{{ edu.institution }}</h4>
              <p v-if="edu.description" class="education-description">{{ edu.description }}</p>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useI18n } from 'vue-i18n'
import SectionTitle from '@/components/ui/SectionTitle.vue'

const { t } = useI18n()

// 兴趣爱好数据
const interests = ref([
  { 
    id: 1, 
    name: '代码', 
    icon: '💻', 
    gradient: 'url(#codeGradient)',
    description: '编程是我最大的热爱，从早期的网页开发到现在的全栈架构，我享受将想法转化为代码的过程。无论是优雅的算法设计，还是高效的系统架构，每一行代码都是对技术的追求。我热衷于学习新技术，探索最佳实践，并通过开源项目分享知识，与全球开发者共同成长。'
  },
  { 
    id: 2, 
    name: '娱乐', 
    icon: '🎬', 
    gradient: 'url(#entertainGradient)',
    description: '在工作之余，我喜欢通过电影、电视剧和综艺节目来放松身心。科幻、悬疑和喜剧是我的最爱，它们不仅能带来欢乐，还能激发创意思维。我也喜欢参加线下活动，与朋友聚会，体验不同的娱乐方式，这些都是我保持工作热情和生活平衡的重要方式。'
  },
  { 
    id: 3, 
    name: '美食', 
    icon: '🍴', 
    gradient: 'url(#foodGradient)',
    description: '美食是生活的艺术，我热爱探索各地特色美食，从街边小吃到高级料理，每一道菜都有其独特的故事。我也喜欢自己动手烹饪，尝试不同的菜系和烹饪技巧，享受制作美食的过程。美食不仅满足味蕾，更是文化交流的桥梁，让我在品尝中感受世界的多样性。'
  },
  { 
    id: 4, 
    name: '音乐', 
    icon: '🎵', 
    gradient: 'url(#musicGradient)',
    description: '音乐是我的精神食粮，无论是古典、流行还是电子音乐，都能触动我的心灵。在编程时，我喜欢听一些舒缓的音乐来提高专注力；在休闲时，动感的节奏能让我放松心情。我也学习过一些乐器，音乐不仅陶冶情操，还能激发创造力，让我在技术与艺术之间找到平衡。'
  },
  { 
    id: 5, 
    name: '小说', 
    icon: '📖', 
    gradient: 'url(#novelGradient)',
    description: '阅读是我的终身爱好，从科幻小说到历史传记，从技术书籍到哲学著作，书籍开阔了我的视野，丰富了我的思想。我特别喜欢科幻和推理类小说，它们培养了我的逻辑思维和想象力。每一本书都是一次思想的旅行，让我在文字中探索未知的世界，汲取智慧的养分。'
  },
  { 
    id: 6, 
    name: '游戏', 
    icon: '🎮', 
    gradient: 'url(#gameGradient)',
    description: '游戏是我放松和社交的重要方式，从策略游戏到角色扮演，从独立游戏到3A大作，我享受游戏带来的沉浸式体验。游戏不仅是娱乐，更是一种艺术形式，它融合了故事、音乐、美术和互动设计。作为开发者，我也从游戏中学习交互设计和用户体验，这些都对我的工作有很大启发。'
  }
])

const hoveredSection = ref<number | null>(null)
const activeSection = ref<number>(0)
const showBubble = ref(false)
const bubbleContent = ref('')
const bubblePosition = ref({ x: 0, y: 0 })

// 步进动画 - 每3秒切换一个扇形
let animationTimer: ReturnType<typeof setInterval> | null = null

onMounted(() => {
  animationTimer = setInterval(() => {
    activeSection.value = (activeSection.value + 1) % 6
  }, 3000)
})

onUnmounted(() => {
  if (animationTimer) {
    clearInterval(animationTimer)
  }
})

// 鼠标悬停事件
const handleMouseEnter = (index: number, event: MouseEvent) => {
  hoveredSection.value = index
  activeSection.value = index // 暂停自动步进，停留在当前
  
  // 计算气泡位置
  const target = event.currentTarget as SVGGraphicsElement
  const rect = target.getBBox()
  const svg = target.ownerSVGElement
  if (svg) {
    const point = svg.createSVGPoint()
    point.x = rect.x + rect.width
    point.y = rect.y + rect.height / 2
    const ctm = svg.getScreenCTM()
    if (ctm) {
      const screenPoint = point.matrixTransform(ctm)
      
      bubblePosition.value = {
        x: screenPoint.x,
        y: screenPoint.y
      }
    }
  }
  
  const interest = interests.value[index]
  if (interest) {
    bubbleContent.value = interest.description
  }
  showBubble.value = true
  
  // 清除定时器
  if (animationTimer) {
    clearInterval(animationTimer)
  }
}

const handleMouseLeave = () => {
  hoveredSection.value = null
  showBubble.value = false
  
  // 重新启动定时器
  animationTimer = setInterval(() => {
    activeSection.value = (activeSection.value + 1) % 6
  }, 3000)
}

// 获取六边形顶点坐标
const getHexagonPoints = (): string => {
  const cx = 250
  const cy = 250
  const radius = 200
  const points: number[][] = []
  
  for (let i = 0; i < 6; i++) {
    const angle = (i * 60 - 90) * Math.PI / 180
    const x = cx + radius * Math.cos(angle)
    const y = cy + radius * Math.sin(angle)
    points.push([x, y])
  }
  
  return points.map(p => p.join(',')).join(' ')
}

// 获取六边形扇形路径（从中心到两个顶点）
const getHexagonSectionPath = (index: number): string => {
  const cx = 250
  const cy = 250
  const innerRadius = 70
  const outerRadius = 200
  const anglePerSection = 60
  const startAngle = index * anglePerSection - 90
  const endAngle = startAngle + anglePerSection
  
  const toRad = (deg: number) => (deg * Math.PI) / 180
  
  // 内圆起点
  const x1 = cx + innerRadius * Math.cos(toRad(startAngle))
  const y1 = cy + innerRadius * Math.sin(toRad(startAngle))
  
  // 外圆第一个顶点
  const x2 = cx + outerRadius * Math.cos(toRad(startAngle))
  const y2 = cy + outerRadius * Math.sin(toRad(startAngle))
  
  // 外圆第二个顶点
  const x3 = cx + outerRadius * Math.cos(toRad(endAngle))
  const y3 = cy + outerRadius * Math.sin(toRad(endAngle))
  
  // 内圆终点
  const x4 = cx + innerRadius * Math.cos(toRad(endAngle))
  const y4 = cy + innerRadius * Math.sin(toRad(endAngle))
  
  return `
    M ${x1} ${y1}
    L ${x2} ${y2}
    L ${x3} ${y3}
    L ${x4} ${y4}
    A ${innerRadius} ${innerRadius} 0 0 0 ${x1} ${y1}
    Z
  `
}

// 获取扇形区域内的标签位置
const getSectionLabelPosition = (index: number) => {
  const cx = 250
  const cy = 250
  const radius = 135 // 在内外圆之间
  const anglePerSection = 60
  const angle = index * anglePerSection - 90 + anglePerSection / 2
  const toRad = (deg: number) => (deg * Math.PI) / 180
  
  return {
    x: cx + radius * Math.cos(toRad(angle)),
    y: cy + radius * Math.sin(toRad(angle))
  }
}

// 模拟工作经验数据
const experiences = ref([
  {
    id: 1,
    date: t('about.experience.job1.date'),
    position: t('about.experience.job1.position'),
    company: t('about.experience.job1.company'),
    description: t('about.experience.job1.description')
  },
  {
    id: 2,
    date: t('about.experience.job2.date'),
    position: t('about.experience.job2.position'),
    company: t('about.experience.job2.company'),
    description: t('about.experience.job2.description')
  },
  {
    id: 3,
    date: t('about.experience.job3.date'),
    position: t('about.experience.job3.position'),
    company: t('about.experience.job3.company'),
    description: t('about.experience.job3.description')
  }
])

// 模拟教育背景数据
const education = ref([
  {
    id: 1,
    date: t('about.education.edu1.date'),
    degree: t('about.education.edu1.degree'),
    institution: t('about.education.edu1.institution'),
    description: t('about.education.edu1.description')
  },
  {
    id: 2,
    date: t('about.education.edu2.date'),
    degree: t('about.education.edu2.degree'),
    institution: t('about.education.edu2.institution'),
    description: t('about.education.edu2.description')
  }
])
</script>

<style scoped lang="scss">
.about {
  .about-hero {
    padding: 2rem 0;
    background-image: url('/src/assets/mywork.jpg');
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    position: relative;

    // 添加半透明遮罩层以确保文字可读
    &::before {
      content: '';
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background: rgba(255, 255, 255, 0.92);
      z-index: 0;
    }

    > .container {
      position: relative;
      z-index: 1;
    }

    .about-content {
      display: flex;
      gap: 0;
      margin-top: 3rem;
      align-items: flex-start;

      // 左侧：L型布局
      .about-left {
        flex: 1;
        display: flex;
        flex-direction: column;
        align-items: flex-start;

        .about-image {
          width: 500px;
          height: 400px;
          border-radius: 10px;
          box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
          margin-bottom: 1.5rem;
          object-fit: cover;
        }

        .about-text {
          width: 100%;

          p {
            font-size: 1rem;
            line-height: 1.8;
            margin-bottom: 1rem;
            color: #555;
            text-align: left;
          }
        }
      }

      // 中间分割线
      .divider {
        width: 4px;
        background: linear-gradient(180deg, #667eea 0%, #764ba2 100%);
        margin: 0 3rem;
        align-self: stretch;
        border-radius: 2px;
      }

      // 右侧：六边形
      .about-right {
        flex: 1;
        display: flex;
        align-items: center;
        justify-content: center;
        position: relative;

        .hexagon-chart {
          width: 100%;
          max-width: 500px;
          height: auto;

          .hex-section {
            cursor: pointer;
            transition: all 0.5s ease;
            opacity: 0.3;
            stroke: white;
            stroke-width: 1.5;
            transform-origin: center;

            &.active {
              opacity: 0.85;
              animation: sectionPulse 3s ease-in-out;
            }

            &.hovered {
              opacity: 1;
              filter: brightness(1.15);
              stroke-width: 2.5;
              transform: scale(1.05);
            }
          }

          .center-circle {
            filter: drop-shadow(0 2px 8px rgba(0, 0, 0, 0.1));
          }

          .center-text {
            font-size: 22px;
            font-weight: bold;
            fill: #333;
            pointer-events: none;
          }

          .center-subtext {
            font-size: 16px;
            font-weight: 500;
            fill: #666;
            pointer-events: none;
          }

          .section-icon {
            font-size: 32px;
            pointer-events: none;
            user-select: none;
            opacity: 0.5;
            transition: all 0.3s ease;

            &.active {
              opacity: 1;
              animation: iconFadeIn 0.5s ease-in;
            }
          }

          .section-label {
            font-size: 16px;
            font-weight: 600;
            fill: #333;
            pointer-events: none;
            text-shadow: 0 1px 3px rgba(255, 255, 255, 0.8);
            opacity: 0.5;
            transition: all 0.3s ease;

            &.active {
              opacity: 1;
              animation: iconFadeIn 0.5s ease-in;
            }
          }
        }

        // 气泡提示框
        .interest-bubble {
          position: fixed;
          background: white;
          border-radius: 12px;
          padding: 1.5rem;
          box-shadow: 0 8px 30px rgba(0, 0, 0, 0.15);
          max-width: 320px;
          z-index: 1000;
          margin-left: 20px;
          transform: translateY(-50%);

          .bubble-arrow {
            position: absolute;
            left: -8px;
            top: 50%;
            transform: translateY(-50%);
            width: 0;
            height: 0;
            border-top: 8px solid transparent;
            border-bottom: 8px solid transparent;
            border-right: 8px solid white;
          }

          .bubble-content {
            font-size: 14px;
            line-height: 1.8;
            color: #333;
            text-align: justify;
          }
        }
      }
    }
  }

  // 动画定义
  @keyframes sectionPulse {
    0%, 100% {
      opacity: 0.85;
    }
    50% {
      opacity: 0.95;
    }
  }

  @keyframes iconFadeIn {
    from {
      opacity: 0;
      transform: scale(0.8);
    }
    to {
      opacity: 1;
      transform: scale(1);
    }
  }

  // 气泡渐变动画
  .bubble-fade-enter-active,
  .bubble-fade-leave-active {
    transition: all 0.3s ease;
  }

  .bubble-fade-enter-from {
    opacity: 0;
    transform: translateY(-50%) translateX(-10px);
  }

  .bubble-fade-leave-to {
    opacity: 0;
    transform: translateY(-50%) translateX(-10px);
  }

  .experience-section {
    padding: 5rem 0;
    background-color: #f8f9fa;

    .timeline {
      position: relative;
      max-width: 800px;
      margin: 3rem auto 0;

      &::before {
        content: '';
        position: absolute;
        top: 0;
        bottom: 0;
        width: 4px;
        background: #1a5fb4;
        left: 50%;
        margin-left: -2px;
      }

      .timeline-item {
        position: relative;
        margin-bottom: 50px;
        width: 100%;
        &.timeline-item-left{
          padding-right:50%;
        }
        &:last-child {
          margin-bottom: 0;
        }

        &.timeline-item-right {
          padding-left: 50%;

          .timeline-content {
            padding: 0 0 0 30px;
          }
        } 
        .timeline-marker {
          position: absolute;
          width: 20px;
          height: 20px;
          border-radius: 50%;
          background: #1a5fb4;
          border: 4px solid white;
          box-shadow: 0 0 0 2px #1a5fb4;
          top: 0;
          left: 50%;
          margin-left: -10px;
          z-index: 1;
        }

        .timeline-content {
          padding: 0 30px 0 0;

          .timeline-date {
            font-weight: bold;
            color: #1a5fb4;
            margin-bottom: 0.5rem;
          }

          .timeline-title {
            margin: 0 0 0.5rem 0;
            color: #0d3b66;
            font-size: 1.25rem;
          }

          .timeline-company {
            margin: 0 0 1rem 0;
            color: #1a5fb4;
            font-size: 1.1rem;
          }

          .timeline-description {
            margin: 0;
            line-height: 1.6;
            color: #555;
          }
        }
      }
    }
  }

  .education-section {
    padding: 5rem 0;

    .education-list {
      margin-top: 3rem;

      .education-item {
        display: flex;
        margin-bottom: 2rem;
        padding: 1.5rem;
        background: white;
        border-radius: 8px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);

        .education-date {
          min-width: 120px;
          font-weight: bold;
          color: #1a5fb4;
          padding-right: 1rem;
          border-right: 2px solid #eee;
        }

        .education-content {
          flex: 1;
          padding-left: 1.5rem;

          .education-degree {
            margin: 0 0 0.5rem 0;
            color: #0d3b66;
          }

          .education-institution {
            margin: 0 0 0.5rem 0;
            color: #1a5fb4;
            font-size: 1.1rem;
          }

          .education-description {
            margin: 0;
            color: #555;
            line-height: 1.6;
          }
        }
      }
    }
  }
}

// 响应式设计
@media (max-width: 768px) {
  .about {
    .about-hero {
      .about-content {
        flex-direction: column;
        align-items: center;

        .about-left {
          align-items: center;
          text-align: center;

          .about-text p {
            text-align: center;
          }
        }

        .divider {
          width: 80%;
          height: 4px;
          margin: 2rem 0;
        }

        .about-right {
          width: 100%;
        }
      }
    }

    .experience-section {
      .timeline {
        &::before {
          left: 30px;
        }

        .timeline-item,
        .timeline-item-right {
          padding-left: 0;
          padding-right: 0;

          .timeline-marker {
            left: 30px;
          }

          .timeline-content {
            padding: 0 0 0 60px;
          }
        }
        
      }
    }

    .education-section {
      .education-list {
        .education-item {
          flex-direction: column;

          .education-date {
            border-right: none;
            border-bottom: 2px solid #eee;
            padding: 0 0 1rem 0;
            margin: 0 0 1rem 0;
          }

          .education-content {
            padding: 1rem 0 0 0;
          }
        }
      }
    }
  }
}
</style>