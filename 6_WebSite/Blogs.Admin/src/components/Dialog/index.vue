<template>
  <el-dialog
    v-model="dialogVisible"
    :title="title"
    :width="width"
    :close-on-click-modal="closeOnClickModal"
    :close-on-press-escape="closeOnPressEscape"
    @closed="handleClosed"
  >
    <slot></slot>
    <template #footer>
      <slot name="footer">
        <el-button @click="handleCancel">{{ cancelText }}</el-button>
        <el-button type="primary" :loading="confirmLoading" @click="handleConfirm">
          {{ confirmText }}
        </el-button>
      </slot>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'

interface Props {
  modelValue: boolean
  title?: string
  width?: string | number
  confirmText?: string
  cancelText?: string
  closeOnClickModal?: boolean
  closeOnPressEscape?: boolean
  confirmLoading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  title: '提示',
  width: '600px',
  confirmText: '确定',
  cancelText: '取消',
  closeOnClickModal: false,
  closeOnPressEscape: true,
  confirmLoading: false,
})

const emit = defineEmits<{
  'update:modelValue': [value: boolean]
  confirm: []
  cancel: []
  closed: []
}>()

const dialogVisible = ref(props.modelValue)

watch(
  () => props.modelValue,
  (val) => {
    dialogVisible.value = val
  }
)

watch(dialogVisible, (val) => {
  emit('update:modelValue', val)
})

const handleConfirm = () => {
  emit('confirm')
}

const handleCancel = () => {
  dialogVisible.value = false
  emit('cancel')
}

const handleClosed = () => {
  emit('closed')
}
</script>
