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
            baseUrl: '/admin/department',
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
            });
        },

        // 加载部门树
        loadDepartmentTree: function () {
            var self = this;
            
            http.get(self.config.baseUrl + '/tree')
                .then(function (res) {
                    
                    if (res.code === 200 && res.data) {
                        self.renderDepartmentTree(res.data);
                    } 
                })
                .catch(function (error) {
                    console.error('加载部门树失败:', error);
                });
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
            // if (treeData && treeData.length > 0) {
            //     self.selectDepartment(treeData[0].id);
            // }
            self.selectDepartment(1);
            // 绑定部门点击事件
            self.bindDepartmentTreeEvents();
        },

        // 选择部门
        selectDepartment: function (departmentId) {
            debugger
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
                depId: 0,// self.config.currentDepartmentId,
                ...searchParams
            };
            
            http.get(self.config.baseUrl+'/list', params)
                .then(function (res) {
                    if (res.code === 200 && res.data) {
                        // 直接传递完整的响应数据，renderDepartmentTable方法会处理items数组
                        self.renderDepartmentTable(res.data);
                        self.initPagination(res.data.total, page, limit);
                    } else {
                        // 使用模拟数据
                        const mockData = {
                            items: self.getMockDepartmentData()
                        };
                        self.renderDepartmentTable(mockData);
                        self.initPagination(20, page, limit);
                    }
                })
                .catch(function (error) {
                    console.error('加载部门数据失败:', error);
                    // 使用模拟数据
                    const mockData = {
                        items: self.getMockDepartmentData()
                    };
                    self.renderDepartmentTable(mockData);
                    self.initPagination(20, page, limit);
                });
        },

        // 获取模拟部门数据
        getMockDepartmentData: function() {
            return   [
            {
                "id": 1,
                "name": "销售部",
                "parentName": "顶级部门",
                "description": "1121",
                "createdAt": "2025-09-29T23:59:09",
                "createdBy": "admin",
                "modifiedAt": "2025-09-29T23:59:14",
                "modifiedBy": "admin",
                "isDeleted": false,
                "children": [
                    {
                        "id": 101,
                        "name": "华东销售部",
                        "parentName": null,
                        "description": "111",
                        "createdAt": "2025-10-01T01:34:40",
                        "createdBy": "admin",
                        "modifiedAt": "2025-10-01T01:34:46",
                        "modifiedBy": null,
                        "isDeleted": false,
                        "children": null
                    },
                    {
                        "id": 102,
                        "name": "华南销售部",
                        "parentName": null,
                        "description": "111",
                        "createdAt": "2025-10-01T01:34:40",
                        "createdBy": "admin",
                        "modifiedAt": "2025-10-01T01:34:46",
                        "modifiedBy": null,
                        "isDeleted": false,
                        "children": [
                            {
                                "id": 103,
                                "name": "广东省销售部",
                                "parentName": null,
                                "description": "111",
                                "createdAt": "2025-10-01T01:34:40",
                                "createdBy": "admin",
                                "modifiedAt": "2025-10-01T01:34:46",
                                "modifiedBy": null,
                                "isDeleted": false,
                                "children": [
                                    {
                                        "id": 104,
                                        "name": "深圳销售部",
                                        "parentName": null,
                                        "description": "111",
                                        "createdAt": "2025-10-01T01:34:40",
                                        "createdBy": "admin",
                                        "modifiedAt": "2025-10-01T01:34:46",
                                        "modifiedBy": null,
                                        "isDeleted": false,
                                        "children": null
                                    },
                                    {
                                        "id": 105,
                                        "name": "汕头销售部",
                                        "parentName": null,
                                        "description": "111",
                                        "createdAt": "2025-10-01T01:34:40",
                                        "createdBy": "admin",
                                        "modifiedAt": "2025-10-01T01:34:46",
                                        "modifiedBy": null,
                                        "isDeleted": false,
                                        "children": null
                                    }
                                ]
                            }
                        ]
                    }
                ]
            },
            {
                "id": 2,
                "name": "实施部",
                "parentName": "顶级部门",
                "description": null,
                "createdAt": "2025-10-01T01:32:56",
                "createdBy": "admin",
                "modifiedAt": "2025-10-01T01:33:02",
                "modifiedBy": "admin",
                "isDeleted": false,
                "children": [
                    {
                        "id": 201,
                        "name": "华南实施部",
                        "parentName": null,
                        "description": "111",
                        "createdAt": "2025-10-01T01:34:40",
                        "createdBy": "admin",
                        "modifiedAt": "2025-10-01T01:34:46",
                        "modifiedBy": null,
                        "isDeleted": false,
                        "children": null
                    }
                ]
            },
            {
                "id": 3,
                "name": "研发部",
                "parentName": "顶级部门",
                "description": "111",
                "createdAt": "2025-10-01T01:34:40",
                "createdBy": "admin",
                "modifiedAt": "2025-10-01T01:34:46",
                "modifiedBy": null,
                "isDeleted": false,
                "children": null
            }
        ]
        },

        // 渲染部门表格
        renderDepartmentTable: function (data) {
            var self = this;
            const tbody = $('#departmentTableBody');
            
            if (data.length === 0) {
                tbody.html('<tr><td colspan="9" class="text-center py-4">暂无数据</td></tr>');
                return;
            }
            
            // 准备树形数据（用户提供的数据已经是树形结构，这里只需要处理数据转换）
            let html = '';
            
            // 递归渲染树形表格
            function renderTreeNode(nodes, level = 0, parentIds = []) {
                nodes.forEach(function (dept) {
                    const currentParentIds = [...parentIds, dept.id];
                    
                    // 格式化创建时间
                    const formattedCreateTime = dept.createdAt ? new Date(dept.createdAt).toLocaleString('zh-CN') : '';
                    const formattedModifyTime = dept.modifiedAt ? new Date(dept.modifiedAt).toLocaleString('zh-CN') : '';
                    
                    // 状态显示
                    const statusBadge = dept.isDeleted ?
                        '<span class="badge bg-danger">已删除</span>' :
                        '<span class="badge bg-success">正常</span>';
                    
                    // 树形结构缩进
                    const indentStyle = level > 0 ? `style="padding-left: ${level * 20}px;"` : '';
                    
                    // 展开/折叠图标
                    const hasChildren = dept.children && dept.children.length > 0;
                    const treeIcon = hasChildren ? 
                        `<i class="fas fa-chevron-down tree-icon mr-1"></i>` : 
                        `<i class="tree-icon-placeholder mr-1"></i>`;
                    
                    html += `
                        <tr class="tree-node" data-id="${dept.id}" data-level="${level}" data-parent-ids="${currentParentIds.join(',')}">
                            <td><input type="checkbox" class="department-checkbox" value="${dept.id}"></td>
                            <td>${dept.id}</td>
                            <td ${indentStyle}>
                                <div class="tree-cell-content">
                                    ${treeIcon}
                                    ${dept.name}
                                </div>
                            </td>
                            <td>${dept.description || ''}</td>
                            <td>${formattedCreateTime}</td>
                            <td>${formattedModifyTime}</td>
                            <td>${statusBadge}</td>
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
                    
                    // 递归渲染子节点
                    if (hasChildren) {
                        renderTreeNode(dept.children, level + 1, currentParentIds);
                    }
                });
            }
            
            // 根据用户提供的数据结构，直接渲染items数组
            renderTreeNode(data.items || data);
            tbody.html(html);
            
            // 绑定树形表格事件
            this.bindTreeTableEvents();
            
            // 绑定普通表格事件
            this.bindTableEvents();
        },

        // 绑定树形表格事件
        bindTreeTableEvents: function() {
            var self = this;
            
            // 点击展开/折叠图标
            $('.tree-icon').on('click', function(e) {
                e.stopPropagation();
                const $icon = $(this);
                const $currentRow = $icon.closest('tr');
                const currentId = $currentRow.data('id');
                const currentLevel = $currentRow.data('level');
                
                // 切换图标
                if ($icon.hasClass('fa-chevron-down')) {
                    $icon.removeClass('fa-chevron-down').addClass('fa-chevron-right');
                    // 隐藏所有子节点（包括子节点的子节点）
                    $(`tr.tree-node`).each(function() {
                        const $childRow = $(this);
                        const childLevel = $childRow.data('level');
                        // 确保parentIds是字符串类型后再调用split
                        const parentIdsStr = String($childRow.data('parent-ids'));
                        const parentIds = parentIdsStr ? parentIdsStr.split(',') : [];
                        
                        if (childLevel > currentLevel && parentIds.includes(currentId.toString())) {
                            $childRow.hide();
                        }
                    });
                } else {
                    $icon.removeClass('fa-chevron-right').addClass('fa-chevron-down');
                    // 显示所有子节点
                    $(`tr.tree-node`).each(function() {
                        const $childRow = $(this);
                        const childLevel = $childRow.data('level');
                        // 确保parentIds是字符串类型后再调用split
                        const parentIdsStr = String($childRow.data('parent-ids'));
                        const parentIds = parentIdsStr ? parentIdsStr.split(',') : [];
                        
                        if (childLevel > currentLevel && parentIds.includes(currentId.toString())) {
                            $childRow.show();
                            // 同时展开直接子节点的图标（如果已展开）
                            const childIcon = $childRow.find('.tree-icon');
                            if (childIcon.hasClass('fa-chevron-right')) {
                                childIcon.click();
                            }
                        }
                    });
                }
            });
        },

        // 检查节点是否是指定父节点的后代
        isDescendantOf: function($row, parentId) {
            // 确保parentIds是字符串类型后再调用split
            const parentIdsStr = String($row.data('parent-ids'));
            const parentIds = parentIdsStr ? parentIdsStr.split(',') : [];
            return parentIds.includes(parentId.toString());
        },

        // 检查节点是否是指定父节点的直接子节点
        isDirectChildOf: function($row, parentId) {
            // 确保parentIds是字符串类型后再调用split
            const parentIdsStr = String($row.data('parent-ids'));
            const parentIds = parentIdsStr ? parentIdsStr.split(',') : [];
            const currentLevel = $row.data('level');
            const parentElement = $(`[data-id="${parentId}"]`);
            const parentLevel = parentElement.data('level');
            
            return parentIds.includes(parentId.toString()) && currentLevel === parentLevel + 1;
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
                        // self.loadDepartmentData(obj.curr, obj.limit);
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
            debugger
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