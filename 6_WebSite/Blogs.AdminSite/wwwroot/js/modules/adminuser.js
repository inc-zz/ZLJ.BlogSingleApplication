/**
 * 用户管理模块
 * @typedef { import('../types/user').UserInfo } UserInfo
 * @typedef { import('../types/user').UserFormData } UserFormData
 * @typedef { import('../types/user').UserListResponse } UserListResponse
 * @typedef { import('../types/user').UserResponse } UserResponse
 */
layui.define(['layer', 'form', 'table', 'laypage', 'adminuser-template', 'core'], function (exports) {

    var $ = layui.$;
    var layer = layui.layer;
    var form = layui.form; 
    var templateManager = layui['adminuser-template'];
    var core = layui.core;
    var http = core.http;

    var UserManager = {
        // 配置参数
        config: {
            baseUrl: '/admin/user',
            pageSize: 10
        },

        // 初始化
        init: function () {
            this.initTable();
            this.bindEvents();
            
            // 绑定表格事件
            this.bindTableEvents();
        },

        // 初始化表格
        initTable: function () {
            var self = this;
            // 使用现有的HTML表格结构，不重新定义表头
            this.loadUserData();
        },

        // 加载用户数据
        loadUserData: function () {
            var self = this;
            http.get(this.config.baseUrl + '/list')
                .then(function (res/** @type {UserListResponse} */) {
                    // 同时检查res.items和res.data，确保能处理不同的数据结构
                    if (res.code === 200) {
                        // 优先使用res.items，如果不存在则尝试使用res.data
                        var userData = res.items || [];
                        self.renderUserTable(userData);
                    } else {
                        // 如果没有数据，显示暂无数据
                        self.renderUserTable([]);
                    }
                })
                .catch(function (error) {
                    console.error('加载用户数据失败:', error);
                    // 显示暂无数据
                    self.renderUserTable([]);
                });
        },

        // 渲染用户表格
        renderUserTable: function (data) {
            var self = this;

            var tbody = $('#userTableBody');
            tbody.empty();

            if (!data || data.length === 0) {
                tbody.html('<tr><td colspan="11" class="text-center">暂无数据</td></tr>');
                return;
            }

            data.forEach(function (user) {
                var row = $('<tr>');
                row.append('<td><input type="checkbox" class="user-checkbox" data-id="' + user.id + '"></td>');
                row.append('<td>' + user.id + '</td>');
                row.append('<td>' + user.userName + '</td>');
                row.append('<td>' + user.realName + '</td>');
                row.append('<td>' + self.getPhoneNumber(user.phoneNumber) + '</td>');
                row.append('<td>' + self.getEmail(user.email) + '</td>');
                row.append('<td>' + self.getRoleBadge(user.roles || user.role) + '</td>');
                 row.append('<td>' + self.formatStatus(user.status, user.id) + '</td>');
                row.append('<td>' + self.formatDateTime(user.lastLoginTime) + '</td>');
                row.append('<td>' + self.formatDateTime(user.createdAt) + '</td>');
                row.append('<td>' + self.getActionButtons(user.id) + '</td>');
                tbody.append(row);
            });

        },
         // 搜索用户
        searchUsers: function () {
            // 获取搜索关键词
            var keyword = $('#searchKeyword').val().trim();
            // 调用加载用户数据方法，并传入搜索关键词
            this.loadUserData(keyword);
        },

        // 格式化手机号
        getPhoneNumber: function(phoneNumber) {
            return phoneNumber==null?'':phoneNumber;
        },  

        //格式化邮箱
        getEmail: function(email) {
            return email==null?'':email;
        },  

        // 获取角色徽章
        getRoleBadge: function (role) {
            var badges = {
                'admin': '<span class="badge bg-danger">管理员</span>',
                'editor': '<span class="badge bg-warning">编辑</span>',
                'user': '<span class="badge bg-primary">普通用户</span>'
            };
            return badges[role] || '<span class="badge bg-secondary">普通用户</span>';
        },

        // 获取状态徽章
        formatStatus: function (status, userId) {
            var buttonClass = status == 1 ? 'btn-success' : 'btn-secondary';
            var buttonText = status == 1 ? '启用' : '禁用';
            var newStatus = status == 1 ? 0 : 1;
            
            return '<button class="btn btn-sm ' + buttonClass + ' toggle-status-btn" ' +
                   'data-user-id="' + userId + '" data-status="' + newStatus + '">' +
                   buttonText + '</button>';
        },

        //格式化时间
        formatDateTime: function(datetime){
            return datetime==null?'':new Date(datetime).toLocaleString();
        },

        // 获取操作按钮
        getActionButtons: function (userId) {
            return '<div class="btn-group btn-group-sm">' +
                '<button class="btn btn-outline-info btn-sm view-user-btn" data-id="' + userId + '" title="查看">' +
                '<i class="fas fa-eye"></i></button>' +
                '<button class="btn btn-outline-warning btn-sm edit-user-btn" data-id="' + userId + '" title="编辑">' +
                '<i class="fas fa-edit"></i></button>' +
                '<button class="btn btn-outline-danger btn-sm delete-user-btn" data-id="' + userId + '" title="删除">' +
                '<i class="fas fa-trash"></i></button>' +
                '</div>';
        },

        // 绑定表格事件
        bindTableEvents: function () {
            var self = this;
            
            // 全选功能
            $('#checkAll').on('change', function () {
                var isChecked = $(this).is(':checked');
                $('.user-checkbox').prop('checked', isChecked);
                self.updateBatchDeleteButton();
            });

            // 单个复选框变化
            $(document).on('change', '.user-checkbox', function () {
                self.updateBatchDeleteButton();
            });

            // 查看用户详情
            $(document).on('click', '.view-user-btn', function () {
                var userId = $(this).data('id');
                self.loadUserDetail(userId, function (user) {
                    self.openUserFormModal('查看用户', false, user);
                });
            });

            // 编辑用户
            $(document).on('click', '.edit-user-btn', function () {
                var userId = $(this).data('id');
                self.showUserForm('edit', userId);
            });

            // 删除用户
            $(document).on('click', '.delete-user-btn', function () {
                var userId = $(this).data('id');
                self.deleteUser(userId);
            });
             // 状态切换按钮事件
            $(document).on('click', '.toggle-status-btn', function () {
                var userId = $(this).data('user-id');
                var newStatus = $(this).data('status');
                self.toggleUserStatus(userId, newStatus, this);
            });
        },

        // 绑定事件
        bindEvents: function () {
            var self = this;
            
            // 添加用户
            $('#addUserBtn').on('click', function () {
                self.showUserForm('add');
            });

            // 刷新数据
            $('#refreshBtn').on('click', function () {
                self.loadUserData();
            });

            // 搜索
            $('#searchBtn').on('click', function () {
                self.loadUserData();
            });

            // 批量删除
            $('#batchDeleteBtn').on('click', function () {
                self.batchDeleteUsers();
            });

            // 回车搜索
            $('#searchKeyword').on('keypress', function (e) {
                if (e.which === 13) {
                    self.searchUsers();
                }
            });

            // 表单验证规则
            form.verify({
                username: function (value) {
                    if (!/^[a-zA-Z0-9_]{3,20}$/.test(value)) {
                        return '用户名必须是3-20位字母、数字或下划线';
                    }
                },
                pass: function (value) {
                    if (value && (value.length < 6 || value.length > 20)) {
                        return '密码必须6-20位';
                    }
                    if (value && /\s/.test(value)) {
                        return '密码不能包含空格';
                    }
                },
                confirmPass: function (value) {
                    var password = $('input[name="password"]').val();
                    if (password && value !== password) {
                        return '两次输入的密码不一致';
                    }
                },
                phoneNumber: function (value) {
                    if (!/^1[3-9]\d{9}$/.test(value)) {
                        return '请输入正确的手机号';
                    }
                },
                email: function (value) {
                    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
                        return '请输入正确的邮箱地址';
                    }
                }
            });
        },

        // 显示用户表单
        showUserForm: function (mode, userId) {
            
            var self = this;
            var isEdit = mode === 'edit';
            var title = isEdit ? '编辑用户' : '添加用户';

            // 预加载用户数据（如果是编辑模式）
            if (isEdit && userId) {
                this.loadUserDetail(userId, function (user) {
                    self.openUserFormModal(title, isEdit, user);
                });
            } else {
                self.openUserFormModal(title, isEdit);
            }
        },

        // 切换用户状态
        toggleUserStatus: function (userId, newStatus, buttonElement) {
            var self = this;
            var button = $(buttonElement);
            
            // 检查是否在5秒内点击过
            if (self.statusToggleLocks[userId] && Date.now() - self.statusToggleLocks[userId] < 5000) {
                layer.msg('操作过于频繁，请稍后再试', {icon: 7});
                return;
            }
            
            // 设置点击锁
            self.statusToggleLocks[userId] = Date.now();
            
            // 禁用按钮防止重复点击
            button.prop('disabled', true);
            
            // 显示加载状态
            var originalText = button.text();
            button.text(newStatus === 1 ? '启用中...' : '禁用中...');
            
            // 调用API切换状态
            $.ajax({
                url: '/api/users/' + userId + '/status',
                type: 'PUT',
                data: { status: newStatus },
                success: function (response) {
                    if (response.success) {
                        // 更新按钮样式和文本
                        button.removeClass('btn-success btn-secondary').addClass(newStatus === 1 ? 'btn-success' : 'btn-secondary');
                        button.text(newStatus === 1 ? '启用' : '禁用');
                        button.data('status', newStatus === 1 ? 0 : 1);
                        layer.msg('状态更新成功', {icon: 6});
                    } else {
                        layer.msg(response.message || '状态更新失败', {icon: 5});
                        // 恢复按钮原始状态
                        button.text(originalText);
                    }
                },
                error: function () {
                    layer.msg('网络错误，状态更新失败', {icon: 5});
                    // 恢复按钮原始状态
                    button.text(originalText);
                },
                complete: function () {
                    // 重新启用按钮
                    button.prop('disabled', false);
                }
            });
        },

        // 打开用户表单模态框
        openUserFormModal: function (title, isEdit, userData/** @type {UserInfo} */) {
            var self = this;

            // 调试模板渲染
            var formContent = templateManager.renderUserForm(isEdit, userData);
            var layerIndex = layer.open({
                type: 1,
                title: title,
                area: ['750px', 'auto'],
                content: formContent,
                success: function (layero, index) {
                    // 初始化表单数据
                    if (userData) {
                        
                        form.val('userForm', {
                            id: userData.id,
                            userName: userData.userName,
                            password: userData.password,
                            departmentId: userData.departmentId,
                            realName: userData.realName,
                            phoneNumber: userData.phoneNumber,
                            email: userData.email,
                            role: userData.role,
                            description: userData.description,
                            status: userData.status // 确保status字段被正确赋值，类型为number
                        });
                    }
                    // 重新渲染表单元素
                    form.render();

                    // 表单提交事件
                    form.on('submit(userSubmit)', function (data) {
                        self.saveUser(data.field, isEdit);
                        return false;
                    });
                }
            });
        },

        // 加载单个用户详情数据
        loadUserDetail: function (userId, callback) {
            var self = this;
            http.get(this.config.baseUrl + '/info?id=' + userId)
                .then(function (res/** @type {UserResponse} */) {
                    debugger
                    if (res.code === 200 && callback) {
                        callback(res.data);
                    } else {
                        layer.msg('加载用户详情失败', { icon: 2 });
                        console.error('加载用户详情失败:', res);
                    }
                })
                .catch(function (error) {
                    layer.msg('加载用户详情失败', { icon: 2 });
                    console.error('加载用户详情失败:', error);
                });
        },
        
        // 保存用户方法
        saveUser: function (userData/** @type {UserFormData} */, isEdit) {
            var self = this;
            var url = this.config.baseUrl;
            var method = 'POST';
        
            if (isEdit) {
                method = 'PUT';
                // 编辑时不更新密码
                delete userData.password;
                delete userData.confirmPassword;
            }

            // 根据method选择正确的http方法
            var requestPromise;
            if (method === 'POST') {
                requestPromise = http.post(url, userData);
            } else if (method === 'PUT') {
                requestPromise = http.put(url, userData);
            }
            
            requestPromise.then(function (res) {
                    if (res.code === 200) {
                        layer.msg(isEdit ? '用户更新成功' : '用户添加成功', { icon: 1 });
                        layer.closeAll();
                        self.loadUserData();
                    } else {
                        layer.msg(res.message || '操作失败', { icon: 2 });
                    }
                })
                .catch(function (error) {
                    layer.msg('操作失败', { icon: 2 });
                    console.error('保存用户失败:', error);
                });
        },

        
    };

    exports('adminuser', UserManager);
});