import { ref, onMounted, onUnmounted } from 'vue'

interface Breakpoints {
  isMobile: boolean
  isTablet: boolean
  isDesktop: boolean
}

export function useBreakpoint() {
  const breakpoints = ref<Breakpoints>({
    isMobile: false,
    isTablet: false,
    isDesktop: false
  })

  const updateBreakpoints = () => {
    const width = window.innerWidth
    
    breakpoints.value.isMobile = width < 768
    breakpoints.value.isTablet = width >= 768 && width < 1024
    breakpoints.value.isDesktop = width >= 1024
  }

  onMounted(() => {
    updateBreakpoints()
    window.addEventListener('resize', updateBreakpoints)
  })

  onUnmounted(() => {
    window.removeEventListener('resize', updateBreakpoints)
  })

  return breakpoints
}