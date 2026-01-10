# 概要
各パラメータ設定項目について説明

## ScriptableObjectsフォルダ配下のアセット

プロジェクト内に配置されている実際のScriptableObjectアセットファイル一覧：

### ルート
- `GameConfig.asset` - ゲーム全体の設定

### Audio/
- `Sound Database.asset` - サウンドデータベース

### Bullet/
- `NormalBulletConfig.asset` - 通常弾の設定
- `BurstBulletConfig.asset` - バースト弾の設定

### Enemy/
- `Normal Enemy Config.asset` - 通常敵の設定
- `Hopper Enemy Config.asset` - ホッパー敵の設定
- `Strong Enemy Config.asset` - 強敵の設定
- `Enemy Spawner Config.asset` - 敵スポーナーの設定

### Gauge/Bullet/
- `BulletGaugeConfig.asset` - 弾ゲージの設定
- `BulletGaugeCostConfig.asset` - 弾ゲージコストの設定

### Gauge/EnemyProgress/
- `Enemy Progress Gauge Config.asset` - 敵進行度ゲージの設定

### Gauge/Ghost/
- `Ghost Gauge Config.asset` - Ghostゲージの設定

### Ghost/
- `Ghost Cost Config.asset` - Ghostコストの設定

### Player/
- `Player Config.asset` - プレイヤーの設定

---

## 各アセットファイルの設定項目

### GameConfig
ゲーム全体の基本設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `playerSpeed` | float | プレイヤーの標準速度（1秒ごとに進む距離） |
| `playerSpeedAcceleration` | float | Ghost解除後のプレイヤー移動速度の加速度（1秒あたり） |
| `distanceToGoal` | float | プレイヤーとゴールの初期距離 |
| `goalPrefab` | GameObject | ゴールのプレハブ |
| `enemyDefeatBonusProgress` | float | Enemyを4体以上同時に倒したときのボーナス進行度（Max1） |
| `strongEnemyDefeatBonusProgress` | float | Strong Enemyを倒したときのボーナス進行度（Max1） |

---

### Player Config
プレイヤーの基本設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `maxHP` | int | プレイヤーの最大体力 |
| `invincibilityDuration` | float | ダメージを食らった後の無敵時間（秒） |
| `blinkInterval` | float | ダメージを食らった後の無敵時間の点滅間隔（秒） |

---

### NormalBulletConfig
通常弾の設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `speed` | float | 弾のスピード（1秒あたり） |
| `bulletType` | BulletType | 弾の種類（Normal） |
| `damageAmount` | int | 敵に当たった時に与えるダメージ量 |
| `isPassThrough` | bool | trueなら貫通弾 |

---

### BurstBulletConfig
バースト弾の設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `speed` | float | 弾のスピード（1秒あたり） |
| `bulletType` | BulletType | 弾の種類（Burst） |
| `damageAmount` | int | 敵に当たった時に与えるダメージ量 |
| `isPassThrough` | bool | trueなら貫通弾 |

---

### BulletGaugeConfig
弾ゲージの設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `max` | float | ゲージの最大値（1 = 100%） |
| `regenPerSec` | float | 1秒あたりの回復量（Max1） |

---

### BulletGaugeCostConfig
弾のゲージ消費量設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `normalBulletCost` | float | 通常弾のゲージ消費量（Max1） |
| `burstBulletCost` | float | バースト弾のゲージ消費量（Max1） |

---

### Ghost Gauge Config
Ghostゲージの設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `max` | float | ゲージの最大値（1 = 100%） |
| `regenPerSec` | float | 1秒あたりの回復量（Max1） |

---

### Ghost Cost Config
Ghost状態のコスト設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `ghostCost` | float | Ghost状態のゲージ消費量（1秒あたり、Max1） |
| `activationTime` | float | Ghost状態に入るためのキー長押し時間（秒） |

---

### Enemy Progress Gauge Config
敵進行度ゲージの設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `max` | float | ゲージの最大値（1 = 100%） |
| `regenPerSec` | float | 1秒あたりの回復量（Max1） |

---

### Normal Enemy Config
通常敵の設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `horizontalSpeed` | float | 横方向の移動速度（1秒あたり） |
| `hp` | int | 敵のmax体力 |
| `enemyType` | EnemyType | 敵のタイプ |

---

### Hopper Enemy Config
ホッパー敵（上下移動する敵）の設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `horizontalSpeed` | float | 横方向の移動速度（1秒あたり） |
| `hp` | int | 敵のmax体力 |
| `enemyType` | EnemyType | 敵のタイプ |
| `verticalSpeed` | float | 縦方向の移動速度（1秒あたり） |
| `switchInterval` | float | 上移動⇔下移動が切り替わる間隔（秒） |

---

### Strong Enemy Config
強敵の設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `horizontalSpeed` | float | 横方向の移動速度（1秒あたり） |
| `hp` | int | 敵のmax体力 |
| `enemyType` | EnemyType | 敵のタイプ |

---

### Enemy Spawner Config
敵のスポーンシステム設定

**enemyInfos（敵情報リスト）**

各要素の設定項目：

| フィールド名 | 型 | 説明 |
|------------|-----|------|
| `enemyType` | EnemyType | 敵のタイプ |
| `enemyPrefab` | GameObject | 敵のPrefab |

**enemyWaveInfos（Wave情報リスト）**

各要素の設定項目：

| フィールド名 | 型 | 説明 |
|------------|-----|------|
| `waveNumber` | int | Wave番号 |
| `spawnStartDistance` | float | Wave開始位置（プレイヤーの移動距離） |
| `spawnInterval` | float | 敵のスポーン間隔（距離） |
| `spawnInfos` | List | スポーンする敵の情報リスト（下記参照） |

**spawnInfos（各Waveのスポーン情報）**

各要素の設定項目：

| フィールド名 | 型 | 説明 |
|------------|-----|------|
| `enemyType` | EnemyType | 敵のタイプ |
| `spawnProbabilityMin` | float | 敵のスポーン確率（最低）Max: 100 |
| `spawnProbabilityMax` | float | 敵のスポーン確率（最高）Max: 100 |

---

### Sound Database
オーディオシステムのデータベース設定

| パラメータ名 | 型 | 説明 |
|-------------|-----|------|
| `sounds` | List\<SoundData\> | サウンドデータのリスト |
| `audioSourcePoolSize` | int | オーディオソースプールのサイズ |
