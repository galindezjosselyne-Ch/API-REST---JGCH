<template>
  <div class="flex items-center justify-center min-h-screen">
    <UCard class="w-full max-w-md shadow-lg">
      <template #header>
        <div class="flex flex-col items-center">
          <UIcon
            name="i-heroicons-lock-closed"
            class="text-4xl text-blue-600 mb-2"
          />
          <h2 class="text-xl font-semibold">Iniciar sesión</h2>
          <p class="text-sm text-gray-500">Accede a tu panel de productos</p>
        </div>
      </template>

      <UForm :state="form" @submit.prevent="submit" class="space-y-4">
        <UFormField label="Correo electrónico" name="username" required>
          <UInput
            class="w-full"
            v-model="form.username"
            type="email"
            placeholder="ejemplo@correo.com"
            icon="i-heroicons-envelope"
            autofocus
          />
        </UFormField>

        <UFormField label="Contraseña" name="password" required>
          <UInput
            class="w-full"
            v-model="form.password"
            type="password"
            placeholder="••••••••"
            icon="i-heroicons-lock-closed"
          />
        </UFormField>

        <UButton
          type="submit"
          color="primary"
          variant="solid"
          size="lg"
          block
          :loading="loading"
        >
          Entrar
        </UButton>

        <UAlert
          v-if="error"
          icon="i-heroicons-exclamation-triangle"
          color="error"
          variant="soft"
          class="mt-2"
          :title="error || 'Error'"
        />
      </UForm>

      <template #footer>
        <p class="text-center text-sm text-gray-500">
          © {{ new Date().getFullYear() }} - Productos Demo
        </p>
      </template>
    </UCard>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
const form = ref({
  username: "test@local",
  password: "Password123",
});
const error = ref("");
const loading = ref(false);
const auth = useAuth();
const config = useRuntimeConfig();

const submit = async () => {
  try {
    loading.value = true;
    error.value = "";

    const res: any = await $fetch(`${config.public.apiBase}/auth/login`, {
      method: "POST",
      body: form.value,
    });

    auth.setToken(res.token);
    navigateTo("/products");
  } catch (err: any) {
    error.value = "Credenciales inválidas o error del servidor.";
  } finally {
    loading.value = false;
  }
};
</script>
