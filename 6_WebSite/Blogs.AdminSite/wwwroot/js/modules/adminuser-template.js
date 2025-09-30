/**
 * 用户管理模板模块
 * 负责模板的加载和管理
 */
layui.define(['jquery', 'laytpl'], function (exports) {
    var $ = layui.$;
    var laytpl = layui.laytpl;

    var TemplateManager = {
        // 模板缓存
        templates: {},
        
        // 模板加载状态
        templatesLoaded: false,
        
        // 模板加载完成后的回调队列
        callbacks: [],

        /**
         * 初始化模板
         */
        init: function () {
            var self = this;
            
            // 确保DOM加载完成
            $(function() {
                // 检查user-container元素是否存在
                if ($('#user-container').length === 0) {
                    console.error('未找到user-container元素，请确保页面中包含该元素');
                    return;
                }
                
                // 加载外部模板文件，并在加载完成后初始化
                $('#user-container').load('/js/templates/adminuser-templates.html', function(response, status, xhr) {
                    if (status === 'success') {
                        console.log('模板文件加载成功');
                        self.templatesLoaded = true;
                        self.loadTemplates();
                        
                        // 执行所有等待的回调函数
                        self.callbacks.forEach(callback => callback());
                        self.callbacks = [];
                    } else {
                        console.error('模板文件加载失败:', status);
                        console.error('错误信息:', xhr.status + ' ' + xhr.statusText);
                        
                        // 尝试从页面直接加载模板作为后备方案
                        self.loadTemplates();
                    }
                });
            });
        },

        /**
         * 加载所有模板
         */
        loadTemplates: function () {
            var self = this;
            var templateElements = document.querySelectorAll('script[type="text/html"]');
            
            templateElements.forEach(function (element) {
                var id = element.id;
                if (id) {
                    self.templates[id] = element.innerHTML;
                }
            });
            
            console.log('加载的模板数量:', Object.keys(this.templates).length);
        },

        /**
         * 获取模板内容
         */
        getTemplate: function (templateId) {
            return this.templates[templateId] || '';
        },

        /**
         * 渲染模板
         */
        render: function (templateId, data) {
            var template = this.getTemplate(templateId);
            if (!template) {
                console.error('模板未找到: ' + templateId);
                return '';
            }

            var html = '';
            laytpl(template).render(data || {}, function (string) {
                html = string;
            });
            return html;
        },

        /**
         * 渲染用户表单模态框
         */
        renderUserForm: function (isEdit, userData) {
            return this.render('userFormModalTpl', {
                isEdit: isEdit,
                user: userData || null
            });
        },

        /**
         * 渲染用户详情模态框
         */
        renderUserDetail: function (userData) {
            return this.render('userDetailModalTpl', {
                user: userData || {}
            });
        },

        /**
         * 渲染删除确认对话框
         */
        renderDeleteConfirm: function (userData) {
            return this.render('confirmDeleteModalTpl', {
                user: userData || {}
            });
        },

        /**
         * 渲染批量删除确认对话框
         */
        renderBatchDeleteConfirm: function (count) {
            return this.render('batchDeleteModalTpl', {
                count: count || 0
            });
        },
        
        /**
         * 在模板加载完成后执行操作
         */
        onTemplatesLoaded: function(callback) {
            if (this.templatesLoaded) {
                // 如果模板已经加载完成，直接执行回调
                callback();
            } else {
                // 否则将回调加入队列
                this.callbacks.push(callback);
            }
        }
    };

    // 初始化模板管理器
    TemplateManager.init();
    console.log("用户管理模板模块加载完成");    
    exports('adminuser-template', TemplateManager);
});