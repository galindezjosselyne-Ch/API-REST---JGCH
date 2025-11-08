FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM node:20-alpine AS runner
WORKDIR /app
ENV NUXT_HOST=0.0.0.0
ENV NUXT_PORT=3000
COPY --from=build /app/.output /app/.output
EXPOSE 3000
CMD ["node", ".output/server/index.mjs"]
