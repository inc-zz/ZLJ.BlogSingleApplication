import { request } from '@/utils/request'
import type { PageParams, PageData } from '@/types'

// 部门信息
export interface Department {
  id: number
  name: string
  parentName: string | null
  description: string
  createdAt: string
  createdBy: string
  modifiedAt: string | null
  modifiedBy: string | null
  isDeleted: boolean
  children: Department[] | null
}

// 部门树节点
export interface DepartmentTreeNode {
  id: number
  name: string
  total: number
  children: DepartmentTreeNode[] | null
}

// 部门列表请求参数
export interface DepartmentListParams extends PageParams {
  DepId?: number
  Where?: string
}

// 获取部门列表
export const getDepartmentList = (params: DepartmentListParams) => {
  return request.get<PageData<Department>>('/api/admin/Department/list', { params })
}

// 获取部门树
export const getDepartmentTree = () => {
  return request.get<DepartmentTreeNode[]>('/api/admin/Department/tree')
}

// 获取部门详情
export const getDepartmentDetail = (id: number) => {
  return request.get<Department>('/api/admin/Department/info', { params: { id } })
}

// 创建部门
export const createDepartment = (data: {
  parentId: number
  name: string
  abbreviation?: string
  sort?: number
  description?: string
}) => {
  return request.post('/api/admin/Department', data)
}

// 更新部门
export const updateDepartment = (data: {
  id: number
  parentId: number
  name: string
  abbreviation?: string
  sort?: number
  description?: string
}) => {
  return request.put('/api/admin/Department', data)
}

// 删除部门
export const deleteDepartment = (id: number) => {
  return request.delete('/api/admin/Department', { data: { id } })
}

