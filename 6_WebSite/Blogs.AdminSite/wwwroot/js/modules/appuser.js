/**
 * 用户管理模块 - 业务逻辑
 */
layui.define(['jquery', 'layer', 'laypage', 'core'], function (exports) {
    "use strict";

    var $ = layui.$;
    var layer = layui.layer;
    var laypage = layui.laypage;
    var core = layui.core;
    
    var UserManager = function (options) {
        this.options = $.extend({
            container: '#userManagement',
            pageSize: 10,
            api: {
                list: '/admin/users',
                detail: '/admin/users/',
                create: '/admin/users',
                update: '/admin/users/',
                delete: '/admin/users/',
                batchDelete: '/admin/users/batch-delete'
            }
        }, options);

        this.currentPage = 1;
        this.searchParams = {};
        this.selectedIds = [];
    };

    /**
     * 初始化
     */
    UserManager.prototype.init = function () {
        var that = this;
        debugger
        // 加载模板
        core.template.load('../js/templates/user.templates.html')
            .then(function () {
                that.render();
                that.bindEvents();
                that.loadUsers(1);
            })
            .catch(function (error) {
                core.msg.error('模板加载失败: ' + error.message);
            });
    };

    /**
     * 渲染界面
     */
    UserManager.prototype.render = function () {
        var container = $(this.options.container);

        // 使用模板渲染主界面
        var html = core.template.render('template-user-management', {});
        container.html(html);
    };

    /**
     * 绑定事件
     */
    UserManager.prototype.bindEvents = function () {
        var that = this;

        // 搜索事件
        $(document).on('click', '#searchBtn', function () {
            that.handleSearch();
        });

        // 回车搜索
        $(document).on('keypress', '#searchKeyword', function (e) {
            if (e.which === 13) that.handleSearch();
        });

        // 添加用户
        $(document).on('click', '#addUserBtn', function () {
            that.showUserModal('add');
        });

        // 刷新
        $(document).on('click', '#refreshBtn', function () {
            that.refresh();
        });

        // 批量删除
        $(document).on('click', '#batchDeleteBtn', function () {
            that.batchDelete();
        });

        // 全选
        $(document).on('change', '#checkAll', function () {
            that.toggleSelectAll(this.checked);
        });

        // 操作按钮
        $(document).on('click', '.view-btn', function () {
            var userId = $(this).data('id');
            that.showUserDetail(userId);
        });

        $(document).on('click', '.edit-btn', function () {
            var userId = $(this).data('id');
            that.showUserModal('edit', userId);
        });

        $(document).on('click', '.delete-btn', function () {
            var userId = $(this).data('id');
            that.deleteUser(userId);
        });
    };

    /**
     * 处理搜索
     */
    UserManager.prototype.handleSearch = function () {
        this.currentPage = 1;
        this.getSearchParams();
        this.loadUsers(1);
    };

    /**
     * 获取搜索参数
     */
    UserManager.prototype.getSearchParams = function () {
        this.searchParams = {
            keyword: $('#searchKeyword').val(),
            role: $('#searchRole').val(),
            status: $('#searchStatus').val()
        };
    };

    /**
     * 加载用户数据
     */
    UserManager.prototype.loadUsers = function (page) {
        var that = this;

        var params = {
            pageIndex: page,
            pageSize: that.options.pageSize
        };

        // 添加搜索参数
        if (!core.util.isEmpty(that.searchParams.keyword)) {
            params.keyword = that.searchParams.keyword;
        }
        if (!core.util.isEmpty(that.searchParams.role)) {
            params.role = that.searchParams.role;
        }
        if (!core.util.isEmpty(that.searchParams.status)) {
            params.status = that.searchParams.status;
        }

        core.http.get(that.options.api.list, params, { loading: true })
            .then(function (response) {
                if (response.success) {
                    that.renderUserTable(response.data);
                    that.renderPagination(response.data);
                } else {
                    core.msg.error(response.message || '加载失败');
                }
            })
            .catch(function (error) {
                core.msg.error('数据加载失败');
            });
    };

    /**
     * 渲染用户表格
     */
    UserManager.prototype.renderUserTable = function (pageData) {
        var tbody = $('#userTableBody');

        if (core.util.isEmpty(pageData.items)) {
            var emptyHtml = core.template.render('template-user-table-empty', {});
            tbody.html(emptyHtml);
            return;
        }

        var rowsHtml = '';
        pageData.items.forEach(function (user) {
            var rowData = {
                id: user.id,
                username: user.username,
                realname: user.realname,
                phone: user.phone,
                email: user.email,
                role: that._getRoleText(user.role),
                roleClass: that._getRoleClass(user.role),
                status: that._getStatusText(user.status),
                statusClass: that._getStatusClass(user.status),
                lastLogin: user.lastLogin || '从未登录',
                createTime: user.createTime
            };

            rowsHtml += core.template.render('template-user-table-row', rowData);
        });

        tbody.html(rowsHtml);
        that.updateSelectedIds();
    };

    /**
     * 渲染分页
     */
    UserManager.prototype.renderPagination = function (pageData) {
        var that = this;

        laypage.render({
            elem: 'userPage',
            count: pageData.totalCount,
            limit: pageData.pageSize,
            curr: pageData.pageIndex,
            layout: ['count', 'prev', 'page', 'next', 'skip'],
            jump: function (obj) {
                if (obj.curr !== that.currentPage) {
                    that.currentPage = obj.curr;
                    that.loadUsers(obj.curr);
                }
            }
        });
    };

    /**
     * 显示用户模态框
     */
    UserManager.prototype.showUserModal = function (mode, userId) {
        var that = this;
        var title = mode === 'add' ? '添加用户' : '编辑用户';

        // 渲染表单
        var formHtml = core.template.render('template-user-form', {
            mode: mode,
            showPassword: mode === 'add'
        });

        $('#userModal .modal-title').text(title);
        $('#userModal .modal-body').html(formHtml);

        // 如果是编辑模式，加载用户数据
        if (mode === 'edit' && userId) {
            that.loadUserForEdit(userId);
        }

        // 显示模态框并绑定事件
        core.modal.show('#userModal', function () {
            that.bindFormEvents(mode, userId);
        });
    };

    /**
     * 加载用户数据用于编辑
     */
    UserManager.prototype.loadUserForEdit = function (userId) {
        var that = this;

        core.http.get(that.options.api.detail + userId)
            .then(function (response) {
                if (response.success) {
                    core.form.fill('#userForm', response.data);
                } else {
                    core.msg.error(response.message || '加载用户数据失败');
                }
            })
            .catch(function () {
                core.msg.error('加载用户数据失败');
            });
    };

    /**
     * 绑定表单事件
     */
    UserManager.prototype.bindFormEvents = function (mode, userId) {
        var that = this;

        // 保存按钮
        $('#saveUserBtn').off('click').on('click', function () {
            that.saveUser(mode, userId);
        });

        // 表单验证
        that.setupFormValidation();
    };

    /**
     * 设置表单验证
     */
    UserManager.prototype.setupFormValidation = function () {
        var that = this;

        $('#userForm input').on('blur', function () {
            var $field = $(this);
            var fieldName = $field.attr('name');
            var rules = that.getValidationRules()[fieldName];

            if (rules) {
                core.form.validate($field, rules);
            }
        });
    };

    /**
     * 获取验证规则
     */
    UserManager.prototype.getValidationRules = function () {
        return {
            username: {
                required: true,
                pattern: '^[a-zA-Z0-9_]{3,20}$',
                message: '用户名必须为3-20位字母、数字或下划线'
            },
            realname: {
                required: true,
                pattern: '^[\u4e00-\u9fa5a-zA-Z]{2,10}$',
                message: '姓名必须为2-10位中英文'
            },
            phone: {
                required: true,
                pattern: '^1[3-9]\\d{9}$',
                message: '请输入正确的手机号'
            },
            email: {
                required: true,
                pattern: '^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$',
                message: '请输入正确的邮箱地址'
            },
            password: {
                required: true,
                pattern: '^\\S{6,20}$',
                message: '密码必须6-20位，且不能包含空格'
            }
        };
    };

    /**
     * 保存用户
     */
    UserManager.prototype.saveUser = function (mode, userId) {
        var that = this;
        var formData = core.form.serialize('#userForm');

        // 验证表单
        if (!that.validateForm(formData, mode)) {
            return;
        }

        // 处理数据
        var requestData = that.prepareUserData(formData, mode);
        var apiUrl = mode === 'add' ? that.options.api.create : that.options.api.update + userId;
        var httpMethod = mode === 'add' ? core.http.post : core.http.put;

        httpMethod.call(core, apiUrl, requestData, { loading: true })
            .then(function (response) {
                if (response.success) {
                    core.msg.success(response.message || '保存成功');
                    core.modal.hide('#userModal');
                    that.loadUsers(that.currentPage);
                } else {
                    core.msg.error(response.message || '保存失败');
                }
            })
            .catch(function () {
                core.msg.error('保存失败');
            });
    };

    /**
     * 准备用户数据
     */
    UserManager.prototype.prepareUserData = function (formData, mode) {
        var data = {
            username: formData.username,
            realname: formData.realname,
            phone: formData.phone,
            email: formData.email,
            role: formData.role,
            status: formData.status ? 1 : 0,
            description: formData.description || ''
        };

        if (mode === 'add' && formData.password) {
            data.password = formData.password;
        }

        return data;
    };

    /**
     * 验证表单
     */
    UserManager.prototype.validateForm = function (formData, mode) {
        var rules = this.getValidationRules();

        // 编辑模式下不需要密码验证
        if (mode === 'edit') {
            delete rules.password;
        }

        var isValid = true;
        for (var field in rules) {
            var $field = $('#userForm [name="' + field + '"]');
            if ($field.length > 0) {
                if (!core.form.validate($field, rules[field])) {
                    isValid = false;
                }
            }
        }

        // 验证确认密码
        if (mode === 'add' && formData.password !== formData.confirmPassword) {
            var $confirmField = $('#confirmPassword');
            $confirmField.addClass('is-invalid');
            var $feedback = $confirmField.next('.invalid-feedback');
            if ($feedback.length === 0) {
                $feedback = $('<div class="invalid-feedback">两次输入的密码不一致</div>');
                $confirmField.after($feedback);
            }
            isValid = false;
        }

        return isValid;
    };

    /**
     * 显示用户详情
     */
    UserManager.prototype.showUserDetail = function (userId) {
        var that = this;

        core.http.get(that.options.api.detail + userId, null, { loading: true })
            .then(function (response) {
                if (response.success) {
                    that.renderUserDetail(response.data);
                    core.modal.show('#userDetailModal');
                } else {
                    core.msg.error(response.message || '加载失败');
                }
            })
            .catch(function () {
                core.msg.error('加载失败');
            });
    };

    /**
     * 渲染用户详情
     */
    UserManager.prototype.renderUserDetail = function (user) {
        var detailData = {
            avatarText: user.realname ? user.realname.charAt(0) : 'U',
            realname: user.realname,
            username: user.username,
            phone: user.phone,
            email: user.email,
            role: that._getRoleText(user.role),
            status: that._getStatusText(user.status),
            statusClass: that._getStatusClass(user.status),
            lastLogin: user.lastLogin || '从未登录',
            createTime: user.createTime,
            description: user.description || '无'
        };

        var detailHtml = core.template.render('template-user-detail', detailData);
        $('#userDetailModal .modal-body').html(detailHtml);
    };

    /**
     * 删除用户
     */
    UserManager.prototype.deleteUser = function (userId) {
        var that = this;

        core.confirm({
            content: '确定要删除这个用户吗？',
            yes: function (index) {
                core.http.delete(that.options.api.delete + userId, null, { loading: true })
                    .then(function (response) {
                        if (response.success) {
                            core.msg.success('删除成功');
                            that.loadUsers(that.currentPage);
                        } else {
                            core.msg.error(response.message || '删除失败');
                        }
                    })
                    .catch(function () {
                        core.msg.error('删除失败');
                    });
            }
        });
    };

    /**
     * 批量删除
     */
    UserManager.prototype.batchDelete = function () {
        var that = this;

        if (core.util.isEmpty(that.selectedIds)) {
            core.msg.warning('请选择要删除的用户');
            return;
        }

        core.confirm({
            content: `确定要删除选中的 ${that.selectedIds.length} 个用户吗？`,
            yes: function (index) {
                core.http.post(that.options.api.batchDelete, { ids: that.selectedIds }, { loading: true })
                    .then(function (response) {
                        if (response.success) {
                            core.msg.success(`已删除 ${that.selectedIds.length} 个用户`);
                            that.selectedIds = [];
                            that.loadUsers(that.currentPage);
                        } else {
                            core.msg.error(response.message || '删除失败');
                        }
                    })
                    .catch(function () {
                        core.msg.error('删除失败');
                    });
            }
        });
    };

    /**
     * 全选/取消全选
     */
    UserManager.prototype.toggleSelectAll = function (checked) {
        $('.user-checkbox').prop('checked', checked);
        this.updateSelectedIds();
    };

    /**
     * 更新选中ID列表
     */
    UserManager.prototype.updateSelectedIds = function () {
        var that = this;
        that.selectedIds = [];

        $('.user-checkbox:checked').each(function () {
            that.selectedIds.push($(this).val());
        });

        $('#batchDeleteBtn').prop('disabled', core.util.isEmpty(that.selectedIds));

        var totalCheckboxes = $('.user-checkbox').length;
        var checkedCount = that.selectedIds.length;
        $('#checkAll').prop('checked', totalCheckboxes > 0 && checkedCount === totalCheckboxes);
    };

    /**
     * 刷新数据
     */
    UserManager.prototype.refresh = function () {
        this.currentPage = 1;
        this.searchParams = {};
        this.selectedIds = [];

        $('#searchKeyword').val('');
        $('#searchRole').val('');
        $('#searchStatus').val('');
        $('#checkAll').prop('checked', false);

        this.loadUsers(1);
        core.msg.success('数据已刷新');
    };

    // ==================== 工具方法 ====================

    /**
     * 获取角色文本
     */
    UserManager.prototype._getRoleText = function (role) {
        var roleMap = {
            'admin': '管理员',
            'editor': '编辑',
            'user': '普通用户'
        };
        return roleMap[role] || role;
    };

    /**
     * 获取角色样式类
     */
    UserManager.prototype._getRoleClass = function (role) {
        var classMap = {
            'admin': 'bg-danger',
            'editor': 'bg-warning',
            'user': 'bg-primary'
        };
        return classMap[role] || 'bg-secondary';
    };

    /**
     * 获取状态文本
     */
    UserManager.prototype._getStatusText = function (status) {
        return status == 1 ? '正常' : '禁用';
    };

    /**
     * 获取状态样式类
     */
    UserManager.prototype._getStatusClass = function (status) {
        return status == 1 ? 'bg-success' : 'bg-danger';
    };

    exports('appuser', UserManager);
});