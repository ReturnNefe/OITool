# OITool

[![GitHub release (with filter)](https://img.shields.io/github/v/release/ReturnNefe/OITool?style=for-the-badge)
](https://github.com/ReturnNefe/OITool/releases) ![GitHub all releases](https://img.shields.io/github/downloads/ReturnNefe/OITool/total?style=for-the-badge)

English | [中文](https://github.com/ReturnNefe/OITool/blob/main/README.zh-CN.md)

**The useful and lite tool for OIers.**
 
Say ~~hello~~ goodbye to WA!

OITool is currently only available on Terminal. It may have a version with a graphical interface in the future.

## Background

OITool started with a suggestion originally made by a friend in a online-group, who is an OIer. He didn't feel very comfortable when using the judger on the editor he used. So he asked me if I could implement this on the IDE I had made. (In fact, That IDE already has this feature, but, it is no longer maintained)

Then I had an idea. Could I made a independent tool for OIers? It doesn't depend on any editor or IDE, and it allow users to load plugins freely so that it can do many things. This tool started as a result of that.

## Features

* **Cross-Platform**: Use it everywhere! No matter what editor/IDE and operating system you use.
* **Easy to Use**: Type a few commands to accomplish what you want to do.
* **Freely**: Allow you to load third-party plugin to get the features you want.

## Visuals



## Install

1. Download [.NET 6 Runtime](https://dotnet.microsoft.com/download/dotnet/6.0).
2. Download and upzip OITool in [Releases](https://github.com/ReturnNefe/OITool/releases).
3. _(Optional)_ Configure Environment Variables.
   [Tutorial(Windows)](https://www.computerhope.com/issues/ch000549.htm)
   
   Add the directory where **oitool** is located to the environment variable.

## Usage

You can type ``oitool [command] --help`` for help.

**Supported Commands**

| Command | Description | Example |
|:--:|:--:|:--:|
| judge * | Try to judge program by given data-file | ```oitool judge program.exe data.in data.out``` |
| server start | Start **OITool.Server** if it isn't running in the background. | ``oitool server start`` |
| help | Get Help Information | ``oitool help`` |

\* Needs **OITool.Server** running in the background, not sleeping💤.

**Example**

```shell
# Get the server up!
# The -b option lets it run in the background.
oitool server start -b
cd /competition/data/

# judge allows data-file(*.in/*.out/*.ans) and data-folder.
# provide data-file
oitool judge program example1.in example2.in example1.out example2.out
# provide data-folder
oitool judge program examples/

# you can also provide some options.
# set timeout:1500ms memory-limit:512MiB report-file:myReport.html
oitool judge program data.in data.out -t 1500 -m 512 -r myReport.html
```

**How It Works**

OITool provides interface to plugins. It lets plugins implement most specific features, while it implements a small part.

Consequently, you may need to configure plugins.

Here are the plugin installed by default:

* [Plugin.Default](https://github.com/ReturnNefe/OITool/blob/main/docs/plugin/default.md)

## License

[GPL-3.0 license](https://github.com/ReturnNefe/OITool/blob/main/LICENSE) © ReturnNefe