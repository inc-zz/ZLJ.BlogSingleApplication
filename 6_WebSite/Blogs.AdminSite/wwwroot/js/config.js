/**
 * 项目主入口文件 - 统一配置
 */
layui.config({
    base: '/js/modules/', // 业务模块目录
    version: new Date().getTime(),//开启动态版本，每次最新 //'20240926',
    debug: true
});

// 全局配置对象
var AppConfig = {
    
    storage: {
        prefix: 'blog_admin_'
    },
    response: {
        statusName: 'code',
        statusCode: {
            success: 200,
            unauthorized: 401,
            forbidden: 403,
            error: 500
        },
        msgName: 'message',
        dataName: 'data'
    },
    page: {
        size: 10,
        sizes: [5, 10, 15, 20]
    }
};

// 扩展模块路径
layui.extend({
    'core': '../core'
});

// 初始化核心模块
layui.use(['core'], function () {
    var core = layui.core;
    console.log('核心工具类加载完成');

    // 可以将配置存储到 core 中（可选）
    core.AppConfig = AppConfig;
});