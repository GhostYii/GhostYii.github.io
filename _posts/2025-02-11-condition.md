---
layout: post
title: "C#通用条件检测机制"
image: ''
date:  2025-02-11 17:00:00
tags:
description: ''
categories:
- 游戏开发
---

---

## 0x0. 前言

游戏项目中经常需要用到条件检测，这些检测基本就是对某个对象或者数据进行一次逻辑判定，某些时候判定通过后还需要执行一些特定逻辑。这个检测机制可以抽象出来搞成一个通用的东西，之前在一些其他的弱类型语言中实现过一个类似的模块，现在使用C#语言造一遍轮子，也当作一个通用工具方便自己各个项目使用。

## 0x1. 动态参数

条件检测的核心简化一下就是一个`if-else`罢了，只是这个`if`的参数是动态的，这点在C#语言中可以使用可变参数`params object[]`来实现，但是这样实现有一个隐患，就是拆装箱导致的GC消耗，我向来是不太喜欢增加这些额外的GC消耗的，因此这个参数最终采用struct+泛型来实现，可以避免拆装箱导致的额外GC，代码如下：

```csharp
// ConditionArgs.cs

namespace Condition
{
    public interface IConditionArgs { }

    public struct ConditionArgs<T> : IConditionArgs
    {
        public readonly T arg;

        public ConditionArgs(T val) => arg = val;

        public static implicit operator T(ConditionArgs<T> args) => args.arg;
    }

    public struct ConditionArgs<T1, T2> : IConditionArgs
    {
        public readonly T1 arg1;
        public readonly T2 arg2;

        public ConditionArgs(T1 arg1, T2 arg2)
        {
            this.arg1 = arg1;
            this.arg2 = arg2;
        }

        public void Deconstruct(out T1 arg1, out T2 arg2)
        {
            arg1 = this.arg1;
            arg2 = this.arg2;
        }
    }

    // u can make more parameter types like ConditionArgs<> here
}
```

这段代码实现了若干个参数的包装，无参数可以使用`null`代替。
为了方便后续的使用，单个参数的实现了向参数类型的类型转换函数，超过一个参数的的泛型结构体都实现了`Deconstruct`函数当作鸭子类型以支持解构。如果需要参数类型可以按此继续拓展。

## 0x2. 条件接口

要想实现一个通用的`if-else`来做条件检测，有一种简单的办法是整一个检测基类`ConditionTargetBase`里头搞一个纯虚函数`bool Check(IConditionArgs args)`来干这个事。但是这样也存在一些问题，譬如所有的检测对象都需要进行手动额外包装变相增大了工作量，对值类型不是很友好等。
为了实现更大的通用性和易用性，另一种办法是接口，接口定义如下：

```csharp
// Interfaces.cs

namespace Condition
{
    public interface IConditionTarget
    {
        bool ConditionCheck(string name, IConditionArgs args);
    }

    public interface IPostConditionReached
    {
        void PostConditionReached(string name, IConditionArgs args);
    }
}
```

当然接口中的条件类型也可以是其他类型，如果项目中更习惯采用枚举，这里可以改成`int`类型。

## 0x3. 通用检测

在上一步中实现了条件接口，但是如何去调用这个条件接口呢？最先想到的办法或许是定义一个管理类`ConditionManager`，然后在这个类中去调用接口中的方法。这是一种可行的办法，但是本机制采用了C#中的另一种方式——拓展方法来干这事儿。参考以下代码：

```csharp
public static class Extensions
{
    public static bool CheckCondition(this IConditionTarget self, string name, IConditionArgs args) => self.ConditionCheck(name, args);
}
```

这样便可以更方便的调用该接口而不需要通过管理类来调用了。然而，正如本文标题所述，既然是通用机制，能否更加通用一些？能不能把这个拓展方法的拓展范围整到`object`？
其实是可以的，但是需要做一点其他的额外工作，为了支持更多类型，还是得存在一个内部的包装器，这个对象会实现所有条件接口，如下：

```csharp
// ConditionWrapper.cs

namespace Condition
{
    public delegate bool ConditionCheckHandler(string name, IConditionArgs args);
    public delegate void PostConditionReachedHandler(string name, IConditionArgs args);

    internal sealed class ConditionWrapper : IConditionTarget, IPostConditionReached
    {
        private ConditionCheckHandler _check = null;
        private PostConditionReachedHandler _post = null;

        public ConditionCheckHandler Check { get => _check; set => _check = value; }
        public PostConditionReachedHandler Post { get => _post; set => _post = value; }

        public ConditionWrapper(ConditionCheckHandler check, PostConditionReachedHandler post = null)
        {
            _check = check;
            _post = post;
        }

        public bool ConditionCheck(string name, IConditionArgs args) => _check?.Invoke(name, args) ?? false;
        public void PostConditionReached(string name, IConditionArgs args) => _post?.Invoke(name, args);
    }
}
```

有了这么个玩意儿之后，管理类需要支持动态增删对象。然后再把拓展方法的拓展范围改一改，最终如下：

```csharp
// Condition.cs

using System;
using System.Collections.Generic;

namespace Condition
{
    public static class Condition
    {
        private static Dictionary<Type, ConditionWrapper> _registerTypes = new();

        public static bool CheckCondition<T>(this T self, string name, IConditionArgs args = null)
        {
            if (self is IConditionTarget ct)
                return ct.ConditionCheck(name, args);

            if (_registerTypes.TryGetValue(self.GetType(), out var wrapper))
                return wrapper.CheckCondition(name, args);

            return false;
        }

        public static bool CheckConditions<T>(this T self, params ValueTuple<string, IConditionArgs>[] targets)
        {
            foreach (var (name, arg) in targets)
            {
                if (!self.CheckCondition(name, arg))
                    return false;
            }
            return true;
        }

        public static bool DoCondition<T>(this T self, string name, IConditionArgs args = null)
        {
            if (!self.CheckCondition(name, args))
                return false;

            if (self is IPostConditionReached post)
                post?.PostConditionReached(name, args);
            else if (_registerTypes.TryGetValue(self.GetType(), out var wrapper))
                wrapper.PostConditionReached(name, args);

            return true;
        }

        public static void SetAsConditionTarget<T>(this T self, ConditionCheck check, PostConditionReached post = null)
        {
            // cant set IConditionTarget as ConditionTarget
            if (self is IConditionTarget)
                return;

            // new or update
            var type = self.GetType();
            if (_registerTypes.TryGetValue(type, out var wrapper))
            {
                wrapper.Check = check;
                wrapper.Post = post;
            }
            else
            {
                _registerTypes[type] = new(check, post);
            }
        }

        public static void UnsetConditionTarget<T>(this T self) => _registerTypes.Remove(self.GetType());
        public static void Clear() => _registerTypes.Clear();
    }
}
```

这里头的`_registerTypes`使用对象的类型当Key而非对象本身，框架本身不保存任何对象的引用，这样使得值类型与引用类型都可以使用同一套逻辑，避免值类型传入时的复制导致条件检测计算错误。更通俗的说，就把这个问题丢到业务层让业务自行处理了（这也是下方样例中为何推荐使用匿名函数的原因）。

## 0x4. 样例

至此，一个简易的通用条件检测机制就完成了，已有对象可以通过`SetAsConditionTarget`拓展方法注册（推荐使用匿名函数），新对象可以通过继承`IConditionTarget`这些接口来实现条件检测，如在Unity引擎环境下有以下示例：

```csharp
// ConditionTest.cs

using System.Collections.Generic;
using UnityEngine;
using Condition;

public class ConditionTest : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData : IConditionTarget, IPostConditionReached
    {
        public int level = 1;
        public int exp = 0;

        public Dictionary<string, int> items = new();

        public bool ConditionCheck(string name, IConditionArgs args)
        {
            switch (name)
            {
                case "HeroLevel":
                    int target = (ConditionArgs<int>)args;
                    return level >= target;
                case "HeroExp":
                    int exp = (ConditionArgs<int>)args;
                    return this.exp >= exp;
                case "EnoughCoin":
                    var (item, count) = (ConditionArgs<string, int>)args;
                    return items.ContainsKey(item) && items[item] >= count;
            }
            return false;
        }

        public void PostConditionReached(string name, IConditionArgs args)
        {
            switch (name)
            {
                case "EnoughCoin":
                    var (item, count) = (ConditionArgs<string, int>)args;
                    items[item] -= count;
                    break;
            }
        }
    }

    [System.Serializable]
    public class MonsterData
    {
        public int level = 2;
        public string name = "哈大奇";
    }

    public PlayerData data = new();
    public int value = 100;
    public MonsterData monster = new();

    void Start()
    {
        data.items["coin"] = 100;
        value.SetAsConditionTarget((name, args) =>
        {
            if (name == "IntTest")
            {
                int target = (ConditionArgs<int>)args;
                return value > target;
            }
            return false;
        });

        monster.SetAsConditionTarget((name, args) =>
        {
            // do something with monster
            switch (name)
            {
                case "HeroLevel":
                    int level = (ConditionArgs<int>)args;
                    return monster.level > level;
            }

            return false;
        });
    }

    private void OnDestroy() => Condition.Condition.Clear();    
}
```