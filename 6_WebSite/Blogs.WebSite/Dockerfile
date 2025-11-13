# 构建阶段
FROM node:18-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci --silent    #安装依赖包
COPY . .
RUN npm run build        # 生成 dist 目录

# 运行阶段
FROM nginx:alpine
# 动态生成 nginx 配置（支持 SPA）
RUN echo "server {\n\
    listen 80;\n\
    location / {\n\
        root /usr/share/nginx/html;\n\
        index index.html;\n\
        try_files \$uri \$uri/ /index.html;\n\
    }\n\
}" > /etc/nginx/conf.d/default.conf
# 注意：这里复制的是 dist，不是 build！
COPY --from=builder /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]