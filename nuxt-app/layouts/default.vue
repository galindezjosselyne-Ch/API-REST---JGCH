<template>
  <UContainer class="min-h-screen flex flex-col">
    <header
      class="py-4 border-b border-gray-200 flex items-center justify-between"
    >
      <h1 class="text-xl font-bold">🛍️ Products App</h1>
      <UButton
        v-if="isLoggedIn"
        color="secondary"
        variant="ghost"
        @click="logout"
      >
        Logout
      </UButton>
    </header>

    <main class="flex-1 py-8">
      <NuxtPage />
    </main>

    <footer class="py-4 text-center text-sm text-gray-500 border-t">
      © {{ new Date().getFullYear() }} Products App
    </footer>
  </UContainer>
</template>

<script setup lang="ts">
import { ref } from "vue";

const isLoggedIn = ref(false);

function logout() {
  navigateTo("/login");
}
</script>
