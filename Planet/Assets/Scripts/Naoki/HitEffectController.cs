using UnityEngine;
using UnityEngine.Pool;

public class HitEffectController : MonoBehaviour
{
    // 変える先のプールを保存する変数
    private IObjectPool<GameObject> originPool;

    // エフェクトを表示し続ける時間
    [SerializeField] private float activeDuration = 1.0f;
    private float timer;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > activeDuration)
        {
            ReturnToPool();
        }
    }

    // ShootingControllerからプールを教えてもらうための関数
    public void SetPool(IObjectPool<GameObject> pool)
    {
        originPool = pool;
    }

    private void OnEnable()
    {
        timer = 0;
        // Animatorを最初から再生させる処理
        if (animator != null)
        {
            animator.Play("hits-1-1 (1)");
        }
    }

    private void ReturnToPool()
    {
        // プールに自分を戻す
        originPool?.Release(this.gameObject);
    }
}