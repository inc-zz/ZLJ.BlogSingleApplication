<template>
  <div class="skills">
    <!-- <section class="skills-hero">
      <div class="container">
        <SectionTitle 
          :title="t('skills.title')" 
          :subtitle="t('skills.subtitle')" 
          :description="t('skills.description')"
          :center="true"
        />
      </div>
    </section> -->

    <section class="tech-radar-section">
      <canvas ref="matrixCanvas" class="matrix-rain"></canvas>
      <div class="container">
        <TechRadar />
      </div>
    </section>
    <div class="content-footer">
      <div>我拥有全面的技术开发能力，能给你带来业务上的突破，期待跟您的合作!</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
// import { useI18n } from 'vue-i18n'
// import SectionTitle from '@/components/ui/SectionTitle.vue'
import TechRadar from '@/components/home/TechRadar.vue'

// const { t } = useI18n()
const matrixCanvas = ref<HTMLCanvasElement | null>(null)

// 代码雨效果
onMounted(() => {
  if (!matrixCanvas.value) return
  
  const canvas = matrixCanvas.value
  const ctx = canvas.getContext('2d')
  if (!ctx) return

  // 设置画布尺寸
  const resizeCanvas = () => {
    canvas.width = canvas.offsetWidth
    canvas.height = canvas.offsetHeight
  }
  resizeCanvas()
  window.addEventListener('resize', resizeCanvas)

  // 字符集
  const chars = '0101010101010101010101010'
  const fontSize = 16
  const columns = canvas.width / fontSize
  const drops: number[] = []

  // 初始化雨滴位置
  for (let i = 0; i < columns; i++) {
    drops[i] = Math.random() * -100
  }

  // 绘制函数
  const draw = () => {
    // 半透明黑色背景，产生拖尾效果
    ctx.fillStyle = 'rgba(255, 255, 255, 0.05)'
    ctx.fillRect(0, 0, canvas.width, canvas.height)
    // 绘制字符 颜色+大小
    ctx.fillStyle = '#000'
    ctx.font = `${fontSize}px monospace`

    for (let i = 0; i < drops.length; i++) {
      const text = chars[Math.floor(Math.random() * chars.length)]
      const x = i * fontSize
      const y = (drops[i] ?? 0) * fontSize

      if (text) {
        ctx.fillText(text, x, y)
      }

      // 重置雨滴
      if (y > canvas.height && Math.random() > 0.95) {
        drops[i] = 0
      }
      const currentDrop = drops[i]
      if (currentDrop !== undefined) {
        drops[i] = currentDrop + 1
      }
    }
  }

  // 动画循环
  const interval = setInterval(draw, 50)

  // 清理
  onUnmounted(() => {
    clearInterval(interval)
    window.removeEventListener('resize', resizeCanvas)
  })
})
</script>

<style scoped lang="scss">
.skills {
  .skills-hero {
    padding: 3rem 0;
    background: linear-gradient(135deg, #f0f8ff 0%, #e6f7ff 100%);
  }

  .tech-radar-section {
    padding: 1rem 0;
    background: #ffffff;
    position: relative;
    
    .matrix-rain {
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      opacity: 0.1;
      pointer-events: none;
      z-index: 0;
    }
    
    .container {
      position: relative;
      z-index: 1;
    }
  }
   svg .radar-svg{
    font-size:20px !important;
   }

  .content-footer{
    width: 100%;
    text-align:center;
    position:absolute;
    font-size:16px;
    bottom:20px;
  }
}

// 响应式设计
@media (max-width: 768px) {
  .skills {
    .skills-hero {
      padding: 1rem 0;
    }

    .tech-radar-section {
      padding: 1rem 0;
    }
  }
}
</style>