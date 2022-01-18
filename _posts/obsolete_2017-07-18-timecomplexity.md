---
layout: post
title: "时间复杂度的计算"
image: ''
date:  2017-07-18 15:03:29
tags:
- invisible
description: ''
categories:
- 数据结构
---

---
## 引言
时间复杂度是算法效率的度量标准之一。  
一个语句的频度是指该语句在算法中被重复执行的次数。算法中所有语句的频度之和记为T(n)，它是该算法问题规模n的函数，时间复杂度主要分析T(n)的数量级。算法中的基本运算（最深层循环内的语句）的频度与T(n)同数量级，所以通常采用算法中基本运算的频度f(n)来分析算法的时间复杂度。因此，算法的时间复杂度记为：
> <center>T(n) = O(f(n))</center>  

O的含义T(n)的数量级。其严格的数学定义为：  
若T(n)和f(n)是定义在正整数集合上的两个函数，则存在正常数C和n0，使得当n≥n0时，都满足0≤T(n)≤C*f(n)。  
算法的时间复杂度不仅依赖于问题的规模n，也取决与待输入数据的性质（如输入数据元素的初始状态）。  

一般考虑最坏情况下的时间复杂度。  

时间复杂度的计算规则：  
1. T(n)=T1(n)+T2(n)=O(f(n))+O(g(n))=O(max(f(n),g(n)))
2. T(n)=T1(n) * T2(n)=O(f(n)) * O(g(n))=O(f(n) * g(n))  

一般的，有：
> O(1)<O(log(2)n)<O(n)<O(nlog(2)n)<O(n^2)<O(n^3)<O(2^n)<O(n!)<O(n^n)  

本文通过几个例子说明时间复杂度的基本计算方法。希望自己能够熟练运用这些方法，并运用于考研中帮助自己取得好成绩。     

---
### 例①
已知如下算法，求该算法时间复杂度：
```cpp
void fun(int n)
{
	int i = 1;
	while(i <= n)
	 i = i*2;
}
```

计算方法如下：
这里的最深层循环语句为i=i*2; 求出该语句最多执行的次数即为本题答案，设执行次数为t，可以列出等式2^t=n，得
t=log(2)n

### 例②　　
```cpp
void fun(int n)
{  
   x=2;
   while(x<n/2)
     x=2*x;
}
```

计算方法如下：：  
这里的最深层循环语句为x=2*x; 设执行次数为t，则有2^(t+1)=n/2,得t=log(2)n+C，其中C为常数可省略，得t=log(2)n

### 例③
```cpp
int fact(int n)
{
	if (n<=1)
	  return 1;
	else
	  return n*fact(n-1);   
}
```

这是使用递归求阶乘的算法，这里只要想到递归的计算公式：n!=n*(n-1)* ...* 1即可发现，这里的n* fact(n-1)只执行了n次，所以答案为O(n)。  
如果要进行推导，过程如下：T(n)=1+T(n-1)=1+1+T(n-2)=...=n-1+T(1) -> T(n)=O(n)

### 例④
两个长度为m、n的升序链表合并为一个长度为m+n的降序链表的算法时间复杂度为？

思路：不停的比较两个链表的第一个元素，将较小的那一个摘下来通过头插法放入新链表中，直到某一个链表被摘完。之后将剩下的链表元素全部插入到新链表中即可。最坏情况是所有元素都经过了两两比较，算法复杂度为O(max(m,n))

该算法实现如下：
```cpp
LL Combine(LL a, LL b)
{
  if (a.isEmpty)
    return b;
  if (b.isEmpty)
    return a;

  LL res = new LL;

  while(true)
  {
    if (a.isEmpty || b.isEmpty)
      break;

    res.AddFirst(a.first>b.first ? b.first : a.first);
    if (a.first > b.first)
      b.Delete(b.first);
    else
      a.Delete(a.first);
  }

  if (a.isEmpty)
  {
    while(!b.isEmpty)
    {
      res.AddFirst(b.first);
      b.Delete(b.first);
    }
  }
  else
  {
    while(!a.isEmpty)
    {
      res.AddFirst(a.first);
      a.Delete(a.first);
    }
  }

  return res;
}
```

### 例⑤
```cpp
void fun(int n)
{
  int count = 0;
  for (int k = 1;k <= n;k*=2)
    for (int j = 1;j <= n; j++)
      count++;
}
```

计算方法如下：  
内层循环的时间复杂度为O(n)，外层循环的时间复杂度为O(log(2)n)。所以总的时间复杂度为O(n*log(2)n)。

### 例⑥
```cpp
void fun(int n)
{
  int i = 0;
  while(i*i*i <=n)
    i++;
}
```

计算方法如下：
设i++执行了t次，则有t^3≤n -> t=∛n 所以时间复杂度为O(n)=∛n

### 例⑦
```cpp
for (int i=n-1;i>1;i--)
	for (int j=1;j<i;j++)
		if (A[j]>A[j+1])
			A[j]与A[j+1]对换
```
其中n为正整数，则最后一行语句频度在最坏情况下是？

计算方法如下：  
当所有相邻元素都为逆序时候，最后一行的语句每次都会执行，则T(n)=∑(i=2)^(n-1)∑(j=1)^(i-1) 1=∑(i=2)^(n-1)-i=(n-2)(n+1)∕2 = O(n^2)  

### 例⑧
```cpp
int m=0,i,j;
for(i=1;i<=n;i++)
 	for(j=1;j<=2*i;j++)
		m++;
```
求m++执行的次数。  

计算方法如下：
∑(i=1)^n∑(j=1)^(2*i) 1=∑(i=1)^n 2i=2∑(i=1)^n i = n(n+1)

### 例⑨
```cpp
int y=5;
while((y+1)*(y+1)<n)
	y++;
```

本题中记t为y++的执行次数，则有y=t-5，即t=y+5，所以有(y+5+1)* (y+5+1)<n -> y^2 < n，即O(n)=√n

---
## 总结
推导时间复杂度的两种形式：  
1. 循环主体中的变量参与循环条件的判断  
&nbsp;&nbsp;&nbsp;&nbsp; 找出主体语句中与T(n)成正比的循环变量，带入条件中进行计算。  
&nbsp;&nbsp;&nbsp;&nbsp; 如例1例2和例9。
2. 循环主体中的变量与循环条件无关  
&nbsp;&nbsp;&nbsp;&nbsp; 采用数学归纳法或直接累计循环次数。  
&nbsp;&nbsp;&nbsp;&nbsp; 多层循环由内往外分析，忽略单步语句、条件判断语句，主关注主体语句的执行次数。  
&nbsp;&nbsp;&nbsp;&nbsp; 一般分为递归和非递归程序。   
&nbsp;&nbsp;&nbsp;&nbsp; 对于 **递归程序**，一般用公式进行推导，如例3。  
&nbsp;&nbsp;&nbsp;&nbsp; 对于 **非递归程序**，可以直接累计次数，如例6和例7。   

---
<center>  
 <a href="../entranceExamSummary">返回目录</a>
</center>
