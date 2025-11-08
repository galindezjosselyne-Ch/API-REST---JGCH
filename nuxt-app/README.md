# 🎨 Frontend - Nuxt 4 Application

Frontend moderno para el sistema de gestión de productos, construido con **Nuxt 4**, **Vue 3** y **Nuxt UI**.

---

## 🚀 Tecnologías

- **Framework**: Nuxt 4 (Vue 3 Composition API)
- **UI Library**: Nuxt UI (componentes modernos basados en Tailwind CSS)
- **Imágenes**: @nuxt/image (optimización automática)
- **Autenticación**: JWT Bearer Token
- **HTTP Client**: $fetch (nativo de Nuxt)
- **TypeScript**: Soporte completo
- **Containerización**: Docker

---

## 📂 Estructura del Proyecto

```
nuxt-app/
├── pages/
│   ├── index.vue              # Página de login
│   └── products/
│       └── index.vue          # CRUD de productos y ventas
├── composables/
│   └── useAuth.ts             # Lógica de autenticación y peticiones
├── layouts/
│   └── default.vue            # Layout principal
├── assets/
│   └── css/
│       └── main.css           # Estilos globales
├── public/                    # Archivos estáticos
├── nuxt.config.ts             # Configuración de Nuxt
├── Dockerfile                 # Configuración Docker
└── package.json               # Dependencias
```

---

## ⚙️ Configuración

### Variables de Entorno

El proyecto usa `runtimeConfig` para configurar la URL del backend:

**`.env.development`** (desarrollo local):
```env
NUXT_PUBLIC_API_BASE=http://localhost:5000
```

**`.env.production`** (producción):
```env
NUXT_PUBLIC_API_BASE=http://api:5000
```

### Configuración en `nuxt.config.ts`

```typescript
runtimeConfig: {
  public: {
    apiBase: import.meta.env.NUXT_PUBLIC_API_BASE
  }
}
```

---

## 🎯 Características Principales

### 1. **Autenticación JWT**
- Login con credenciales (`test@local` / `Password123`)
- Token almacenado en estado reactivo con `useState`
- Auto-redirección al login si el token expira (401/403)
- Header `Authorization: Bearer <token>` en todas las peticiones protegidas

### 2. **Gestión de Productos**
- ✅ Listar todos los productos con imágenes
- ✅ Crear productos con upload de imágenes (multipart/form-data)
- ✅ Eliminar productos
- ✅ Visualización de precio y descripción

### 3. **Sistema de Ventas**
- Registrar ventas de productos (botón "Comprar")
- Descargar reportes de ventas en formato CSV
- Filtrado por rango de fechas

### 4. **UI/UX Moderna**
- Componentes de Nuxt UI (UCard, UButton, UInput, etc.)
- Diseño responsive con Tailwind CSS
- Iconos de Heroicons
- Optimización de imágenes con @nuxt/image

---

## 🛠️ Instalación y Uso

### Desarrollo Local

1. **Instalar dependencias**:
```bash
npm install
```

2. **Configurar variable de entorno**:
Asegúrate de que el backend esté corriendo en `http://localhost:5000`

3. **Iniciar servidor de desarrollo**:
```bash
npm run dev
```

4. **Acceder a la aplicación**:
Abre http://localhost:3000

### Con Docker

```bash
# Desde la raíz del proyecto
docker compose up --build
```

El frontend estará disponible en http://localhost:3000

---

## 📄 Páginas

### `/` - Login
- Formulario de autenticación
- Validación de credenciales
- Redirección automática a `/products` tras login exitoso
- Manejo de errores de autenticación

### `/products` - Gestión de Productos
- **Listar productos**: Grid responsive con cards
- **Crear producto**: Formulario con nombre, descripción, precio e imagen
- **Eliminar producto**: Botón de eliminación por producto
- **Comprar producto**: Registra una venta
- **Descargar reporte**: CSV de ventas por rango de fechas

---

## 🔧 Composables

### `useAuth.ts`

Composable centralizado para autenticación y peticiones HTTP:

```typescript
const auth = useAuth();

// Métodos disponibles:
auth.token           // Token JWT actual
auth.setToken(token) // Guardar token
auth.isLogged()      // Verificar si está autenticado
auth.fetchWithAuth(url, options) // Petición HTTP con token
```

**Características**:
- Añade automáticamente el header `Authorization`
- Maneja errores 401/403 redirigiendo al login
- Usa `$fetch` de Nuxt para peticiones

---

## 🌐 Comunicación con el Backend

### Endpoints Consumidos

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|---------------|
| POST | `/auth/login` | Iniciar sesión | ❌ |
| GET | `/products` | Listar productos | ✅ |
| POST | `/products` | Crear producto | ✅ |
| DELETE | `/products/{id}` | Eliminar producto | ✅ |
| POST | `/products/sales` | Registrar venta | ✅ |
| GET | `/products/sales-report` | Descargar CSV | ✅ |

### Ejemplo de Petición

```typescript
// Login
const res = await $fetch(`${config.public.apiBase}/auth/login`, {
  method: 'POST',
  body: { username: 'test@local', password: 'Password123' }
});

// Listar productos (con autenticación)
const products = await auth.fetchWithAuth('/products');

// Crear producto con imagen
const formData = new FormData();
formData.append('name', 'Producto');
formData.append('description', 'Descripción');
formData.append('price', '99.99');
formData.append('image', file);

await fetch(`${config.public.apiBase}/products`, {
  method: 'POST',
  body: formData,
  headers: { Authorization: `Bearer ${auth.token.value}` }
});
```

---

## 🐋 Docker

### Dockerfile

Multi-stage build optimizado:
- **Stage 1**: Build de la aplicación Nuxt
- **Stage 2**: Runtime con Node.js Alpine (imagen ligera)

### Variables de Entorno en Docker

```yaml
environment:
  - NUXT_PUBLIC_API_BASE=http://localhost:5000
```

---

## 🔐 Credenciales de Prueba

| Campo | Valor |
|-------|-------|
| Usuario | `test@local` |
| Contraseña | `Password123` |

---

## 📦 Scripts Disponibles

```bash
npm run dev       # Servidor de desarrollo
npm run build     # Build para producción
npm run preview   # Preview del build de producción
npm run generate  # Generar sitio estático
```

---

## 🎨 Componentes UI Utilizados

- **UCard**: Contenedores con sombra y bordes
- **UButton**: Botones con variantes (solid, outline, etc.)
- **UInput**: Inputs de formulario
- **UForm**: Formularios reactivos
- **UFormField**: Campos de formulario con labels
- **UAlert**: Alertas y notificaciones
- **UFileUpload**: Upload de archivos
- **NuxtImg**: Imágenes optimizadas

---

## 🚨 Manejo de Errores

- **401/403**: Redirección automática al login
- **Errores de red**: Mensajes de error en consola
- **Validación**: Alertas visuales en formularios

---

## 📚 Recursos

- [Nuxt 4 Documentation](https://nuxt.com/docs)
- [Nuxt UI Components](https://ui.nuxt.com/)
- [Vue 3 Composition API](https://vuejs.org/guide/extras/composition-api-faq.html)
- [Tailwind CSS](https://tailwindcss.com/)

---

## 📝 Notas

- El token JWT se almacena en memoria (se pierde al recargar la página)
- Las imágenes se cargan desde el backend usando la URL base configurada
- El formato CSV de reportes usa punto y coma (`;`) como delimitador
- Compatible con Node.js 18+
