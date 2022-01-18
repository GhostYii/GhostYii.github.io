---
layout: post
title: "Unity 实现2D伪阴影"
image: '/assets/img/Unity2D/preview3.png'
date:  2017-04-14 00:00:00
tags:
- Unity
- Unity2D
description: ''
categories:
- 游戏开发
---

***
## 概述
  本文将讲述如何使用Unity（版本5.5.2f1）实现2D游戏中的伪阴影效果，其最终效果如下图：
  ![最终效果](..\assets\img\Unity2D\effect.gif)

***
## 基本思路
  为了实现这样一个伪阴影效果，要先思考该效果的实现原理，在开发之前，我在网上查找过相关资料，发现有许多开发者使用Shader来剔除物体，即从3D角度入手，使用一个带剔除功能的shader来动态生成遮罩。

  但是我对shader的了解实在是有限，也没有什么基础，所以我决定采用另外一种办法来解决这个问题。

  我最开始注意到了`LineRenderer`这一组件，这个组件在最新版的Unity（Unity5.6）中进行了一次比较大的更新，已经可以绘制出非常平滑的线段。

  于是我想到了这样一条思路：
  >  使用射线检测来检查顶点，找到一圈可以围起来的顶点，然后使用LineRenderer绘制出封闭图形，然后填充该区域。

  使用射线检测，在一个确定的圆心[本例中采用脚本挂载的gameObject的position]，向周围发射一圈射线来确定顶点。这样做的好处还有一个，就是可以分层检测，即可以实现部分物体遮挡光线的效果，如上图中蓝色物体不会遮挡光线。仅有红色物体能够遮挡光线。

***
## 实现
  想通了原理之后就可以着手编码了，首先，实现一个向四面八方发射射线的效果，这个难度不大，稍有数学知识的人就能解决：

  ```csharp
  void RayCast360(Vector2 origin,float distance, int mask)
  {
      for (int i = 0; i < 360; i++)
      {        
          Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * i), Mathf.Sin(Mathf.Deg2Rad * i));
          Debug.DrawRay(origin, direction * distance);
      }
  }
  ```
  为了测试我们先使用`Debug.DrawRay`方法在Gizmos里预览一下。

  这样得到的效果如下图所示：
  ![RayCast360](..\assets\img\Unity2D\preview.png)

  如愿以偿的产生了360条射线，这还只是一个简单的圆形，如果加上射线检测，便可成为一个可以被遮挡的多边形，只需要修改脚本如下：
  ```csharp
  void RayCast360(Vector2 origin,float distance, int mask)
   {
       for (int i = 0; i < 360; i++)
       {        
           Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * i), Mathf.Sin(Mathf.Deg2Rad * i));
           RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, mask);
           float realDistance = hit.collider == null ? distance : hit.distance;

           Debug.DrawRay(origin, direction * realDistance);
       }
   }
  ```

  这里我们先把遮罩选为`Everything`，即所有带Collider的物体都会遮挡射线，此时只要在场景中存在带Collider的物体，即可被遮挡住，如下图所示：
  ![ColliderCulling](..\assets\img\Unity2D\preview2.png)

  如果此时存在一个数组来保存顶点，我们就可以得到一圈顶点，即可以使用LineRenderer来绘制封闭区域。
  修改脚本，添加保存顶点的数组，然后在物体上挂载LineRenderer（如果该物体必须依赖某些组件，可以使用`RequireComponent(Type requiredComponent);`Arritube来修饰，这样在挂载脚本的时候会自动添加所依赖的组件）。

  在这里需要注意的一点是，在上面的脚本中，`direction*realDistance`这一字段并非终点位置。而是一个方向向量乘以距离。我们如果要得到目标终点，应该使用  `Vector2 end = new Vector2(position.x + distance * direction.x / direction.magnitude, position.y + distance * direction.y / direction.magnitude);`
  来计算终点。

  修改脚本如下

  ```csharp
  void RayCast360(Vector2 origin,float distance, int mask)
  {
    List<Vector2> vertexLst = new List<Vector2>();

    for (int i = 0; i < 360; i++)
    {        
        Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * i), Mathf.Sin(Mathf.Deg2Rad * i));
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, mask);

        float realDistance = hit.collider == null ? distance : hit.distance;
        Vector2 end = new Vector2(origin.x + realDistance * direction.x / direction.magnitude, origin.y + realDistance * direction.y / direction.magnitude);
        vertexLst.Add(end);

        Debug.DrawLine(origin, end);            
    }
  }
  ```
  启动场景发现效果不变，说明顶点数组已经保存，原先的DrawRay方法已经修改为DrawLine方法，这也验证了我们的目的点计算无误。

  下一步就是使用LineRenderer渲染封闭区域了。需要注意的一点是，如果要形成封闭图形，需要顶点数组的最后一个位置与第一个位置相同，也就是首尾相连。

  渲染完成后的图形如下：
  ![LineLight](..\assets\img\Unity2D\preview3.png)
  完整代码如下：
  ```csharp
    void RayCast360(Vector2 origin,float distance, int mask)
    {
        line = GetComponent<LineRenderer>();
        List<Vector3> vertexLst = new List<Vector3>();

        for (int i = 0; i < 360; i++)
        {        
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * i), Mathf.Sin(Mathf.Deg2Rad * i));
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, mask);

            float realDistance = hit.collider == null ? distance : hit.distance;
            Vector2 end = new Vector2(origin.x + realDistance * direction.x / direction.magnitude, origin.y + realDistance * direction.y / direction.magnitude);
            vertexLst.Add(end);
        }

        line.numPositions = vertexLst.Count;
        vertexLst[vertexLst.Count - 1] = vertexLst[0];
        line.SetPositions(vertexLst.ToArray());
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
    }
  ```
  下一步就是填充该封闭图形，然后这个效果就大功告成了。

***

## 问题
  做到这一步的时候发现填充LineRenderer并不是一件容易的事情，我并不会做啊！
  这个时候我就只能寻找其他绘图方式了。

***
## 解决方案
  这时候采用`MeshRenderer`是不错的选择，使用`MeshRenderer`的话，基本思路就应该是
  > 获取顶点集，将该顶点集形成一个封闭多边形，然后将这个多边形triangulate，形成一个Mesh，通过MeshRenderer渲染出来

### Mesh的创建
  在Unity中，创建一个Mesh，需要三个数组，一个是顶点数组，一个是三角形顶点数组，一个是uv数组，这三个数组确定后，就可以绘制出一个唯一的Mesh，在上面的过程中，已经得到了顶点数组，
  只需要计算出三角形顶点数组，这里没有使用到Material的贴图，所以在Mesh中，使用默认的uv即可。

  那么如何去计算三角形顶点数组呢？

  先看一个简单的例子，如果要渲染出一个三角形Mesh，其三个顶点坐标分别为 (0,0,0)、(0,1,0)、(1,0,0)，如图所示，
  ![](..\assets\img\Unity2D\point3.png)
  那么要去构成三角形，就需要把三个顶点按照一定顺序连接起来，这里可以用0-1-2这样的顺序，也可以使用2-1-0这样的顺序，但是要注意的一点是，在Unity中，绘制Mesh一定要顺时针构建三角形。
  那么这样的话，我们的三角形顶点数组就可以是{0，1，2}了。其实其本质上就是顶点数组的下标。

  现在已经可以绘制三角形了，那么我们如何去绘制一个圆呢？
  其实，一个圆可以是很多小三角形拼接而成，如图所示：
  ![](..\assets\img\Unity2D\circle.gif)

  只要确定了圆心与顶点数组，就可以绘制出一个“圆”，当然，这个顶点数组的大小一定要足够大，不然就会变成一个多边形而不那么像圆了。那么这个三角形顶点数组是什么呢？
  从上图可以看到，所有的三角形都有一个共同的顶点，也就是圆心。如果顶点数组的第0号元素是圆心坐标，那么这个顶点数组就可以是
  ```
   { 0, 1, 2, 0, 2, 3, ..., 0, n-2, n-1, 0, n-1, n }
  ```
  那么就可以开始绘制Mesh了。

  值得注意的一点是，Mesh中的顶点是基于模型空间的，如果将Unity中的世界坐标作为顶点传入会产生偏移，即会产生如下图的效果：
  ![](..\assets\img\Unity2D\light2d2.gif)

  需要使用`Transform.InverseTransformPoint`API来将世界坐标转为模型空间坐标才能得到最终的效果。

  参考资料： [Unity3D Mesh小课堂（三）圆形与圆环](http://blog.csdn.net/ecidevilin/article/details/52456107)

***
## 拓展
  从上方的分析来看，只需要对顶点数组的大小进行改变，就可以实现多边形的精度控制，如下图所示：
  ![](..\assets\img\Unity2D\effect2.gif)  
  但是在实际测试中发现，这个对性能的影响并不大，而且这一设定的存在让后面的操作有些问题，所以就取消了这一设定。

  如果需要对Mesh的角度进行控制，则只需要对射线初始投射的方向进行约束就可以了。即将初始的角度换为物体自身欧拉角的z（即绕z轴旋转的度数）
  作为初始角度。

  将上述功能一一实现后，最终效果如下图所示：
  ![](..\assets\img\Unity2D\fin.gif)  

***
## 完整代码
  完整代码请点击下方链接或者访问该项目github地址（<a href="https://github.com/GhostYii/Light2D" target="_blank">地址点我</a>）下载。  
  1. [Light2D.cs](..\assets\downloadable\Light2D.cs)  

***

### 使用方法
  将该脚本挂载到场景中的任意一个空物体上即可预览效果。
