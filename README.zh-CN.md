# OITool

[![GitHub release (with filter)](https://img.shields.io/github/v/release/ReturnNefe/OITool?style=for-the-badge)
](https://github.com/ReturnNefe/OITool/releases) ![GitHub all releases](https://img.shields.io/github/downloads/ReturnNefe/OITool/total?style=for-the-badge)

中文 | [English](https://github.com/ReturnNefe/OITool/blob/main/README.md)

**为 OIer 们打造的实用工具。**
 
对 WA 说 ~~你好~~ 再见!

OITool 目前只支持在终端上使用。之后它可能会有具有图形界面的版本。

## 背景

OITool 最初是受到一位群友的启发而提出的。他觉得他用的编辑器上内置的评测器用起来怪怪的，所以他想让我在我开发的IDE上实现这一个功能。（事实上我已经实现了，但是那个IDE已经不再维护了）

突然我有了一个想法。我是否可以为 OIer 们开发一个独立的工具。这个工具不依赖任何一个编辑器或 IDE，同时它支持自由地加载插件，因此它可以具有许多功能。这个工具由此诞生。

## 特点

* **跨平台**: 在任何操作系统上的编辑器或 IDE 中使用它!
* **简易**: 输入几行简单的命令来实现你的想法。
* **自由**: 通过加载任何第三方插件来获得你想要的功能。

## 演示



## 安装

1. 下载 [.NET 6 Runtime](https://dotnet.microsoft.com/download/dotnet/6.0)。
2. 下载并解压 [OITool](https://github.com/ReturnNefe/OITool/releases)。
3. _(可选的)_ 配置环境变量。
   [教程(Windows)](https://www.computerhope.com/issues/ch000549.htm)
   
   添加 **oitool** 所在的文件夹到环境变量中。

## 使用

你可以在终端输入 ``oitool [command] --help`` 来查看帮助。

**支持的命令**

| Command | Description | Example |
|:--:|:--:|:--:|
| judge * | 尝试通过所给的 data-file 评测程序 | ```oitool judge program.exe data.in data.out``` |
| server start | 启动 **OITool.Server** | ``oitool server start`` |
| help | 获取帮助 | ``oitool help`` |

\* 需要 **OITool.Server** 在后台运行，而不是在硬盘里睡大觉。

**示例**

```shell
# 叫 server 汽车！
# -b 选项让 server 在后台运行
oitool server start -b
cd /competition/data/

# judge 支持文件(*.in/*.out/*.ans)和文件夹
# 传入样例文件
oitool judge program example1.in example2.in example1.out example2.out
# 传入样例文件夹
oitool judge program examples/

# 你还可以指定一些选项
# 设置
# 时间限制为 1500ms
# 内存限制为 512MiB
# 报告文件输出到 myReport.html
oitool judge program data.in data.out -t 1500 -m 512 -r myReport.html
```

**工作原理**

OITool 为插件提供了一些接口。主要的功能实现都由这些插件完成，剩下的一小部分由 OITool 完成。

因此，您可能需要单独配置插件。

以下是默认安装的插件：

* [Plugin.Default](https://github.com/ReturnNefe/OITool/blob/main/docs/plugin/default.md)

## 协议

[GPL-3.0 license](https://github.com/ReturnNefe/OITool/blob/main/LICENSE) © ReturnNefe