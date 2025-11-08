# 🧩 Fullstack CRUD de Productos (Nuxt 4 + .NET 8)

Aplicación fullstack completa que implementa un sistema de gestión de productos y ventas con autenticación JWT, subida de imágenes y reportes CSV. Construida con **.NET 8** (Arquitectura Hexagonal + EF Core SQLite) y **Nuxt 4** (Vue 3 + Nuxt UI).

---

## 🚀 Tecnologías principales

- **Backend:** .NET 8, ASP.NET Core Web API, Arquitectura Hexagonal (Ports & Adapters), EF Core (SQLite), JWT Authentication
- **Frontend:** Nuxt 4, Vue 3 Composition API, Nuxt UI, TypeScript
- **Contenedores:** Docker + Docker Compose
- **Base de datos:** SQLite (persistente) / InMemory (opcional para testing)
- **Almacenamiento de imágenes:** Local (wwwroot/images) / Azure Blob Storage (opcional)
- **Documentación:** Swagger/OpenAPI
- **Reportes:** Exportación CSV de ventas

---

## 📂 Estructura del proyecto

```
📦 josselyne/
├── ProductsApi/                    # Backend (.NET 8)
│   ├── Web/                        # Capa de presentación
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   └── ProductsController.cs
│   │   └── Program.cs
│   ├── Application/                # Capa de aplicación
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   └── Services/
│   ├── Domain/                     # Capa de dominio
│   │   └── Entities/
│   ├── Infrastructure/             # Capa de infraestructura
│   │   ├── Persistence/
│   │   ├── Security/
│   │   └── Storage/
│   ├── Migrations/
│   ├── Dockerfile
│   └── ProductsApi.csproj
│
├── nuxt-app/                       # Frontend (Nuxt 4)
│   ├── pages/
│   │   ├── index.vue               # Login
│   │   └── products/
│   │       └── index.vue           # CRUD + Ventas
│   ├── composables/
│   │   └── useAuth.ts              # Autenticación
│   ├── layouts/
│   │   └── default.vue
│   ├── nuxt.config.ts
│   ├── package.json
│   └── Dockerfile
│
├── nginx/                          # Reverse proxy
├── docker-compose.yaml
└── README.MD
```

---

## ⚙️ Requisitos previos

Antes de comenzar asegúrate de tener instalado:

- [Docker](https://docs.docker.com/get-docker/)
- [Docker Compose](https://docs.docker.com/compose/install/)
- (opcional para desarrollo local)
  - [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
  - [Node.js 18+](https://nodejs.org/)

---

## 🧾 Variables de entorno

Crea un archivo `.env` en la raíz del proyecto con las siguientes variables:

```bash
# 🔐 JWT
JWT_SECRET=supersecreto_local_123
JWT_ISSUER=ProductsApi

# ☁️ Azure Blob Storage (opcional)
# Si no tienes cuenta Azure, déjalo vacío.
AZURE_BLOB_CONN=DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net
````

---

## 🐋 Cómo levantar todo con Docker Compose

Desde la raíz del proyecto:

```bash
docker compose up --build
```

Esto levantará:

| Servicio              | Puerto local | Descripción                          |
| --------------------- | ------------ | ------------------------------------ |
| **Backend (.NET 8)**  | `5000`       | API REST con JWT y CRUD              |
| **Frontend (Nuxt 4)** | `3000`       | Interfaz web con login y CRUD visual |

Cuando todo esté listo:

* Backend: [http://localhost:5000/swagger](http://localhost:5000/swagger)
* Frontend: [http://localhost:3000](http://localhost:3000)

---

## 🔐 Credenciales de prueba

Por defecto, el backend tiene un usuario **hardcoded** para autenticación:

| Campo      | Valor         |
| ---------- | ------------- |
| Usuario    | `test@local`  |
| Contraseña | `Password123` |

---

## 🧠 Flujo de autenticación

1. **Login**

   * Endpoint: `POST /auth/login`
   * Body:

     ```json
     {
       "username": "test@local",
       "password": "Password123"
     }
     ```
   * Devuelve un `token` JWT válido por 8 horas.

2. **Uso del token**

   * Todas las rutas de productos (`/products`, `/products/{id}`, etc.) requieren el header:

     ```
     Authorization: Bearer <token>
     ```

---

## 🛠️ Endpoints principales

### Autenticación
| Método   | Ruta             | Descripción                                     | Auth |
| -------- | ---------------- | ----------------------------------------------- | ---- |
| `POST`   | `/auth/login`    | Login y obtención del JWT                       | ❌    |

### Productos
| Método   | Ruta             | Descripción                                     | Auth |
| -------- | ---------------- | ----------------------------------------------- | ---- |
| `GET`    | `/products`      | Listar productos                                | ✅    |
| `GET`    | `/products/{id}` | Obtener producto por ID                         | ✅    |
| `POST`   | `/products`      | Crear producto (multipart/form-data con imagen) | ✅    |
| `PUT`    | `/products/{id}` | Editar producto                                 | ✅    |
| `DELETE` | `/products/{id}` | Eliminar producto                               | ✅    |

### Ventas
| Método   | Ruta                      | Descripción                          | Auth |
| -------- | ------------------------- | ------------------------------------ | ---- |
| `POST`   | `/products/sales`         | Registrar una venta                  | ✅    |
| `GET`    | `/products/sales-report`  | Descargar reporte CSV por fechas     | ✅    |

**Ejemplo de creación de producto con imagen:**

```bash
curl -X POST http://localhost:5000/products \
  -H "Authorization: Bearer <token>" \
  -F "name=Camisa" \
  -F "description=Camisa azul" \
  -F "price=39.99" \
  -F "image=@camisa.jpg"
```

---

## 💻 Desarrollo local (sin Docker)

Si prefieres correr los servicios manualmente:

### Backend

```bash
cd ProductsApi
dotnet restore
dotnet run --project Web
```

Por defecto corre en `http://localhost:5000`.

**Acceder a Swagger**: http://localhost:5000/swagger

### Frontend

```bash
cd nuxt-app
npm install
npm run dev
```

Accede a `http://localhost:3000`.

> Asegúrate de que la variable `NUXT_PUBLIC_API_BASE` en `.env.development` apunte a `http://localhost:5000`.

---

## ☁️ Configuración opcional de Azure Blob Storage

1. Crea un contenedor en Azure Blob Storage, por ejemplo `product-images`.
2. Obtén la **connection string** en el portal de Azure (Configuración > Claves de acceso).
3. Colócala en la variable `AZURE_BLOB_CONN` de tu `.env`.
4. Las imágenes se subirán automáticamente y la API devolverá una `imageUrl` con el enlace público al blob.

Si no configuras Azure:

* Las imágenes se guardarán temporalmente en `/tmp`.
* La API devolverá una URL `file://` de referencia (suficiente para pruebas locales).

---

## 🧱 Persistencia

* La base de datos usa **SQLite** por defecto → los datos **se persisten** en el archivo `products.db`.
* Ideal para desarrollo y producción ligera.
* Para usar InMemory (solo testing), cambia en `Web/Program.cs`:

  ```csharp
  builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseInMemoryDatabase("ProductsDb"));
  ```

* Para usar SQL Server:

  ```csharp
  builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseSqlServer(connectionString));
  ```

---

## 🧩 Interfaz Frontend

### Login (`/`)

* Formulario de autenticación con validación
* Guarda el token JWT en estado reactivo usando `useState`
* Redirige automáticamente a `/products` tras login exitoso
* Manejo de errores de autenticación

### Productos (`/products`)

* **Listar productos**: Grid responsive con cards que muestran imagen, nombre y precio
* **Crear producto**: Formulario con nombre, descripción, precio y upload de imagen
* **Eliminar producto**: Botón de eliminación por producto
* **Comprar producto**: Registra una venta con un clic
* **Descargar reporte**: Genera CSV de ventas filtrado por rango de fechas

### Características UI

* Componentes modernos de **Nuxt UI** (UCard, UButton, UInput, etc.)
* Diseño responsive con **Tailwind CSS**
* Iconos de **Heroicons**
* Optimización de imágenes con **@nuxt/image**
* Auto-redirección al login si el token expira (401/403)

---

## 🧰 Comandos Docker útiles

```bash
# Construir sin levantar
docker compose build

# Levantar (background)
docker compose up -d

# Detener y eliminar contenedores
docker compose down

# Ver logs
docker compose logs -f
```

---

## 🔎 Rutas rápidas

| URL                                                            | Descripción                         |
| -------------------------------------------------------------- | ----------------------------------- |
| [http://localhost:5000/swagger](http://localhost:5000/swagger) | Documentación interactiva de la API |
| [http://localhost:3000](http://localhost:3000)                 | Interfaz web Nuxt                   |

---

## 🧱 Licencia

Uso libre para fines educativos, de pruebas o evaluación técnica.
Distribución con atribución requerida.