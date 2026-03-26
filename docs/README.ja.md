# Unity BMFont Importer 日本語版

- [English README](../README.md)
- [简体中文](README.zh-CN.md)

## 概要
BMFontImporter.cs は、AngelCode BMFont 形式の `.fnt` ファイルからカスタムビットマップフォントを Unity に取り込むためのスクリプトです。このスクリプトは、CJK フォントのように文字数が非常に多いフォントの作成に特に向いており、`Unity Default Font` システムの拡張手段として、あらかじめ生成したビットマップフォントデータを利用し、動的フォントが大量文字処理で抱えやすい性能面・メモリ面の負担を軽減します。Unity 5.6.4f1 では、外部の BMFont データを解析し、各文字の UV 座標、オフセット、基線補正を計算し、その結果を `Custom Font` の `Character Rects` フィールドへ反映することで、手作業による設定量を大きく減らせます。

この文書は、Unity をローカライズ作業のために使うものの、Unity 開発には必ずしも慣れていない読者も想定しています。

> **公開済みゲームのフォントを置き換える予定がある場合、たとえば第三者ローカライズや正規ライセンス下でのローカライズ作業を行う場合は、下の「既にリリース済みゲームのフォント置換」を先に確認してください。**

## このプロジェクトが主に想定している作業

1. 旧版 Unity ゲームに対する第三者ローカライズ。
2. 旧版 Unity ゲームに対する正規ライセンス下でのローカライズ。
3. 元の資産構造をできるだけ壊さず、比較的低コストでより広い CJK 対応を導入したいケース。

## このプロジェクトが解決する問題

- 旧版 Unity ではビットマップ形式の `Unity Default Font` が使われていることがあり、TTF の差し替えだけでは足りない。
- CJK のような大規模文字集合を手作業で管理するのは大変。
- ローカライズ時に、元の資産構造をできるだけ維持したまま文字対応を広げたい。
- 正規ローカライズ案件でも、非破壊的に近い形で CJK 対応を増やしたい。

## リポジトリ構成

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

## 同梱ファイル

- [src/BMFontImporter.cs](../src/BMFontImporter.cs): BMFont データを Unity の `Custom Font` に流し込むスクリプト
- [config/CustomFontConfig.bmfc](../config/CustomFontConfig.bmfc): BMFont 用の設定ファイル
- [characters/Characters_BOM.txt](../characters/Characters_BOM.txt): BMFont の `Edit -> Select Chars From File` で使える文字列リスト例

## `Characters_BOM.txt` について

[characters/Characters_BOM.txt](../characters/Characters_BOM.txt) は CJK の完全な標準文字集合ではなく、実務向けのたたき台です。

主な意図は次の通りです。

- BOM 付きテキストとして BMFont に安定して読み込ませやすくする
- 常用文字だけでなく、やや特殊な CJK 文字や記号もある程度含める
- 音楽ゲームやリズムゲームなど、特殊な文字列や装飾記号を使いやすい作品にも対応しやすくする

実際には、対象ゲーム、UI、翻訳テキスト、QA 結果に合わせて調整してください。

## 必要な準備物
1. ビットマップテクスチャファイル。通常は `.dds` または `.png`。加えて、[Bitmap Font Generator](https://www.angelcode.com/products/bmfont/) などで生成した AngelCode BMFont の字体インデックスファイル。
2. Unity Editor。
3. 本リポジトリの [src/BMFontImporter.cs](../src/BMFontImporter.cs)。

## 成果物
Unity Editor 上で利用できる、カスタム `Unity Default Font` ビットマップフォント。

## 使用手順

### 1. プロジェクトの作成とアセットのインポート
1. Unity プロジェクトを新規作成して開く。
2. [src/BMFontImporter.cs](../src/BMFontImporter.cs)、生成済みの `.fnt` ファイル、および対応するテクスチャファイルを `Assets` フォルダにインポートする。
3. `Assets` フォルダで右クリックし、**Create → Custom Font** を選択して新しい `Custom Font` を作成し、名前を付ける。
4. `Assets` フォルダで右クリックし、**Create → Material** を選択して新しい Material を作成する。
5. Hierarchy パネルで **Create → Create Empty** を選び、空の GameObject を作成する。推奨名は `FontManager`。その後、Inspector で **Add Component** またはドラッグ＆ドロップにより [src/BMFontImporter.cs](../src/BMFontImporter.cs) を追加する。
6. Hierarchy パネルで **Create → 3D Object → 3D Text** を選び、`3D Text` オブジェクトを作成する。

### 2. アセットの設定調整
1. **テクスチャ設定**  
   - `Assets` フォルダでテクスチャを選択し、Inspector で以下を確認する  
   - **Read/Write Enabled** → 有効にする。フォント置換や Unity 内での再利用に重要  
   - **Wrap Mode** → `Clamp`、または元ファイルと同じ設定にする  
   - **Filter Mode** → `Bilinear`、または元ファイルと同じ設定にする  
   - 設定後に **Apply** を押す。
2. **マテリアル設定**  
   - 作成した Material を選び、Inspector で以下を設定する  
   - **Shader** → `Unlit/Transparent` を選ぶ  
   - テクスチャを Material のテクスチャ欄へ割り当てる。ここでいう割り当てとは、通常は現在 `None (Texture)` と表示されている枠へドラッグ＆ドロップすることを指します。
3. **フォント設定**  
   - 作成した `Custom Font` を選び、Inspector で以下を設定する  
   - **Line Spacing** → 使いたいフォントサイズ、または元ファイルに合わせて設定する  
   - Material をフォントへ割り当てる。
4. **Text オブジェクト設定**  
   - Hierarchy で作成した `3D Text` オブジェクトを選び、Inspector で以下を確認する  
   - `Rect Transform` の Height を目的のフォントサイズに合わせ、文字位置確認に使う  
   - Text コンポーネントのテキスト欄にテスト文字列を入力する。CJK を含めると確認しやすい  
   - `Custom Font` を Text コンポーネントの Font フィールドへ割り当てる。
5. **BMFontImporter スクリプト設定**  
   - Hierarchy で `FontManager` を選び、Inspector で以下を設定する  
   - `Custom Font` を **Custom Font** へ割り当てる  
   - テクスチャを **Font Texture** へ割り当てる  
   - `.fnt` ファイルを **Fnt File** へ割り当てる。

### 3. スクリプトの適用
1. `Ctrl+P` または再生ボタンでシーンを実行する。スクリプトが `.fnt` を解析し、文字データを `Custom Font` の `Character Rects` に反映する。
2. Scene ビューでテスト文字列が新しいフォントで表示されれば、インポート成功。

## 既にリリース済みゲームのフォント置換

> この部分は以下のようなケース向けです。  
> 1. 置換対象のフォントが、ビットマップ形式の `Unity Default Font` である。  
> 2. TrueType フォントの差し替えだけでは無効、あるいは TrueType 自体が存在しない。または AssetStudio 上ではテクスチャ付きのフォントに見えるが、TextMeshPro の特徴、たとえば対応する MonoBehaviour が見られない。こうした場合には本方式が向いていることがあります。  
> 3. このような状況では、本プロジェクトの方法で必要なフォントを生成し、対応ファイルを作成して公開済みゲームの資源を置き換えます。  
> 4. 基本的な流れは、元フォントの情報を抽出し、機能や属性が近いフォントを選んだ上で、一時的な Unity プロジェクトに取り込んで処理し、その結果を元ゲームへ戻す、というものです。

### 必要な準備物
1. ゲームへ導入したい TrueType フォントファイル。
2. [AssetStudio](https://github.com/Perfare/AssetStudio/) または更新系の分岐、たとえば [zhangjiequan/AssetStudio](https://github.com/zhangjiequan/AssetStudio/) を用いて、元フォントに対応する Texture2D を確認し、次の属性を記録する：
   - `Format`
   - `Filter Mode`
   - `Wrap Mode`
   - `Channels`
3. 元フォントテクスチャの配色タイプを確認する：
   - 白字＋不透明黒背景
   - 黒字＋不透明白背景
   - 白字＋透明背景
   - 黒字＋透明背景
4. [UABEAvalonia](https://github.com/nesrak1/UABEA) などを使い、元フォントの `Font` と `Texture` のテキストファイルを Export Dump 形式で出力する。複数フォントを扱う場合は、フォント名ごとにフォルダ分けすると分かりやすい。
5. [Bitmap Font Generator](https://www.angelcode.com/products/bmfont/) を用意する。
6. リポジトリ内の [config/CustomFontConfig.bmfc](../config/CustomFontConfig.bmfc) を用意する。
7. 必要に応じて、TrueType フォントの一部の文字だけを使うためのテキストファイルを用意する。UTF-8 BOM または UTF-16 BOM が必要。出発点として [characters/Characters_BOM.txt](../characters/Characters_BOM.txt) も利用できる。

### 手順
1. **フォントファイルのインストール**  
   対象の TrueType フォントをシステムへインストールする。複数ウェイトがある場合は、不要なものを一時的に外し、必要なウェイトだけを残しておくと BMFont が正しく認識しやすい。
2. **設定の読み込み**  
   Bitmap Font Generator を開き、**Options → Load Configuration** を順に選択し、[config/CustomFontConfig.bmfc](../config/CustomFontConfig.bmfc) を読み込む。
3. **フォントパラメータの設定**  
   **Options → Font Settings** を開き、次を確認または調整する：
   - **Font** → 対象フォントに変更する  
   - **Size (px)** → 対象サイズに変更する  
   - **Match char height** → 推奨は有効  
   - **Super sampling** → 有効のまま、level を `4` にする  
   - その他は必要に応じて調整し、**OK** を押して保存する。
4. **エクスポートオプションの設定**  
   **Options → Export Options** を開き、次を確認する：
   - `Padding` は `2` を基本とし、縁にノイズが出る場合のみ増やす  
   - **Width** と **Height** は必要に応じて調整する。古い Unity では `8192×8192` 付近が実用上の上限になることが多い  
   - **Bit depth** は通常 `32`  
   - 元ファイルに合わせて Presets のチャンネル設定を選ぶ：  
     - 白字＋透明背景 → `White text with alpha`  
     - 黒字＋透明背景 → `Black text with alpha`  
     - 白字＋不透明黒背景 → `White text on black (no alpha)`  
     - 黒字＋不透明白背景 → `Black text on white (no alpha)`  
   - **File/Font** は `Text` 形式  
   - 元ファイルの `Format` に合わせ、Textures と Compression を選ぶ。たとえば `DXT1` なら `dds` と `DXT1` を選ぶ。設定後に **OK** を押す。
5. **必要に応じて部分文字を導入**  
   部分文字だけを生成したい場合は、**Edit → Select Chars From File** から対象文字を含むテキストファイルを読み込む。UTF-8 BOM または UTF-16 BOM を使う。
6. **フォントのプレビュー**  
   左側の Unicode ビューと右側の選択パネルを使ってフォント状態を確認し、問題なければ **Options → Visualize** でテクスチャをプレビューする。  
   > プレビュータイトルが `Preview: 1/1` なら、全文字が 1 枚のテクスチャに収まっていることを意味します。複数枚になる場合は、テクスチャサイズを上げるか、含める文字数を減らす必要があります。
7. **生成したフォントの保存**  
   プレビューを閉じて **Options → Save bitmap font as...** を押し、テクスチャファイルと `.fnt` を保存する。
8. **基線補正**  
   表示を正しくするため、生成した `.fnt` をテキストエディタで開き、`base=` の値を整数に修正して保存する。  
   > 経験上、適切な `base` 値は `lineHeight` の約 `0.2` 倍、またはそれに近い整数から調整し始めると合わせやすいです。
9. ここまで終わると、必要なフォントテクスチャと `.fnt` 設定ファイルが揃います。上の「必要な準備物」を確認しながら次工程へ進んでください。

## テストフォント、暫定ゲームの構築とフォントのエクスポート

### 成果物
Export Dump により出力した、新しいフォントの `Font` と `Texture` テキストファイル。

### 1. Text 内の文字位置確認
1. Hierarchy で作成済みの `3D Text` オブジェクトを選択し、テスト文字列が Text コンポーネントに対して正しい位置にあるか確認する。灰色の `Rect Transform` 内に収まり、上下の境界までの距離ができるだけ均等なのが目安です。
2. 文字全体が高すぎる場合は `.fnt` の `base=` を小さくし、低すぎる場合は大きくする。
3. より正確に合わせたい場合は、Scene ビューを拡大し、必要なオフセット量を見ながら少しずつ調整する。

### 2. プロジェクトの保存と暫定ゲームのビルド
1. `Ctrl+S` または **File → Save Project** でプロジェクトを保存し、変更が反映されていることを確認する。
2. **File → Build Settings** を開き、対象プラットフォームを選ぶ。たとえば `PC, Mac & Linux Standalone`。
3. **Add Open Scenes** を押し、現在のシーンをビルド対象に追加する。
4. **Build** を押し、保存先を指定して暫定ゲームをビルドする。

### 3. 作成したフォントのエクスポート
1. ビルド後の `GameName_Data` フォルダを開く。
2. UABEAvalonia などのツールで `resources.assets` を開く。
3. 新しく作成したフォントの `Font` と `Texture` テキストファイルを Export Dump 方式で書き出す。元のフォントと取り違えないよう注意する。
