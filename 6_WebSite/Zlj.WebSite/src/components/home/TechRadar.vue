<template>
  <div class="tech-radar">
    <svg viewBox="0 0 1200 800" class="radar-svg">
      <!-- 延伸线和文字 -->
      <g class="extensions">
        <g 
          v-for="(_item, index) in outerTechs" 
          :key="'ext-' + index"
        >
          <!-- 延伸线 -->
          <line
            :x1="getLineStart(index).x"
            :y1="getLineStart(index).y"
            :x2="getLineEnd(index).x"
            :y2="getLineEnd(index).y"
            stroke="#ccc"
            stroke-width="1.5"
            stroke-dasharray="5,5"
          />
          <!-- 文字描述 -->
          <text
            :x="getTextPosition(index).x"
            :y="getTextPosition(index).y"
            :text-anchor="getTextAnchor(index)"
            class="extension-text"
            :fill="getOuterColor(index)"
          >
            {{ techDescriptions[index] }}
          </text>
        </g>
      </g>
      
      <!-- 外层圆 -->
      <g class="outer-ring">
        <!-- 15个扇形，每3个对应内层1个 -->
        <path 
          v-for="(tech, index) in outerTechs" 
          :key="index"
          :d="getOuterSegmentPath(index)"
          :fill="getOuterColor(index)"
          :class="{'tech-segment': true, 'active': hoveredOuter === index}"
          @mouseenter="hoveredOuter = index"
          @mouseleave="hoveredOuter = null"
        >
          <title>{{ tech }}</title>
        </path>
      </g>
      
      <!-- 内层圆 -->
      <g class="inner-ring">
        <path 
          v-for="(area, index) in tm('skills.innerAreas').slice(0,6)" 
          :key="index"
          :d="getInnerSegmentPath(index)"
          :fill="area.color"
          :class="{'tech-segment': true, 'active': hoveredInner === index}"
          @mouseenter="hoveredInner = index"
          @mouseleave="hoveredInner = null"
        >
          <title>{{ area.name }}111</title>
        </path>
      </g>
      
      <!-- 中心圆 -->
      <circle cx="600" cy="400" r="80" fill="url(#centerGradient)" class="center-circle"/>
      
      <!-- 渐变定义 -->
      <defs>
        <radialGradient id="centerGradient">
          <stop offset="0%" style="stop-color:#fff;stop-opacity:1" />
          <stop offset="100%" style="stop-color:#e6f7ff;stop-opacity:1" />
        </radialGradient>
      </defs>
      
      <!-- 文字标签 - 内层 -->
      <text 
        v-for="(area, index) in tm('skills.innerAreas').slice(0,6 )" 
        :key="'text-inner-' + index"
        :x="getInnerTextPosition(index).x"
        :y="getInnerTextPosition(index).y"
        text-anchor="middle"
        class="inner-label"
        :fill="area.color"
      >
        {{ area.name }}
      </text>
      
      <!-- 文字标签 - 外层 -->
      <text 
        v-for="(tech, index) in outerTechs" 
        :key="'text-outer-' + index"
        :x="getOuterTextPosition(index).x"
        :y="getOuterTextPosition(index).y"
        text-anchor="middle"
        class="outer-label"
        :fill="getOuterColor(index)"
      >
        {{ tech }}
      </text>
      
      <!-- 中心文字 -->
      <text x="600" y="395" text-anchor="middle" class="center-text">技术栈</text>
      <text x="600" y="415" text-anchor="middle" class="center-subtext">Tech Stack</text>
    </svg>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useI18n } from 'vue-i18n'

const { tm } = useI18n()

const hoveredOuter = ref<number | null>(null)
const hoveredInner = ref<number | null>(null)

// 内层5大块
const innerAreas = tm('skills.innerAreas') as { name: string; color: string }[]

// 外层15块技术
const outerTechs = [
  // 业务处理 (3个)
  '需求分析', '方案设计', '流程优化',
  // 前端开发 (3个)
  'Vue', 'React', 'Angular',
  // 后端开发 (3个)
  'Python', 'Golang', 'C#',
  // 服务器运维 (3个)
  'Docker', 'K8s', 'Prometheus',
  // 技术支撑 (3个)
  'Canal', 'DevOps', 'Sonarqube'
]

// 技术描述（延伸线上的文字）
const techDescriptions = [
  '深入理解客户需求，精准把握业务痛点',
  '设计高可用、可扩展的系统架构',
  '持续优化业务流程，提升效率',
  '构建现代化交互体验的前端应用',
  '打造高性能可复用的组件库',
  '类型安全的大型应用开发',
  '高并发异步处理的服务端应用',
  '数据科学与人工智能应用开发',
  '企业级稳定可靠的后端服务',
  '容器化部署，快速交付',
  '云原生应用编排与管理',
  '持续监控稳定运行',
  '版本控制与团队协作',
  '高性能数据存储与查询',
  '分布式缓存与会话管理'
]

// 获取内层扇形路径
const getInnerSegmentPath = (index: number): string => {
  const cx = 600
  const cy = 400
  const innerRadius = 80
  const outerRadius = 160
  const anglePerSegment = 360 / 5
  const startAngle = index * anglePerSegment - 90
  const endAngle = startAngle + anglePerSegment
  
  return createArcPath(cx, cy, innerRadius, outerRadius, startAngle, endAngle)
}

// 获取外层扇形路径
const getOuterSegmentPath = (index: number): string => {
  const cx = 600
  const cy = 400
  const innerRadius = 160
  const outerRadius = 260
  const anglePerSegment = 360 / 15
  const startAngle = index * anglePerSegment - 90
  const endAngle = startAngle + anglePerSegment
  
  return createArcPath(cx, cy, innerRadius, outerRadius, startAngle, endAngle)
}

// 创建弧形路径
const createArcPath = (
  cx: number, 
  cy: number, 
  innerR: number, 
  outerR: number, 
  startAngle: number, 
  endAngle: number
): string => {
  const toRad = (deg: number) => (deg * Math.PI) / 180
  
  const x1 = cx + innerR * Math.cos(toRad(startAngle))
  const y1 = cy + innerR * Math.sin(toRad(startAngle))
  const x2 = cx + outerR * Math.cos(toRad(startAngle))
  const y2 = cy + outerR * Math.sin(toRad(startAngle))
  const x3 = cx + outerR * Math.cos(toRad(endAngle))
  const y3 = cy + outerR * Math.sin(toRad(endAngle))
  const x4 = cx + innerR * Math.cos(toRad(endAngle))
  const y4 = cy + innerR * Math.sin(toRad(endAngle))
  
  const largeArc = endAngle - startAngle > 180 ? 1 : 0
  
  return `
    M ${x1} ${y1}
    L ${x2} ${y2}
    A ${outerR} ${outerR} 0 ${largeArc} 1 ${x3} ${y3}
    L ${x4} ${y4}
    A ${innerR} ${innerR} 0 ${largeArc} 0 ${x1} ${y1}
    Z
  `
}

// 获取内层文字位置
const getInnerTextPosition = (index: number) => {
  const cx = 600
  const cy = 400
  const radius = 120
  const anglePerSegment = 360 / 5
  const angle = index * anglePerSegment - 90 + anglePerSegment / 2
  const toRad = (deg: number) => (deg * Math.PI) / 180
  
  return {
    x: cx + radius * Math.cos(toRad(angle)),
    y: cy + radius * Math.sin(toRad(angle)) + 5
  }
}

// 获取外层文字位置
const getOuterTextPosition = (index: number) => {
  const cx = 600
  const cy = 400
  const radius = 210
  const anglePerSegment = 360 / 15
  const angle = index * anglePerSegment - 90 + anglePerSegment / 2
  const toRad = (deg: number) => (deg * Math.PI) / 180
  
  return {
    x: cx + radius * Math.cos(toRad(angle)),
    y: cy + radius * Math.sin(toRad(angle)) + 4
  }
}

// 获取延伸线起点（从外层圆边缘）
const getLineStart = (index: number) => {
  const cx = 600
  const cy = 400
  const radius = 270
  const anglePerSegment = 360 / 15
  const angle = index * anglePerSegment - 90 + anglePerSegment / 2
  const toRad = (deg: number) => (deg * Math.PI) / 180
  
  return {
    x: cx + radius * Math.cos(toRad(angle)),
    y: cy + radius * Math.sin(toRad(angle))
  }
}

// 获取延伸线终点
const getLineEnd = (index: number) => {
  const start = getLineStart(index)
  const cx = 600
  const isLeft = start.x < cx
  
  return {
    x: isLeft ? start.x - 120 : start.x + 120,
    y: start.y
  }
}

// 获取文字位置
const getTextPosition = (index: number) => {
  const end = getLineEnd(index)
  const cx = 600
  const isLeft = end.x < cx
  
  return {
    x: isLeft ? end.x - 10 : end.x + 10,
    y: end.y
  }
}

// 获取文字对齐方式
const getTextAnchor = (index: number) => {
  const start = getLineStart(index)
  const cx = 600
  return start.x < cx ? 'end' : 'start'
}

// 获取外层颜色（根据所属内层区域）
const getOuterColor = (index: number): string => {
  const innerIndex = Math.floor(index / 3)
  const baseColor = innerAreas[innerIndex]?.color || '#666'
  // 返回稍微浅一点的颜色
  return baseColor + '88' // 添加透明度
}
</script>

<style scoped lang="scss">
.tech-radar {
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
  
  .radar-svg {
    width: 100%;
    height: auto;
    
    .extensions {
      line {
        transition: all 0.3s ease;
      }
      
      .extension-text {
        font-size: 1rem;
        font-weight: 500;
        pointer-events: none;
        fill: #000000;
      }
    }
    
    .tech-segment {
      cursor: pointer;
      transition: all 0.3s ease;
      opacity: 0.8;
      
      &:hover, &.active {
        opacity: 1;
        filter: brightness(1.1);
        stroke: white;
        stroke-width: 2;
      }
    }
    
    .center-circle {
      filter: drop-shadow(0 0 10px rgba(0,0,0,0.1));
    }
    
    .inner-label {
      font-size: 1rem;
      font-weight: bold;
      pointer-events: none;
      fill: #000000;
    }
    
    .outer-label {
      font-size: 1rem;
      font-weight: 500;
      pointer-events: none;
      fill: #000000;
    }
    
    .center-text {
      font-size: 1.25rem;
      font-weight: bold;
      fill: #000000;
      pointer-events: none;
    }
    
    .center-subtext {
      font-size: 0.875rem;
      fill: #333333;
      pointer-events: none;
    }
  }
}

@media (max-width: 768px) {
  .tech-radar {
    .radar-svg {
      .inner-label {
        font-size: 12px;
      }
      
      .outer-label {
        font-size: 9px;
      }
      
      .center-text {
        font-size: 16px;
      }
      
      .center-subtext {
        font-size: 10px;
      }
    }
  }
}
</style>