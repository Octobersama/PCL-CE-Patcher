﻿<div align="center">
<img src="Resources\icon.png"  width="250" height="250" />

# PCL CE Patcher
[![GitHub Repo stars](https://img.shields.io/github/stars/Octobersama/PCL-CE-Patcher?style=flat&logo=data%3Aimage%2Fsvg%2Bxml%3Bbase64%2CPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNiIgaGVpZ2h0PSIxNiIgdmlld0JveD0iMCAwIDE2IDE2IiBmaWxsPSIjZTNiMzQxIj48cGF0aCBkPSJNOCAuMjVhLjc1Ljc1IDAgMCAxIC42NzMuNDE4bDEuODgyIDMuODE1IDQuMjEuNjEyYS43NS43NSAwIDAgMSAuNDE2IDEuMjc5bC0zLjA0NiAyLjk3LjcxOSA0LjE5MmEuNzUxLjc1MSAwIDAgMS0xLjA4OC43OTFMOCAxMi4zNDdsLTMuNzY2IDEuOThhLjc1Ljc1IDAgMCAxLTEuMDg4LS43OWwuNzItNC4xOTRMLjgxOCA2LjM3NGEuNzUuNzUgMCAwIDEgLjQxNi0xLjI4bDQuMjEtLjYxMUw3LjMyNy42NjhBLjc1Ljc1IDAgMCAxIDggLjI1WiIvPjwvc3ZnPg%3D%3D&label=Stars&color=%23ffd548)](https://github.com/Octobersama/PCL-CE-Patcher)
[![GitHub Release](https://img.shields.io/github/v/release/Octobersama/PCL-CE-Patcher?display_name=tag&style=flat&logo=github&label=Releases)](https://github.com/Octobersama/PCL-CE-Patcher/releases)
[![GitHub License](https://img.shields.io/github/license/Octobersama/PCL-CE-Patcher?logo=data%3Aimage%2Fsvg%2Bxml%3Bbase64%2CPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxNiAxNiIgd2lkdGg9IjE2IiBoZWlnaHQ9IjE2IiBmaWxsPSIjZmZmZmZmIj48cGF0aCBkPSJNOC43NS43NVYyaC45ODVjLjMwNCAwIC42MDMuMDguODY3LjIzMWwxLjI5LjczNmMuMDM4LjAyMi4wOC4wMzMuMTI0LjAzM2gyLjIzNGEuNzUuNzUgMCAwIDEgMCAxLjVoLS40MjdsMi4xMTEgNC42OTJhLjc1Ljc1IDAgMCAxLS4xNTQuODM4bC0uNTMtLjUzLjUyOS41MzEtLjAwMS4wMDItLjAwMi4wMDItLjAwNi4wMDYtLjAwNi4wMDUtLjAxLjAxLS4wNDUuMDRjLS4yMS4xNzYtLjQ0MS4zMjctLjY4Ni40NUMxNC41NTYgMTAuNzggMTMuODggMTEgMTMgMTFhNC40OTggNC40OTggMCAwIDEtMi4wMjMtLjQ1NCAzLjU0NCAzLjU0NCAwIDAgMS0uNjg2LS40NWwtLjA0NS0uMDQtLjAxNi0uMDE1LS4wMDYtLjAwNi0uMDA0LS4wMDR2LS4wMDFhLjc1Ljc1IDAgMCAxLS4xNTQtLjgzOEwxMi4xNzggNC41aC0uMTYyYy0uMzA1IDAtLjYwNC0uMDc5LS44NjgtLjIzMWwtMS4yOS0uNzM2YS4yNDUuMjQ1IDAgMCAwLS4xMjQtLjAzM0g4Ljc1VjEzaDIuNWEuNzUuNzUgMCAwIDEgMCAxLjVoLTYuNWEuNzUuNzUgMCAwIDEgMC0xLjVoMi41VjMuNWgtLjk4NGEuMjQ1LjI0NSAwIDAgMC0uMTI0LjAzM2wtMS4yODkuNzM3Yy0uMjY1LjE1LS41NjQuMjMtLjg2OS4yM2gtLjE2MmwyLjExMiA0LjY5MmEuNzUuNzUgMCAwIDEtLjE1NC44MzhsLS41My0uNTMuNTI5LjUzMS0uMDAxLjAwMi0uMDAyLjAwMi0uMDA2LjAwNi0uMDE2LjAxNS0uMDQ1LjA0Yy0uMjEuMTc2LS40NDEuMzI3LS42ODYuNDVDNC41NTYgMTAuNzggMy44OCAxMSAzIDExYTQuNDk4IDQuNDk4IDAgMCAxLTIuMDIzLS40NTQgMy41NDQgMy41NDQgMCAwIDEtLjY4Ni0uNDVsLS4wNDUtLjA0LS4wMTYtLjAxNS0uMDA2LS4wMDYtLjAwNC0uMDA0di0uMDAxYS43NS43NSAwIDAgMS0uMTU0LS44MzhMMi4xNzggNC41SDEuNzVhLjc1Ljc1IDAgMCAxIDAtMS41aDIuMjM0YS4yNDkuMjQ5IDAgMCAwIC4xMjUtLjAzM2wxLjI4OC0uNzM3Yy4yNjUtLjE1LjU2NC0uMjMuODY5LS4yM2guOTg0Vi43NWEuNzUuNzUgMCAwIDEgMS41IDBabTIuOTQ1IDguNDc3Yy4yODUuMTM1LjcxOC4yNzMgMS4zMDUuMjczczEuMDItLjEzOCAxLjMwNS0uMjczTDEzIDYuMzI3Wm0tMTAgMGMuMjg1LjEzNS43MTguMjczIDEuMzA1LjI3M3MxLjAyLS4xMzggMS4zMDUtLjI3M0wzIDYuMzI3WiI%2BPC9wYXRoPjwvc3ZnPg%3D%3D)](https://github.com/Octobersama/PCL-CE-Patcher/blob/master/LICENSE)

**一个用于 [PCL2 CE](https://github.com/PCL-Community/PCL2-CE) 的**修补器。
</div>

## 项目介绍

PCL CE Patcher 是一个针对 **PCL CE (社区版)** 的自动化修补工具。
鉴于 PCL CE 在近期更新中引入了强制正版验证，导致部分用户无法添加离线或第三方登录账号，本工具旨在通过修改本地文件，移除这一限制，恢复启动器的离线使用功能。

本项目大量使用了 AI，因此可能存在一些代码质量问题。若您发现了问题，欢迎您向我指出问题或提供建议。

## 免责声明

0. **完全免费**：本项目完全开源免费，且作者没有从中获取任何收益。
    * 如果您是付费获得此工具的，建议您立即退款并举报商家。
1. **工具性质**：本项目是一个独立的 C# 实用工具。本项目源码及发布文件中 **不包含** 任何属于 [PCL2 CE](https://github.com/PCL-Community/PCL2-CE) 或 [PCL2](https://github.com/Meloong-Git/PCL) 的源代码、二进制文件、API 密钥或美术资源。
2. **本地操作**：本工具仅对用户电脑上**已存在**的文件进行修改。用户需自行下载原版 PCL CE 文件。
3. **互操作性**：本工具的目的是**恢复** PCL2 原版中固有的、但在 PCL CE 近期版本中被限制使用的“离线模式”等功能。
4. **非官方项目**：本项目与 PCL2 原作者（龙腾猫跃）及 PCL CE 社区开发组**无任何关联**。
    * **禁止分发成品**：请不要分发使用本工具修补后的 PCL CE 二进制文件（EXE）。这可能会侵犯 PCL2 和 PCL CE 作者的权益。请仅分发本 Patcher 工具，让用户自行修补。
    * **无官方支持**：使用本工具修补后，您将**失去** PCL CE 提供的社区支持。此时遇到的遇到任何问题（崩溃、报错），请**务必不要**向 PCL CE 或 PCL2 官方仓库反馈，以免干扰官方开发者的正常工作。<br>如果您需要官方支持，请使用原版 PCL CE。
5. **风险自担**：本软件按“原样”提供，在开发时期望其有用，但不提供任何担保。使用本工具产生的任何后果（包括但不限于软件崩溃、账号风险、法律风险等）由用户自行承担。

## 使用方法

从 [Releases](https://github.com/Octobersama/PCL-CE-Patcher/releases) 中下载最新版本并运行。

首次使用时，您需要输入本项目的开源地址 <code>https://github.com/Octobersama/PCL-CE-Patcher</code> 以通过验证，验证后30天内有效。
* 此举旨在防止本项目被倒卖。

在软件的主界面中，选择一个<code>2.13.4-beta.2</code>版本（限制在此版本中首次加入）之后的原版 PCL CE 二进制文件，点击修补按钮。
* 若修补成功，您可以在选择的文件同目录找到一个文件名带有<code>_Patched</code>的文件。运行它，在预期的情况下限制将被解除。
* 若修补失败或修补后无效/遇到bug，请您将截图/复现录屏、详细的PCL CE和Patcher版本信息、您的运行环境信息、<code>%LocalAppData%\PCL CE Patcher </code> 目录下的 <code>latest.log</code> 一并通过 [issues](https://github.com/Octobersama/PCL-CE-Patcher/issues) 提交给我。我可能会对其进行修复。

### CUI 模式
您可以通过命令行终端使用本工具，使用前需提前打开GUI通过[上述验证](#使用方法)。或者...

命令格式：
<code>PCL_CE_Patcher.exe <input_file> [output_file]</code>

其中 <code>input_file</code> 是将修补文件的地址，<code>output_file</code> 是修补后输出的地址。
* <code>output_file</code> 可忽略，忽略时输出目录与 <code>input_file</code> 的目录相同，文件名为<code>原文件名</code> + <code>_Patched</code>。

## 许可证与致谢

本项目源代码遵循 **MIT License** 开源协议。详情请参阅 [LICENSE](LICENSE) 文件。

### 第三方组件声明

本项目在编译和运行过程中包含了以下第三方组件，请参阅根目录下的 [ThirdPartyNotices.txt](ThirdPartyNotices.txt) 获取完整的法律声明与协议文本。

*   **AsmResolver**  
    用于 .NET 程序集的读取、修改和写入。基于 MIT License 授权。  
    Copyright © 2016-2025 Washi

*   **Microsoft .NET Library (CleanHost.exe)**  
    位于 `Assets/` 目录下的 `CleanHost.exe` 是 Microsoft .NET SDK 的一部分。  
    该文件遵循 **Microsoft Software License Terms**，仅作为重打包模板使用，不属于本项目源码的 MIT 协议覆盖范围。  
    
    更多详情请参阅 [Assets/README.md](Assets/README.md)。

## Stars
[![Stargazers over time](https://starchart.cc/Octobersama/PCL-CE-Patcher.svg?variant=adaptive)](https://starchart.cc/Octobersama/PCL-CE-Patcher)