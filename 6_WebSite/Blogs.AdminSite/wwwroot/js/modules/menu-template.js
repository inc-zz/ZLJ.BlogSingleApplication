/**
 * 菜单管理模板模块
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
                // 检查menu-container元素是否存在
                if ($('#menu-container').length === 0) {
                    console.error('未找到menu-container元素，请确保页面中包含该元素');
                    return;
                }
                
                // 加载外部模板文件
                $('#menu-container').load('/js/templates/menu-templates.html', function(response, status, xhr) {
                    if (status === 'success') {
                        console.log('菜单模板文件加载成功');
                        self.templatesLoaded = true;
                        
                        // 执行所有等待的回调函数
                        self.callbacks.forEach(callback => callback());
                        self.callbacks = [];
                    } else {
                        console.error('菜单模板文件加载失败:', status);
                        console.error('错误信息:', xhr.status + ' ' + xhr.statusText);
                        
                        // 显示错误信息
                        $('#menu-container').html('<div class="alert alert-danger" role="alert">模板加载失败，请刷新页面重试</div>');
                    }
                });
            });
        },

        /**
         * 等待模板加载完成
         */
        waitForTemplates: function(callback) {
            if (this.templatesLoaded) {
                // 如果模板已经加载完成，立即执行回调
                callback();
            } else {
                // 否则将回调加入队列等待执行
                this.callbacks.push(callback);
            }
        }
    };

    // 初始化模板
    TemplateManager.init();

    exports('menu-template', TemplateManager);
});