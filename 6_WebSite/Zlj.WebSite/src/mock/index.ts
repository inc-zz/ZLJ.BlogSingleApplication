import Mock from 'mockjs'

// 模拟项目数据
const projects = Mock.mock({
  'personal|5-10': [
    {
      'id|+1': 1,
      name: '@ctitle(3, 5)',
      description: '@cparagraph(1, 3)',
      'technologies|2-4': ['@word(3, 8)'],
      date: '@date("yyyy")',
      'liveUrl': 'https://example.com',
      'githubUrl': 'https://github.com/example/project'
    }
  ],
  'openSource|3-6': [
    {
      'id|+1': 1,
      name: '@ctitle(3, 6)',
      description: '@cparagraph(1, 2)',
      'stars|100-2000': 100,
      'forks|10-200': 10
    }
  ]
})

// 模拟技能数据
const skills = Mock.mock({
  'categories|4-6': [
    {
      'id|+1': 1,
      name: '@ctitle(2, 4)',
      'skills|3-7': [
        {
          'id|+1': 1,
          name: '@word(3, 8)',
          'level|60-95': 60
        }
      ]
    }
  ]
})

// 模拟工作经验数据
const experiences = Mock.mock({
  'data|3-5': [
    {
      'id|+1': 1,
      date: '@date("yyyy-MM")',
      position: '@ctitle(2, 5)',
      company: '@cword(2, 4)公司',
      description: '@cparagraph(1, 3)'
    }
  ]
})

// 模拟教育背景数据
const education = Mock.mock({
  'data|2-3': [
    {
      'id|+1': 1,
      date: '@date("yyyy-MM")',
      degree: '@cword(4, 8)学位',
      institution: '@cword(3, 6)大学',
      description: '@cparagraph(1, 2)'
    }
  ]
})

// 设置 Mock 路由
Mock.mock('/api/projects', 'get', projects)
Mock.mock('/api/skills', 'get', skills)
Mock.mock('/api/experiences', 'get', experiences)
Mock.mock('/api/education', 'get', education)

export default Mock