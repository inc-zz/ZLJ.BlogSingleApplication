import { request } from '@/utils/request'
import type { PageParams, PageData } from '@/types'

// 菜单信息
export interface Menu {
  id: number
  parentId: number
  name: string
  url: string
  sort: number
  iCon: string | null
  buttons?: string | number[]
  type?: number
  children: Menu[] | null
}

// 菜单树节点
export interface MenuTreeNode {
  id: number
  name: string
  iCon: string
  children: MenuTreeNode[] | null
}

// 菜单列表请求参数
export interface MenuListParams extends PageParams {
  MenuName?: string
  ParentId?: number
}

// 获取菜单列表
export const getMenuList = (params: MenuListParams) => {
  return request.get<PageData<Menu>>('/api/admin/Menu/list', { params })
}

// 获取菜单详情
export const getMenuDetail = (id: number) => {
  return request.get<Menu>('/api/admin/Menu/info', { params: { Id: id } })
}

// 获取菜单树形结构
export const getMenuTree = (status?: number) => {
  const params = status !== undefined ? { Status: status } : {}
  return request.get<MenuTreeNode[]>('/api/admin/Menu/tree', { params })
}

// 创建菜单
export const createMenu = (data: {
  parentId: number
  name: string
  url: string
  icon?: string
  buttons?: number[]
  type: number
  sort: number
}) => {
  return request.post('/api/admin/Menu', data)
}

// 编辑菜单
export const updateMenu = (data: {
  id: number
  parentId: number
  name: string
  url: string
  icon?: string
  buttons?: number[]
  type: number
  sort: number
}) => {
  return request.put('/api/admin/Menu', data)
}

// 删除菜单
export const deleteMenu = (id: number) => {
  return request.delete('/api/admin/Menu', { data: { id } })
}
