// SPA路由和面包屑管理
const breadcrumbManager = {
    // 存储打开的页面
    openPages: [],
    
    // 初始化
    init: function() {
        // 确保在DOM加载完成后初始化
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', this.initialize.bind(this));
        } else {
            this.initialize();
        }
    },
    
    initialize: function() {
        this.setupMenuClickHandlers();
        this.setupBreadcrumbContextMenu();
        this.loadCurrentPage();
    },
    
    // 设置菜单点击处理
   
setupMenuClickHandlers: function() {
    const menuItems = document.querySelectorAll('.sidebar-menu a:not(.submenu-toggle)');
    
    menuItems.forEach(item => {
        item.addEventListener('click', function(e) {
            // 阻止默认的页面跳转
            e.preventDefault();
            
            const url = this.getAttribute('href');
            // 修复：检查是否有span元素，如果没有则直接获取a标签的文本内容
            const spanElement = this.querySelector('span');
            const title = spanElement ? spanElement.textContent : this.textContent.trim();
            const icon = this.querySelector('i')?.className || '';
            
            // 添加到面包屑
            breadcrumbManager.addBreadcrumbItem(url, title, icon);
            
            // 更新菜单激活状态
            menuItems.forEach(i => i.classList.remove('active'));
            this.classList.add('active');
            
            // 加载页面内容
            breadcrumbManager.loadPageContent(url);
            
            // 添加到浏览器历史但不刷新页面
            history.pushState({ url, title }, title, url);
        });
    });
},
    
    // 添加面包屑项
    addBreadcrumbItem: function(url, title, icon) {
        // 检查是否已存在相同的页面
        const existingIndex = this.openPages.findIndex(page => page.url === url);
        
        if (existingIndex === -1) {
            // 新建页面记录
            const pageId = 'page-' + Date.now();
            this.openPages.push({ id: pageId, url, title, icon });
            
            // 更新面包屑UI
            this.renderBreadcrumb();
        } else {
            // 已存在的页面，直接激活
            this.activateBreadcrumbItem(existingIndex);
        }
    },
    
    // 渲染面包屑
    renderBreadcrumb: function() {
        const breadcrumbList = document.getElementById('breadcrumb-list');
        breadcrumbList.innerHTML = '';
        
        // 添加首页
        const homeItem = document.createElement('li');
        homeItem.className = 'breadcrumb-item';
        homeItem.innerHTML = '<a href="/" class="breadcrumb-link"><i class="fas fa-home"></i> 首页</a>';
        homeItem.querySelector('a').addEventListener('click', function(e) {
            e.preventDefault();
            breadcrumbManager.navigateToHome();
        });
        breadcrumbList.appendChild(homeItem);
        
        // 添加其他页面
        this.openPages.forEach((page, index) => {
            const item = document.createElement('li');
            item.className = 'breadcrumb-item breadcrumb-page';
            item.setAttribute('data-page-id', page.id);
            item.setAttribute('data-index', index);
            
            item.innerHTML = `
                <div class="breadcrumb-item-content">
                    <a href="${page.url}" class="breadcrumb-link">${page.title}</a>
                    <button class="breadcrumb-close" title="关闭">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
            `;
            
            // 添加点击事件
            item.querySelector('a').addEventListener('click', function(e) {
                e.preventDefault();
                breadcrumbManager.activateBreadcrumbItem(index);
            });
            
            // 添加关闭事件
            item.querySelector('.breadcrumb-close').addEventListener('click', function(e) {
                e.stopPropagation();
                breadcrumbManager.closeBreadcrumbItem(index);
            });
            
            breadcrumbList.appendChild(item);
        });
    },
    
    // 激活面包屑项
    activateBreadcrumbItem: function(index) {
        const page = this.openPages[index];
        
        // 加载页面内容
        this.loadPageContent(page.url);
        
        // 更新菜单激活状态
        const menuItems = document.querySelectorAll('.sidebar-menu a:not(.submenu-toggle)');
        menuItems.forEach(item => {
            if (item.getAttribute('href') === page.url) {
                item.classList.add('active');
            } else {
                item.classList.remove('active');
            }
        });
        
        // 更新面包屑激活状态
        document.querySelectorAll('.breadcrumb-item').forEach((item, i) => {
            if (i > 0 && i - 1 === index) {
                item.classList.add('active');
            } else {
                item.classList.remove('active');
            }
        });
        
        // 更新URL但不刷新页面
        history.pushState({ url: page.url, title: page.title }, page.title, page.url);
    },
    
    // 关闭面包屑项
    closeBreadcrumbItem: function(index) {
        if (this.openPages.length === 0) return;
        
        const removedPage = this.openPages.splice(index, 1)[0];
        this.renderBreadcrumb();
        
        // 如果关闭的是当前激活的页面，跳转到上一个页面
        if (window.location.pathname === removedPage.url) {
            if (this.openPages.length > 0) {
                this.activateBreadcrumbItem(this.openPages.length - 1);
            } else {
                this.navigateToHome();
            }
        }
    },
    
    // 关闭其他面包屑项
    closeOtherBreadcrumbItems: function(keepIndex) {
        const keepPage = this.openPages[keepIndex];
        this.openPages = [keepPage];
        this.renderBreadcrumb();
        this.activateBreadcrumbItem(0);
    },
    
    // 关闭所有面包屑项
    closeAllBreadcrumbItems: function() {
        this.openPages = [];
        this.renderBreadcrumb();
        this.navigateToHome();
    },
    
    // 导航到首页
    navigateToHome: function() {
        this.loadPageContent('/');
        
        // 更新菜单激活状态
        const menuItems = document.querySelectorAll('.sidebar-menu a:not(.submenu-toggle)');
        menuItems.forEach(item => {
            if (item.getAttribute('href') === '/') {
                item.classList.add('active');
            } else {
                item.classList.remove('active');
            }
        });
        
        history.pushState({ url: '/', title: '仪表盘' }, '仪表盘', '/');
    },
    
    // 设置面包屑右键菜单
    setupBreadcrumbContextMenu: function() {
        // 检查是否已存在右键菜单，如果存在则移除
        let contextMenu = document.getElementById('breadcrumb-context-menu');
        if (contextMenu) {
            document.body.removeChild(contextMenu);
        }
        
        // 创建右键菜单
        contextMenu = document.createElement('div');
        contextMenu.id = 'breadcrumb-context-menu';
        contextMenu.className = 'breadcrumb-context-menu';
        contextMenu.style.display = 'none';
        contextMenu.innerHTML = `
            <ul>
                <li data-action="close">关闭</li>
                <li data-action="close-other">关闭其他</li>
                <li data-action="close-all">关闭所有</li>
            </ul>
        `;
        document.body.appendChild(contextMenu);
        
        // 显示右键菜单
        document.addEventListener('contextmenu', function(e) {
            const breadcrumbItem = e.target.closest('.breadcrumb-item-content');
            if (breadcrumbItem) {
                e.preventDefault();
                
                const pageIndex = parseInt(breadcrumbItem.closest('.breadcrumb-page').getAttribute('data-index'));
                
                // 保存当前操作的页面索引
                contextMenu.setAttribute('data-target-index', pageIndex);
                
                // 显示菜单
                contextMenu.style.position = 'fixed';
                contextMenu.style.left = e.clientX + 'px';
                contextMenu.style.top = e.clientY + 'px';
                contextMenu.style.display = 'block';
            }
        });
        
        // 点击菜单项
        contextMenu.querySelectorAll('li').forEach(item => {
            item.addEventListener('click', function() {
                const action = this.getAttribute('data-action');
                const pageIndex = parseInt(contextMenu.getAttribute('data-target-index'));
                
                switch (action) {
                    case 'close':
                        breadcrumbManager.closeBreadcrumbItem(pageIndex);
                        break;
                    case 'close-other':
                        breadcrumbManager.closeOtherBreadcrumbItems(pageIndex);
                        break;
                    case 'close-all':
                        breadcrumbManager.closeAllBreadcrumbItems();
                        break;
                }
                
                contextMenu.style.display = 'none';
            });
        });
        
        // 点击其他地方关闭菜单
        document.addEventListener('click', function() {
            contextMenu.style.display = 'none';
        });
    },
    
    // 加载页面内容
    loadPageContent: function(url) {
        const mainContent = document.getElementById('main-content');
        
        // 显示加载中
        mainContent.innerHTML = '<div class="loading-container"><div class="loading-spinner"></div> 加载中...</div>';
        
        // 使用fetch加载页面内容
        fetch(url)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.text();
            })
            .then(html => {
                // 提取body内容
                const parser = new DOMParser();
                const doc = parser.parseFromString(html, 'text/html');
                const newContent = doc.querySelector('#main-content').innerHTML;
                
                // 更新内容
                mainContent.innerHTML = newContent;
                
                // 提取title
                const newTitle = doc.querySelector('title').textContent;
                document.title = newTitle;
                
                // 执行页面中的脚本
                const scripts = doc.querySelectorAll('#main-content script');
                scripts.forEach(oldScript => {
                    const newScript = document.createElement('script');
                    Array.from(oldScript.attributes).forEach(attr => {
                        newScript.setAttribute(attr.name, attr.value);
                    });
                    newScript.textContent = oldScript.textContent;
                    document.body.appendChild(newScript);
                });
            })
            .catch(error => {
                mainContent.innerHTML = `<div class="alert alert-danger">加载页面失败: ${error.message}</div>`;
                console.error('Error loading page:', error);
            });
    },
    
    // 加载当前页面
   loadCurrentPage: function() {
        const currentPath = window.location.pathname;
        if (currentPath !== '/') {
            // 尝试找到匹配的菜单项
            const menuItem = Array.from(document.querySelectorAll('.sidebar-menu a:not(.submenu-toggle)')).find(
                item => item.getAttribute('href') === currentPath
            );
            
            if (menuItem) {
                // 修复：检查是否有span元素
                const spanElement = menuItem.querySelector('span');
                const title = spanElement ? spanElement.textContent : menuItem.textContent.trim();
                const icon = menuItem.querySelector('i')?.className || '';
                this.addBreadcrumbItem(currentPath, title, icon);
            } else {
                // 未知页面，直接加载
                this.loadPageContent(currentPath);
            }
        }
    }
};

// 初始化面包屑管理器
// 将初始化逻辑移到DOMContentLoaded事件中，确保在页面加载完成后执行
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function() {
        breadcrumbManager.init();
    });
} else {
    breadcrumbManager.init();
}

// 处理浏览器后退/前进按钮
window.addEventListener('popstate', function(event) {
    if (event.state) {
        const url = event.state.url;
        // 加载页面内容但不添加到面包屑
        breadcrumbManager.loadPageContent(url);
        
        // 更新菜单激活状态
        const menuItems = document.querySelectorAll('.sidebar-menu a:not(.submenu-toggle)');
        menuItems.forEach(item => {
            if (item.getAttribute('href') === url) {
                item.classList.add('active');
            } else {
                item.classList.remove('active');
            }
        });
    }
});