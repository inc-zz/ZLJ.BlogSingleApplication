<script setup lang="ts">
import { ref } from 'vue'
import Header from '@/components/layout/Header.vue'
import Footer from '@/components/layout/Footer.vue'
const isMobileMenuOpen = ref(false)

</script>
<template>
  <div class="main-layout">
    <Header v-model:is-mobile-menu-open="isMobileMenuOpen" />
    <main class="main-content">
      <router-view />
    </main>
    <Footer />
  </div>
</template>

<style scoped>
.main-layout {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}
</style>