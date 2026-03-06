using UnityEngine;

// 手游版勇者移动脚本（适配触屏+竖屏，保留PC测试逻辑）
public class PlayerMove : MonoBehaviour
{
    // 移动速度（手游建议8-10，数值越小越稳，避免手机操作太灵敏）
    public float moveSpeed = 8f;
    // 2D刚体组件（控制勇者物理移动，手游端物理逻辑和PC一致）
    private Rigidbody2D rb;

    // 游戏启动时执行一次（初始化组件，避免重复获取）
    void Awake()
    {
        // 获取勇者身上挂载的Rigidbody2D组件
        rb = GetComponent<Rigidbody2D>();
    }

    // 每帧执行（处理移动逻辑，手游触屏检测核心）
    void Update()
    {
        // 1. PC端测试逻辑（保留！方便你在电脑上模拟手机操作，不用装到手机测试）
        float horizontal = Input.GetAxis("Horizontal"); // 键盘左右（A/D/方向键）
        float vertical = Input.GetAxis("Vertical");     // 键盘上下（W/S/方向键）
        Vector2 moveDir = new Vector2(horizontal, vertical).normalized;

        // 2. 手游触屏核心逻辑（★★适配手机★★）
        // 检测是否有手指触碰屏幕（支持单指操作，符合手游习惯）
        if (Input.touchCount > 0)
        {
            // 获取第一个触碰屏幕的手指（手游单指操作足够）
            Touch touch = Input.GetTouch(0);
            // 关键：把手机屏幕坐标转换成游戏世界坐标（避免位置偏移）
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            // 计算勇者到触屏位置的移动方向（normalized避免斜向移动更快）
            moveDir = (touchPos - rb.position).normalized;
        }

        // 给勇者设置移动速度（物理移动，手游端更流畅）
        rb.velocity = moveDir * moveSpeed;

        // 3. 手游竖屏边界限制（★★适配手机竖屏1080x1920★★）
        // 避免勇者移出手机屏幕，数值适配主流手机分辨率
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -4.5f, 4.5f); // 左右边界（竖屏宽度窄）
        pos.y = Mathf.Clamp(pos.y, -8f, 8f);     // 上下边界（竖屏高度长）
        transform.position = pos;
    }
}