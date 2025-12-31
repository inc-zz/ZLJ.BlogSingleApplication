import { createI18n } from 'vue-i18n'

// 中文语言包
const zh = {
  website: {
    title: 'zhenglijun-独立开发者'
  },
  nav: {
    home: '首页',
    about: '关于我',
    skills: '技术栈',
    projects: '项目展示',
    contact: '联系我'
  },
  footer: {
    contact: '联系方式',
    phone: '电话',
    email: '邮箱',
    follow: '关注我',
    github: 'GitHub',
    gitee: 'Gitee',
    copyright: '© 2025 zhenglijun-独立开发者. 保留所有权利.'
  },
  home: {
    hero: {
      title: '你好，我是郑利军',
      subtitle: '全栈开发工程师',
      description: `我致力于将复杂的技术挑战转化为可靠的业务解决方案，帮助企业构建支撑业务高速发展的技术底座：
                    我能为您解决的业务问题:
                    系统性能瓶颈：通过全链路性能调优，提升用户体验，支撑业务快速增长
                    技术债务困扰：用清晰的架构设计和规范的开发流程，重构遗留系统
                    高并发挑战：设计分布式架构，确保系统在流量高峰期的稳定运行
                    团队效率提升：建立完整的微服务体系和DevOps流程，提升开发效率`,
      capability: {
        title:'能力展示',
        fullStack:{
          icon:'⚽️',
          title:'全栈开发',
          content:'精通前端与后端技术，能够独立开发完整的前后端项目',
        },
        uidesign:{
          icon:'🎨',
          title:'UI/UX设计',
          content:'精通原型/UI设计，注重用户体验，创建美丽且易用的产品'
        },
        optimization:{
          icon:'🚀',
          title:'性能优化',
          content:'熟悉数据库优化，API性能调优，SEO优化，数据存储优化'
        },
        mobileDevelopment:{
          icon:'📱',
          title:'移动端开发',
          content:'熟练使用跨平台APP开发，快速实现响应式应用开发'
        }
      },
      contactBtn: '联系我',
      projectsBtn: '查看项目',
      imageAlt: '个人头像'
    },
    tech: {
      title: '技术栈',
      description: '全栈技术能力图谱，涵盖业务处理、前后端开发、运维及技术支撑'
    },
    skills: {
      title: '核心技能',
      frontendDesc: '精通现代前端框架，包括Vue.js、React和Angular，能够构建响应式和高性能的用户界面。',
      backendDesc: '熟悉后端开发技术，包括Node.js、Python和Java，能够构建稳定可靠的服务器端应用。',
      databaseDesc: '掌握多种数据库技术，包括关系型数据库和NoSQL数据库，能够设计高效的数据存储方案。',
      devopsDesc: '了解DevOps实践，包括容器化部署、持续集成和云服务，能够提高开发效率和系统稳定性。'
    },
    projects: {
      title: '研究成果',
      description: '',
      viewAllBtn: '查看项目',
      list: [
        {
          id: 1,
          name: '企业知识库系统',
          description: '解决企业内部人才培养，知识分享，经验沉淀的难题，快速形成企业知识文化体系，帮助新老员工巩固职业技能，提升工作效率',
          image: '/assets/project1.jpg'
        },
        {
          id: 2,
          name: 'AI与项目集成落地',
          description: '帮助企业快速实现AI集成，AI知识训练，智能客服，帮助企业落地自动化工作流',
          image: '/assets/project2.jpg'
        },
        {
          id: 3,
          name: '企业级微服务架构',
          description: '帮助企业构建高性能微服务架构，处理不同微服务之间数据流转，日志处理，权限管理，性能监控，AI智能体集成，N8N工作流集成，分布式文件存储，中间件管理等一些列架构基础难题',
          image: '/assets/project3.jpg'
        },
        {
          id: 4,
          name: 'Project Gamma1',
          description: 'A real-time collaboration tool for teams',
          image: '/assets/project3.jpg'
        }
      ]
    },
    clients: {
      title: '项目集成',
      description: '每一份耕耘，必定会有收获~',
      list:[
        {
          id: 1,
          name: '信任的力量',
          description: '虚位以待',
          logo: '/assets/client1.jpg'
        },
        {
          id: 2,
          name: '从来都是',
          description: '虚位以待',
          logo: '/assets/client2.jpg'
        },
        {
          id: 3,
          name: '日积月累',
          description: '虚位以待',
          logo: '/assets/client3.jpg'
        }
      ]
    }
  },
  about: {
    title: '关于我',
    subtitle: '我的职业历程',
    description: '我是一名全栈开发工程师，拥有12年的软件开发经验。我热爱技术，喜欢学习新技术并将其应用到实际项目中。',
    passion: '我对编程充满热情，始终追求编写高质量、可维护的代码。我相信技术可以改变世界，希望通过我的技能为社会创造价值。',
    imageAlt: '个人照片',
    experience: {
      title: '工作经历',
      description: '以下是我的主要工作经历，展示了我在不同公司和项目中的成长历程。',
      job1: {
        date: '2025年至今',
        position: '自由职业开发者',
        company: '个人开发者',
        description: '主要研究微服务架构,微服务数据对接，第三方对接服务，数据监控，性能调优，AI集成，服务器运维以及盈利模式探索'
      },
      job2: {
        date: '2021-2025',
        position: '全栈开发工程师',
        company: '医疗互联网行业',
        description: '参与基础架构服务开发，docker/k8s集成，第三方对接，信息通信，云存储服务研发。'
      },
      job3: {
        date: '2018-2021',
        position: 'dotNet开发工程师',
        company: '进出口报关行业',
        description: '参与公司系统研发，项目重构架构升级和优化，定制化服务开发。'
      },
      job4: {
        date: '2013-2018',
        position: 'dotNet开发工程师',
        company: '某民营互联网软件公司',
        description: '参与公司项目需求分析，业务代码研发，需求对接，后期升级与维护。'
      }
    },
    education: {
      title: '教育背景',
      description: '以下是我的教育背景，为我的技术生涯奠定了坚实的基础。',
      edu1: {
        date: '2011-2013',
        degree: '民办学校',
        institution: '某大学',
        description: '在校期间主修计算机科学相关课程，包括数据结构、算法、数据库和网络编程等。参与多个课程项目，积累了丰富的实践经验。'
      },
      edu2: {
        date: '2011-2013',
        degree: 'About my school',
        institution: 'A Graduate School',
        description: 'In-depth study of software engineering theory and practice, focusing on web application development and distributed system design. Completed master\'s thesis "Design and Implementation of E-commerce Platform Based on Microservices Architecture".'
      }
    }
  },
  skills: {
    title: '技术栈',
    subtitle: '我的技能概览',
    description: '我拥有全面的技术开发能力，能给你带来业务上的突破，期待跟您的沟通，下面是我的一些技术栈展示',
    frontend: '前端开发',
    backend: '后端开发',
    database: '数据库',
    devops: '运维部署',
    categories: {
      frontend: '前端技术',
      backend: '后端技术',
      database: '数据库',
      tools: '开发工具'
    },
    projects: {
      title: '技能应用案例',
      description: '以下是一些展示我技能应用的项目案例。',
      project1: {
        title: '电商平台前端',
        description: '使用Vue.js构建的现代化电商平台前端，支持响应式设计和高性能渲染。'
      },
      project2: {
        title: '数据管理系统',
        description: '使用React和Node.js构建的数据管理系统，支持复杂的数据查询和可视化展示。'
      },
      project3: {
        title: '移动应用',
        description: '使用Angular和Ionic构建的跨平台移动应用，支持iOS和Android平台。'
      }
    },
    innerAreas:[
      { name: '业务处理', color: '#ef4444' },    // 红
      { name: '前端开发', color: '#f97316' },    // 橙
      { name: '后端开发', color: '#22c55e' },    // 绿
      { name: '服务器运维', color: '#3b82f6' },  // 蓝
      { name: '技术支撑', color: '#a855f7' }     // 紫
    ]
  },
  projects: {
    title: '项目展示',
    subtitle: '我的作品集',
    description: '以下是我参与或主导的一些项目，涵盖了不同的技术栈和应用场景。',
    personal: {
      title: '项目展示',
      description: '以下是我开源的自研项目，主要解决中小企业dotNet前后端分离开发架构',
      list: [
          {
            id: 1,
            name: '企业知识分享平台',
            description: '一个功能强大的企业内部知识分享与博客系统，旨在集中存储、管理和分享企业知识资产。支持多层级权限管理，确保知识安全；提供全文搜索、标签分类、评论互动和版本控制等功能，便于员工快速查找和学习。同时，系统支持响应式布局，方便多终端访问，是企业知识沉淀、员工培训与团队协作的理想平台',
            image: '/assets/website1.jpg',
            technologies: ['React', 'DotNet8', 'DDD'],
            date: '2024',
            liveUrl: 'https://blog.zhenglijun.com.com',
            githubUrl: 'https://github.com/inc-zz/ZLJ.BlogSingleApplication'
          },
          {
            id: 2,
            name: '权限管理平台',
            description: '本平台是一个基于前后端分离架构的权限管理系统，为企业项目开发提供高效起点。它集成了用户、角色、权限等核心基础服务，并内置了操作日志、性能监控与链路追踪等可观测性功能。平台提供清晰的API接口，模块化设计易于二次开发，能显著减少重复工作，帮助企业团队快速构建安全、稳定且可扩展的后台管理系统，有效降低开发成本，加速项目上线。',
            image: '/assets/website2.jpg', 
            technologies: ['SqlSugar', 'AbpVnext', 'MySql'],
            date: '2025',
            liveUrl: 'https://admin.zhenglijun.com',
            githubUrl: 'https://github.com/inc-zz'
          },
          {
            id: 3,
            name: '独立站电商微服务架构',
            description: '一个基于Dapr和.NET 8构建的企业级电商独立站微服务系统。采用Dapr分布式应用运行时，提供包括服务调用、状态管理、发布订阅、配置管理、分布式跟踪等构建块。系统集成Ocelot作为API网关，使用Dapr内置服务发现，Nacos作为配置中心，ELK日志中心，Skywallking链路追踪，Nexus包管理，Harbor镜像仓库，全程支持Docker容器化部署。为企业电商业务提供稳定可靠的技术支撑',
            image: '/assets/website3.jpg',
            technologies: ['DotNet10', 'React', 'Dapr'],
            date: '2026',
            liveUrl: 'https://shop.zhenglijun.com',
            githubUrl: 'https://github.com/inc-zz'
          }
        ]
    },
    details: {
      title: '项目详细信息',
      description: '关于企业级微服务架构设计',
      painPoints: {
        title: '业务痛点',
        list: [
          '平台流量限制，导致推广成本过高',
          '同行竞争激烈，产品同质化严重，利润偏低',
          '平台抽取过高的手续费，竞价排行增加运营成本',
          '定制化困难，成本过高'
        ]
      },
      techStack: {
        title: '技术栈',
        datas:{
          frontend: {
            name: '前端技术',
            items: ['Vue3', 'TypeScript', 'Layui', 'React']
          },
          backend: {
            name: '后端技术',
            items: ['DotNet', 'Dapr', 'Redis', 'AbpVnext']  
          },
          devops: {
            name: '运维部署',
            items: ['Docker', 'Nginx', 'Jenkins', 'K8s']
          },
          tools: {
            name: '数据库',
            items: ['MySql', 'PgSql', 'MsSql', 'Sqlite']
          }
        }
      },
      futurePlans: {
        title: '未来升级方向',
        list: [
          '在不同架构系统中抽离一套完整的开发框架，以此为基础研发高性能高可用微服务架构',
          '集成AI应用，从数据源到用户行为分析，形成用户行为数据库',
          '添加实时聊天客服功能，提升用户服务体验',
          '引入大数据分析平台，实现精准营销和智能运营'
        ]
      },
      deployment: {
        title: '项目部署方案',

      }
    },
    viewLive: '访问项目',
    viewCode: '访问源码',
    viewProject: '查看项目'
  },
  contact: {
    title: '合作共赢,携手共进',
    subtitle: '保持联系',
    description: '如果您有任何问题或合作机会，请随时通过以下方式联系我。',
    info: {
      title: '联系信息',
      phone: '电话',
      email: '邮箱'
    },
    social: {
      title: '社交媒体'
    },
    form: {
      title: '发送消息',
      name: '姓名',
      namePlaceholder: '请输入您的姓名',
      email: '邮箱',
      emailPlaceholder: '请输入您的邮箱',
      subject: '主题',
      subjectPlaceholder: '请输入消息主题',
      message: '消息',
      messagePlaceholder: '请输入您的消息',
      send: '发送消息',
      sending: '发送中...',
      success: '消息发送成功！我会尽快回复您。',
      suggestionSuccess: '您的宝贵建议我们已经收到，感谢您的提议。'
    }
  }
}

// 英文语言包
const en = {
  website: {
    title: 'zhenglijun Independent developer'
  },
  nav: {
    home: 'Home',
    about: 'About',
    skills: 'Skills',
    projects: 'Projects',
    contact: 'Contact'
  },
  footer: {
    contact: 'Contact',
    phone: 'Phone',
    email: 'Email',
    follow: 'Follow Me',
    github: 'GitHub',
    gitee: 'Gitee',
    copyright: '© 2025 zhenglijun Independent developer. All rights reserved.',
    backupNumber: '备案号: <a href="https://beian.miit.gov.cn/">粤ICP备2025481000号</a>'
  },
  home: {
    hero: {
      title: 'Hello, I\'m Zhenglijun',
      subtitle: 'Full Stack Developer',
      description: `I am committed to transforming complex technological challenges into reliable business solutions, helping businesses build a technological foundation that supports rapid business development
The business problems I can solve for you:
System performance bottleneck: By optimizing the performance of the entire chain, improving user experience, and supporting rapid business growth
Technical debt dilemma: Refactoring legacy systems with clear architecture design and standardized development processes
High concurrency challenge: Design a distributed architecture to ensure stable operation of the system during peak traffic periods
Team efficiency improvement: Establish a complete microservice system and DevOps process to enhance development efficiency`,
      capability:{
        title:'Capability Demonstration',
        fullStack:{
          icon:'⚽️',
          title:'Full stack development',
          content:'Proficient in front-end and back-end technologies, able to independently develop complete front-end and back-end projects',
        },
        uidesign:{
          icon:'🎨',
          title:'UI/UX Design',
          content:'Proficient in prototyping/UI design, emphasizing user experience, creating beautiful and easy-to-use products'
        },
        optimization:{
          icon:'🚀',
          title:'Performance optimization',
          content:'Familiar with backend database optimization, API performance optimization, SEO optimization'
        },
        mobileDevelopment:{
          icon:'📱',
          title:'Mobile Development',
          content:'Proficient in using cross platform app development to quickly achieve responsive application development'
        }
      },
      contactBtn: 'Contact Me',
      projectsBtn: 'View Projects',
      imageAlt: 'Profile Picture'
    },
    tech: {
      title: 'Tech Stack',
      description: 'Full-stack technical capability map, covering business processing, front-end and back-end development, operations and technical support'
    },
    skills: {
      title: 'Core Skills',
      frontendDesc: 'Proficient in modern front-end frameworks including Vue.js, React, and Angular, capable of building responsive and high-performance user interfaces.',
      backendDesc: 'Familiar with back-end development technologies including Node.js, Python, and Java, capable of building stable and reliable server-side applications.',
      databaseDesc: 'Master multiple database technologies including relational and NoSQL databases, capable of designing efficient data storage solutions.',
      devopsDesc: 'Understand DevOps practices including containerized deployment, continuous integration, and cloud services, capable of improving development efficiency and system stability.'
    },
    projects: {
      title: 'New project',
      description: 'Here are some of my recent projects that cover different technology stacks and application scenarios.',
      viewAllBtn: 'View All Projects',
      list: [
        {
          id: 1,
          name: 'Enterprise Knowledge Base System',
          description: `Resolve the challenges of talent cultivation, knowledge sharing, and experience accumulation within the enterprise, quickly establish a corporate knowledge and culture system, help new and old employees consolidate their professional skills, and improve work efficiency`,
          image: '/assets/project1.jpg'
        },
        {
          id: 2,
          name: 'AI and project integration landing',
          description: 'Help enterprises quickly achieve AI integration, AI knowledge training, intelligent customer service, and assist enterprises in implementing automated workflows',
          image: '/assets/project2.jpg'
        },
        {
          id: 3,
          name: 'Enterprise level microservice architecture',
          description: 'Assist enterprises in building high-performance microservice architectures, handling data flow between different microservices, log processing, permission management, performance monitoring, AI agent integration, N8N workflow integration, distributed file storage, middleware management, and other fundamental architectural challenges',
          image: '/assets/project3.jpg'
        },
        {
          id: 4,
          name: 'Project Gamma1',
          description: 'A real-time collaboration tool for teams',
          image: '/assets/project3.jpg'
        }
      ]
    },
    clients: {
      title: 'Our Clients',
      description: 'Trusted partners, witnessing growth together',
      list:[
        {
          id: 1,
          name: 'The Power of Trust',
          description: 'Looking forward to cooperation',
          logo: '/assets/client1.jpg'
        },
        {
          id: 2,
          name: 'Always have been',
          description: 'Looking forward to cooperation',
          logo: '/assets/client2.jpg'
        },
        {
          id: 3,
          name: 'Rome was not built in a day',
          description: 'Looking forward to cooperation',
          logo: '/assets/client3.jpg'
        }
      ]
    }
  },
  about: {
    title: 'About Me',
    subtitle: 'My Career Journey',
    description: 'I am a full stack developer with 12 years of software development experience. I am passionate about technology and enjoy learning new technologies and applying them to real projects.',
    passion: 'I am passionate about programming and always strive to write high-quality, maintainable code. I believe technology can change the world and hope to create value for society through my skills.',
    imageAlt: 'Personal Photo',
    experience: {
      title: 'Work Experience',
      description: 'Here is my main work experience, showing my growth journey in different companies and projects.',
      job1: {
        date: '2020-2024',
        position: 'Senior Front-end Engineer',
        company: 'A Tech Company',
        description: 'Responsible for front-end development of the company\'s core products, using Vue.js and React to build high-performance user interfaces. Led the front-end team to complete multiple important projects, improving team development efficiency.'
      },
      job2: {
        date: '2018-2020',
        position: 'Full Stack Developer',
        company: 'An Internet Company',
        description: 'Participated in full stack development of multiple company products, using Node.js and Python to build back-end services, and React and Vue.js to build front-end interfaces.'
      },
      job3: {
        date: '2013-2018',
        position: 'Junior Developer',
        company: 'A Software Company',
        description: 'Participated in the development of internal management systems, using Java and Spring framework to build back-end services, and jQuery and Bootstrap to build front-end interfaces.'
      }
    },
    education: {
      title: 'Education',
      description: 'Here is my educational background, which laid a solid foundation for my technical career.',
      edu1: {
        date: '2012-2016',
        degree: 'Bachelor of Computer Science and Technology',
        institution: 'A University',
        description: 'Majoring in computer science related courses during school, including data structures, algorithms, databases, and network programming. Participated in multiple course projects and accumulated rich practical experience.'
      },
      edu2: {
        date: '2016-2018',
        degree: 'Master of Software Engineering',
        institution: 'A Graduate School',
        description: 'In-depth study of software engineering theory and practice, focusing on web application development and distributed system design. Completed master\'s thesis "Design and Implementation of E-commerce Platform Based on Microservices Architecture".'
      }
    }
  },
  skills: {
    title: 'Skills',
    subtitle: 'My Skill Overview',
    description: 'Here are my main technical skills, organized and displayed by category.',
    frontend: 'Frontend Development',
    backend: 'Backend Development',
    database: 'Database',
    devops: 'DevOps',
    categories: {
      frontend: 'Frontend Technologies',
      backend: 'Backend Technologies',
      database: 'Database',
      tools: 'Development Tools'
    },
    projects: {
      title: 'Skill Application Cases',
      description: 'Here are some project cases demonstrating my skill application.',
      project1: {
        title: '电商独立站',
        description: 'Modern e-commerce platform frontend built with Vue.js, supporting responsive design and high-performance rendering.'
      },
      project2: {
        title: 'Data Management System',
        description: 'Data management system built with React and Node.js, supporting complex data queries and visualization display.'
      },
      project3: {
        title: 'Mobile Application',
        description: 'Cross-platform mobile application built with Angular and Ionic, supporting iOS and Android platforms.'
      }
    },
    innerAreas:
    {

      
      list:[
        { name: 'Product R&D', color: '#ef4444' },    // 红
        { name: 'Client Dev', color: '#f97316' },    // 橙
        { name: 'API Dev', color: '#22c55e' },    // 绿
        { name: 'Server Ops', color: '#3b82f6' },  // 蓝
        { name: 'Tech Support', color: '#a855f7' }     // 紫
      ]
    }
  },
  projects: {
    title: 'Projects',
    subtitle: 'My Portfolio',
    description: 'Here are some projects I have participated in or led, covering different technology stacks and application scenarios.',
    personal: {
      title: 'Personal Projects',
      description: 'Here are my personal projects completed independently or led, demonstrating my technical capabilities and creativity.',
      list: [
          {
            id: 1,
            name: 'E-commerce Platform',
            description: 'A full-featured e-commerce platform with product catalog, shopping cart, and payment integration.',
            image: '/assets/website1.jpg',
            technologies: ['React', 'WebApi', 'Redis'],
            date: '2023',
            liveUrl: 'https://github.com/',
            githubUrl: 'https://github.com/inc-zz'
          },
          {
            id: 2,
            name: 'Task Management App',
            description: 'A collaborative task management application with real-time updates and team features.',
            image: '/assets/website2.jpg',
            technologies: ['Vue3', 'DDD', 'CQRS'],
            date: '2022',
            liveUrl: 'https://github.com',
            githubUrl: 'https://github.com/inc-zz'
          },
          {
            id: 3,
            name: 'Weather Dashboard',
            description: 'A responsive weather dashboard with location-based forecasts and historical data visualization.',
            image: '/assets/website3.jpg',
            technologies: ['Dapr', 'AbpVnext', 'MySql'],
            date: '2021',
            liveUrl: 'https://github.com/inc-zz',
            githubUrl: 'https://github.com/inc-zz'
          }
        ]
    },
    details: {
      title: 'Project Technical Details',
      description: 'About Enterprise level Microservice Architecture Design',
      architecture: {
        title: 'System Architecture',
        description: 'The project adopts a microservices architecture, with front-end built using Vue.js, back-end using Node.js and Python, and databases using MongoDB and Redis.'
      },
      challenges: {
        title: 'Technical Challenges',
        description: 'The main technical challenges faced by the project include high-concurrency processing, data consistency assurance, and system scalability design.'
      },
      solutions: {
        title: 'Solutions',
        description: 'Successfully solved technical challenges in the project through technical means such as load balancing, caching strategies, and database sharding.'
      },
      painPoints: {
        title: 'Business pain points',
        list: [
          'Platform traffic restrictions result in excessively high promotion costs',
          'Intense competition among peers, severe product homogenization, and low profits',
          'The platform extracts excessively high transaction fees, resulting in increased operating costs for bidding rankings',
          'Customization is difficult and the cost is too high'
        ]
      },
      techStack: {
        title: 'Tech stack',
        datas:{
          frontend: {
            name: 'Frontend Technology',
            items: ['Vue 3', 'TypeScript', 'Pinia', 'Element Plus', 'Vite']
          },
          backend: {
            name: 'Backend Technology',
            items: ['Node.js', 'Express', 'Redis', 'MongoDB', 'MySQL']
          },
          devops: {
            name: 'Devops',
            items: ['Docker', 'Nginx', 'Jenkins', 'AWS', 'PM2']
          },
          tools: {
            name: 'Tools',
            items: ['Git', 'Webpack', 'ESLint', 'Jest', 'Postman']
          }
        }
      },
      futurePlans: {
        title: 'optimization direction',
        list: [
          'Extract a complete development framework from different architecture systems and develop high-performance and highly available microservice architectures based on it',
          'Integrating AI applications, from data sources to user behavior analysis, to form a user behavior database,',
          'Add real-time chat customer service function to enhance user service experience',
          'Introduce a big data analysis platform to achieve precise marketing and intelligent operations'
        ]
      },
      deployment: {
        title: 'Project Upgrade plan',
        
      }
    },
    
    viewLive: 'View Demo',
    viewCode: 'View Code',
    viewProject: 'View Project'
  },
  contact: {
    title: 'Contact Me',
    subtitle: 'Get In Touch',
    description: 'If you have any questions or collaboration opportunities, please feel free to contact me through the following methods.',
    info: {
      title: 'Contact Information',
      phone: 'Phone',
      email: 'Email'
    },
    social: {
      title: 'Social Media'
    },
    form: {
      title: 'Send Message',
      name: 'Name',
      namePlaceholder: 'Please enter your name',
      email: 'Email',
      emailPlaceholder: 'Please enter your email',
      subject: 'Subject',
      subjectPlaceholder: 'Please enter message subject',
      message: 'Message',
      messagePlaceholder: 'Please enter your message',
      send: 'Send Message',
      sending: 'Sending...',
      success: 'Message sent successfully! I will reply to you as soon as possible.',
      suggestionSuccess: 'We have received your valuable suggestions, thank you for your proposal.'
    }
  },
  tellMe:{
    title: 'Contact Me',
    phone: {
      title:'Phone Number',
      value:'+86 17302602302'
    },
    email: {
      title: 'Email',
      value:'392090057@qq.com'
    },
    wechat:{
      title: 'WeChat',
      value:'zlj392090057'
    },
    whatsApp:{
      title:'WhatsApp',
      value:'+86 17302602302'
    }
  }
}

const i18n = createI18n({
  legacy: false, // 使用 Composition API 模式
  locale: 'zh', // 默认语言
  fallbackLocale: 'en', // 回退语言
  messages: {
    zh,
    en
  }
})

export default i18n