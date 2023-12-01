# GameFramework
> 由于这个框架用的是Unity编辑器2022.3.13f1c1,2022会有一些Bug,❗️如果不使用这个版本，导入文件后，只需要自行去PackageManager中安装NewtonsoftJson、InputSystem和Addressables即可消除报错。
>
> - Excel数据表读取模块：过使用Unity的编辑器自定义功能和Unity官方提供的ExcelDll，编写了一个可以直接在Unity编辑器Project窗口选择对应的Excel文件夹或是直接读取对应路径文件自动生成Binary和Json数据文件，以及相对应的数据结构类的工具。
>
> - 数据持久化管理模块：中包含四种数据持久化的方式：PlayerPres、XML、Json、Binary。开发者可以直接相对应的模块管理器里的方法直接对数据进行序列化和反序列化，其中的部分功能实现用到了一些反射知识，由于官方的JsonUtilities不支持字典的序列化，我增加了一个第三方Json插件NewtonsoftJson，开发者可以通过枚举进行选择自己想要的方式进行数据的序列化和反序列化并进行本地数据持久化存储。
> - 事件分发中心：使用一个字典容器，存储所有的事件(泛型带参，不带参数，带多个参数)，然后结合Unity的UnityAction和观察者模式是实现了这个事件中心模块，需要更多的带参数事件，可自定义增加。
> - 旧输入系统的事件监听模块：通过三个字典分别存储按键的按下、按住、抬起事件(UnityAction)进行监听存储，然后通过三个协程分别监听对应字典中的按键事件是否有触发，然后执行字典中存储的事件。
>
>   ✨(重构旧输入系统监听模块：使用了命令模式来编写这个模块，具体的内容在该文件夹中有具体说明。建议使用命令模式的这个输入管理器，更加简洁明了易用)
> - Mono模块：这个模块主要是为了给一些没有继承MonoBehaviour的类也能够使用到MonoBehaviour里的生命周期函数，Unity协程等功能，例如上面的旧输入系统中的管理器是没有继承MonoBehaviour的，但是通过这个Mono管理器也能够在Unpdate中执行三个协程方法监听按键，主要通过事件委托是实现。
> - 对象池管理器模块：对象池模块主要实现了两个，一个是Unity官方自带的ObjectPool对象池类，加上字典容器实现了可以根据用户输入的不同名字的物体创建不同的ObjectPool存到字典中。还有一个自定义实现的对象池类，为了应对ObjectPool不能解决的情况，实现原理和官方的ObjectPool是差不多的，用到的也是字典和List容器，在这个自定义池子里，还实现了定时清理对象池缓存功能，并且结合下面的资源动态加载模块，实现了动态加载资源的缓存池。
> - 资源动态加载模块：把Unity的Resources同步加载和异步加载方法进行了封装，并且判断加载的对象是GameObject，则直接实例化后再返回出去，无需外部进行实例化。
> - 音乐音效管理器模块：这个模块不仅实现了常规的背景音乐和环境音乐播放，还通过使用上面的对象池管理模块实现了音效的缓存池模块，只需要输入对应的名字的音乐就可以在场景中创建对应的音效组件进行播放，且播放完后可以收回缓存池。
> - 场景加载模块：把Unity的异步和同步场景加载封装起来，并结合事件中心模块，实现了场景加载事件的分发管理。
> - UGUI管理器模块：通过创建一个基类的Panel，并在这个基类创建的时候，把自己身上的所有UI控件找到并存储到字典容器中，子类只需要继承这个基类，在实例化出来时，即可通过字典找到自己身上的所有控件，并添加对应的事件即可，无需手动在Unity的编辑窗口进行拖曳绑定控件，还实现了UI面板的渐入渐出功能。并通过一个UI管理类进行对游戏场景中实例化出来的UI面板进行管理，用户只需要通过这个UI管理器对面板进行管理即可，新创建面板只需继承基类Panel重写里面的方法实现自定义功能效果即可。UI管理器还提供了Unity新输入系统的改建功能和控件自定义事件功能。
> - AB包热更新上传下载模块：通过MD5加密功能把AB包资源打包进行了唯一性验证，并存储为对比文件，再结合Serv-U工具搭建的FTP服务器实现了AB包的上传下载以及AB包资源对比功能，与Ftp服务器之间通信用到了Task异步编程实现，并通过Unity的编辑器实现了一个工具窗口，用户可在窗口中自定义自己的服务器地址，对AB包的资源进行管理，结合事件中心模块可实现资源更新进度条功能。
> - AB包资源管理模块：把Unity的AB包资源的同步和异步加载进行了封装，通过加锁标志，防止多个资源同时异步调用造成资源重复访问加载问题，并结合事件中心模块实现了在资源动态加载的开始，过程以及结束后可执行相对应的事件功能。
> - 有限状态机
> - 延时计时器
> - Lua管理器
>
> 持续更新优化......
