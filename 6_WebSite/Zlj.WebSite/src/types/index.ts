// 项目接口
export interface Project {
  id: number
  name: string
  description: string
  image?: string
  technologies: string[]
  date: string
  liveUrl?: string
  githubUrl?: string
}

// 开源项目接口
export interface OpenSourceProject {
  id: number
  name: string
  description: string
  thumbnail?: string
  stars: number
  forks: number
  url?: string
}

// 技能接口
export interface Skill {
  id: number
  name: string
  level: number
}

// 技能分类接口
export interface SkillCategory {
  id: number
  name: string
  skills: Skill[]
}

// 工作经验接口
export interface Experience {
  id: number
  date: string
  position: string
  company: string
  description: string
}

// 教育背景接口
export interface Education {
  id: number
  date: string
  degree: string
  institution: string
  description?: string
}

// 联系表单接口
export interface ContactForm {
  name: string
  email: string
  subject: string
  message: string
}