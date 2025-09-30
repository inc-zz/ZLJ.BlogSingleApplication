/**
 * 用户角色类型
 */
export type UserRole = 'admin' | 'editor' | 'user';

/**
 * 用户状态类型
 */
export type UserStatus = 0 | 1;

/**
 * 用户信息模型
 */
export interface UserInfo {
  id: number;
  userName: string;
  realName: string;
  password?: string;
  departmentId?: number;
  phoneNumber?: string;
  email?: string;
  role: UserRole;
  status: UserStatus;
  lastLoginTime?: string;
  createdAt: string;
  description?: string;
}

/**
 * 用户表单数据模型
 */
export interface UserFormData extends Partial<UserInfo> {
  confirmPassword?: string;
}

/**
 * 用户列表响应模型
 */
export interface UserListResponse {
  code: number;
  message?: string;
  items?: UserInfo[];
  data?: UserInfo[];
}

/**
 * 单个用户响应模型
 */
export interface UserResponse {
  code: number;
  message?: string;
  data?: UserInfo;
}