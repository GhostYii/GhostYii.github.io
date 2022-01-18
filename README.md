# Space Jekyll

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

A simple and elegant Jekyll theme based on Spacemacs. The theme works well on mobile devices as well.

See a live demo [here](https://victorvoid.github.io/space-jekyll-template/).

![](https://github.com/victorvoid/space-jekyll-template/blob/master/screenshot.png?raw=true)

# Site/User Settings

customize your site in ``_config.yml``

```ruby

# Site settings
description: A blog about lorem ipsum
baseurl: "" # the subpath
url: "" # the base hostname &/|| protocol for your site

# User settings
username: Lorem Ipsum
user_description: Lorem Developer
user_title: Lorem Ipsum
email: lorem@ipsum.com
twitter_username: loremipsum
github_username:  loremipsum
gplus_username:  loremipsum
disqus_username: loremipsum

```

## How to create a post ?

_posts create a file .md with structure:

```md
---
layout: post
title: "Lorem ipsum speak.."
date: 2016-09-13 01:00:00
image: '/assets/img/post-image.png'
description: 'about tech'
tags:
- lorem
- tech
categories:
- Lorem ipsum
twitter_text: 'How to speak with Lorem'
---
```

## License
The MIT License (MIT)

Copyright (c) 2016 Victor Igor

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
