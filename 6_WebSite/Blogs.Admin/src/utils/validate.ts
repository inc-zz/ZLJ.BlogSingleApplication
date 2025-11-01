// 邮箱验证
export const validateEmail = (email: string): boolean => {
  const reg = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/
  return reg.test(email)
}

// 手机号验证
export const validatePhone = (phone: string): boolean => {
  const reg = /^1[3-9]\d{9}$/
  return reg.test(phone)
}

// 密码强度验证（至少6位，包含字母和数字）
export const validatePassword = (password: string): boolean => {
  const reg = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{6,}$/
  return reg.test(password)
}

// URL验证
export const validateURL = (url: string): boolean => {
  const reg =
    /^(https?:\/\/)?([\da-z.-]+)\.([a-z.]{2,6})([/\w .-]*)*\/?$/
  return reg.test(url)
}

// 用户名验证（4-16位字母数字下划线）
export const validateUsername = (username: string): boolean => {
  const reg = /^[a-zA-Z0-9_]{4,16}$/
  return reg.test(username)
}
