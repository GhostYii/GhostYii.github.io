# 改动说明

## 说明
本工程来自于Victor Igor的开源项目 Space，经过本人修改用于本人个人博客的展示，在此非常感谢Victor Igor的初始项目

## 改动详细说明
本工程对源工程的改动如下：

- 修改了网站的色调，更改了绝大部分显示文字
- 将菜单中的五个5项菜单更改为4项，将右侧菜单的4项修改为2项
- 修改了部分css样式
- 修改了文章列表的显示格式（从title修改为title - date）
- 将首页的五个图标修改为四个（删除了推特、Google+、RSS订阅，新增了Bilibili和Steam）
- 删除了文章页面的评论功能
- 删除了文章页面的分享功能
- 增加了CNAME文件并绑定了域名
- 重写了关于界面（about.html）
- 删除了Recent projects显示区域
- 修改首页文章显示最多条目，增加了一个分页功能
- 在所有文章显示界面增加了一个返回顶部按钮
- 修改了文章显示列表的筛选方式
- 添加了Google Analytics追踪服务

## 使用说明
本工程相对与原始项目来说需要注意的几点如下：

- 修改首页和关于界面图片请以“myphoto.png”名称的文件更换assets/img/myphoto.png文件
- 首页最多显示条目的修改请修改_config.yml中的paginate字段
- 如果需要增加一篇隐藏的文章，请将文章的tags标签中第一个tag填写为_config.yml中的hidetag字段（默认为invisible）
