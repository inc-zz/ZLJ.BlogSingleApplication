/**
 * 菜单管理模块
 */
layui.define(['layer', 'form', 'laypage', 'menu-template', 'core'], function (exports) {
    var $ = layui.$;
    var layer = layui.layer;
    var form = layui.form;
    var laypage = layui.laypage;
    var templateManager = layui['menu-template'];
    var core = layui.core;
    var http = core.http;

    var MenuManager = {
        // 配置参数
        config: {
            baseUrl: '/admin/menu',
            pageSize: 10,
            currentMenuId: null, // 当前选中的菜单ID
            // 修改defaultPermissions，从字符串数组改为对象数组，每个对象包含code、name和description属性
            defaultPermissions: [
                // 基础权限
                {code: 'Add', name: '新增', description: '允许新增数据'},
                {code: 'Update', name: '修改', description: '允许修改数据'},
                {code: 'Delete', name: '删除', description: '允许删除数据'},
                {code: 'View', name: '查看', description: '允许查看数据'},
                // 额外权限
                {code: 'Download', name: '下载', description: '允许下载数据'},
                {code: 'Import', name: '导入', description: '允许导入数据'},
                {code: 'Export', name: '导出', description: '允许导出数据'},
                {code: 'Upload', name: '上传', description: '允许上传文件'},
                {code: 'Enable', name: '启用', description: '允许启用功能'},
                {code: 'Disable', name: '禁用', description: '允许禁用功能'}
            ]
        },

        // 初始化
        init: function () {
            var self = this;
            
            // 等待模板加载完成
            templateManager.waitForTemplates(function() {
                self.bindEvents();
                self.loadMenuTree();
            });
        },

        // 绑定事件
        bindEvents: function() {
            var self = this;
            
            // 搜索按钮点击事件
            $('#searchMenuBtn').on('click', function() {
                self.loadMenuData();
            });
            
            // 刷新按钮点击事件
            $('#refreshMenuBtn').on('click', function() {
                self.loadMenuTree();
            });
            
            // 添加菜单按钮点击事件
            $('#addMenuBtn').on('click', function() {
                self.openMenuModal();
            });
            
            // 添加子菜单按钮点击事件
            $('#addSubMenuBtn').on('click', function() {
                if (!self.config.currentMenuId) {
                    layer.msg('请先选择一个父级菜单', {icon: 5});
                    return;
                }
                self.openMenuModal(self.config.currentMenuId);
            });
            
            // 批量删除按钮点击事件
            $('#batchDeleteMenuBtn').on('click', function() {
                self.batchDeleteMenus();
            });
            
            // 展开全部按钮点击事件
            $('#expandAllMenuBtn').on('click', function() {
                self.expandAllMenuTree();
            });
            
            // 保存菜单按钮点击事件
            $('#saveMenuBtn').on('click', function() {
                self.saveMenu();
            });
            
            // 保存权限按钮点击事件
            $('#savePermissionBtn').on('click', function() {
                self.saveMenuPermissions();
            });
            
            // 全选复选框点击事件
            $('#checkAllMenus').on('change', function() {
                var checked = $(this).prop('checked');
                $('#menuTableBody input[type="checkbox"]').prop('checked', checked);
            });
        },

        // 加载菜单树
        loadMenuTree: function () {
            var self = this;
            debugger
            http.get(self.config.baseUrl + '/tree')
                .then(function (res) {
                    debugger
                    if (res.code === 200 && res.data) {
                        self.renderMenuTree(res.data);
                    } else {
                        // 使用模拟数据
                        const mockData = self.getMockMenuTreeData();
                        self.renderMenuTree(mockData);
                    }
                })
                .catch(function (error) {
                    debugger
                    console.error('加载菜单树失败:', error);
                    // 使用模拟数据
                    const mockData = self.getMockMenuTreeData();
                    self.renderMenuTree(mockData);
                });
        },

        // 渲染菜单树
        renderMenuTree: function (treeData) {
            var self = this;
            var treeContainer = $('#menuTree');
            treeContainer.empty();
                
            // 递归渲染菜单树
            function renderNode(nodes, level = 0) {
                nodes.forEach(function (node) {
                    var $item = $('<div class="menu-item" data-id="' + node.id + '">');
                    $item.css('margin-left', level * 20 + 'px');
                    
                    // 设置图标
                    var iconClass = node.icon || (level === 0 ? 'fa-th-large' : 
                                  (node.children && node.children.length > 0 ? 'fa-folder' : 'fa-file'));
                    
                    $item.html('<i class="fas ' + iconClass + ' me-2"></i>' + node.name);
                    
                    treeContainer.append($item);
                    
                    // 递归渲染子节点
                    if (node.children && node.children.length > 0) {
                        renderNode(node.children, level + 1);
                    }
                });
            }
            
            renderNode(treeData);
            
            // 默认选中第一个菜单
            if (treeData && treeData.length > 0) {
                self.selectMenu(treeData[0].id);
            }
            
            // 绑定菜单点击事件
            self.bindMenuTreeEvents();
        },

        // 选择菜单
        selectMenu: function (menuId) {
            this.config.currentMenuId = menuId;
            $('.menu-item').removeClass('active');
            $('.menu-item[data-id="' + menuId + '"]').addClass('active');
            
            // 加载选中菜单的数据
            this.loadMenuData();
        },

        // 绑定菜单树事件
        bindMenuTreeEvents: function () {
            var self = this;
            
            // 菜单项点击事件
            $('.menu-item').on('click', function () {
                var menuId = $(this).data('id');
                self.selectMenu(menuId);
            });
        },

        // 展开全部菜单树
        expandAllMenuTree: function() {
            layer.msg('已展开所有菜单', {icon: 1});
        },

        // 加载菜单数据
        loadMenuData: function (page = 1, limit = this.config.pageSize) {
            var self = this;
            var searchParams = this.getSearchParams();
            
            // 显示加载状态
            const tbody = $('#menuTableBody');
            tbody.html('<tr><td colspan="9" class="data-loading"><i class="fas fa-spinner fa-spin me-2"></i>数据加载中...</td></tr>');
            
            // 构建请求参数
            var params = {
                page: page,
                limit: limit,
                parentId: self.config.currentMenuId || 0,
                ...searchParams
            };
            
            http.get(self.config.baseUrl+'/list', params)
                .then(function (res) {
                    debugger
                    if (res.code === 200 && res.items) {
                        self.renderMenuTable(res.items);
                        self.initPagination(res.total, page, limit);
                    } else {
                        // 使用模拟数据
                        const mockData = {
                            items: self.getMockMenuData(),
                            total: 10
                        };
                        self.renderMenuTable(mockData);
                        self.initPagination(10, page, limit);
                    }
                })
                .catch(function (error) {
                    console.error('加载菜单数据失败:', error);
                    // 使用模拟数据
                    const mockData = {
                        items: self.getMockMenuData(),
                        total: 10
                    };
                    self.renderMenuTable(mockData);
                    self.initPagination(10, page, limit);
                });
        },

        // 渲染菜单表格
        renderMenuTable: function (data) {
            var self = this;
            var tbody = $('#menuTableBody');
            tbody.empty();
            debugger
            if (data && data.length > 0) {
                data.forEach(function (menu) {
                    var statusText = menu.status ? '<span class="badge bg-success">启用</span>' : '<span class="badge bg-danger">禁用</span>';
                    var typeText = menu.type === 0 ? '页面菜单' : '父级菜单';
                    var iconHtml = menu.icon ? '<i class="fas ' + menu.icon + '"></i>' : '-';
                    
                    var $tr = $('<tr>');
                    $tr.html(
                        '<td><input type="checkbox" data-id="' + menu.id + '"></td>' +
                        '<td>' + menu.id + '</td>' +
                        '<td>' + menu.name + '</td>' +
                        '<td>' + typeText + '</td>' +
                        '<td>' + (menu.routePath || '-') + '</td>' +
                        '<td>' + iconHtml + '</td>' +
                        '<td>' + (menu.sort || 0) + '</td>' +
                        '<td>' + statusText + '</td>' +
                        '<td>' +
                        '<button class="btn btn-sm btn-primary edit-menu-btn" data-id="' + menu.id + '">编辑</button> ' +
                        '<button class="btn btn-sm btn-info set-permission-btn" data-id="' + menu.id + '" data-name="' + menu.name + '" data-type="' + menu.type + '">设置权限</button> ' +
                        '<button class="btn btn-sm btn-danger delete-menu-btn" data-id="' + menu.id + '">删除</button>' +
                        '</td>'
                    );
                    
                    tbody.append($tr);
                });
                
                // 绑定表格事件
                self.bindTableEvents();
            } else {
                tbody.html('<tr><td colspan="9" class="text-center">暂无数据</td></tr>');
            }
        },

        // 绑定表格事件
        bindTableEvents: function() {
            var self = this;
            
            // 编辑按钮点击事件
            $('.edit-menu-btn').on('click', function() {
                var menuId = $(this).data('id');
                self.openMenuModal(null, menuId);
            });
            
            // 设置权限按钮点击事件
            $('.set-permission-btn').on('click', function() {
                var menuId = $(this).data('id');
                var menuName = $(this).data('name');
                var menuType = $(this).data('type');
                
                if (menuType === 1) {
                    layer.msg('父级菜单无需设置权限', {icon: 5});
                    return;
                }
                
                self.openPermissionModal(menuId, menuName);
            });
            
            // 删除按钮点击事件
            $('.delete-menu-btn').on('click', function() {
                var menuId = $(this).data('id');
                self.deleteMenu(menuId);
            });
        },

        // 打开菜单模态框
        openMenuModal: function(parentId = null, menuId = null) {
            var self = this;
            
            // 重置表单
            $('#menuForm')[0].reset();
            $('#menuId').val('');
            
            // 设置模态框标题
            $('#menuModalTitle').text(menuId ? '编辑菜单' : '添加菜单');
            
            // 设置上级菜单
            if (parentId) {
                $('#parentMenu').val(parentId);
            } else if (self.config.currentMenuId && !menuId) {
                $('#parentMenu').val(self.config.currentMenuId);
            }
            
            // 重新渲染表单
            form.render();
            
            // 如果是编辑模式，加载菜单详情
            if (menuId) {
                self.loadMenuDetail(menuId);
            }
            
            // 打开模态框
            $('#menuModal').modal('show');
        },

        // 加载菜单详情
        loadMenuDetail: function(menuId) {
            var self = this;
            
            http.get(self.config.baseUrl + '/' + menuId)
                .then(function(res) {
                    if (res.code === 200 && res.data) {
                        var menu = res.data;
                        $('#menuId').val(menu.id);
                        $('#parentMenu').val(menu.parentId || 0);
                        $('#menuName').val(menu.name);
                        $('#menuType').val(menu.type);
                        $('#routePath').val(menu.routePath || '');
                        $('#menuIcon').val(menu.icon || '');
                        $('#menuSort').val(menu.sort || 0);
                        $('#menuStatus').prop('checked', menu.status);
                        
                        // 重新渲染表单
                        form.render();
                    }
                })
                .catch(function(error) {
                    console.error('加载菜单详情失败:', error);
                    layer.msg('加载菜单详情失败', {icon: 5});
                });
        },

        // 保存菜单
        saveMenu: function() {
            var self = this;
            
            // 获取表单数据
            var menuData = {
                id: $('#menuId').val(),
                parentId: $('#parentMenu').val(),
                name: $('#menuName').val(),
                type: $('#menuType').val(),
                routePath: $('#routePath').val(),
                icon: $('#menuIcon').val(),
                sort: $('#menuSort').val(),
                status: $('#menuStatus').prop('checked')
            };
            
            // 验证表单
            if (!menuData.name) {
                layer.msg('请输入菜单名称', {icon: 5});
                return;
            }
            
            // 保存菜单
            var url = menuData.id ? self.config.baseUrl + '/' + menuData.id : self.config.baseUrl;
            var method = menuData.id ? 'put' : 'post';
            
            http[method](url, menuData)
                .then(function(res) {
                    if (res.code === 200) {
                        layer.msg('保存成功', {icon: 1});
                        $('#menuModal').modal('hide');
                        self.loadMenuTree();
                    } else {
                        layer.msg(res.msg || '保存失败', {icon: 5});
                    }
                })
                .catch(function(error) {
                    console.error('保存菜单失败:', error);
                    layer.msg('保存失败', {icon: 5});
                });
        },

        // 打开权限设置模态框
        openPermissionModal: function(menuId, menuName) {
            var self = this;
            
            $('#permissionMenuId').val(menuId);
            $('#permissionMenuName').val(menuName);
            
            // 加载权限列表
            self.loadMenuPermissions(menuId);
            
            // 打开模态框
            $('#menuPermissionModal').modal('show');
        },

        // 加载菜单权限
        loadMenuPermissions: function(menuId) {
            var self = this;
            var permissionList = $('#permissionList');
            permissionList.empty();
            
            // 获取默认权限列表
            self.config.defaultPermissions.forEach(function(permission) {
                var $item = $('<div class="permission-item">');
                var $label = $('<label class="permission-label" title="' + permission.description + '">');
                var $checkbox = $('<input type="checkbox" name="permission" value="' + permission.code + '" lay-skin="primary">');
                var $span = $('<span>' + permission.name + '</span>');
                var $code = $('<span class="permission-code">(' + permission.code + ')</span>');
                
                $label.append($checkbox);
                $label.append($span);
                $label.append($code);
                $item.append($label);
                
                permissionList.append($item);
            });
            
            // 重新渲染表单，使layui样式生效
            form.render('checkbox');
            
            // 模拟加载已设置的权限
            // 实际项目中应该从服务器获取
            // 这里简单模拟，假设所有权限默认都是勾选的
            permissionList.find('input[type="checkbox"]').prop('checked', true);
            form.render('checkbox');
            
            // 这里应该调用API获取实际的权限设置
            /*
            http.get(self.config.baseUrl + '/' + menuId + '/permissions')
                .then(function(res) {
                    if (res.code === 200) {
                        // 清空所有选中状态
                        permissionList.find('input[type="checkbox"]').prop('checked', false);
                        
                        // 根据服务器返回的权限设置选中状态
                        res.data.forEach(function(permissionCode) {
                            permissionList.find('input[type="checkbox"][value="' + permissionCode + '"]').prop('checked', true);
                        });
                        
                        // 重新渲染表单
                        form.render('checkbox');
                    }
                });
            */
        },

        // 保存菜单权限
        saveMenuPermissions: function() {
            var self = this;
            var menuId = $('#permissionMenuId').val();
            var selectedPermissions = [];
            
            // 获取选中的权限
            $('#permissionList input[type="checkbox"]:checked').each(function() {
                selectedPermissions.push($(this).val());
            });
            
            // 保存权限
            http.post(self.config.baseUrl + '/' + menuId + '/permissions', {
                permissions: selectedPermissions
            })
                .then(function(res) {
                    if (res.code === 200) {
                        layer.msg('权限设置成功', {icon: 1});
                        $('#menuPermissionModal').modal('hide');
                    } else {
                        layer.msg(res.msg || '保存失败', {icon: 5});
                    }
                })
                .catch(function(error) {
                    console.error('保存权限失败:', error);
                    layer.msg('保存失败', {icon: 5});
                });
        },

        // 删除菜单
        deleteMenu: function(menuId) {
            var self = this;
            
            layer.confirm('确定要删除这个菜单吗？', function(index) {
                http.delete(self.config.baseUrl + '/' + menuId)
                    .then(function(res) {
                        if (res.code === 200) {
                            layer.msg('删除成功', {icon: 1});
                            self.loadMenuTree();
                        } else {
                            layer.msg(res.msg || '删除失败', {icon: 5});
                        }
                    })
                    .catch(function(error) {
                        console.error('删除菜单失败:', error);
                        layer.msg('删除失败', {icon: 5});
                    });
                
                layer.close(index);
            });
        },

        // 批量删除菜单
        batchDeleteMenus: function() {
            var self = this;
            var selectedMenuIds = [];
            
            // 获取选中的菜单ID
            $('#menuTableBody input[type="checkbox"]:checked').each(function() {
                selectedMenuIds.push($(this).data('id'));
            });
            
            if (selectedMenuIds.length === 0) {
                layer.msg('请选择要删除的菜单', {icon: 5});
                return;
            }
            
            layer.confirm('确定要删除选中的 ' + selectedMenuIds.length + ' 个菜单吗？', function(index) {
                http.delete(self.config.baseUrl + '/batch', {
                    ids: selectedMenuIds
                })
                    .then(function(res) {
                        if (res.code === 200) {
                            layer.msg('删除成功', {icon: 1});
                            self.loadMenuTree();
                        } else {
                            layer.msg(res.msg || '删除失败', {icon: 5});
                        }
                    })
                    .catch(function(error) {
                        console.error('批量删除菜单失败:', error);
                        layer.msg('删除失败', {icon: 5});
                    });
                
                layer.close(index);
            });
        },

        // 初始化分页
        initPagination: function(total, page, limit) {
            var self = this;
            
            laypage.render({
                elem: 'menuPage',
                count: total,
                limit: limit,
                curr: page,
                layout: ['count', 'prev', 'page', 'next', 'limit', 'refresh', 'skip'],
                jump: function(obj, first) {
                    if (!first) {
                        self.loadMenuData(obj.curr, obj.limit);
                    }
                }
            });
        },

        // 获取搜索参数
        getSearchParams: function() {
            return {
                name: $('#searchMenuName').val(),
                type: $('#searchMenuType').val()
            };
        },

        // 获取模拟菜单树数据
        getMockMenuTreeData: function() {
            return [
                {
                    id: 1,
                    name: '系统管理',
                    icon: 'fa-cog',
                    children: [
                        {
                            id: 2,
                            name: '用户管理',
                            icon: 'fa-users'
                        },
                        {
                            id: 3,
                            name: '角色管理',
                            icon: 'fa-user-tag'
                        },
                        {
                            id: 4,
                            name: '权限管理',
                            icon: 'fa-lock',
                            children: [
                                {
                                    id: 5,
                                    name: '菜单权限',
                                    icon: 'fa-list'
                                },
                                {
                                    id: 6,
                                    name: '操作权限',
                                    icon: 'fa-key'
                                }
                            ]
                        }
                    ]
                },
                {
                    id: 7,
                    name: '内容管理',
                    icon: 'fa-file-alt',
                    children: [
                        {
                            id: 8,
                            name: '文章管理',
                            icon: 'fa-file-text'
                        },
                        {
                            id: 9,
                            name: '分类管理',
                            icon: 'fa-folder'
                        },
                        {
                            id: 10,
                            name: '标签管理',
                            icon: 'fa-tags'
                        }
                    ]
                }
            ];
        },

        // 获取模拟菜单数据
        getMockMenuData: function() {
            return [
                {
                    id: 1,
                    name: '系统管理',
                    type: 1,
                    routePath: '',
                    icon: 'fa-cog',
                    sort: 1,
                    status: true,
                    parentId: 0
                },
                {
                    id: 2,
                    name: '用户管理',
                    type: 0,
                    routePath: '/users',
                    icon: 'fa-users',
                    sort: 1,
                    status: true,
                    parentId: 1
                },
                {
                    id: 3,
                    name: '角色管理',
                    type: 0,
                    routePath: '/roles',
                    icon: 'fa-user-tag',
                    sort: 2,
                    status: true,
                    parentId: 1
                },
                {
                    id: 4,
                    name: '权限管理',
                    type: 1,
                    routePath: '',
                    icon: 'fa-lock',
                    sort: 3,
                    status: true,
                    parentId: 1
                },
                {
                    id: 5,
                    name: '菜单权限',
                    type: 0,
                    routePath: '/permissions/menu',
                    icon: 'fa-list',
                    sort: 1,
                    status: true,
                    parentId: 4
                }
            ];
        }
    };

    exports('menu', MenuManager);
});