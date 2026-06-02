using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AfterImageWorldCanvas : MonoBehaviour
{
    [Header("残像の発生設定")]
    [SerializeField] private float timeBetweenImages = 0.1f; // 残像を出す間隔（秒）

    [Tooltip("残像にさらに重ねる色（白にするとRainbowSpriteの綺麗な虹色のまま残ります）")]
    [SerializeField] private Color multiplyColor = Color.white;

    [Tooltip("残像生成時の初期の透明度 (0が透明、1がくっきり)")]
    [Range(0f, 1f)]
    [SerializeField] private float initialAlpha = 0.6f;

    [Header("残像の消滅設定")]
    [SerializeField] private float alphaDecay = 2f; // 消えるスピード

    private SpriteRenderer playerSR;
    private float timeStamp;

    [Header("コントロール")]
    public bool isDashing = false; // これがtrueの間だけ残像が出る

    void Start()
    {
        playerSR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 【テスト用】Spaceキーを押している間だけ残像をON
        if (Input.GetKeyDown(KeyCode.Space)) isDashing = true;
        if (Input.GetKeyUp(KeyCode.Space)) isDashing = false;

        if (isDashing && Time.time >= timeStamp)
        {
            SpawnAfterImage();
            timeStamp = Time.time + timeBetweenImages;
        }
    }

    void SpawnAfterImage()
    {
        // 1. 【最重要】親を指定せずにオブジェクトを作成（＝世界のトップ階層に生まれる）
        GameObject afterImageObj = new GameObject("AfterImage_Isolated_Instance");

        // 2. 勝手に動く親の影響を 100% 完全に遮断するため、親を明示的に null（無し）にする
        afterImageObj.transform.parent = null;

        // 3. このアイテムが「今まさに画面上で見えている絶対的な世界座標・回転・サイズ」をそのまま固定
        afterImageObj.transform.position = this.transform.position;
        afterImageObj.transform.rotation = this.transform.rotation;

        // 親（空オブジェクトやCanvasScaler）の拡大率をすべて計算に含んだ「最終的な見た目のサイズ」を固定
        afterImageObj.transform.localScale = this.transform.lossyScale;

        // 4. 見た目のコピー
        SpriteRenderer targetSR = afterImageObj.AddComponent<SpriteRenderer>();
        targetSR.sprite = playerSR.sprite;

        // 反転状態の同期
        targetSR.flipX = playerSR.flipX;
        targetSR.flipY = playerSR.flipY;

        // 描画順（Sorting Layer）を設定。アイテム本体の一歩後ろに描画
        targetSR.sortingLayerID = playerSR.sortingLayerID;
        targetSR.sortingOrder = playerSR.sortingOrder - 1;

        // RainbowSpriteが変更した「その瞬間の虹色」を取得して適用
        Color currentRainbowColor = playerSR.color;
        Color finalColor = currentRainbowColor * multiplyColor;
        finalColor.a = initialAlpha;
        targetSR.color = finalColor;

        // 5. 消滅スクリプトの追加
        AfterImageWorldInstance instanceScript = afterImageObj.AddComponent<AfterImageWorldInstance>();
        instanceScript.Setup(alphaDecay, finalColor);
    }
}

// --- 残像オブジェクト自体を消滅させるためのミニクラス ---
public class AfterImageWorldInstance : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color color;
    private float decaySpeed;
    private float alpha;

    public void Setup(float speed, Color baseColor)
    {
        sr = GetComponent<SpriteRenderer>();
        decaySpeed = speed;
        color = baseColor;
        alpha = baseColor.a;
    }

    void Update()
    {
        alpha -= decaySpeed * Time.deltaTime;
        color.a = alpha;
        sr.color = color;

        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}