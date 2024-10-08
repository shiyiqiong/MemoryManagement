# 内存管理
版本：Unity 2022.3.14f1c1。
***
**APP占用内存：**
- App调用公共库占用的内存：例如调用第三方SDK占用的内存。
- App自身Unity内存：
  - Native Memory（本机内存）：引擎C++生成的内存，加载资源内存。
  - Managed Memory（托管内存）：C#生成的内存。
  - 显存：纹理贴图、渲染贴图、网格。
- 未知内存：（通常包含：lua 虚拟机占用的内存）
***
**Unity GC原理**
- 标记清除算法：
  - 标记阶段：GC从根对象（全局变量，栈中对象等）开始，递归遍历所有可达对象，并标记活动对象。
  - 清除阶段：GC会清除未标记的对象，并回收它们所占用内存。
***
**Unity Boehm 内存回收机制**
- 非分带式：容易产生碎片；
- 非压缩式：不进行碎片整理。
***
**C++内存分区**
- 静态存储区：在程序整个运行期间都存在，主要存放静态数据，全局数据和常量。
- 栈：栈上存储函数内局部存储单元，执行函数是创建，函数执行结束，自动释放。
  - 栈溢出：执行函数时，栈空间不够用。 
- 堆：动态内存分配，可以由程序控制大小，以及创建和释放。
  - 内存泄漏：不再被使用的内存，没有被释放。 
***
**内存检查工具：**
- Memory Profiler
- Lua Profiler：https://github.com/leinlin/Miku-LuaProfiler.git

![image](https://github.com/user-attachments/assets/242a2709-bc1e-4506-8be3-13f0403fdf5c)

***
**内存管理主要问题：**
- 内存峰值高：
  - 配表数据
  - 纹理贴图
  - 音频资源
- 内存泄漏：
  - 代码中认为以及释放的内存，实际上没有释放
- 频繁GC：
  - 高频调用对象，使用缓存池，避免频繁创建销毁
  - 频繁装箱拆箱
  - SetActive：频繁设置可见不可见
  - 字符串拼接 
- CPU访问数据，频繁cache miss：
  - CPU访问CPU缓冲区速度很快，访问内存速度慢
  - 访问数据时如果在缓冲区找不到， 就会从内存拿一段连续指令到缓冲区
  - 频繁遍历数据，尽量让数据连续存储。
***
**硬件分类：**
- 物理内存：
  - 移动设备没有显存，显存与内存公用 
- 虚拟内存
  - 移动设备没有虚拟内存：（IO速度慢，硬件容易损坏）
  - IOS有内存压缩技术，压缩不活跃的内存，给活跃APP腾出空间
***
**Android内存管理：**
内存杀手：
- 系统内核 > 系统级服务 > 用户级服务 > 前端当前应用 > 辅助应用 > 后台服务 > 桌面 > 前一个使用过的应用 > 缓存应用
- 杀死当前引用时，应用闪退
- 杀手系统级服务时，手机重启
***
参考资料：https://zhuanlan.zhihu.com/p/146740326
