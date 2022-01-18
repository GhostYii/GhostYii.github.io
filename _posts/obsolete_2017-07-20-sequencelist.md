---
layout: post
title: "基础数据结构之线性表"
image: ''
date:  2017-07-20 14:48:28
tags:
- invisible
description: ''
categories:
- 数据结构
---

---
## 线性表的定义与基本操作
### 定义
线性表是具有**相同**数据类型的n(n>=0)个数据元素的**有限**序列。  

### 基本操作
主要操作如下：
```cpp
+ SeqList Init(); - 初始化，构建一个空的线性表
+ int Length(); 求表长
+ SeqData FindByValue(value ); - 按值查找，给定关键字值查找对应元素
+ SeqData FindByPos(pos ); - 按位查找，给定位置查找该位置上的元素
+ void Insert(data , pos); - 插入操作，在pos位置插入元素
+ void Delete(pos); - 删除操作，删除pos位置上的元素
+ void Print(); - 按顺序输出该表内所有元素
+ bool IsEmpty(); - 判空操作
+ void Destroy(); - 销毁线性表并释放空间
```

---
## 线性表的顺序表示
### 定义
线性表的顺序表示又称为顺序表，其特点是**表中元素的逻辑顺序与物理顺序相同**，其典型代表为数组。  
假定顺序表中的元素类型为SeqData，则线性表的存储类型描述为：  
```cpp
#define MAXSIZE 1024
typedef struct SeqList
{
  SeqData* data;
  int maxSize;
  int length;
};
```
### 动态分配空间与静态分配空间
线性表的空间可以是动态分配的，也可以是静态分配的。在程序中，一般采用动态分配的方式来申请空间，防止线性表内存溢出导致程序崩溃。  
动态分配的语句为：
```cpp
SeqList seqLst;
//cpp
seqLst.data = new SeqData[MAXSIZE];
//c
seqLst.data = (SeqData*)malloc(sizeof(SeqData) * MAXSIZE);
```
静态分配语句即为数组分配语句：
```cpp
#define MAXSIZE 1024
typedef struct SeqList
{
  SeqData data[MAXSIZE];
  int length;
};

SeqList seqLst;
```
注意的是，**动态分配内存的方式在物理上仍然是连续的。**  
顺序表最主要的特点是随机访问，只要知道了首地址和元素序号即可在O(1)的时间内找到该元素。  

### 顺序表基本操作的实现
由于其他操作非常简单，这里只实现插入(Insert)，删除(Delete)和按值查找(FindByValue)。
```cpp
typedef struct SeqData
{
	int value;
};

class SeqList
{
	private:
		int length;
	public:
		SeqData* data;
		int maxLen;

		SeqList(int maxLen)
		{
			data = new SeqData[maxLen];
			this->maxLen = maxLen;
			length = 0;
		}
		void Insert(int pos, SeqData e)
		{
			if (pos<1 || pos>maxLen)
			   return ;
			if (GetLength() >= maxLen)
			   return ;

			for (int i=GetLength(); i>=pos; i--)
			{
				data[i]=data[i-1];
			}
			data[pos-1]=e;
			length++;
		}
		void Delete(int pos)
		{
			if (pos<1 || pos>max)
			   return ;

			for (int i=pos; i<GetLength(); i++)
				data[i-1]=data[i];

			length--;
		}
		SeqData FindByValue(int v)
		{
			int pos = 0;
			for (; pos<GetLength(); pos++)
				if (data[pos].value == v)
				   return data[pos];

	        return NULL;
		}
		int GetLength()
		{
			return length;
		}
}

```

---
## 线性表的链性表示
### 定义
线性表的链式存储称为**单链表**，它是指通过一组任意的存储单元来存储线性表中的线性关系。  
对于每一个链表结点，除了存放自身的信息之外，还需要存放一个指向其后继的指针。  
单链表的结点类型描述如下：  
```cpp
typedef struct Node
{
	SeqData data;		//数据域
	struct Node *next;	//指针域
}Node, *pNode;
```
由于单链表是离散分布的，所以单链表是**非随机存取**的数据结构。在查找特定结点时，需要从表头开始遍历，依次查找。  
通常用**头指针**来标识一个单链表，如单链表LL，头指针为“NULL”时表示一个空链表，为了操作上的方便，在单链表的第一个结点之前附加一个结点，称为**头结点**。头结点的数据域里可以不存储任何信息，也可以记录表长等信息。头结点的指针域指向线性表的第一个元素结点。如下图所示：  
![带头结点的单链表](..\assets\img\EEImgs\llwithhead.png)

### 头结点和头指针的区分
不管带不带头结点，头指针始终指向链表的第一个结点，而头结点是带头结点链表中的第一个结点，结点内通常不存储信息。  

引入头结点有两个优点：  
1. 由于开始结点的位置被存放在头结点的指针域中，所以在链表的第一个位置上的操作和在表的其他位置上的操作一致，无需进行特殊处理。
2. 无论链表是否为空，其头指针是指向头结点的非空指针（空表中头结点的指针域为空）。因此空表和非空表的处理也统一了。

## 单链表上基本操作的实现
此处的链表数据为int型数据，其单链表的存储类型描述如下：  
```cpp
typedef struct Node
{
	int value;
	struct Node *next;
}Node, *pNode, *LinkList;

```

### 头插法建立单链表
```cpp
LinkList CreateByHead(int *values)
{
	LinkList res;
	res->next = NULL;

	int len = sizeof(values) / sizeof(int);
	for　(int i=0; i<len; i++)
	{
		pNode node = new pNode;
		node->value = values[i];
		node->next = res->next;
		res->next = node;
	}

	return res;
}
```

### 尾插法建立单链表
```cpp
LinkList CreateByTail(int *values)
{
	LinkList res;
	pNode tail;

	res->next = NULL;
	tail = res;

	int len = sizeof(values) / sizeof(int);
	for　(int i=0; i<len; i++)
	{
		pNode node = new pNode;
		node->value = values[i];
		
		tail->next = node;
		tail = node;
	}

	tail->next = NULL;

	return res;
}
```

### 按序号查找
```cpp
pNode FindByIndex(LinkList ll, int index)
{
	int currentIndex = 1;

	pNode itor = ll->next;
	while (itor->next)
	{
		if (currentIndex == index)
			return itor;
		
		currentIndex++;
		itor = itor->next;
	}

	puts("not find anything");
	return NULL;
}
```
### 按值查找
```cpp
pNode FindByValue(LinkList ll, int value)
{
	int currentIndex = 1;

	pNode itor = ll->next;
	while (itor->next)
	{
		if (itor->value == value)
			return itor;
		
		currentIndex++;
		itor = itor->next;
	}

	puts("not find anything");
	return NULL;
}
```

### 插入、删除、求表长
```cpp
void Insert(LinkList &ll, int pos, pNode value)
{
	pNode tmp = ll->next;
	for (int i=1; tmp->next; i++)
	{
		if (i-1 != pos)
		{
			tmp=tmp->next;
			continue;
		}

		value->next = tmp->next;
		tmp->next = value;
	}
}

void Delete(LinkList &ll, int pos)
{
	pNode tmp = ll->next;
	for (int i=0; tmp->next; i++)
	{
		if (pos-1 == i)
		{
			pNode v = tmp->next;
			tmp->next = v->next;
			delete v;
		}
		
		tmp = tmp->next;
	}
}

int GetLength(LinkList &ll)
{
	int len = 0;
	pNode tmp = ll->next;

	while (tmp->next)
	{
		len++;
		tmp = tmp->next;
	}

	return len;
}
```

--- 
## 双链表
单链表结点中只有一个指向其后继的指针，这使得单链表只能通过从前往后遍历的方式查找元素，如果要查找某个结点的前驱结点，只能从头开始遍历。访问前驱结点的时间复杂度为O(n)。  
为了克服单链表的上述缺点。引入了双链表，双链表与单链表类似，只是在结点中新增了一个指向其前驱结点的指针。如下图所示：   
![doublelinklist](../assets/img/EEImgs/doublelinklist.PNG)  

双链表中结点类型描述如下：  
```cpp
typedef struct DNode
{
	SeqData data;
	struct DNode *prev, *next;
}DNode, *pDNode, *DLinklist;
```

### 双链表的插入、删除 
```cpp
typedef struct DNode
{
	int value;
	struct DNode *prev, *next;
}DNode, *pDNode, *DLinklist;

DLinkList Insert(DLinkList dll, int pos, int value)
{
	pDNode tmp = dll->next;
	int i = 1;

	while (tmp)
	{
		if (pos-1 == i)
		{
			pDNode e;
			e->value = value;
			
			e->next = tmp->next;	//1
			tmp->next->prev = e;	//2
			e->prev = tmp;			//3
			tmp->next = e;			//4

			return dll;
		}

		i++;
		tmp = tmp->next;
	}
	
	//插入失败，返回原始链表
	return dll;
}

DLinkList Delete(DLinkList dll, int pos)
{
	pDNode tmp = dll->next;
	int i = 1;

	while (tmp)
	{
		if (pos-1 == i)
		{
			pDNode aim = tmp->next;
			aim->next->prev = tmp;
			tmp->next = aim->next;

			delete aim;
			return dll;
		}

		i++;
		tmp = tmp->next;
	}
	
	//删除失败，返回原始链表
	return dll;
}

```
上面的插入操作中指针移动如下图所示：  
![doublelinklistinsert](../assets/img/EEImgs/doublelinklist2.PNG)

值得注意的一点是，这个顺序不是固定的，但是一定保证①②两步在④之前，否则tmp->next就再也找不到了，导致插入失败。

--- 
## 循环链表
### 循环单链表
循环单链表和单链表的区别在于表中的最后一个结点不是NULL，而改为指向头结点，从而使整个链表形成一个环。  
循环单链表中一般不再存在单独的头指针，取而代之的是尾指针。所以链表的“头”是```尾指针->next```。这样使得对表尾的操作的时间复杂度O(1)而不是O(n)。  
循环单链表的插入删除操作与单链表几乎一样，只是在处理表尾的情况时需要特殊处理以保证仍然是循环的。

### 循环双链表
双向循环链表和单循环链表很相似，唯一不一样的地方在循环双链表中头结点的prev指针指向表尾结点。  
空表的头结点指针域为NULL。  

--- 
## 静态链表
静态链表是借助数组来描述线性表的**链式存储结构**，与前面的链表不同的一点是，这里的指针不是真正意义上的指针(*p)，而是结点的相对地址，也就是数组的下标。  
静态链表需要预先分配一块连续的内存空间。  
静态链表的结构类型描述如下：
```cpp
#define MAXSIZE 50

typedef struct
{
	SeqData data;
	int next;
}SLinkList[MAXSIZE];
```
静态链表结束以```next==-1```作为结束的标志。

---
## 顺序表与链表的比较
1. 存储结构不同，顺序表是随机存取，链表只能从表头顺序存取。
2. 顺序表的逻辑相邻，物理结构也相邻。而链表则不一定，对应关系是通过指针实现的。
3. 查找、删除和插入操作处理不同。按值查找，顺序表在无序情况下，两者的时间复杂度均为O(n)，但是当顺序表有序时，可采用折半查找，此时时间复杂度为O(log(2)n)。按序号查找，顺序表支持随机访问，时间复杂度为O(1)，而链表的平均时间复杂度为O(n)。
4. 由于链表的每个结点都带有指针域，因此在存储空间上比顺序存储要付出更大的代价，存储密度不够大。
5. 空间分配上，顺序表一旦装满就不能扩充，预先分配较大空间则会造成后部分的浪费。动态分配虽然可以解决这一问题，但是由于需要移动大量元素，导致操作效率过低。链表则可以在需要的时候申请分配，只要内存还有剩余空间即可分配，操作灵活高效。

## 归纳总结
对于链表，常用的方法有**头插法、尾插法、逆置法、归并法、双指针法**。  
对于顺序表，由于可以直接存取，经常结合排序和查找的几种算法设计思路进行设计，如归并排序、二分查找等。

---
<center>  
 <a href="../entranceExamSummary">返回目录</a>
</center>
