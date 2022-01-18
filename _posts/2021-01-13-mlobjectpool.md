---
layout: post
title: "MLObject Pool 简易手册"
image: ''
date:  2021-01-13 00:00:00
tags:
- Unity
description: ''
categories:
- 游戏开发
---

---
## 0x0. 引言
对象池是游戏开发中老生常谈的东西了。一个好的对象池对于游戏项目的整体性能提升非常大，本文主要介绍一个开源方便的对象池系统——MLObjectPool。  
MLObjectPool是本人在去年开发的一个针对Unity游戏引擎的对象池系统，其以DLL动态库的形式挂载在Unity Engine中。  
本模块所有代码已经在github中开源，如需源代码请<a href="https://github.com/GhostYii/MLObjectPool" target="_blank">点击此处</a>。  
> 注意：未来MLObjectPool将会整合进MLToolBox运行库项目中。

---
## 0x1. 如何使用？
MLObjectPool工程分为两部分——编辑器拓展与运行时托管库，如有需要对源码进行改造，请将编辑器代码写在MLObjectPool.Editor工程中，其余代码写在MLObjectPool中。  
MLObjectPool已经存在两种类型的build-in对象池:Pool< T >和PrefabPool。 

**如需创建build-in对象池，请使用且仅可使用如下API：**
```csharp
/// <summary>
/// 创建一个普通对象池
/// </summary>
/// <typeparam name="T">对象池对象类型</typeparam>
/// <param name="name">对象池名称（不可重复）</param>
/// <param name="size">对象次初始大小</param>
/// <param name="autoExpand">是否自动扩容</param>
ObjectPoolManager.Instance.CreatePool<T>(string name, int size, bool autoExpand);

/// <summary>
/// 创建预制体对象池
/// </summary>
/// <param name="name">对象池名称（不可重复）</param>
/// <param name="go">对象池预制体</param>
/// <param name="size">对象池大小</param>
/// <param name="autoExpand">是否自动扩容</param>
ObjectPoolManager.Instance.CreatePrefabPool(string name, GameObject go, int size, bool autoExpand);
```
Pool对象池和PrefabPool对象池的区别在于PrefabPool中的对象已经限定为GameObject，而Pool则是一个通用对象池，同时**PrefabPool中分配的对象将都带有PrefabPoolObject脚本**。

参数'autoExpand'表示当池被填充时，系统将自动创建更多对象（原始池的大小）以供下一次分配。这意味着**对象池每次填满时都会自动扩展为当前对象池大小的两倍**。  

Pool< T>和PrefabPool都会在初始化时进行预热处理，即初始化阶段一次性生成初始大小的所有对象。

对象池通用分配和回收可以使用以下API实现：
```csharp
//分配
ObjectPool.Instance.AllocationFromPool(string name);
//回收
ObjectPool.Instance.RecycleFromPool<T>(string name, T obj);
```

---
## 0x2. Pool< T>和PrefabPool如何使用？
Pool< T>是MLObjectPool中通用的对象池，但是其分配和回收操作需要手动定义。对Pool< T>，创建、分配和回收调用参考以下API：
```csharp
//create
var goPool = ObjectPoolManager.Instance.CreatePool<GameObject>("gameObject", 100, true);

//allocation
var obj = goPool.allocation();

//recycle
goPool.Recycle(obj);
```

PrefabPool是针对Prefab而言的一种特殊的对象池，其定义了默认的分配与回收操作。对PrefabPool，创建、分配和回收调用参考以下API：
```csharp
//create
var prefabPool = ObjectPoolManager.Instance.CreatePrefabPool("prefab", prefab, 100, true);

//allocation
var prefab = prefabPool.Allocation();

//recycle
prefabPool.Recycle(prefab);
```

---
## 0x3. 其他功能
### 关于拓展
MLObjectPool提供了一个基类'**PoolBase**'供自定义类型的ObjectPool继承。

### 关于build-in interface
MLObjectPool中提供了6个接口供pool object自定义对象池各个阶段的操作，其6个接口分别为：
- IAllocationHandler : 在对象被分配时调用
- IRecycleHandler   : 在对象被回收时调用
- IBeforeAllocationHandler  : 在对象被分配前调用
- IBeforeRecycleHandler  : 在对象被回收前调用
- IAfterAllocationHandler   : 在对象被分配后调用
- IAfterRecycleHandler  : 在对象被回收后调用

其内部调用顺序如下：  
在分配阶段：OnBeforeAllocation -> OnAllocation/DefaultAllocation -> OnAfterAllocation  
在回收阶段：OnBeforeRecycle -> OnRecycle/DefaultRecycle -> OnAfterRecycle

注意：**pool object实现了IAllocationHandler或者IRecycleHandler接口，其默认的分配/回收操作将被替代**。

### 关于PrefabPool的默认分配/回收操作
其默认分配/回收代码如下：
```csharp
//分配
void OnGameObjectSpawn(GameObject obj)
{
    obj.transform.SetParent(null);
    obj.SetActive(true);
}

//回收
void OnGameObjectDespawn(GameObject obj)
{
    obj.transform.parent = poolRoot;
    obj.transform.position = Vector3.zero;
    obj.transform.rotation = Quaternion.identity;
    obj.transform.localScale = Vector3.one;
    obj.SetActive(false);
}
```
这些接口可以在任意mono脚本中被实现。但是这些接口的调用只能通过Pool< T>或PrefabPool。  
如果有自己定义的Pool，需要添加额外的代码以支持这些接口。

### 关于PoolEventTrigger
PoolEventTrigger是一个PoolEvent的触发器（也是Mono脚本），用户可以添加该触发器在任意GameObject上以实现PoolEvent，PoolEvent即为6个接口所代表的事件。  
PoolEventTrigger的使用方法同UGUI中的EventTrigger。

---
## 0x4. 附录
如果你需要MLObjectPool的动态库文件，你可以在下方找到下载地址：  
### MLObject v1.0.5
[MLObjectPool.dll](https://github.com/GhostYii/MLObjectPool/releases/download/v1.0.5/MLObjectPool.dll)  
[MLObjectPool.Editor.dll](https://github.com/GhostYii/MLObjectPool/releases/download/v1.0.5/MLObjectPool.Editor.dll)

### MLObject v1.0.4.1
[MLObjectPool.dll](https://github.com/GhostYii/MLObjectPool/releases/download/v1.0.4.1/MLObjectPool.dll)  
[MLObjectPool.Editor.dll](https://github.com/GhostYii/MLObjectPool/releases/download/v1.0.4.1/MLObjectPool.Editor.dll)


---

