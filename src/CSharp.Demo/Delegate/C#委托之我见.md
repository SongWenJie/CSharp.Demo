## C#委托之我见

委托的使用方式很简单，了解一下基本语法就可以开撸了。但是使用委托的真正难题是不知道应用场景，就像习得了一门新功夫，但是却找不到任何施展拳脚的地方。这个难题一直困然着我，直到最近仿佛有所领悟，所以赶紧记下这可能尚不成熟的观点。如果有什么错误的想法，还望各位园友指正。



## 解耦合

**其实委托最大的作用是解耦合，转移程序方法的功能定义方**。在不使用委托的情况下，方法的功能和行为（能做的事）都是由方法提供方决定的，方法一经定义，能做的事情也就固定了，这就相当于方法是静态的。但是如果方法使用了委托参数类型，方法功能的定义方就发生了转移，此时方法能做什么事是由方法的调用方决定的。这样就相当于方法有了生命力，这种生命力是方法的调用方赋予的。并且方法的可重用性得到了提高，以前是做一件事情，现在是做一类事情。同时，委托可以看做是把方法作为方法的参数，这样会避免掉一些不必要的判断（因为作为参数的方法会定义做什么事情，不用再额外判断），简化程序逻辑。

Talk is cheap，show me the code.

### 方法作为方法的参数，避免掉不必要的判断

我们写程序时经常会遇到这样一种情况。在分支判断中，每个分支中做的操作都可以归属于一类事情，方法的签名也能保持一致。这时可以考虑使用委托消除掉这些分支判断。假设现在要做一个四则运算的功能，其拥有四个方法，它们的签名都相同，都接受两个double输入，并输出一个double。一般的做法是：

```c#
public enum Operate
{
    Add,
    Subtrac,
    Multip,
    Divisi
}

public static double Calculate(double a,double b, Operate operate)
{
    switch(operate)
    {
        case Operate.Add:
            return Add(a, b);
        case Operate.Subtrac:
            return Subtrac(a, b);
        case Operate.Multip:
            return Multip(a, b);
        case Operate.Divisi:
            return Divisi(a, b);
        default:
            return 0;
    }
}

public static double Add(double a , double b)
{
    return a + b;
}
public static double Subtrac(double a, double b)
{
    return a - b;
}
public static double Multip(double a, double b)
{
    return a * b;
}
public static double Divisi(double a, double b)
{
    if (b == 0) throw new Exception("分母不能为0");
    return a / b;
}
```

这样实现有一个缺点，现在是四则运算，万一以后加入其它类型的运算呢？每加入一个类型的运算都要新增一个分支判断，这样的话维护成本就有点高了，也不符合对修改关闭，对扩展开放的开闭原则。要是为每种类型的操作建个类，用多态的思想解决又有点小题大做了。可以考虑使用委托解决这个问题，使用和方法签名相同的委托代替枚举类型的参数。

首先新建一个和方法签名相同的委托类型，然后使用和方法签名相同的委托代替枚举类型的参数：

```c#
public delegate double CalculateDelegate(double a, double b);
```

```c#
public static double Calculate(double a, double b, CalculateDelegate operate)
{
    return operate(a, b);
}
```

调用方决定具体的运算：

```c#
static void Main(string[] args)
{
    Calculate(1, 2, Add);
    Calculate(1, 1, Divisi);
}
```

利用委托来解决这种问题看似很好，但是也有缺点，需要为每一种计算类型定义相应的方法，而且其中有些方法使用频率并不高，程序中可能会大量出现这样的计算方法，维护这些方法反而是不小的负担。C#提供了匿名函数的方式来解决这个问题。

```c#
static void Main(string[] args)
{
    Calculate(1, 2, delegate (double a,double b) { return a + b; });
    Calculate(1, 1, delegate (double a, double b) { return a / b; });
}
```
嗯，解决了上面的问题。但是似乎代码可读性不够高，那就继续进化，C#提供了lambda表达式，让我们以几乎感觉不到委托存在的方式，顺其自然的使用C#委托，原生C#委托几乎被遗忘，委托三步走不复存在，委托=>匿名函数=>lambda表达式 究极进化，C#就是这么强大！

你可以这么玩：`  Calculate(1, 2, (double a,double b) => { return a + b; });`

还可以这么玩：` Calculate(1, 2, (a,b) => { return a + b; });`



### 方法调用方决定方法做什么事

C#中的Linq可谓是将委托用到了极致，以Where方法为例，Where方法本身只负责筛选集合中的元素这类事，但是至于具体是哪件事，并不关心。具体做哪件事是由方法的调用方来指定的，比如筛选大于10的元素、或是小于5的元素，这些都是由调用方决定的。方法的灵活性、可重用性都得到了提高。设想一下，如果为每个元素筛选条件规则都去写一个除了筛选条件不同其他操作都相同的新方法，心态爆炸不？使用委托类型的参数，这一切将变得很简单。做一件事情变为做一类事情，至于是哪一件事情，方法调用方来决定喽。

这种方式最重要的应用就是回调函数。

> **回调函数**就是一个通过**函数**指针调用的**函数**。 如果你把**函数**的指针（地址）作为参数传递给另一个**函数**，当这个指针被用来调用其所指向的**函数**时，我们就说这是**回调函数**。 **回调函数**不是由该**函数**的实现方直接调用，而是在特定的事件或条件发生时由另外的一方调用的，用于对该事件或条件进行响应。

简单理解，当我们将函数A传递给函数B，并由B来执行A时，A就成了一个回调函数（callback functions）。回调函数肯定是方法调用方负责定义的，当方法执行时，满足相应的条件就会触发此回调函数。在C#中实现回调函数的方式就是委托。

假设现在我们有两个方法，一个方法负责将数组中的每个元素翻倍，另一个方法负责加1，现在需要翻倍再加一。如果不使用委托（回调函数），则需要进行两次for循环，性能上无法接受，这个时候就可以使用委托（回调函数）来解决，只需要一次for循环就可以。

不使用委托（回调函数）：

```c#
public static void Double(int[] nums)
{
    for (int i = 0; i < nums.Length; i++)
    {
        nums[i] = nums[i] * 2;
    }
}

public static void AddOne(int[] nums)
{
    for (int i = 0; i < nums.Length; i++)
    {
        nums[i] = nums[i] + 1;
    }
}
```

```c#
static void Main(string[] args)
{
    int[] nums = { 1, 2, 3 };

    Double(nums);
    AddOne(nums);
}
```

使用委托（回调函数）：

```c#
public static void DoubleAndAddOne(int[] nums,Func<int,int> func)
{
    for (int i = 0; i < nums.Length; i++)
    {
        nums[i] = func(nums[i] * 2);
    }
}
```

```c#
DoubleAndAddOne(nums, n => n + 1);
```























