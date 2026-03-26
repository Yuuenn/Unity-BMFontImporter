# Unity BMFont Importer 简体中文版

- [English README](../README.md)
- [日本語](README.ja.md)

## 概述

`Unity BMFont Importer` 是一个面向旧版 Unity 工作流的小型工具项目，适合仍然使用位图版 `Unity Default Font` 字体、而不是 TextMeshPro 的场景。

这份文档也默认读者可能只是为了做本地化而临时接触 Unity，并不一定熟悉 Unity 开发。

这个项目主要面向以下工作：

1. 旧版 Unity 游戏的第三方本地化。
2. 旧版 Unity 游戏的授权本地化。
3. 在尽量非破坏性的前提下，以更低成本为旧项目引入更完整的 CJK 支持。

[src/BMFontImporter.cs](../src/BMFontImporter.cs) 会读取 AngelCode BMFont 的 `.fnt` 数据，并把字形度量写入 Unity 里创建好的 `Custom Font`，从而减少手动填写 `Character Rects` 这一栏的工作量。

## 仓库结构

```text
/
├─ README.md
├─ LICENSE
├─ .gitignore
├─ src/
│  └─ BMFontImporter.cs
├─ config/
│  └─ CustomFontConfig.bmfc
├─ characters/
│  └─ Characters_BOM.txt
└─ docs/
   ├─ README.ja.md
   └─ README.zh-CN.md
```

## 这个项目解决什么问题

- 旧版 Unity 项目可能使用的是位图版 `Unity Default Font` 字体，单纯替换 TTF 并不一定有效
- 大量 CJK 字符如果靠手工维护，成本很高
- 本地化项目通常希望尽量保留原始资源结构，同时扩展字符覆盖
- 授权本地化项目也常常需要一种更稳妥、成本更低的方式来增强 CJK 支持

## 仓库内文件说明

- [src/BMFontImporter.cs](../src/BMFontImporter.cs)：把 BMFont 数据导入 `Custom Font` 的 Unity 脚本
- [config/CustomFontConfig.bmfc](../config/CustomFontConfig.bmfc)：BMFont 导出配置的起始模板
- [characters/Characters_BOM.txt](../characters/Characters_BOM.txt)：供 BMFont 的 `Edit -> Select Chars From File` 使用的示例字符列表

## 关于 `Characters_BOM.txt`

[characters/Characters_BOM.txt](../characters/Characters_BOM.txt) 不是所谓“标准 CJK 字符全集”，而是一个更偏实务的起始模板。

它的设计目的包括：

- 提供带 BOM 的文本文件，方便 BMFont 稳定导入字符
- 除了常用字符，也包含一部分较少见的 CJK 字符和符号
- 更适合音乐游戏、节奏游戏，或其他喜欢使用奇怪字符串、装饰符号、非常规文本片段的作品

实际使用时，仍然应该根据游戏正文、UI 文本、翻译脚本和 QA 结果来增删。

## 需要准备的内容

1. 位图纹理文件，以及由 [Bitmap Font Generator](https://www.angelcode.com/products/bmfont/) 等工具生成的 `.fnt`
2. Unity Editor
3. [src/BMFontImporter.cs](../src/BMFontImporter.cs)

## 获得成品

可用于旧版 `Unity Default Font` 显示流程的 `Custom Font`

## 使用步骤

### 1. 创建项目并导入资产
1. 创建并打开一个 Unity 项目。
2. 将 [src/BMFontImporter.cs](../src/BMFontImporter.cs)、生成好的 `.fnt` 和纹理导入 `Assets`。
3. 在 `Assets` 中右键选择 **Create -> Custom Font** 创建 `Custom Font`。
4. 在 `Assets` 中右键选择 **Create -> Material** 创建材质。
5. 在 Hierarchy 中选择 **Create -> Create Empty** 创建空物体，并挂载 [src/BMFontImporter.cs](../src/BMFontImporter.cs)。
6. 在 Hierarchy 中选择 **Create -> 3D Object -> 3D Text** 创建 `3D Text` 对象。

### 2. 调整资产属性
1. **纹理设置**
   - 确认 **Read/Write Enabled** 已勾选
   - 确认 **Wrap Mode** 为 `Clamp` 或与原文件一致
   - 确认 **Filter Mode** 为 `Bilinear` 或与原文件一致
   - `Read/Write Enabled` 对字体替换和在 Unity 里重复利用纹理都很重要，建议保持开启
   - 修改后点击 **Apply**。
2. **材质设置**
   - 将 **Shader** 设为 `Unlit/Transparent`
   - 绑定纹理。
3. **字体设置**
   - 设置 **Line Spacing**
   - 绑定材质。
4. **文本对象设置**
   - 设置 `Rect Transform` 高度
   - 输入测试字符串
   - 给 Font 字段指定刚才创建的 `Custom Font`。
5. **BMFontImporter 设置**
   - 指定 **Custom Font**
   - 指定 **Font Texture**
   - 指定 **Fnt File**。

### 3. 应用脚本
1. 按 `Ctrl+P` 或点击播放按钮运行场景。
2. 如果测试字符串以新字体显示，说明导入成功。

如果你还要替换已发布游戏中的字体，请继续阅读下面的部分。

## 替换已经发布的游戏中的字体

这一部分适用于：

1. 目标字体是位图版 `Unity Default Font` 字体
2. 仅替换 TTF 不能解决问题
3. 正在进行第三方本地化或授权本地化
4. 希望以相对非破坏性、成本较低的方式引入更完整的 CJK 支持

## 需要准备的内容

1. 想导入游戏中的 TrueType 字体
2. [AssetStudio](https://github.com/Perfare/AssetStudio/) 等工具，用于确认原纹理的 `Format`、`Filter Mode`、`Wrap Mode`、`Channels`
3. 原纹理配色类型的确认
4. [UABEAvalonia](https://github.com/nesrak1/UABEA) 等工具，用于导出原始 `Font` 和 `Texture` 文本 dump
5. [Bitmap Font Generator](https://www.angelcode.com/products/bmfont/)
6. [config/CustomFontConfig.bmfc](../config/CustomFontConfig.bmfc)
7. 如有需要，可准备 [characters/Characters_BOM.txt](../characters/Characters_BOM.txt) 这样的 BOM 编码字符表

## 步骤

1. **安装字体文件**
   把目标字体安装到系统中。
2. **加载配置**
   在 BMFont 中通过 **Options -> Load Configuration** 载入 [config/CustomFontConfig.bmfc](../config/CustomFontConfig.bmfc)。
3. **设置字体参数**
   检查 `Font`、`Size (px)`、`Match char height`、`Super sampling` 等参数。
4. **设置导出选项**
   调整 `Padding`、纹理尺寸、`Bit depth`、通道设定、`File/Font` 和压缩方式。
   - `Padding` 通常可以先从 `2` 开始，如果边缘出现杂色再逐步增大
   - 旧版 Unity 对大纹理往往有限制，所以必要时要同时调整纹理尺寸和字符数量
5. **可选：只导入部分字符**
   通过 **Edit -> Select Chars From File** 导入 [characters/Characters_BOM.txt](../characters/Characters_BOM.txt) 或你自己的字符表。
6. **预览字体**
   用 **Options -> Visualize** 预览输出。
   - 如果预览标题显示 `Preview: 1/1`，说明所有字符都放进了单张纹理
   - 如果生成了多张纹理，通常就需要增大纹理尺寸，或者减少字符数量
7. **保存字体**
   保存纹理文件和 `.fnt`。
8. **调整基线**
   手动修改 `.fnt` 中的 `base=`。
   - 一个很实用的起始经验值，是先试 `lineHeight` 的大约 `20%`
9. 准备好这些文件后，再回到上面的正常导入步骤。

## 测试字体、构建临时游戏并导出字体

### 获得成品

从临时构建中导出的 `Font` 与 `Texture2D` 文本 dump

### 步骤

#### 1. 检查文字位置
1. 在 Hierarchy 面板中选中已创建的 `3D Text` 对象，检查测试文本与 Text 组件的相对位置是否正确。它应位于灰色 `Rect Transform` 边界框内，并且与上下边界的距离尽量均等。
2. 如果偏高就减小 `base=`，如果偏低就增大 `base=`。
3. 如果想调得更准，可以放大 Scene 视图，按可见偏移量一点点修正。

#### 2. 保存并构建临时游戏
1. 使用 `Ctrl+S` 或 **File -> Save Project** 保存。
2. 打开 **File -> Build Settings**。
3. 点击 **Add Open Scenes**。
4. 点击 **Build**。
5. 构建前最好确认当前修改已经都保存进项目里。

#### 3. 导出创建出的字体
1. 打开 `GameName_Data`。
2. 打开 `resources.assets`。
3. 通过 Export Dump 导出新的 `Font` 和 `Texture2D`。
4. 注意不要把它们和原游戏已有的字体文件弄混。
