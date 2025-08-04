# Etapa 1: Build da aplicação
FROM node:20 AS build
WORKDIR /app

# Copiar arquivos de dependências e instalar
COPY package*.json ./
RUN npm install

# Copiar código do projeto
COPY . .

# Gerar build de produção
RUN npm run build

# Etapa 2: Servir com Nginx
FROM nginx:alpine
# Copiar arquivos de build para o Nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Copiar configuração customizada do Nginx (opcional)
# COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expor a porta 80 para acesso externo
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
