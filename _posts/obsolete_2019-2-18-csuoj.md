---
layout: post
title: "OJ解题报告"
image: ''
date:  2019-02-18 00:00:00
tags:
- 算法
description: ''
categories:
- 学习总结
---

--- 
## 前言
本文是本人对即将参加的中南大学2019计算机考研复试上机考试的练习题解题报告汇总，绝大部分题目来自于<a href="http://dengdengoj.cc/problemset.php" target="_blank">DengDengOJ</a>，小部分来自<a href="http://acm.csu.edu.cn:20080/csuoj/problemset" target="_blank">CSU-ACM Online Judge</a>，提交语言均为C++。

--- 
## 目录
* [CSU-1000 - 1002: a+b问题](#1)
* [CSU-1003: UC Browser](#2)
* [CSU-1005: Binary Search Tree analog](#3)
* [PIPIOJ-1026: a+b问题（中南大学2018机试原题）](#4)
* [DengDengOJ-1009: 切木棍（中南大学2017机试原题）](#5)
* [DengDengOJ-1056: A+B（登登OJ第一周测试题A）](#6)
* [DengDengOJ-1074: 高斯日志（登登OJ第一周测试题B）](#7)
* [DengDengOJ-1073: 回文数（登登OJ第一周测试题C）](#8)
* [DengDengOJ-1034: 排名（登登OJ第一周测试题D）](#9)
* [DengDengOJ-1072: Insertion or Heap Sort (25)（登登OJ第一周测试题E）](#10)

--- 
## 正文
<h4 id="1">1000 - 1002: a+b问题</h4>

题目链接：<a href="http://acm.csu.edu.cn:20080/csuoj/problemset/problem?pid=1000" target="_blank">1000: A+B (I)</a>，<a href="http://acm.csu.edu.cn:20080/csuoj/problemset/problem?pid=1001" target="_blank">1001: A+B (II)</a>，<a href="http://acm.csu.edu.cn:20080/csuoj/problemset/problem?pid=1002" target="_blank">1002: A+B (III)</a>

 这三道题都是a+b的问题，均为非常基础的OJ题，考试中理应不会出现这样初级的题目，解这几道题目的目的是用于测试本OJ网站的一些特点（如对数据格式精度要求等）。  
 1000与1001基本可以说是同一道题，均为简单实数求和，看清题目要求与格式要求应该可以一遍AC。但是在提交1001时发现题目规定的数字范围仅在[0,1000000]之间，理论上来说使用float就可以满足要求。但是实际测试发现float会导致WA，而使用双精度double时成功AC。  
 AC代码如下：
 ```cpp
1000:
#include <iostream>
#include <cstdio>
using namespace std;

int main(int argc, char** argv)
{
	int a=0, b=0;
	while (scanf("%d%d",&a, &b) != EOF)
	{ cout<<a+b<<endl; }
	
	return 0;
}

1001:
#include <iostream>
#include <cstdio>
using namespace std;

int main(int argc, char** argv)
{
	double a=0.0, b=0.0;
	while (scanf("%lf%lf", &a, &b) != EOF)
	{ printf("%.4lf\n", a+b); }
	
	return 0;
}
 ```

 需要提一点的是1002题，我第一眼看到这个题目就想到了大一时刷到的<a href="http://acm.hdu.edu.cn/showproblem.php?pid=1002" target="_blank">杭电1002题</a>。以为是一道非字符串处理不可的题，但是我在使用字符串模拟算术之前先用long long精度测试了一下发现直接AC了。故这题数据可能出的不太好或者是本意就是long long可以过的题。对字符串处理加法有兴趣的朋友可以移步杭电1002题讨论区。本文仅给出CSU可以AC的代码。 
 ```cpp
 1002:
#include <iostream>
#include <cstdio>
using namespace std;

int main(int argc, char** argv)
{
	long long a=0, b=0;
	while (cin>>a>>b)
	{
		if (!a && !b) return 0;
	 	cout<<a+b<<endl; 
    }
    
	return 0;
}
```
<h4 id="2">1003: UC Browser</h4>
题目链接：<a href="http://acm.csu.edu.cn:20080/csuoj/problemset/problem?pid=1003" target="_blank">1003: UC Browser</a>  

题目大意为给定一个只包含01的字符串，按照1出现的次数来计算结果值，连续的1出现时会影响最终结果，其规律为1个1为10分，2个1为10+20=30分，一直到5个1为10+20+30+40+50=150分，循环节为5，也就是当6个1连续时前5个可以算连续分，第6个数字开始重新计算分数，以此循环。  
代码写起来也没有什么大问题。首先注意一下格式，算法主体是对01字符串进行分析，为此可以直接设置一个标记位记录连续出现的1的个数。遇0则更新分数。  
然后将单个循环节打成数组分数表方便计算，这样很快就能算出总分，然后换算为相应等级即可。  
有部分提交者可能会单独写一个分数转等级的函数，使用一堆if-else来计算，但是观察等级与分数表格发现规律还是很明显的可以通过一行代码计算出来，这样可以简化代码使得看起来更加清晰。  
代码如下:
```cpp
#include <iostream>
#include <cstdio>
using namespace std;

int main(int argc, char** argv)
{
	int count=0, days=0;
	int map[] = {0, 10, 30, 60, 100, 150};  //分数表
	string record="";
	
	cin>>count;
	while (count--)
	{		
		cin>>days>>record;
		int sum=0, ctn=0;
		for(int i=0; i<record.length(); i++)
		{
			if (record[i]=='0') //遇0则刷新一遍分数
			{
				int t = ctn/5;  //t为循环总次数，单次总分数为150
				sum += t*150+map[ctn%5];
				ctn = 0;    //记得重置标记
			}
			else if (record[i]=='1')
				ctn++;  //更新标记
	    }
	    sum+=map[ctn];  //不加此行代码如果最后一位是1则会漏掉一部分分数
	    cout<<(sum-50)/100+1<<endl; //将分数转为等级输出
	}
	
	return 0;
}

```

<h4 id="3">1005: Binary Search Tree analog</h4>
题目链接：<a href="http://acm.csu.edu.cn:20080/csuoj/problemset/problem?pid=1005" target="_blank">1005: Binary Search Tree analog</a> 

本题题干很长，但是基本都是废话，题目的意思就是通过序列创建二叉搜索树并输入先中后序列，属于基础知识题，主要算法是二叉搜索树的插入与二叉树的遍历算法。  
值得注意的一点是本题对格式要求非常低，杭电常见的最后一个空白字符PE在CSUOJ竟然可以直接AC。下面的AC代码在每个遍历序列后都会多输出一个空格，如果对格式要求严格建议使用输出流进行格式控制。  
```cpp
#include <iostream>
#include <cstdio>
using namespace std;

//二叉树数据结构表示
typedef struct node
{
	int data;
	struct node *lchild, *rchild;
}btreenode, *pbtreenode, *btree;

//二叉搜索树的插入函数
btree insert(btree t, pbtreenode node)
{
	pbtreenode tmp = t, prev=NULL;
	while (tmp) //先找到插入位置并记录其父节点
	{
		prev = tmp;
		tmp = node->data < tmp->data ? tmp->lchild : tmp->rchild;
	}
	
	if (!t) t = node;   //依情况进行插入
	else if (prev->data > node->data)
		 prev->lchild = node; 
    else prev->rchild = node;
	
	return t;
}

//先序遍历（递归调用）
void preorder(btree t)
{
	if (t)
	{
		cout<<t->data<<" ";
		preorder(t->lchild);
		preorder(t->rchild);		
	}
}

//中序遍历（递归调用）
void inorder(btree t)
{
	if (t)
	{		
		inorder(t->lchild);
		cout<<t->data<<" ";
		inorder(t->rchild);		
	}
}

//后序遍历（递归调用）
void postorder(btree t)
{
	if (t)
	{		
		postorder(t->lchild);		
		postorder(t->rchild);
		cout<<t->data<<" ";
	}
}

int main(int argc, char** argv)
{
	int count=0;
	btree t=NULL;
	
	cin>>count;
	while (count--)
	{
		int nodecount=0;
		cin>>nodecount;
		while (nodecount--)
		{
			pbtreenode node = (pbtreenode)new btreenode;
			cin>>node->data;
			node->lchild = NULL; node->rchild = NULL;
			t = insert(t, node);
		}
		
		preorder(t); cout<<endl;
		inorder(t); cout<<endl;
		postorder(t); cout<<endl;
	}

	return 0;
}

```
<h4 id="4">PIPIOJ-1026: a+b问题（中南大学2018机试原题）</h4>

题目链接：<a href="http://39.106.164.46/problem.php?id=1026" target="_blank">1026: a+b问题</a>

这题只是一道简单的A+B问题，由于我很久没有使用过C++语言了，在面对C++的字符串处理上花了一点时间才搞明白字符串分割，结果这道题我写的字符串分割函数提交上去竟然直接返回运行错误的结果。但是下面的代码在本地运行是没有问题的，现在不知道中南的判题机与这个OJ上的是否是同一个。

```cpp
#include <iostream>
#include <vector>
#include <map>
#include <cstdio>
#include <cstring>
#include <cmath>
using namespace std;

#define EXITCODE "zero + zero ="

//字符串分割函数
vector<string> split(const string &str, const string &pattern)
{
    char * strc = new char[strlen(str.c_str())+1];
    strcpy(strc, str.c_str());
    vector<string> res;
    char* temp = strtok(strc, pattern.c_str());
    while(temp != NULL)
    {
        res.push_back(string(temp));
        temp = strtok(NULL, pattern.c_str());
    }
    delete[] strc;
    return res;
}

//英文与数字的对应使用map打成一个表会很方便
map<string, int> getmap()
{
	map<string, int> res;
	res["zero"]=0; res["one"]=1; res["two"]=2;
	res["three"]=3;	res["four"]=4; res["five"]=5;
	res["six"]=6; res["seven"]=7; res["eight"]=8;
	res["nine"]=9;
	return res;
}

int main(int argc, char** argv)
{
	map<string, int> dic = getmap();
	while (true)
	{
		string cmd;
		getline(cin, cmd);
		if (cmd == EXITCODE)
			return 0;

		vector<string> params = split(cmd, "=");
		vector<string> numbers = split(params[0], "+");

		vector<string> numa = split(numbers[0], " ");
		vector<string> numb = split(numbers[1], " ");
		int a=0, b=0, lena=numa.size(), lenb=numb.size();

		for (int i=0, k=lena-1; i<lena; i++)
			a += pow(10, k--)*dic[numa[i]];
		for (int i=0, k=lenb-1; i<lenb; i++)
			b += pow(10, k--)*dic[numb[i]];

		cout<<a+b<<endl;
	}
		
	return 0;
}

```
下面给定另外一种解决办法，删去了上面代码中的split函数，但是在PIPIOJ上提交的结果是OLE，真是奇了怪了，合计才一行输出语句。还能OLE。  
```cpp
#include <iostream>
#include <vector>
#include <map>
#include <cstdio>
#include <cstring>
#include <cmath>
using namespace std;

#define EXITCODE "zero + zero ="

map<string, int> getmap()
{
	map<string, int> res;
	res["zero"]=0; res["one"]=1; res["two"]=2;
	res["three"]=3;	res["four"]=4; res["five"]=5;
	res["six"]=6; res["seven"]=7; res["eight"]=8;
	res["nine"]=9;
	return res;
}

int main(int argc, char** argv)
{
    map<string, int> dic = getmap();
    while (true)
    {
        string cmd;
        getline(cin, cmd);

        if (cmd == EXITCODE)
        return 0;

        string tmp=""; bool is2=false;
        vector<string> numa, numb;
        for (int i=0; i<cmd.length(); i++)
        {
            if (cmd[i] == '+')
            { is2 = true; continue; }

            if (cmd[i] == ' ')
            {
                if (!is2) numa.push_back(tmp);
                else numb.push_back(tmp);
                tmp = "";
            }
            else  tmp.insert(tmp.length(), cmd.substr(i, 1));
        }

        int a=0, b=0, lena=numa.size(), lenb=numb.size();        
        for (int i=0, k=lena-1; i<lena; i++)
            a += pow(10, k--)*dic[numa[i]];
        for (int i=0, k=lenb-1; i<lenb; i++)
            b += pow(10, k--)*dic[numb[i]];

        cout<<a+b<<endl;
    }

    return 0;
}

```
<h4 id="5">DengDengOJ-1009: 切木棍（中南大学2017机试原题）</h4>

题目链接：<a href="http://acm.csu.edu.cn:20080/csuoj/problemset/problem?pid=1000" target="_blank">1009: 切木棍</a>  

这道题乍一看非常简单，但是我却提交了四次才过。而且在DengDengOJ上AC的代码在PIPIOJ上却出现TLE错误。  
下面给出我在登登OJ上AC的代码，我用了一个比较绕的代码解决了这个问题。其实可以在n为奇数时直接输出0的。  
```cpp
#include <iostream>
#include <cstdio>
using namespace std;

bool isint(double num)
{ return (int)(num/1.0) == num && num!=0; }

int main(int argc, char** argv)
{
	int n;
	while(cin>>n)
	{
		double y=0.0;
		int x=1, count=0;
		
		while (true)
		{	
			if (x > n/4) break;
						
			y = (n/1.0-2*x/1.0)/2.0;
			if (!isint(y) || y<=0)
			{ x++; continue; }
			else
			{ int yy = (int)y; if (x != yy) count++; }
			
			x++;
		}
		cout<<count<<endl;
	}
	return 0;

}

```
<h4 id="6">问题 A：A+B</h4>

题目链接：<a href="http://dengdengoj.cc/problem.php?cid=1004&pid=0" target="_blank">问题A：A+B</a>

一个字符串处理的A+B，我的AC代码略显繁琐，应该还有更加精简的代码。
```cpp
#include <iostream>
#include <string>
#include <cstdio>
#include <cmath>
using namespace std;
 
int main(int argc, char** argv)
{
    string a, b;
    long long res=0;	//防止越界直接用了long long
    while (cin>>a>>b)
    {
        int n1=0, n2=0;     
        bool isnt1 = a[0]=='-', isnt2 = b[0]=='-';	//判断正负
        int para=0, parb=0, lena=0, lenb=0;	//a,b中逗号数量和实际位数
        for (int i=0; i<a.length(); i++)
            if (a[i] == ',') para++;
        for (int i=0; i<b.length(); i++)
            if (b[i] == ',') parb++;
        lena= isnt1?a.length()-para-2:a.length()-para-1;
        lenb= isnt2?b.length()-parb-2:b.length()-parb-1;
 
        for (int i=0; i<a.length(); i++)
        {
            if (a[i]==',' || a[i]=='-') continue;
            n1 += (a[i]-'0')*pow(10,lena--);	//计算出实际的a的绝对值
        }       
        for (int i=0; i<b.length(); i++)
        {
            if (b[i]==',' || b[i]=='-') continue;   
            n2 += (b[i]-'0')*pow(10, lenb--);	//计算出实际的b的绝对值
        }
        n1 = isnt1? -n1 : n1;	//实际的a
        n2 = isnt2? -n2 : n2;	//实际的b
        res = n1+n2;	//算出结果
        cout<<res<<endl;
    }
    return 0;
}
 
/**************************************************************
    Problem: 1056
    User: 14408400124
    Language: C++
    Result: 正确
    Time:0 ms
    Memory:1904 kb
****************************************************************/
```

<h4 id="7">问题 B: 高斯日志</h4>

题目链接：<a href="http://dengdengoj.cc/problem.php?cid=1004&pid=1" target="_blank">1074: 高斯日志</a>

这题属于算日期的题目，应该可以直接总结一个模板来使用。难度不大，我的AC代码运行时长略长，还有改进空间。
```cpp
#include <iostream>
#include <cstdio>
using namespace std;
bool isleapyear(int y)	//判闰/平年
{ return (y%4==0 && y%100!=0) || y%400==0; } 
int days[] = {0,31,28,31,30,31,30,31,31,30,31,30,31};	//每个月的天数表

void getdate(int y, int num)
{
    int mouth=1;
    days[2] = isleapyear(y)?29:28;	//根据是否为闰年更新月份表2月天数
    while (num-days[mouth]>0)
          num-=days[mouth++];
    printf("%d-%02d-%02d\n", y, mouth, num);
}
 
int main(int argc, char** argv)
{     
    int y=1777, m=4, d=30;	//起始日期为高斯出生日期
    int num=0;
     
    while (cin>>num)
    {
        if (num < 247)	//未超过一年单独处理
           getdate(y, num+119);
        else
        {
            int addy=1, yd=isleapyear(y+addy)?366:365;	//超过一年的计算超过的年限
            num-=246;
            while (num > yd)
            {
                num -= yd;
                addy++;
                yd=isleapyear(y+addy)?366:365;              
            }
            getdate(y+addy, num);	//计算出最终的年份后可以算出准确日期
        }
    }
     
    return 0;
}
 
/**************************************************************
    Problem: 1074
    User: 14408400124
    Language: C++
    Result: 正确
    Time:188 ms
    Memory:1708 kb
****************************************************************/
```
<h4 id="8">问题 C: 回文数</h4>

题目链接：<a href="http://dengdengoj.cc/problem.php?cid=1004&pid=2" target="_blank">1073: 回文数</a>

本题提交了4次才过，第1次提交是没有代码有个地方写错了导致非10进制的题目会出现计算错误。之后的2次是因为代码里的debug语句没有删除干净导致了OLE和TLE。直到第4次提交才没有问题。  
题目的意思非常简单，主要是涉及到字符串表示的数字的加法运算，这题还需要考虑进制的问题，由于问题中最大进制是16，所以我直接打了一个转换表。  
字符串表示的数字加法思路就是模拟列竖式计算的过程，值得注意的是如果位数能够按照数字的低位在字符串的高位这个规律来算的话比较好算。  
至于进制的问题，算法中的进制是作为参数传进去的，所以处理10进制和处理其他进制的代码是一样的。  
```cpp
#include <iostream>
#include <cstdio>
#include <cstring>
#include <map>
using namespace std;

//进制字符转换表，用string数组是要传入insert函数中
string ch[] = {"0","1","2","3","4","5","6","7",
               "8","9","A","B","C","D","E","F"};
//判断一个字符串是否是回文字符串
bool isloop(const string n)
{
    for (int i=0; i<n.length()/2; i++)
        if (n[i] != n[n.length()-i-1])
           return false;
    return true;
     
}
//转置字符串
string rstring(string str)
{
    string res = str;
    for (int i=0; i<str.length(); i++)
        res[i] = str[str.length()-i-1];
    return res;
}
//模拟加法
string addbydcm(string num, int dcm)
{
    string num2 = rstring(num);
    int tmp=0;	//进位标记
    string res="";
    for (int i=0; i<num.length(); i++)
    {
        int n1 = num[i]>='A'?num[i]-'A'+10:num[i]-'0';	//num[i]才是需要的数而不是i
        int n2 = num2[i]>='A'?num2[i]-'A'+10:num2[i]-'0';
        int n = n1+n2+tmp;
        res.insert(0, ch[n%dcm]);
        tmp = n/dcm;
    }
    if (tmp)	//如果最后还有进位需要加进去
       res.insert(0, ch[tmp]);
        
    return res;
}
 
int main(int argc, char** argv)
{
    int step=1, dcm=0;	//最小步数应该是1
    string num;
     
    cin>>dcm>>num;  
    while (1)
    {
        if (step>=30) { puts("Impossible"); break; }     
        num = addbydcm(num, dcm);       
        if (!isloop(num)) step++;
        else { cout<<step<<endl; break; }
    }
    return 0;
}
 
/**************************************************************
    Problem: 1073
    User: 14408400124
    Language: C++
    Result: 正确
    Time:0 ms
    Memory:1712 kb
****************************************************************/
```

<h4 id="9">问题 D: 排名</h4>

题目链接：<a href="http://dengdengoj.cc/problem.php?cid=1004&pid=3" target="_blank">1034: 排名</a>

简单的排序题，输入略繁琐。为了达到题目要求，可以排两次，第一次先根据准考证升序排，第二次根据总分降序排名。
```cpp
#include <iostream>
#include <cstdio>
#include <cstring>
#include <algorithm>
using namespace std;
 
typedef struct
{
    string id;
    int sum;
}info;
 
//总分排名规则
bool sumcmp(const info &i1, const info &i2)	
{ return i1.sum > i2.sum; }
//准考证号排名规则
bool idcmp(const info &i1, const info &i2)
{ return i1.id < i2.id; }
 
info stu[1001];	//防止stackoverflow较大数组作为全局变量
int main(int argc, char** argv)
{
    int num=0, subnum=0, line=0;    
    int qsum[11];
    
	//输入数据
    while (cin>>num && num)
    {       
        memset(qsum, 0, sizeof(qsum));  
        cin>>subnum>>line;
         
        int pn=0;
        for (int i=1; i<subnum+1; i++)
            cin>>qsum[i];
        for (int i=0; i<num; i++)
        {
            int qs=0, n=0;
            info tmp; tmp.sum=0;
            cin>>tmp.id>>qs;
            bool ispass=false;
            for (int j=0; j<qs; j++)
            {
                cin>>n;   
                tmp.sum += qsum[n];
                ispass = tmp.sum>=line;
            }			
            if (ispass)	//过线则加入待排序数组中
            {
                stu[pn].id = tmp.id;                
                stu[pn++].sum = tmp.sum;
            }      
        }
        sort(stu, stu+pn, idcmp);	//按照准考证号升序
        sort(stu, stu+pn, sumcmp);	//按照学号降序
         
        cout<<pn<<endl;
        for (int i=0; i<pn; i++)
            cout<<stu[i].id<<" "<<stu[i].sum<<endl;
    }
     
    return 0;
}
 
/**************************************************************
    Problem: 1034
    User: 14408400124
    Language: C++
    Result: 正确
    Time:0 ms
    Memory:1732 kb
****************************************************************/
```
<h4 id="10">问题 E: Insertion or Heap Sort (25)</h4>

题目链接：<a href="http://dengdengoj.cc/problem.php?cid=1004&pid=4" target="_blank">1072: Insertion or Heap Sort (25)</a>  

本题是我花时间花的最多的题（WA了2次才过）。题目意思非常明确，先给出了插入排序和堆排序的定义，然后问题是给定一组数，然后给定一个序列，问这个序列是使用哪一种排序方法排出来的，由于这个序列不会是排序过程中最后一趟结果序列，所以还需要输出用该排序方法排出来的下一趟排序序列。  

插入排序的时间复杂度为O(n^2)，堆排序的时间复杂度为O(nlogn)。但是由于本题数据规模非常小（待排序序列最长为100），所以我先用两种排序方法将序列排好序，用一个vector保存了两个排序序列中的每一趟结果，然后与输入序列依次比较来确定序列出现在哪一个排序中间结果中。然后输出下一趟结果即可。代码如下，代码比较长，应该还有精简的办法。
```cpp
#include <iostream>
#include <cstdio>
#include <cstring>
#include <vector>
using namespace std;
 
//iss 保存插入排序中间序列，hss保存堆排序中间序列
vector<vector<int> > iss, hss;
//插入排序的实现
void insertsortseq(int* a, int len)
{
    iss.clear();
    int tmp[len];   
    for (int i=0; i<len; i++)
        tmp[i] = a[i];
     
    for (int i=1; i<len; i++)
    {
        if (tmp[i] < tmp[i-1])  //找到插入位置
        {
            int n=tmp[i], j=i-1;
            for (; n<tmp[j]; j--)   //后移腾出插入位置
                tmp[j+1] = tmp[j];
            tmp[j+1] = n;   //插入正确位置
        }
         
        vector<int> cache;
        for (int i=0; i<len; i++)
            cache.push_back(tmp[i]);                 
        iss.push_back(cache);
    }
 
}

//大根堆的向下调整算法
void adjustdown(int* a, int k, int len)
{
    int tmp = a[k]; //哨兵
    int i = 2*k+1;
    while(i < len)
    {
        if(i+1<len && a[i]<a[i+1]) i++; //使用较大孩子的位置
         
        if(tmp<a[i]) { a[k] = a[i]; k = i; i = 2*k+1; } //a[i]调整到双亲结点上，并修改k值以便继续向下筛选
        else break; //筛选结束
    }
    a[k] = tmp; //放入正确位置形成大根堆
}

//堆排序的实现
void heapsortseq(int* a, int len)
{
    hss.clear();
    for(int i = len/2-1; i>=0; i--)
        adjustdown(a, i, len);  //建大根堆
 
    vector<int> cache;  //建立大根堆就是第一趟排序
    for (int i=0; i<len; i++)
        cache.push_back(a[i]);
    hss.push_back(cache);
     
    for(int i = len-1; i>=0; i--)
    {
        a[0] ^= a[i] ^= a[0] ^= a[i];   //交换堆顶与堆底元素
        adjustdown(a, 0, i);    //整理接下来的i-1个元素
         
        vector<int> cache2;
        for (int i=0; i<len; i++)
            cache2.push_back(a[i]);
        hss.push_back(cache2);
    }
}
 
int main(int argc, char** argv)
{
    int a[101], count=0;        
     
    while (cin>>count)
    {
        memset(a, 0, sizeof(a));    
        for (int i=0; i<count; i++)
            cin>>a[i];
         
        insertsortseq(a, count);    //插入排序
        heapsortseq(a, count);      //堆排
     
        vector<int> seq;
        for (int i=0; i<count; i++)
        {
            int t=0;
            cin>>t; seq.push_back(t);
        }
         
        bool isheap=false;
        int step=0; //记录序列是第几趟排序结果        
        for (int i=0; i<iss.size(); i++)
        {   //匹配插入排序的中间结果
            int matchsum=0;
            for (int j=0; j<iss[i].size(); j++)
                if (iss[i][j]==seq[j]) 
                   matchsum++;
            if (matchsum == seq.size())
            { step = i; isheap = false; break; }
            else isheap = true; //在插入排序中未找到序列
        }
        if (isheap)
        {   //在堆排结果中查找
            for (int i=0; i<hss.size(); i++)
            {
                int matchsum=0;
                for (int j=0; j<hss[i].size(); j++)
                    if (hss[i][j]==seq[j]) 
                       matchsum++;
                if (matchsum == seq.size())
                { step = i; isheap = true; break; }
            }
        }
         
        if (isheap) puts("Heap Sort");
        else puts("Insertion Sort");
         
        //输出下一趟序列
        seq.clear();
        seq = isheap ? hss[step+1] : iss[step+1];
        cout<<seq[0];   //注意格式
        for (int i=1; i<seq.size(); i++)     
            cout<<" "<<seq[i];
        cout<<endl;
    }
     
    return 0;
}
 
/**************************************************************
    Problem: 1072
    User: 14408400124
    Language: C++
    Result: 正确
    Time:0 ms
    Memory:1728 kb
****************************************************************/
```

---