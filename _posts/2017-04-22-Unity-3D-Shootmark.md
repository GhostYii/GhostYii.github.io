---
layout: post
title: "使用Unity制作FPS游戏弹痕效果"
image: ''
date:  2017-04-22 11:34:10
tags:
- Unity
- Unity3D
description: ''
categories:
- 游戏开发
---

***
## 概述
本文讲述如何使用Unity（版本5.6.0f3）制作FPS游戏中的弹痕效果，其最终效果如下图所示，其中弹痕与准心位置存在偏移是由于设置了弹道导致，并非本文讲述重点。
![](..\assets\img\Shootmark\fin.gif)

***
## 基本思路与原理
要实现该效果，其基本思路是`贴图融合`。即使得墙面或者需要产生弹痕的物体上的贴图与弹痕贴图融合，产生一张混合贴图。

***
## 具体实现  

### 准备工作
在实现这个效果之前，先需要准备一个测试环境。基本步骤如下：  
1. 在一个3D场景中创建一个3D Plane（如果是3D模型后面的设置会有所不同，在此先以平面为例）。将其Transform设置为(0,0,10)、(90,0,180)、(1,1,1)。
2. 创建一个材质球，将需要产生弹痕的贴图设置为该材质的主贴图。
3. 将材质球赋予该3D Plane。并给Plane添加Rigidbody，并且将isKinematic设置为true。
4. 将Main Camera的Transform设置为(0,0,-10)、(0,0,0)、(1,1,1)。
4. 创建一个可以自由移动摄像机的脚本CameraView.cs（此脚本的编写较为简单，在此不讲述如何编写。如有需要在本文的末尾中可以找到源代码），
挂载在Main Camera上，选择一张可以作为准心的图片填充pointer 变量。此时运行场景应该可以控制Camera自由观察。  
5. 给Main Camera添加一个射击功能，这一部分Unity官方给出了Demo，参考这里：[ [Unity-Let's Try: Shooting with Raycasts ](https://unity3d.com/cn/learn/tutorials/projects/lets-try-assignments/lets-try-shooting-raycasts-article?playlist=41639) ]，
但是该Demo中的脚本无法直接用于这个例子中，需要进行修改，修改后的脚本请查看本文最后的`Shooter.cs`脚本。

上述准备工作做完之后，可以开始思考如何实现弹痕效果了。

### 实现
通过上方的准备工作，我们已经可以开始制作弹痕效果了。
创建一个脚本，这个脚本应该是挂载在需要产生弹痕效果的物体上，命名为`ShootmarkMaker.cs`，首先我们通过`Shooter.GetHitObject()`方法来获取是否有射中的物体，然后通过传回来的hit的name与自身的name进行对比，相同则在本物体上产生弹痕，弹痕贴图在Shooter脚本中已经有定义。  
下一步是获取射击的点，要注意的是这个点的坐标**不是hit.point**，这个hit.point是指的在Unity坐标系中的碰撞点坐标，然而在这里应该获取的是物体本身的**UV坐标**。  
那如何去获取UV坐标呢？在上一篇文章（传送门[Unity 实现2D伪阴影](http://ghostyii.com/Unity-2D-Shadow/)）中，提到了如何把Mesh中的顶点坐标从世界坐标转为模型空间坐标的方法，但是这里又是另外一种坐标系了（Unity中存在的坐标系是真的多啊…），这个坐标通过查阅API手册，可以通过`hit.textureCoord`来获取，Unity中给出的说明是`The uv texture coordinate at the collision location.`  
这正是我们所需要的坐标。  

因为需要保存所有的点并且要有弹痕消失的效果，可以使用一个队列来保存所有的点的位置，每次将点入队。然后就可以开始绘制融合贴图了。

***
## 问题
这里涉及到一个问题，如何在代码中去绘制一张贴图呢？

***
## 解决方案
先说明一点：
`一张图片在计算机中，一般是通过像素点来渲染出来的，也就是所有的图片都是一个像素点列表，每个像素点都拥有一个颜色信息，而这个像素点的数量也就是通常所说的分辨率。`
例如128*128的图片分辨率就是拥有128*128 = 16384个像素点。
那如何在Unity中获取像素点呢？这倒是挺方便，因为在Unity的Texture类中可以通过`public Color GetPixel(int x, int y);`方法来获取某一个像素点的颜色，通过`texture.Width和texture.Height`来获取像素点。

那这样的话，就可以使用两层for循环来对贴图进行修改了！值得注意的一点是**需要修改的贴图需要在导入设置中将Read/Write Enable设置为true**  
![img](..\assets\img\Shootmark\read.PNG)

在循环中，需要做的唯一一件事情就是将子弹贴图在该UV坐标的颜色与物体贴图在该UV坐标的颜色进行融合，也就是进行乘法操作。

那么该代码就可以这么写：
```csharp
public void MakeShootMark()
{
    if (gun.GetHitObject().HasValue && gun.GetHitObject().Value.transform.name == this.name)
    {
        Vector2 uv = gun.GetHitObject().Value.textureCoord;

        uvQueues.Enqueue(uv);

        for (int i = 0; i < bulletWidth; i++)
        {
            for (int j = 0; j < bulletHeight; j++)
            {
                float w = uv.x * wallWidth - bulletWidth / 2 + i;
                float h = uv.y * wallHeight - bulletHeight / 2 + j;

                Color wallColor = newWallTexture.GetPixel((int)w, (int)h);
                Color bulletColor = gun.bullueTexture.GetPixel(i, j);

                newWallTexture.SetPixel((int)w, (int)h, wallColor * bulletColor);
            }
        }
        newWallTexture.Apply();
    }
}
```

这样的话，最核心的代码也就搞定了，然后就是去依次消除产生的贴图，这个消除贴图，其实可以看作是上述操作的反向操作，代码是很相似的，如下：
```csharp
private void ResetShootMark()
{
    Vector2 uv = uvQueues.Dequeue();
    for (int i = 0; i < bulletWidth; i++)
    {
        for (int j = 0; j < bulletHeight; j++)
        {
            float w = uv.x * wallWidth - bulletWidth / 2 + i;
            float h = uv.y * wallHeight - bulletHeight / 2 + j;

            Color wallColor = wallTexture.GetPixel((int)w, (int)h);
            newWallTexture.SetPixel((int)w, (int)h, wallColor);
        }
    }

    newWallTexture.Apply();
}
```
将代码编写完成后，我们将该脚本挂载到之前创建的Plane物体上，然后设置好相关变量，运行场景，发现已经可以产生弹痕了，效果如下图：  
![fin](..\assets\img\Shootmark\fin2.gif)

***
## 附录

点击下方脚本名称以下载。

1. [CameraView.cs](../assets/downloadable/CameraView.cs) 
2. [Shooter.cs](../assets/downloadable/Shooter.cs)
3. [ShootMarkMaker.cs](../assets/downloadable/ShootMarkMaker.cs)

