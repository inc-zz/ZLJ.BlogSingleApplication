// main.js - 主入口文件
require.config({
    paths: {
        'jquery': 'https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.min',
        'layui': 'https://cdn.jsdelivr.net/npm/layui@2.8.18/dist/layui.min',
        'http': './http',
        'login': './login'
    },
    shim: {
        'layui': {
            exports: 'layui',
            deps: ['jquery', 'css!https://cdn.jsdelivr.net/npm/layui@2.8.18/dist/css/layui.css']
        }
    },
    map: {
        '*': {
            'css': 'https://cdn.jsdelivr.net/npm/require-css@0.1.10/css.min.js'
        }
    }
});
 