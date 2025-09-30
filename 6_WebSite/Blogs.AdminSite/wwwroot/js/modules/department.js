/*
 * 部门管理模块
 */
layui.define(['layer', 'form', 'laypage', 'department-template', 'core'], function (exports) {
    var $ = layui.$;
    var layer = layui.layer;
    var form = layui.form;
    var laypage = layui.laypage;
    var templateManager = layui['department-template'];
    var core = layui.core;
    var http = core.http;

    var DepartmentManager = {
        // 配置参数
        config: {
            baseUrl: '/api/departments',
            pageSize: 10,
            currentDepartmentId: null // 当前选中的部门ID
        },

        // 初始化
        init: function () {
            var self = this;
            
            // 等待模板加载完成
            templateManager.waitForTemplates(function() {
                self.bindEvents();
                self.loadDepartmentTree();
                self.loadDepartmentData();
            });
        },

        // 加载部门树
        loadDepartmentTree: function () {
            var self = this;
            
            http.get(self.config.baseUrl + '/tree')
                .then(function (res) {
                    if (res.code === 200 && res.data) {
                        self.renderDepartmentTree(res.data);
                    } else {
                        // 使用模拟数据
                        self.renderDepartmentTree(self.getMockDepartmentTree());
                    }
                })
                .catch(function (error) {
                    console.error('加载部门树失败:', error);
                    // 使用模拟数据
                    self.renderDepartmentTree(self.getMockDepartmentTree());
                });
        },

        // 获取模拟部门树数据
        getMockDepartmentTree: function() {
            return [
                {id: 1, name: '总公司', userCount: 156, children: [
                    {id: 2, name: '技术部', userCount: 42, children: [
                        {id: 3, name: '前端开发组', userCount: 15},
                        {id: 4, name: '后端开发组', userCount: 20}
                    ]},
                    {id: 5, name: '内容部', userCount: 35, children: [
                        {id: 6, name: '编辑组', userCount: 25}
                    ]},
                    {id: 7, name: '市场部', userCount: 28}
                ]}
            ];
        },

        // 渲染部门树
        renderDepartmentTree: function (treeData) {
            var self = this;
            var treeContainer = $('#departmentTree');
            treeContainer.empty();
            
            // 递归渲染部门树
            function renderNode(nodes, level = 0) {
                nodes.forEach(function (node) {
                    var $item = $('<div class="department-item" data-id="' + node.id + '">');
                    $item.css('margin-left', level * 20 + 'px');
                    
                    // 设置图标（根据层级和是否有子节点）
                    var iconClass = level === 0 ? 'fa-building' : 
                                  (node.children && node.children.length > 0 ? 'fa-folder' : 'fa-folder-open');
                    
                    $item.html('<i class="fas ' + iconClass + ' me-2"></i>' + 
                              node.name + 
                              '<span class="user-count">(' + (node.userCount || 0) + ')</span>');
                    
                    treeContainer.append($item);
                    
                    // 递归渲染子节点
                    if (node.children && node.children.length > 0) {
                        renderNode(node.children, level + 1);
                    }
                });
            }
            
            renderNode(treeData);
            
            // 默认选中第一个部门
            if (treeData && treeData.length > 0) {
                self.selectDepartment(treeData[0].id);
            }
            
            // 绑定部门点击事件
            self.bindDepartmentTreeEvents();
        },

        // 选择部门
        selectDepartment: function (departmentId) {
            this.config.currentDepartmentId = departmentId;
            $('.department-item').removeClass('active');
            $('.department-item[data-id="' + departmentId + '"]').addClass('active');
            
            // 加载选中部门的数据
            this.loadDepartmentData();
        },

        // 绑定部门树事件
        bindDepartmentTreeEvents: function () {
            var self = this;
            
            // 部门项点击事件
            $('.department-item').on('click', function () {
                var departmentId = $(this).data('id');
                self.selectDepartment(departmentId);
            });
            
            // 全部展开按钮点击事件
            $('#expandAllBtn').on('click', function () {
                layer.msg('已展开所有部门', {icon: 1});
            });
        },

        // 加载部门数据
        loadDepartmentData: function (page = 1, limit = this.config.pageSize) {
            var self = this;
            var searchParams = this.getSearchParams();
            
            // 显示加载状态
            const tbody = $('#departmentTableBody');
            tbody.html('<tr><td colspan="9" class="data-loading"><i class="fas fa-spinner fa-spin me-2"></i>数据加载中...</td></tr>');
            
            // 构建请求参数
            var params = {
                page: page,
                limit: limit,
                departmentId: self.config.currentDepartmentId,
                ...searchParams
            };
            
            http.get(self.config.baseUrl, params)
                .then(function (res) {
                    if (res.code === 200 && res.data) {
                        self.renderDepartmentTable(res.data.items || []);
                        self.initPagination(res.data.total, page, limit);
                    } else {
                        // 使用模拟数据
                        self.renderDepartmentTable(self.getMockDepartmentData());
                        self.initPagination(20, page, limit);
                    }
                })
                .catch(function (error) {
                    console.error('加载部门数据失败:', error);
                    // 使用模拟数据
                    self.renderDepartmentTable(self.getMockDepartmentData());
                    self.initPagination(20, page, limit);
                });
        },

        // 获取模拟部门数据
        getMockDepartmentData: function() {
            return [
                {id: 1, name: '总公司', code: 'HEAD', manager: '张总', userCount: 156, status: '启用', createTime: '2023-01-15'},
                {id: 2, name: '技术部', code: 'TECH', manager: '李经理', userCount: 42, status: '启用', createTime: '2023-02-20'},
                {id: 3, name: '前端开发组', code: 'FRONTEND', manager: '王组长', userCount: 15, status: '启用', createTime: '2023-03-10'},
                {id: 4, name: '后端开发组', code: 'BACKEND', manager: '赵组长', userCount: 20, status: '启用', createTime: '2023-03-15'},
                {id: 5, name: '内容部', code: 'CONTENT', manager: '钱经理', userCount: 35, status: '启用', createTime: '2023-04-01'},
                {id: 6, name: '编辑组', code: 'EDIT', manager: '孙组长', userCount: 25, status: '启用', createTime: '2023-04-10'},
                {id: 7, name: '市场部', code: 'MARKET', manager: '周经理', userCount: 28, status: '启用', createTime: '2023-04-15'},
                {id: 8, name: '销售部', code: 'SALES', manager: '吴经理', userCount: 32, status: '启用', createTime: '2023-05-01'},
                {id: 9, name: '客服部', code: 'SERVICE', manager: '郑经理', userCount: 18, status: '启用', createTime: '2023-05-10'},
                {id: 10, name: '财务部', code: 'FINANCE', manager: '王总监', userCount: 12, status: '启用', createTime: '2023-05-15'}
            ];
        },

        // 渲染部门表格
        renderDepartmentTable: function (data) {
            var self = this;
            const tbody = $('#departmentTableBody');
            
            if (data.length === 0) {
                tbody.html('<tr><td colspan="9" class="text-center py-4">暂无数据</td></tr>');
                return;
            }
            
            let html = '';
            data.forEach(function (dept) {
                const statusBadge = dept.status === '启用' ?
                    '<span class="badge bg-success">启用</span>' :
                    '<span class="badge bg-secondary">禁用</span>';
                
                html += `
                    <tr>
                        <td><input type="checkbox" class="department-checkbox" value="${dept.id}"></td>
                        <td>${dept.id}</td>
                        <td>${dept.name}</td>
                        <td>${dept.code}</td>
                        <td>${dept.manager}</td>
                        <td>${dept.userCount}</td>
                        <td>${statusBadge}</td>
                        <td>${dept.createTime}</td>
                        <td>
                            <button class="btn btn-sm btn-primary edit-department-btn btn-fixed-width" data-id="${dept.id}">
                                <i class="fas fa-edit btn-icon"></i>编辑
                            </button>
                            <button class="btn btn-sm btn-danger delete-department-btn btn-fixed-width" data-id="${dept.id}">
                                <i class="fas fa-trash btn-icon"></i>删除
                            </button>
                        </td>
                    </tr>
                `;
            });
            
            tbody.html(html);
            
            // 重新绑定事件
            self.bindTableEvents();
        },

        // 初始化分页
        initPagination: function (total, page = 1, limit = 10) {
            var self = this;
            laypage.render({
                elem: 'departmentPage',
                count: total,
                curr: page,
                limit: limit,
                limits: [5, 10, 15, 20],
                layout: ['count', 'prev', 'page', 'next', 'limit', 'skip'],
                theme: '#3498db',
                jump: function(obj, first) {
                    if (!first) {
                        self.loadDepartmentData(obj.curr, obj.limit);
                    }
                }
            });
        },

        // 获取搜索参数
        getSearchParams: function () {
            return {
                name: $('#searchDepartmentName').val().trim(),
                status: $('#searchDepartmentStatus').val()
            };
        },

        // 绑定表格事件
        bindTableEvents: function () {
            var self = this;
            
            // 全选/取消全选
            $('#checkAllDepartments').on('change', function() {
                $('.department-checkbox').prop('checked', $(this).prop('checked'));
            });
            
            // 编辑部门按钮点击事件
            $('.edit-department-btn').on('click', function() {
                const departmentId = $(this).data('id');
                self.showDepartmentForm('edit', departmentId);
            });
            
            // 删除部门按钮点击事件
            $('.delete-department-btn').on('click', function() {
                const departmentId = $(this).data('id');
                self.deleteDepartment(departmentId);
            });
        },

        // 绑定事件
        bindEvents: function () {
            var self = this;
            
            // 搜索按钮点击事件
            $('#searchDepartmentBtn').on('click', function() {
                self.loadDepartmentData();
            });
            
            // 刷新按钮点击事件
            $('#refreshDepartmentBtn').on('click', function() {
                self.refreshDepartmentData();
            });
            
            // 添加部门按钮点击事件
            $('#addDepartmentBtn').on('click', function() {
                self.showDepartmentForm('add');
            });
            
            // 添加子部门按钮点击事件
            $('#addSubDepartmentBtn').on('click', function() {
                if (self.config.currentDepartmentId) {
                    self.showDepartmentForm('add', null, self.config.currentDepartmentId);
                } else {
                    layer.msg('请先选择一个父部门', {icon: 5});
                }
            });
            
            // 批量删除按钮点击事件
            $('#batchDeleteDepartmentBtn').on('click', function() {
                self.batchDeleteDepartments();
            });
            
            // 保存部门按钮点击事件
            $('#saveDepartmentBtn').on('click', function() {
                self.saveDepartment();
            });
            
            // 表单验证规则
            form.verify({
                departmentName: function (value) {
                    if (!value || value.trim() === '') {
                        return '部门名称不能为空';
                    }
                    if (value.length > 50) {
                        return '部门名称不能超过50个字符';
                    }
                },
                departmentCode: function (value) {
                    if (!value || value.trim() === '') {
                        return '部门代码不能为空';
                    }
                    if (!/^[a-zA-Z0-9_]+$/.test(value)) {
                        return '部门代码只能包含字母、数字和下划线';
                    }
                    if (value.length > 20) {
                        return '部门代码不能超过20个字符';
                    }
                }
            });
        },

        // 刷新部门数据
        refreshDepartmentData: function () {
            $('#searchDepartmentName').val('');
            $('#searchDepartmentStatus').val('');
            this.loadDepartmentData();
            this.loadDepartmentTree();
            layer.msg('数据已刷新', {icon: 1});
        },

        // 显示部门表单
        showDepartmentForm: function (mode, departmentId, parentDepartmentId = null) {
            var self = this;
            var isEdit = mode === 'edit';
            var title = isEdit ? '编辑部门' : '添加部门';
            
            // 重置表单
            $('#departmentForm')[0].reset();
            $('#departmentId').val('');
            
            // 设置模态框标题
            $('#departmentModalTitle').text(title);
            
            // 如果有父部门ID，设置父部门
            if (parentDepartmentId) {
                $('#parentDepartment').val(parentDepartmentId);
            }
            
            // 加载上级部门选项
            self.loadParentDepartmentOptions(isEdit ? departmentId : null);
            
            // 加载负责人选项
            self.loadManagerOptions();
            
            // 预加载部门数据（如果是编辑模式）
            if (isEdit && departmentId) {
                self.loadDepartmentDetail(departmentId, function (department) {
                    self.fillDepartmentForm(department);
                    self.openDepartmentModal();
                });
            } else {
                self.openDepartmentModal();
            }
        },

        // 加载上级部门选项
        loadParentDepartmentOptions: function (currentDepartmentId = null) {
            var self = this;
            var parentDepartmentSelect = $('#parentDepartment');
            
            http.get(self.config.baseUrl + '/tree')
                .then(function (res) {
                    if (res.code === 200 && res.data) {
                        self.renderParentDepartmentOptions(res.data, parentDepartmentSelect, currentDepartmentId);
                    } else {
                        // 使用模拟数据
                        self.renderParentDepartmentOptions(self.getMockDepartmentTree(), parentDepartmentSelect, currentDepartmentId);
                    }
                })
                .catch(function (error) {
                    console.error('加载上级部门选项失败:', error);
                    // 使用模拟数据
                    self.renderParentDepartmentOptions(self.getMockDepartmentTree(), parentDepartmentSelect, currentDepartmentId);
                });
        },

        // 渲染上级部门选项
        renderParentDepartmentOptions: function (treeData, selectElement, currentDepartmentId) {
            var options = '';
            
            // 递归构建部门选项
            function buildOptions(nodes, level = 0) {
                nodes.forEach(function (node) {
                    // 排除当前编辑的部门
                    if (node.id !== currentDepartmentId) {
                        var indent = level > 0 ? Array(level + 1).join('&nbsp;&nbsp;&nbsp;') : '';
                        options += `<option value="${node.id}">${indent}${node.name}</option>`;
                        
                        if (node.children && node.children.length > 0) {
                            buildOptions(node.children, level + 1);
                        }
                    }
                });
            }
            
            buildOptions(treeData);
            
            // 插入选项
            selectElement.html('<option value="0">无上级部门</option>' + options);
            form.render('select');
        },

        // 加载负责人选项
        loadManagerOptions: function () {
            var self = this;
            var managerSelect = $('#departmentManager');
            
            http.get('/api/users/managers')
                .then(function (res) {
                    if (res.code === 200 && res.data) {
                        self.renderManagerOptions(res.data, managerSelect);
                    } else {
                        // 使用模拟数据
                        self.renderManagerOptions(self.getMockManagers(), managerSelect);
                    }
                })
                .catch(function (error) {
                    console.error('加载负责人选项失败:', error);
                    // 使用模拟数据
                    self.renderManagerOptions(self.getMockManagers(), managerSelect);
                });
        },

        // 获取模拟负责人数据
        getMockManagers: function() {
            return [
                {id: 1, name: '张总'},
                {id: 2, name: '李经理'},
                {id: 3, name: '王组长'},
                {id: 4, name: '赵组长'},
                {id: 5, name: '钱经理'},
                {id: 6, name: '孙组长'},
                {id: 7, name: '周经理'}
            ];
        },

        // 渲染负责人选项
        renderManagerOptions: function (managers, selectElement) {
            var options = '';
            managers.forEach(function (manager) {
                options += `<option value="${manager.id}">${manager.name}</option>`;
            });
            
            selectElement.html('<option value="">请选择负责人</option>' + options);
            form.render('select');
        },

        // 加载部门详情
        loadDepartmentDetail: function (departmentId, callback) {
            var self = this;
            
            http.get(self.config.baseUrl + '/' + departmentId)
                .then(function (res) {
                    if (res.code === 200 && res.data) {
                        callback(res.data);
                    } else {
                        layer.msg('部门数据加载失败', {icon: 5});
                    }
                })
                .catch(function (error) {
                    console.error('加载部门详情失败:', error);
                    layer.msg('部门数据加载失败', {icon: 5});
                });
        },

        // 填充部门表单
        fillDepartmentForm: function (department) {
            $('#departmentId').val(department.id);
            $('#departmentName').val(department.name);
            $('#departmentCode').val(department.code);
            $('#parentDepartment').val(department.parentId || 0);
            $('#departmentManager').val(department.managerId || '');
            $('#departmentDescription').val(department.description || '');
            
            // 设置状态
            if (department.status === '禁用') {
                $('#departmentStatus').prop('checked', false);
            }
            
            form.render();
        },

        // 打开部门模态框
        openDepartmentModal: function () {
            var modal = new bootstrap.Modal(document.getElementById('departmentModal'));
            modal.show();
        },

        // 保存部门
        saveDepartment: function () {
            var self = this;
            var departmentId = $('#departmentId').val();
            var isEdit = !!departmentId;
            
            // 验证表单
            if (!form.verify().departmentName($('#departmentName').val()) && 
                !form.verify().departmentCode($('#departmentCode').val())) {
                
                var departmentData = {
                    name: $('#departmentName').val().trim(),
                    code: $('#departmentCode').val().trim(),
                    parentId: $('#parentDepartment').val(),
                    managerId: $('#departmentManager').val(),
                    description: $('#departmentDescription').val().trim(),
                    status: $('#departmentStatus').prop('checked') ? 1 : 0
                };
                
                var url = isEdit ? self.config.baseUrl + '/' + departmentId : self.config.baseUrl;
                var method = isEdit ? 'put' : 'post';
                
                http[method](url, departmentData)
                    .then(function (res) {
                        if (res.code === 200) {
                            layer.msg('保存成功', {icon: 6});
                            
                            // 关闭模态框
                            $('#departmentModal').modal('hide');
                            
                            // 刷新数据
                            self.refreshDepartmentData();
                        } else {
                            layer.msg(res.message || '保存失败', {icon: 5});
                        }
                    })
                    .catch(function (error) {
                        console.error('保存部门失败:', error);
                        layer.msg('保存失败', {icon: 5});
                    });
            }
        },

        // 删除部门
        deleteDepartment: function (departmentId) {
            var self = this;
            var departmentName = $('.delete-department-btn[data-id="' + departmentId + '"]').closest('tr').find('td:eq(2)').text();
            
            layer.confirm('确定要删除部门 "' + departmentName + '" 吗？', {
                icon: 3,
                title: '删除确认'
            }, function(index) {
                http.delete(self.config.baseUrl + '/' + departmentId)
                    .then(function (res) {
                        if (res.code === 200) {
                            layer.msg('删除成功', {icon: 1});
                            
                            // 刷新数据
                            self.refreshDepartmentData();
                        } else {
                            layer.msg(res.message || '删除失败', {icon: 5});
                        }
                    })
                    .catch(function (error) {
                        console.error('删除部门失败:', error);
                        layer.msg('删除失败', {icon: 5});
                    });
                
                layer.close(index);
            });
        },

        // 批量删除部门
        batchDeleteDepartments: function () {
            var self = this;
            var selectedDepartments = $('.department-checkbox:checked');
            
            if (selectedDepartments.length === 0) {
                layer.msg('请选择要删除的部门', {icon: 5});
                return;
            }
            
            var departmentIds = selectedDepartments.map(function() {
                return $(this).val();
            }).get();
            
            layer.confirm('确定要删除选中的 ' + selectedDepartments.length + ' 个部门吗？', {
                icon: 3,
                title: '批量删除确认'
            }, function(index) {
                http.delete(self.config.baseUrl + '/batch', { ids: departmentIds })
                    .then(function (res) {
                        if (res.code === 200) {
                            layer.msg('删除成功', {icon: 1});
                            
                            // 刷新数据
                            self.refreshDepartmentData();
                        } else {
                            layer.msg(res.message || '删除失败', {icon: 5});
                        }
                    })
                    .catch(function (error) {
                        console.error('批量删除部门失败:', error);
                        layer.msg('删除失败', {icon: 5});
                    });
                
                layer.close(index);
            });
        }
    };

    exports('department', DepartmentManager);
});