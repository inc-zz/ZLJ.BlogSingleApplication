<template>
  <div class="login-container">
    <div class="login-box">
      <h2 class="login-title">博客管理后台</h2>
      <el-form ref="formRef" :model="loginForm" :rules="rules" size="large">
        <el-form-item prop="account">
          <el-input
            v-model="loginForm.account"
            placeholder="请输入用户名"
            prefix-icon="User"
          />
        </el-form-item>
        <el-form-item prop="password">
          <el-input
            v-model="loginForm.password"
            type="password"
            placeholder="请输入密码"
            prefix-icon="Lock"
            show-password
            @keyup.enter="handleLogin"
          />
        </el-form-item>
        <el-form-item prop="captcha">
          <div class="captcha-wrapper">
            <el-input
              v-model="loginForm.captcha"
              placeholder="请输入验证码"
              prefix-icon="Key"
              maxlength="4"
              @keyup.enter="handleLogin"
            />
            <div class="captcha-image" @click="refreshCaptcha">
              {{ captchaText }}
            </div>
          </div>
        </el-form-item>
        <el-form-item>
          <el-checkbox v-model="loginForm.remember">记住密码</el-checkbox>
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            :loading="loading"
            style="width: 100%"
            @click="handleLogin"
          >
            登录
          </el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage, type FormInstance, type FormRules } from 'element-plus'
import { useUserStore } from '@/stores/user'
import { login } from '@/api/auth'
import type { LoginForm } from '@/types'

const router = useRouter()
const userStore = useUserStore()

const formRef = ref<FormInstance>()
const loading = ref(false)
const captchaText = ref('')

const loginForm = reactive<LoginForm>({
  account: 'admin',
  password: '123456',
  captcha: '',
  remember: false,
})

const rules: FormRules = {
  account: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  password: [{ required: true, message: '请输入密码', trigger: 'blur' }],
  captcha: [
    { required: true, message: '请输入验证码', trigger: 'blur' },
    { len: 4, message: '验证码为4位', trigger: 'blur' },
  ],
}

// 生成随机验证码
const generateCaptcha = () => {
  const chars = 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789'
  let result = ''
  for (let i = 0; i < 4; i++) {
    result += chars.charAt(Math.floor(Math.random() * chars.length))
  }
  return result
}

// 刷新验证码
const refreshCaptcha = () => {
  captchaText.value = generateCaptcha()
  loginForm.captcha = ''
}

const handleLogin = async () => {
  if (!formRef.value) return

  await formRef.value.validate(async (valid) => {
    if (valid) {
      // 验证验证码
      if (loginForm.captcha.toUpperCase() !== captchaText.value.toUpperCase()) {
        ElMessage.error('验证码错误')
        refreshCaptcha()
        return
      }

      loading.value = true
      try {
        const { accessToken, refreshToken, userInfo } = await login(loginForm)
        
        // 存储 token 和用户信息
        userStore.setToken(accessToken, refreshToken)
        userStore.setUserInfo(userInfo)
        
        ElMessage.success('登录成功')
        router.push('/')
      } catch (error) {
        console.error('登录失败:', error)
        refreshCaptcha()
      } finally {
        loading.value = false
      }
    }
  })
}

onMounted(() => {
  refreshCaptcha()
})
</script>

<style scoped lang="scss">
.login-container {
  width: 100%;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);

  .login-box {
    width: 400px;
    padding: 40px;
    background: #fff;
    border-radius: 10px;
    box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);

    .login-title {
      text-align: center;
      margin-bottom: 40px;
      font-size: 24px;
      font-weight: bold;
      color: #303133;
    }

    .captcha-wrapper {
      display: flex;
      gap: 10px;
      width: 100%;

      .el-input {
        flex: 1;
      }

      .captcha-image {
        width: 100px;
        height: 40px;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        border-radius: 4px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 20px;
        font-weight: bold;
        color: #fff;
        letter-spacing: 5px;
        cursor: pointer;
        user-select: none;
        transition: transform 0.2s;

        &:hover {
          transform: scale(1.05);
        }

        &:active {
          transform: scale(0.95);
        }
      }
    }
  }
}
</style>
