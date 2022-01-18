---
layout: post
title: "Unity Shader 基础"
image: ''
date:  2019-09-26 00:00:00
tags:
- invisible
- Unity
- Shader
description: ''
categories:
- 学习总结
---

## 概述
着色器的学习并非易事，但是在最近的学习过程中，我发现如果对着色器一无所知将很难在游戏开发这条道路上继续学习下去。故以本文记录一些Shader学习笔记。

## ShaderLab
正如Unity联合创始人之一的Nicholas Francis所说：“ShaderLab is a friend you can afford”。在Unity中学习着色器相关知识，必不可少要与ShaderLab打交道，这是Unity提供的一种抽象中间层便于开发者开发的着色器说明性语言。

## Unity Shader结构
Unity Shader结构更多的指的是ShaderLab的结构，它是由一些嵌套在花括号内的语义来描述的。其主要结构包括**Properities、SubShader、Fallback**等，下文对这些结构进行简单性的描述与介绍。  
### Shader
每个Shader文件都需要通过*Shader*语义来确定一个名称，其语法规则为:
```c
Shader "shaderName" {}
```
每个ShaderName均为字符串组成，可以使用'\\'字符实现多层级划分。
### Properties
*Properties*语义块则是ShaderLab沟通材质与Unity Shader的桥梁，在此部分定义的属性都会被显示在材质面板中，相当于cpp文件的全局变量，其语法定义如下：
```c
Properties {
    Name ("display name", PropertyType) = DefaultValue
    Name ("display name", PropertyType) = DefaultValue
    ...
}
```
其中Name字段为属性名称，在Unity中，这类名称一般由下划线开始，如“_Color”。"display name"则为显示在属性面板上的名称，每个属性拥有类型，PropertyType字段便是指定该属性类型，常见的属性类型如下表所示：

|名称|类型|默认值定义|示例|  
|整型|Int|number|_Int("Heath", Int) = 2|  
|浮点型|Float|number|_Float("Speed", Float) = 2.0|  
|范围型|Range|number|_Range("Intensity", Range(0.0, 10.0)) = 3.0|  
|颜色|Color|(r,g,b,a)|_Color("Color", Color) = (0,0,0,1)|  
|向量|Vector|(x,y,z,w)|_Vector("Vector", Vector) = (1,2,3,4)|  
|2D纹理|2D|"defaultTexture" {}|_2D("Texture", 2D) = ""{}|  
|立方体纹理|Cube|"defaultTexture" {}|_Cube("Texture", Cube) = "white" {}|  
|3D纹理|3D|"defaultTexture" {}|_3D("Texture", 3D) = "black"{}|  

为了在Shader中访问到这些属性，我们需要在cg代码中定义和这些属性类型相匹配的变量，但是Properties属性中未定义任何变量我们同样可以在cg代码中定义变量。可以说Properties语义的作用便只是在Unity属性面板中显示而已。
### SubShader
*SubShader*是Shader文件中一个重量级成员，每个Shader文件中可以包括若干个*SubShader*语义块，但是至少要有一个。



