# BlogMonolith - .NET 8 单体架构博客系统

## 项目简介

BlogMonolith 是一个基于 .NET 8 构建的单体架构博客系统，采用清晰的分层架构设计，使用 SqlSugar + MySQL + OpenIdDict + Autofac + JWT + Redis 等技术栈实现。

## 技术栈

- **框架**: .NET 8, ASP.NET Core Web API
- **ORM**: SqlSugar
- **数据库**: MySQL
- **认证授权**: OpenIdDict + JWT
- **依赖注入**: Autofac
- **缓存**: Redis
- **对象映射**: AutoMapper
- **验证**: FluentValidation
- **CQRS**: MediatR
- **容器化**: Docker + Docker Compose

## 项目结构

BlogMonolith/
├── 0_Shared/                      # 共享层（所有层都可以引用）
│   ├── Constants/                 # 全局常量
│   ├── Enums/                     # 全局枚举
│   ├── CommonModels/              # 通用模型（不依赖任何业务）
│   └── Extensions/                # 通用扩展方法
│
├── 1_Domain/                      # 领域层（核心业务模型，不依赖其他层）
│   ├── Entities/                  # 领域实体
│   ├── ValueObjects/              # 值对象
│   ├── Events/                    # 领域事件
│   ├── Interfaces/                # 领域服务接口
│   └── Exceptions/                # 领域异常
│
├── 2_Infrastructure/              # 基础设施层（实现技术细节）
│   ├── Persistence/               # 数据持久化
│   │   ├── Repositories/          # 仓储实现（SqlSugar）
│   │   ├── DbContext/             # 数据库上下文
│   │   ├── Migrations/            # 数据库迁移
│   │   └── Seeders/               # 数据种子
│   │
│   ├── Caching/                   # 缓存实现（Redis）
│   ├── Identity/                  # 身份认证实现（OpenIdDict）
│   ├── FileStorage/               # 文件存储实现
│   └── Email/                     # 邮件服务实现
│
├── 3_Application/                 # 应用层（协调领域和基础设施）
│   ├── Services/                  # 应用服务实现
│   ├── DTOs/                      # 数据传输对象
│   ├── Queries/                   # 查询对象
│   ├── Commands/                  # 命令对象
│   ├── Validators/                # 验证器（FluentValidation）
│   ├── Mappers/                   # 对象映射（AutoMapper配置）
│   └── Behaviors/                 # 管道行为（MediatR）
│
├── 4_WebApi/                      # 表现层（Web API）
│   ├── Controllers/               # API控制器
│   ├── Middleware/                # 自定义中间件
│   ├── Filters/                   # 过滤器
│   ├── Attributes/                # 自定义属性
│   ├── ViewModels/                # 视图模型
│   └── Configurations/            # 启动配置
│
└── 5_Tests/                       # 测试项目
    ├── UnitTests/                 # 单元测试
    ├── IntegrationTests/          # 集成测试
    └── TestHelpers/               # 测试辅助类

## 功能特性

- 用户认证与授权 (JWT + OpenIdDict)
- 文章管理 (CRUD, 分类, 标签)
- 评论系统 (多级评论, 审核)
- 文件上传与管理
- 全文搜索功能
- Redis缓存支持
- Docker容器化部署
- API文档 (Swagger/OpenAPI)

## 快速开始

### prerequisites

- .NET 8 SDK
- Docker Desktop
- MySQL 8.0+
- Redis

### 运行步骤

1. 克隆项目

```shell
git clone <repository-url>
cd BlogMonolith
```



1. 配置数据库连接字符串

```json
# 在 appsettings.json 或 appsettings.Development.json 中配置数据库连接
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=blogdb;User=root;Password=your_password;",
  "Redis": "localhost:6379"
}
```

1. 应用数据库迁移

```cmd
dotnet ef database update --project 2_Infrastructure
```

1. 运行项目

```cmd
dotnet run --project 4_WebApi
```

1. 访问 API 文档

```bash
http://localhost:5000/swagger
```

### 使用 Docker 运行

1. 构建并启动容器

```shell
docker-compose up -d
```

1. 查看运行状态

```shell
docker-compose ps
```

1. 停止服务

```bash
docker-compose down
```

## API 文档

项目启动后，访问 `/swagger` 路径查看完整的 API 文档。所有 API 端点都按照 RESTful 原则设计，并使用统一的响应格式。

### 统一响应格式

```bash
{
  "success": true,
  "message": "操作成功",
  "data": { ... },
  "errors": []
}
```

## 开发规范

### 命名约定

- 接口: `I{ServiceName}`
- 实现类: `{ServiceName}`
- 实体类: 单数名词 (如 `User`, `Post`)
- 仓储接口: `I{EntityName}Repository`
- DTO: `{EntityName}Dto`, `Create{EntityName}Dto`, `Update{EntityName}Dto`

### 代码组织原则

1. **依赖方向**: 下层不依赖上层，遵循 0_Shared → 1_Domain → 2_Infrastructure → 3_Application → 4_WebApi 的依赖关系
2. **单一职责**: 每个类/方法只负责一个明确的功能
3. **接口分离**: 使用接口定义契约，实现细节在基础设施层
4. **测试驱动**: 为核心业务逻辑编写单元测试和集成测试

## 测试

### 运行单元测试

```
dotnet test 5_Tests/UnitTests/
```

### 运行集成测试

```
dotnet test 5_Tests/IntegrationTests/
```

## 部署

### 生产环境部署

1. 使用 Docker 生产配置

```bash
docker-compose -f docker-compose.prod.yml up -d
```

1. 使用环境变量覆盖配置

```
export ConnectionStrings__DefaultConnection="Server=prod-mysql;Port=3306;Database=blogdb;User=prod_user;Password=prod_password;"
export ASPNETCORE_ENVIRONMENT=Production
```

## 常见问题

### 数据库连接问题

确保 MySQL 服务已启动，并且连接字符串中的用户名和密码正确。

### Redis 连接问题

检查 Redis 服务是否运行在指定端口，以及防火墙设置是否允许连接。

### 认证问题

确认 JWT 密钥配置正确，并且 Token 在请求头中正确传递。

## 贡献指南

1. Fork 项目
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开 Pull Request

## 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](https://license/) 文件了解详情。

## 联系方式

如有问题或建议，请通过以下方式联系：

- 创建 [Issue](https://github.com/your-username/BlogMonolith/issues)
- 发送邮件至: [your-email@example.com](https://mailto:your-email@example.com/)

## 更新日志

### v1.0.0 (2024-01-01)

- 初始版本发布
- 实现基础博客功能
- 支持 Docker 容器化部署
- 提供完整的 API 文档
