# 后端接口集成说明

## 概述

本文档说明了前端如何与后端 API 集成，包括接口调用、数据格式和配置说明。

## 后端 API 配置

### API 基础地址
- **开发环境**: `https://localhost:7235`
- **配置文件**: `.env.development`

```bash
VITE_API_BASE_URL=https://localhost:7235
```

### SSL 证书处理
由于后端使用 HTTPS 自签名证书，Vite 配置中已设置：
```typescript
proxy: {
  '/api': {
    target: 'https://localhost:7235',
    changeOrigin: true,
    secure: false,  // 忽略 SSL 证书验证
    rewrite: (path) => path,
  },
}
```

## 登录接口集成

### 接口详情

**URL**: `/api/admin/Account/login`  
**Method**: `POST`  
**Content-Type**: `application/json`

### 请求参数

```typescript
interface LoginForm {
  account: string      // 用户账号
  password: string     // 密码
  captcha: string      // 4位验证码
  remember?: boolean   // 记住密码（可选）
}
```

示例：
```json
{
  "account": "admin",
  "password": "123456",
  "captcha": "SE32"
}
```

### 响应数据格式

```typescript
interface LoginResponse {
  data: {
    userInfo: {
      userName: string       // 用户名
      realName: string       // 真实姓名
      phoneNumber: string    // 手机号
      email: string         // 邮箱
    }
    accessToken: string     // 访问令牌
    refreshToken: string    // 刷新令牌
    expiresIn: string       // 过期时间
    tokenType: string       // 令牌类型 (Bearer)
  }
  success: boolean          // 成功标志
  message: string           // 响应消息
  code: number             // 状态码
}
```

示例响应：
```json
{
  "data": {
    "userInfo": {
      "userName": "admin",
      "realName": "admin",
      "phoneNumber": "15816814415",
      "email": "admin@sing.com"
    },
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "83302f74ee9d48a1baef08ddde61161f",
    "expiresIn": "2025-10-30T10:11:41.9943325+08:00",
    "tokenType": "Bearer"
  },
  "success": true,
  "message": "登录成功",
  "code": 200
}
```

## 前端实现细节

### 1. 类型定义更新

**文件**: `src/types/index.ts`

```typescript
// 更新通用响应接口，增加 success 字段
export interface ApiResponse<T = any> {
  code: number
  message: string
  data: T
  success: boolean  // 新增
}

// 更新用户信息结构
export interface UserInfo {
  userName: string      // 由 username 改为 userName
  realName: string      // 新增
  phoneNumber: string   // 新增
  email: string
  avatar?: string
}

// 更新登录表单
export interface LoginForm {
  account: string       // 由 username 改为 account
  password: string
  captcha: string       // 新增验证码
  remember?: boolean
}

// 新增登录响应类型
export interface LoginResponse {
  userInfo: UserInfo
  accessToken: string
  refreshToken: string
  expiresIn: string
  tokenType: string
}
```

### 2. API 接口更新

**文件**: `src/api/auth.ts`

```typescript
// 登录接口
export const login = (data: LoginForm) => {
  return request.post<LoginResponse>('/api/admin/Account/login', data)
}

// 获取用户信息
export const getUserInfo = () => {
  return request.get('/api/admin/Account/userinfo')
}
```

### 3. 状态管理更新

**文件**: `src/stores/user.ts`

增加了 `refreshToken` 的存储和管理：

```typescript
const refreshToken = ref<string>(storage.get('admin_refresh_token') || '')

const setToken = (newToken: string, newRefreshToken?: string) => {
  token.value = newToken
  storage.set(STORAGE_KEYS.TOKEN, newToken)
  
  if (newRefreshToken) {
    refreshToken.value = newRefreshToken
    storage.set('admin_refresh_token', newRefreshToken)
  }
}
```

### 4. 响应拦截器更新

**文件**: `src/utils/request.ts`

```typescript
service.interceptors.response.use(
  (response: AxiosResponse) => {
    const { code, message, data, success } = response.data

    // 检查 success 或 code
    if (success || code === 200 || code === 0) {
      return data
    } else if (code === 401) {
      // 处理未授权
      const userStore = useUserStore()
      userStore.logout()
      router.push('/login')
      ElMessage.error('登录已过期，请重新登录')
      return Promise.reject(new Error(message || '登录已过期'))
    } else {
      ElMessage.error(message || '请求失败')
      return Promise.reject(new Error(message || '请求失败'))
    }
  },
  // ... 错误处理
)
```

### 5. 登录页面更新

**文件**: `src/views/login/index.vue`

#### 新增功能：

1. **验证码输入框**
   ```vue
   <el-form-item prop="captcha">
     <div class="captcha-wrapper">
       <el-input
         v-model="loginForm.captcha"
         placeholder="请输入验证码"
         prefix-icon="Key"
         maxlength="4"
       />
       <div class="captcha-image" @click="refreshCaptcha">
         {{ captchaText }}
       </div>
     </div>
   </el-form-item>
   ```

2. **验证码生成逻辑**
   ```typescript
   const generateCaptcha = () => {
     const chars = 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789'
     let result = ''
     for (let i = 0; i < 4; i++) {
       result += chars.charAt(Math.floor(Math.random() * chars.length))
     }
     return result
   }
   ```

3. **验证码验证**
   ```typescript
   // 验证验证码
   if (loginForm.captcha.toUpperCase() !== captchaText.value.toUpperCase()) {
     ElMessage.error('验证码错误')
     refreshCaptcha()
     return
   }
   ```

4. **登录数据处理**
   ```typescript
   const { accessToken, refreshToken, userInfo } = await login(loginForm)
   
   // 存储 token 和用户信息
   userStore.setToken(accessToken, refreshToken)
   userStore.setUserInfo(userInfo)
   ```

## 数据存储

### LocalStorage 存储项

1. **admin_token**: 访问令牌 (accessToken)
2. **admin_refresh_token**: 刷新令牌
3. **admin_user_info**: 用户信息对象

```typescript
// 存储示例
{
  "userName": "admin",
  "realName": "admin",
  "phoneNumber": "15816814415",
  "email": "admin@sing.com"
}
```

## 请求头配置

所有 API 请求会自动携带 Authorization 请求头：

```typescript
// 请求拦截器自动添加
if (userStore.token) {
  config.headers.Authorization = `Bearer ${userStore.token}`
}
```

格式：`Bearer {accessToken}`

## Mock 数据配置

Mock 数据已被禁用，所有请求都会发送到真实后端：

```typescript
// vite.config.ts
viteMockServe({
  mockPath: 'mock',
  enable: false, // 禁用 Mock
})
```

## 错误处理

### 401 未授权
- 自动清除本地存储
- 跳转到登录页
- 显示"登录已过期"提示

### 其他错误
- 显示后端返回的错误消息
- 登录失败时刷新验证码

## 使用说明

### 1. 启动后端服务
确保后端服务运行在 `https://localhost:7235`

### 2. 启动前端开发服务器
```bash
npm run dev
```

### 3. 登录系统
- **账号**: admin
- **密码**: 123456
- **验证码**: 输入页面显示的4位验证码

### 4. 验证码特性
- 自动生成4位随机验证码
- 不区分大小写
- 登录失败或点击验证码区域会刷新
- 登录成功后自动存储用户信息

## 注意事项

1. **HTTPS 证书**: 开发环境使用自签名证书，已配置跳过验证
2. **跨域处理**: 通过 Vite proxy 代理解决跨域问题
3. **Token 过期**: 当前版本未实现自动刷新，需要重新登录
4. **验证码**: 前端生成随机验证码，实际应从后端获取

## 后续扩展建议

1. **Token 刷新机制**
   - 监听 token 过期时间
   - 使用 refreshToken 自动刷新
   - 无感知续期

2. **验证码优化**
   - 从后端获取图形验证码
   - 增加验证码失效时间
   - 支持刷新验证码接口

3. **用户信息完善**
   - 添加用户头像上传
   - 支持修改个人信息
   - 角色权限管理

4. **请求重试机制**
   - 网络错误自动重试
   - 指数退避策略

## 调试技巧

### 查看请求详情
打开浏览器开发者工具 -> Network 标签页

### 查看存储数据
开发者工具 -> Application -> Local Storage

### API 测试
可以使用 Postman 或类似工具测试后端接口

## 常见问题

### Q: 提示"登录已过期"
A: 清除浏览器 LocalStorage 或重新登录

### Q: 验证码一直显示错误
A: 验证码不区分大小写，点击验证码区域刷新

### Q: 跨域错误
A: 确保 Vite 配置的 proxy 正确，后端允许跨域请求

### Q: SSL 证书错误
A: 已在配置中设置 `secure: false`，如仍有问题，检查防火墙设置
