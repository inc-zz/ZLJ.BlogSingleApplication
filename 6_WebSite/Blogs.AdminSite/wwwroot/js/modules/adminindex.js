layui.define(['layer', 'form', 'table', 'laypage', 'core', 'laytpl'], function (exports) {

    var $ = layui.$;
    var layer = layui.layer;
    var form = layui.form;
    var core = layui.core;
    var http = core.http;
    var laytpl = layui.laytpl;

    var AdminIndexCtl = {
        // 配置参数
        config: {
            baseUrl: '/admin/home',
            pageSize: 10
        },
        init: function () {
            $("#homeUserArticleList").load("../js/templates/adminhome-templates.html");
            this.loadArticleList();
            this.updateServerTime();
            this.getHomeStatistics();
        },
        //首页文章列表
        loadArticleList: function () {
            var self = this;
            http.get(this.config.baseUrl + '/articleList')
                .then(function (res) {
                    if (res.code === 200) {
                        var articleData = res.items || [];
                        self.renderArticleList(articleData);
                    } else {
                        console.error('加载文章数据失败:', res.message);
                        self.renderArticleList([]);
                    }
                })
                .catch(function (error) {
                    console.error('加载文章数据失败:', error);
                    self.renderArticleList([]);
                });
        },
        //首页数据统计
        getHomeStatistics: function () {
            var self = this;
            http.get(this.config.baseUrl + '/statistics')
                .then(function (res) {
                    if (res.code === 200) {
                        var data = res.data;
                        var container = $('#homeStatisticsArea');
                        container.empty();
                        var templateElem = document.getElementById('homeStatisticsTpl');
                        if (!templateElem) {
                            throw new Error('模板元素未找到');
                        }
                        var templateContent = templateElem.innerHTML;
                        if (!templateContent) {
                            templateContent = $("#homeStatisticsArea").innerHTML();
                        }
                        // 使用laytpl渲染数据
                        var html = laytpl(templateContent).render(data);
                        container.html(html);


                    } else {
                        console.error('加载统计数据失败:', res.message);
                    }
                })
                .catch(function (error) {
                    console.error('加载统计数据失败:', error);
                });
        },

        // 渲染数据列表
        renderArticleList: function (data) {
            var container = $('#topUserArticleList');
            container.empty();

            if (data && data.length > 0) {
                try {
                    // 获取模板内容
                    var templateElem = document.getElementById('topUserArticleListTpl');
                    if (!templateElem) {
                        throw new Error('模板元素未找到');
                    }

                    var templateContent = templateElem.innerHTML;
                    if (!templateContent) {
                        debugger
                        templateContent = $("#homeUserArticleList").innerHTML();
                    }

                    // 使用laytpl渲染数据
                    var html = laytpl(templateContent).render(data);
                    container.html(html);

                    // 如果有lay-done回调，执行它
                    if (typeof layui.data !== 'undefined' && typeof layui.data.done === 'function') {
                        layui.data.done(data);
                    }

                } catch (error) {
                    console.error('模板渲染失败:', error);
                    //this.renderFallbackList(data);
                }
            } else {
                this.renderEmptyList();
            }
        }, 

        // 渲染空列表
        renderEmptyList: function () {
            var container = $('#topUserArticleList');
            container.html(`
                <div class="card-body">
                    <div class="text-center text-muted py-4">
                        <i class="fas fa-inbox fa-3x mb-3"></i>
                        <p>暂无文章数据</p>
                    </div>
                </div>
            `);
        },

        // HTML转义函数
        escapeHtml: function (unsafe) {
            if (!unsafe) return '';
            return unsafe
                .toString()
                .replace(/&/g, "&amp;")
                .replace(/</g, "&lt;")
                .replace(/>/g, "&gt;")
                .replace(/"/g, "&quot;")
                .replace(/'/g, "&#039;");
        },

        // 更新服务器时间
        updateServerTime: function () {
            var updateTime = function () {
                var now = new Date();
                var timeString = now.getFullYear() + '-' +
                    (now.getMonth() + 1).toString().padStart(2, '0') + '-' +
                    now.getDate().toString().padStart(2, '0') + ' ' +
                    now.getHours().toString().padStart(2, '0') + ':' +
                    now.getMinutes().toString().padStart(2, '0') + ':' +
                    now.getSeconds().toString().padStart(2, '0');
                $('#serverTime').text(timeString);
            };

            updateTime();
            setInterval(updateTime, 1000);
        }
    };

    exports('adminindex', AdminIndexCtl);
});