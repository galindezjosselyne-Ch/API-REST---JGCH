<template>
  <main class="p-6 max-w-5xl mx-auto">
    <h1 class="text-3xl font-bold mb-6 text-center">Productos</h1>

    <div class="flex flex-wrap gap-4 mb-6">
      <UButton @click="showForm = !showForm">
        {{ showForm ? "Cerrar Formulario" : "Nuevo Producto" }}
      </UButton>

      <div class="flex gap-2 items-center">
        <span class="font-semibold">Filtrar reporte:</span>
        <UInput type="date" v-model="reportStart" />
        <UInput type="date" v-model="reportEnd" />
        <UButton @click="downloadReport">Descargar CSV</UButton>
      </div>
    </div>

    <UCard v-if="showForm" class="mb-6 p-6">
      <form @submit.prevent="create" class="space-y-4">
        <UInput v-model="form.name" placeholder="Nombre" />
        <UInput v-model="form.description" placeholder="Descripción" />
        <UInput v-model.number="form.price" type="number" placeholder="Precio" />
        <UFileUpload v-model="fileRef" type="file" />
        <UButton type="submit" variant="solid">Crear Producto</UButton>
      </form>
    </UCard>

    <div class="grid md:grid-cols-2 gap-4">
      <UCard
        v-for="p in products"
        :key="p.id"
        class="flex items-center justify-between p-4"
      >
        <div class="flex items-center gap-4">
          <NuxtImg
            v-if="p.imageUrl"
            :src="getURL(p.imageUrl)"
            class="w-20 h-20 object-cover rounded"
          />
          <div>
            <strong class="block text-lg">{{ p.name }}</strong>
            <span class="text-sm text-gray-500">Precio: {{ p.price }}</span>
          </div>
        </div>

        <div class="flex flex-col gap-2 pt-3">
          <UButton variant="solid" size="sm" @click="buy(p.id)">
            Comprar
          </UButton>
          <UButton variant="outline" color="error" size="sm" @click="remove(p.id)">
            Eliminar
          </UButton>
        </div>
      </UCard>
    </div>
  </main>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";

const auth = useAuth();
const config = useRuntimeConfig();
const products = ref<any[]>([]);
const showForm = ref(false);
const form = ref({ name: "", description: "", price: 0 });
const fileRef = ref<any | HTMLInputElement>(null);

const reportStart = ref<string>(new Date().toISOString().slice(0, 10));
const reportEnd = ref<string>(new Date().toISOString().slice(0, 10));

const load = async () => {
  try {
    const res: any = await auth.fetchWithAuth("/products");
    products.value = res;
  } catch (e) {
    console.error(e);
  }
};

onMounted(load);

function getURL(image: string) {
  return config.public.apiBase + image;
}

const create = async () => {
  const fd = new FormData();
  fd.append("name", form.value.name);
  fd.append("description", form.value.description);
  fd.append("price", String(form.value.price));

  fd.append("image", fileRef.value, fileRef.value.name || "Image");

  await fetch(`${config.public.apiBase}/products`, {
    method: "POST",
    body: fd,
    headers: { Authorization: `Bearer ${auth.token.value}` },
  });

  form.value = { name: "", description: "", price: 0 };
  if (fileRef.value) fileRef.value.value = "";
  showForm.value = false;
  await load();
};

async function buy(id: number) {
  await auth.fetchWithAuth(`/products/sales`, {
    method: "POST",
    body: { productId: id, quantity: 1 },
  });
  alert("Venta registrada!");
}

const remove = async (id: number) => {
  await auth.fetchWithAuth(`/products/${id}`, { method: "DELETE" });
  await load();
};

async function downloadReport() {
  try {
    const start = reportStart.value;
    const end = reportEnd.value;

    const res = await fetch(
      `${config.public.apiBase}/products/sales-report?start=${start}&end=${end}&format=csv`,
      {
        headers: { Authorization: `Bearer ${auth.token.value}` },
      }
    );

    if (!res.ok) throw new Error("Error al descargar CSV");

    const blob = await res.blob();
    const url = window.URL.createObjectURL(blob);

    const link = document.createElement("a");
    link.href = url;
    link.setAttribute(
      "download",
      `sales-report-${start}_to_${end}.csv`
    );
    document.body.appendChild(link);
    link.click();
    link.remove();
    window.URL.revokeObjectURL(url);
  } catch (e) {
    console.error(e);
    alert("Error al descargar el reporte");
  }
}
</script>
