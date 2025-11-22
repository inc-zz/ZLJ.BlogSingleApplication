<template>
  <button 
    class="btn" 
    :class="[
      `btn-${variant}`, 
      `btn-${size}`,
      { 'btn-block': block },
      { 'btn-disabled': disabled }
    ]"
    :disabled="disabled"
    @click="handleClick"
  >
    <slot></slot>
  </button>
</template>

<script setup lang="ts">
interface Props {
  variant?: 'primary' | 'secondary' | 'success' | 'danger' | 'outline'
  size?: 'small' | 'medium' | 'large'
  block?: boolean
  disabled?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  variant: 'primary',
  size: 'medium',
  block: false,
  disabled: false
})

const emit = defineEmits<{
  (e: 'click', event: MouseEvent): void
}>()

const handleClick = (event: MouseEvent) => {
  if (!props.disabled) {
    emit('click', event)
  }
}
</script>

<style scoped lang="scss">
.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  border: none;
  border-radius: 4px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;
  text-decoration: none;
  white-space: nowrap;
  vertical-align: middle;
  user-select: none;
  padding: 0.5rem 1rem;
  font-size: 1rem;
  line-height: 1.5;

  &:focus {
    outline: 0;
    box-shadow: 0 0 0 0.2rem rgba(26, 95, 180, 0.25);
  }

  &:hover:not(.btn-disabled) {
    transform: translateY(-2px);
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
  }

  &.btn-disabled {
    opacity: 0.6;
    cursor: not-allowed;
    transform: none;
    box-shadow: none;
  }

  // 按钮变体
  &.btn-primary {
    background-color: #1a5fb4;
    color: white;
    &:hover:not(.btn-disabled) {
      background-color: #154a8a;
    }
  }

  &.btn-secondary {
    background-color: #6c757d;
    color: white;

    &:hover:not(.btn-disabled) {
      background-color: #5a6268;
    }
  }

  &.btn-success {
    background-color: #28a745;
    color: white;

    &:hover:not(.btn-disabled) {
      background-color: #218838;
    }
  }

  &.btn-danger {
    background-color: #dc3545;
    color: white;

    &:hover:not(.btn-disabled) {
      background-color: #c82333;
    }
  }

  &.btn-outline {
    background-color: transparent;
    border: 1px solid #1a5fb4;
    color: #1a5fb4;

    &:hover:not(.btn-disabled) {
      background-color: #1a5fb4;
      color: white;
    }
  }

  // 按钮尺寸
  &.btn-small {
    padding: 0.25rem 0.5rem;
    font-size: 0.875rem;
    line-height: 1.5;
  }

  &.btn-large {
    padding: 0.75rem 1.5rem;
    font-size: 1.25rem;
    line-height: 1.5;
  }

  // 块级按钮
  &.btn-block {
    display: block;
    width: 100%;
  }
}
</style>