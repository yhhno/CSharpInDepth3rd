using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tip1._1从简单的数据类型开始
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        /********************************
        
        本章，我将让C#编译器实现一些神奇功能，但不会告诉你怎么做，也很少会提及这是什么以及为什么会这样。这是唯一一次我不会解释原理，或者说不会按部就班地展示例子的情形。
         事实上，我的计划是先给你留下一个不可磨灭的印象，而不是教给你具体的知识。在你读完本节的内容之后，如果对C#能做到的事情仍然没有感到丝毫的兴奋，那么本书或许真的不适合你，相反你迫切地想知道我的魔术是怎么玩的。------想让我放慢手法，直至看清楚发生的所有事情---那么就是本书剩余部分要做的事情。
         
         事先要提醒你的是，这个例子可能显得十分牵强----为了在尽可能短的代码中包含尽可能多的新特性而“生搬硬造”，
         我们将看到各种任务是如何实现的，体验随着C#版本的提高，如何更简单、更优雅地完成相同的任务，
         
        
        
        
        
        
         * **/

        //tip1.1.1 C#中定义的产品类型

        //我们从以定义一个表示产品的类型作为开始，然后处理。在product类型内部，没有特别吸引别人的东西，只是封装了几个属性，为方面演示，我们还要在这个地方创建预定义产品的一个列表。


        //用C#1写的Product类型，稍后还会演示在更高版本中如何达到相同的效果，我们将按照这个模式演示其他所有的代码。

        //以下代码证明了C#1代码存在3个局限
        //1. ArrayList 没有提供与其内部内容相关的编译时信息，不慎在GetSampleProducts创建的列表中添加一个字符串是完全有可能的，而编译器对此没有任何反应
        //2.代码中为属性提供了公共的取值方法，这意味着如果添加对应的赋值防范，那么赋值方法也必须是公共的。
        //3.用于创建属性和变量的代码很复杂---封装一个字符串和一个十进制数应该是一个十分见得任务，不该这么复杂
        public class Product
        {
            public Product(string name, decimal price)
            {
                this.name = name;
                this.price = price;
            }

            string name;
            public string Name { get { return name; } }
            decimal price;
            public decimal Price { get { return price; } }

            public static ArrayList GetSampleProducts()
            {
                ArrayList list=new ArrayList();
                list.Add(new Product("p1", 9.99m));//此时的Product为构造函数。
                list.Add(new Product("p2", 1m));
                list.Add(new Product("p3", 4m));
                return list;
            }

            public override string ToString()
            {
                return string.Format("{0}: {1}", name, price);
            }
        }


        //来看看C#2 做了哪些改进  

        //1.1.2 C#2的强类型集合
        //C#2 中最重要的改变：泛型
        public class ProductV2
        {
            string name;

            public string Name
            {
                get { return name; }
                private set { name = value; }//属性拥有了私有的赋值方法，（我们在构造函数中使用了这两个赋值方法，）
            }

            decimal price;

            public decimal Price
            {
                get { return price; }
                private  set { price = value; }
            }

            public ProductV2(string name, decimal price)
            {
                Name = name;//我们在构造函数中使用了这两个赋值方法，
                Price = price;
            }

            //能非常聪明地猜出List<ProductV2>是告知编译器列表中只能包含Product。试图讲一个不同的类型添加到列表中，会造成编译错误。并且当你从列表中获取结果时，也并不需要转换结果的类型-------泛型的好处
            public static List<ProductV2> GetSampleProducts()
            {
                List<ProductV2> list=new List<ProductV2>();
                list.Add(new ProductV2("p1", 9.99m));//此时的ProductV2为构造函数。
                list.Add(new ProductV2("p2", 1m));
                list.Add(new ProductV2("p3", 4m));
                return list;
            }

            public override string ToString()
            {
                return string.Format("{0}: {1}", name, price);
            }
        }


        //来看C#3 做了哪些改进

        //tip1.1.3  C#3中自动实现的属性
        // 介绍一些C#3中相对乏味的一些特性， 自动实现的属性和简化的初始化。相对于Lambda表达式等特性有些微不足道，不过达达简化代码
        public class ProductV3
        {
            public string Name { get; private set; }//现在不再有任何代码（或者可见的变量）与属性关联，
            public decimal Price { get; private set; }

            public ProductV3(string name, decimal price)//在本例中，实际上可以完全删除旧的公共构造函数。但这样一来，外部代码就不能再创建其他的产品实例了
            {
                Name = name;//由于没有name和price变量可供访问，我们必须在类中处处使用属性，这增强了一致性
                Price = price;
            }

             ProductV3()//现在有一个私有的无参构造函数，用于新的基于属性的初始化（设置这些属性之前，会对每一项调用这个构造函数）
            {
            }

            public static List<ProductV3> GetSampleProducts()
            {
                return new List<ProductV3>//简单的初始化 。而且硬编码的列表是以一种全然不同的方式构建的，
                {
                    new ProductV3 {Name = "p1", Price = 1m},//由于没有name和price变量可供访问，我们必须在类中处处使用属性，这增强了一致性
                    new ProductV3 {Name = "p2", Price = 2m}
                };
            }

            public override string ToString()
            {
                return string.Format("{0}: {1}", Name, Price);
            }
        }


        //来看C#4 做了哪些改进

        //tip1.1.4 C#4 中的命名实参

        //对于C#4，涉及属性和构造函数时，我们需要回到原始代码，其中一个原因是为了让它不易变;尽管拥有私有赋值方法的类型不能被公共地改变，但是如果它也不能被私有地改变，就会显得更加清晰，不幸的是，对于只读属性，没有快捷方式。但是C#4允许我们在调用构造函数时指定实参的名称。
        //它为我们提供了和C#3的初始化程序一样的清晰度，而且还移除了易变性（mutability）。

        public class ProductV4
        {
            readonly string name;
            public string Name { get { return name; } }

             readonly decimal price;
            public  decimal Price { get { return price; } }

            public ProductV4(string name, decimal price)
            {
                this.name = name;
                this.price = price;
            }

            public static List<ProductV4> GetSampleProducts()
            {
                return new List<ProductV4>
                {
                    new ProductV4(name: "p1", price: 1m),//命名实参特性在这个实例中的好处不是很明显，当方法或构造函数包含多个参数时，它可以让代码的含义更加清楚---特别是当参数类型相同，或者某个参数为null时。当然，你可以选择什么时候使用该特性，只在使代码更好理解时才指定参数的名称。
                    new ProductV4(name: "p2", price: 1.0m)
                };
            }

            public override string ToString()
            {
                return string.Format("{0}: {1}", Name, Price);
            }
        }



        //C#1 只读属性、弱类型集合      C#2私有属性赋值方法、强类型集合     C#3自动实现的属性、增强的集合和对象初始化   C#4用命名实参更清晰地调用构造函数和方法
        
        //以上总结了Product类型的演变历程。完成每个任务之后，我都会提供一幅类似的示意图，便于你体会C#在增强代码时，遵循的是一个什么样的模式



    }
}
