import { request } from '@/utils/request'
import type { PageParams, PageData } from '@/types'

// 角色信息
export interface Role {
  id: number
  isSystem: number
  name: string
  code: string
  remark: string | null
  status: number
  createdAt: string
  createdBy: string
  modifiedAt: string | null
  modifiedBy: string | null
  isDeleted: boolean
  statusName: string
}

// 角色列表请求参数
export interface RoleListParams extends PageParams {
  SearchTerm?: string
  Status?: number
}

// 菜单权限
export interface MenuPermission {
  menuId: number
  menuName: string
  menuUrl: string
  parentId: number
  hasPermission: boolean
  buttonPermissions: string[]
}

// 角色授权请求参数
export interface RoleAuthRequest {
  roleId: number
  menuPermissions: {
    menuId: number
    buttonCodes: string[]
  }[]
}

// 获取角色列表
export const getRoleList = (params: RoleListParams) => {
  return request.get<PageData<Role>>('/api/admin/Role/list', { params })
}

// 获取角色详情
export const getRoleDetail = (id: number) => {
  return request.get<Role>('/api/admin/Role/info', { params: { id } })
}

// 添加角色
export const createRole = (data: {
  isSystem: number
  name: string
  code: string
  remark?: string
}) => {
  return request.post('/api/admin/Role', data)
}

// 编辑角色
export const updateRole = (data: {
  id: number
  name: string
  code: string
  isSystem: number
  sort?: number
  remark?: string
}) => {
  return request.put('/api/admin/Role', data)
}

// 删除角色
export const deleteRole = (id: number) => {
  return request.delete('/api/admin/Role', { data: { id } })
}

// 获取角色已授权模块和权限
export const getRoleAuth = (id: number) => {
  return request.get<MenuPermission[]>('/api/admin/Role/roleauth', { params: { id } })
}

// 角色菜单按钮授权请求参数
export interface RoleMenuAuthRequest {
  roleId: number
  roleMenus: {
    menuId: number
    buttonIds: number[]
  }[]
}

// 设置角色菜单权限
export const setRoleMenuAuth = (data: RoleMenuAuthRequest) => {
  return request.put('/api/admin/AuthManager/setMenuAuth', data)
}

// 获取所有角色列表（不分页）
export const getAllRoles = () => {
  return request.get<Role[]>('/api/admin/Role/all')
}

// 菜单按钮权限信息
export interface MenuActionButton {
  menuId: number
  buttonId?: number  // 按钮ID (可选，可能从buttonCode中解析)
  buttonCode: string  // 按钮代码 (可能是数字字符串如"100001"或文本如"Create")
  buttonName: string
  hasPermission: boolean
}

// 菜单权限树节点
export interface MenuPermissionNode {
  menuId: number
  parentId: number
  sort: number
  name: string
  icon: string | null
  menuButtons: MenuActionButton[]
  children: MenuPermissionNode[]
}

// 获取菜单权限树（支持树形结构）
export const getMenuPermissionTree = (roleId?: number) => {
  const params = roleId ? { roleId } : {}
  return request.get<MenuPermissionNode[]>('/api/admin/AuthManager/menuActions', { params })
}
