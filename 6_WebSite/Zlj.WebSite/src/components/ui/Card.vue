<template>
  <div class="card">
    <div v-if="image" class="card-image">
      <img :src="image" :alt="title" />
    </div>
    <div class="card-content">
      <h3 v-if="title" class="card-title">{{ title }}</h3>
      <p v-if="content" class="card-text">{{ content }}</p>
      <div v-if="$slots.default" class="card-slot">
        <slot></slot>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Props {
  title?: string
  content?: string
  image?: string
}

defineProps<Props>()
</script>

<style scoped lang="scss">
.card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  display: flex;
  flex-direction: column;
  height: 100%;

  &:hover {
    transform: translateY(-5px);
    box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
  }

  .card-image {
    width: 100%;
    height: 200px;
    overflow: hidden;

    img {
      width: 100%;
      height: 100%;
      object-fit: cover;
      transition: transform 0.3s ease;

      &:hover {
        transform: scale(1.05);
      }
    }
  }

  .card-content {
    padding: 1.5rem;

    .card-title {
      margin-top: 0;
      margin-bottom: 1rem;
      color: #0d3b66;
      font-size: 1.25rem;
    }

    .card-text {
      margin: 0 0 1rem 0;
      color: #555;
      line-height: 1.6;
    }

    .card-slot {
      margin-top: 1rem;
    }
  }
}
</style>