<template>
  <div class="section-title" :class="{ 'center': center }">
    <h2 class="title">{{ title }}</h2>
    <div v-if="subtitle" class="subtitle">{{ subtitle }}</div>
    <div v-if="description" class="description">{{ description }}</div>
  </div>
</template>

<script setup lang="ts">
interface Props {
  title?: string
  subtitle?: string
  description?: string
  center?: boolean
}

defineProps<Props>()
</script>

<style scoped lang="scss">
.section-title {
  margin-bottom: 2rem;

  .title {
    font-size: 2rem;
    color: #7869d4;
    margin: 0 0 0.5rem 0;
    position: relative;
    display: inline-block;

    &::after {
      content: '';
      position: absolute;
      bottom: -10px;
      left: 0;
      width: 50px;
      height: 3px;
      background: #1a5fb4;
    }
  }

  .subtitle {
    font-size: 1.25rem;
    color: #051937;
    margin: 0.5rem 0;
    font-weight: 500;
  }

  .description {
    font-size: 1.2rem;
    color: #051937;
    margin: 0.5rem 0;
    line-height: 1.6;
  }

  &.center {
    text-align: center;

    .title {
      &::after {
        left: 50%;
        transform: translateX(-50%);
      }
    }

    .description {
      margin-left: auto;
      margin-right: auto;
    }
  }
}

// 响应式设计
@media (max-width: 768px) {
  .section-title {
    .title {
      font-size: 2rem;
    }

    .subtitle {
      font-size: 2rem;
    }
  }
}
</style>