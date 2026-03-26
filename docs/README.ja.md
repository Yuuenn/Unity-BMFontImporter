# Unity BMFont Importer 日本語版

- [English README](../README.md)
- [简体中文](README.zh-CN.md)

## 概要

`Unity BMFont Importer` は、TextMeshPro 以前のワークフローや、ビットマップベースの `Unity Default Font` フォントを使う旧世代 Unity プロジェクト向けの小規模ユーティリティです。

この文書は、Unity 開発経験が十分でないローカライズ作業者も読者として想定しています。

このプロジェクトが主に想定しているのは、次のような作業です。

1. 旧版 Unity ゲームに対する第三者ローカライズ。
2. 旧版 Unity ゲームに対する正規ライセンス下でのローカライズ。
3. 元のアセット構造を大きく壊さず、比較的低コストでより広い CJK 対応を導入したいケース。

[src/BMFontImporter.cs](../src/BMFontImporter.cs) は AngelCode BMFont の `.fnt` を読み取り、Unity で作成した `Custom Font` へ文字メトリクスを書き込むことで、`Character Rects` 欄の手入力作業を減らします。

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

## このプロジェクトが解決する問題

- 旧版 Unity ではビットマップベースの `Unity Default Font` フォントが使われていることがあり、TTF の差し替えだけでは足りない
- CJK のような大規模文字集合を手作業で管理するのは大変
- ローカライズ時に、元のアセット構造をできるだけ維持したまま文字対応を広げたい
- 正規ローカライズ案件でも、非破壊的に近い形で CJK 対応を増やしたい

## 同梱ファイル

- [src/BMFontImporter.cs](../src/BMFontImporter.cs): BMFont 情報を `Custom Font` へ流し込む Unity スクリプト
- [config/CustomFontConfig.bmfc](../config/CustomFontConfig.bmfc): BMFont 用の出力設定の出発点
- [characters/Characters_BOM.txt](../characters/Characters_BOM.txt): BMFont の `Edit -> Select Chars From File` 用サンプル文字リスト

## `Characters_BOM.txt` について

[characters/Characters_BOM.txt](../characters/Characters_BOM.txt) は、CJK の完全標準セットではなく、実務用のたたき台です。

意図は次の通りです。

- BMFont のファイル読み込みで扱いやすい BOM 付き文字リストを用意する
- 常用文字だけでなく、少し特殊な CJK 文字や記号もある程度含める
- 音楽ゲームやリズムゲームのように、独特な文字列や装飾記号を使いがちな作品にも合わせやすくする

実際には、対象ゲーム本編、UI、翻訳テキスト、QA の結果に合わせて調整してください。

## 必要な準備物

1. ビットマップテクスチャファイルと、[Bitmap Font Generator](https://www.angelcode.com/products/bmfont/) などで生成した `.fnt`
2. Unity Editor
3. [src/BMFontImporter.cs](../src/BMFontImporter.cs)

## 成果物

旧版 Unity の `Unity Default Font` 表示で使える `Custom Font`

## 使用手順

### 1. プロジェクトの作成とアセットのインポート
1. Unity で新規プロジェクトを作成して開く。
2. [src/BMFontImporter.cs](../src/BMFontImporter.cs)、生成済みの `.fnt`、対応するテクスチャを `Assets` にインポートする。
3. `Assets` で **Create -> Custom Font** を選んで `Custom Font` を作成する。
4. `Assets` で **Create -> Material** を選んでマテリアルを作成する。
5. Hierarchy で **Create -> Create Empty** を選び、空の GameObject を作成して [src/BMFontImporter.cs](../src/BMFontImporter.cs) を追加する。
6. Hierarchy で **Create -> 3D Object -> 3D Text** を選び、`3D Text` オブジェクトを作成する。

### 2. アセットの設定調整
1. **テクスチャ設定**
   - `Assets` でテクスチャを選び、Inspector で以下を確認する
   - **Read/Write Enabled** を有効
   - **Wrap Mode** を `Clamp` または元ファイルに合わせる
   - **Filter Mode** を `Bilinear` または元ファイルに合わせる
   - `Read/Write Enabled` は、フォント置換や Unity 内での再利用時に重要なので、有効のままにしておく
   - 調整後に **Apply** を押す。
2. **マテリアル設定**
   - マテリアルを選択し、**Shader** を `Unlit/Transparent` にする
   - テクスチャを割り当てる。
3. **フォント設定**
   - 作成した `Custom Font` を選択し、**Line Spacing** を使用サイズに合わせる
   - マテリアルを割り当てる。
4. **Text オブジェクト設定**
   - `Rect Transform` の Height を目的サイズに合わせる
   - テスト文字列を入力する
   - 作成した `Custom Font` を Font フィールドへ割り当てる。
5. **BMFontImporter の設定**
   - **Custom Font**
   - **Font Texture**
   - **Fnt File**
   をそれぞれ割り当てる。

### 3. スクリプトの適用
1. `Ctrl+P` または再生ボタンでシーンを実行する。
2. テスト文字列が新フォントで表示されれば成功。

リリース済みゲームの置換を行う場合は、以下も参照してください。

## 既にリリース済みゲームのフォント置換

この節は次のようなケース向けです。

1. 置換対象がビットマップベースの `Unity Default Font` フォントである
2. TTF 差し替えだけでは解決しない
3. 第三者ローカライズまたは正規ライセンス下でのローカライズを行う
4. 元資産を大きく壊さずに、より広い CJK 対応を導入したい

## 必要な準備物

1. 導入したい TrueType フォント
2. [AssetStudio](https://github.com/Perfare/AssetStudio/) などで元テクスチャの `Format`、`Filter Mode`、`Wrap Mode`、`Channels` を確認する環境
3. 元テクスチャの配色タイプの確認
4. [UABEAvalonia](https://github.com/nesrak1/UABEA) などで `Font` と `Texture` の dump を取得できる環境
5. [Bitmap Font Generator](https://www.angelcode.com/products/bmfont/)
6. [config/CustomFontConfig.bmfc](../config/CustomFontConfig.bmfc)
7. 必要に応じて [characters/Characters_BOM.txt](../characters/Characters_BOM.txt) などの BOM 付き文字リスト

## 手順

1. **フォントファイルのインストール**
   対象フォントをシステムへインストールする。
2. **設定の読み込み**
   BMFont で **Options -> Load Configuration** を選び、[config/CustomFontConfig.bmfc](../config/CustomFontConfig.bmfc) を読み込む。
3. **フォントパラメータの設定**
   `Font`、`Size (px)`、`Match char height`、`Super sampling` などを確認する。
4. **エクスポートオプションの設定**
   `Padding`、テクスチャサイズ、`Bit depth`、チャンネル設定、`File/Font`、圧縮方式を元アセットに合わせて調整する。
   - `Padding` は基本的に `2` を出発点にし、縁にノイズが出る場合のみ増やす
   - 古い Unity では大きなテクスチャに制限があるため、必要に応じてサイズと文字数を両方見直す
5. **必要に応じて部分文字のみを導入**
   **Edit -> Select Chars From File** から [characters/Characters_BOM.txt](../characters/Characters_BOM.txt) などを読み込む。
6. **フォントのプレビュー**
   **Options -> Visualize** で確認する。
   - プレビューのタイトルが `Preview: 1/1` なら、全文字が単一テクスチャに収まっている
   - 複数テクスチャになる場合は、テクスチャサイズを上げるか、含める文字数を減らす必要がある
7. **フォントの保存**
   テクスチャと `.fnt` を保存する。
8. **基線補正**
   `.fnt` の `base=` を手動調整する。
   - 目安としては、`lineHeight` のおよそ `20%` 前後から試すと合わせやすい
9. 必要ファイルが揃ったら、上の通常手順に戻って Unity 側へ取り込む。

## テストフォント、暫定ゲームの構築とフォントのエクスポート

### 成果物

暫定ビルドから Export Dump した `Font` と `Texture2D` テキストファイル

### 手順

#### 1. Text 内の文字位置確認
1. Hierarchy で作成した `3D Text` オブジェクトを選択し、テスト文字列が Text コンポーネントに対して正しい位置にあるか確認する。灰色の `Rect Transform` 内に収まり、上下の境界までの距離ができるだけ均等になっているのが目安です。
2. 高すぎる場合は `base=` を下げ、低すぎる場合は上げる。
3. より正確に詰めたい場合は、Scene ビューを拡大して見た目のずれ量を測りながら調整する。

#### 2. プロジェクト保存と暫定ゲームのビルド
1. `Ctrl+S` または **File -> Save Project** で保存する。
2. **File -> Build Settings** を開く。
3. **Add Open Scenes** を押す。
4. **Build** を実行する。
5. ビルド前に保存されていない変更が残っていないかを確認しておく。

#### 3. 作成したフォントのエクスポート
1. `GameName_Data` を開く。
2. `resources.assets` を開く。
3. `Font` と `Texture2D` を Export Dump で書き出す。
4. 既存の元フォントと新しく作ったフォントを取り違えないよう注意する。
