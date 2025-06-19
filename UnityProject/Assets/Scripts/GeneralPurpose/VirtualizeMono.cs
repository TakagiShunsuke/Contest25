/*=====
<VirtualizeMono.cs>
└作成者：takagi

＞内容
MonoBehaviorのイベント関数を仮想化(sealed修飾子用)

＞注意事項
・MonoBehaviorの各イベント関数をsealedすることができるようになります
	書き換えは防ぎたいがそのタイミングで追加で処理したいものがあればそれぞれここで仮想的に定義されている関数(Custom~~()関数)を使うのが早いと思います。
	なお、ここで定義されているイベントでない関数は定義のみであり、あくまで各子クラスで定義しなおす手間を省いているにすぎません。
	呼び出しはされていないのでsealedを定義する際に呼び出してください(overrideで上書きしているので当たり前ですが)
・また、protectedの修飾子は変更が効きません	：	これをいじらなければならない場合はこのクラスの使用はお控えください。
	(publicがないのはイベント関数をイベントらしく実装するため、privateがないのはオーバーロードを防ぐためです)

＞更新履歴
__Y24
_M06
D
06:プログラム作成:takagi
21:リファクタリング:takagi
__Y25
_M06
D
14:リファクタリング:takagi
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CVirtualizeMono : MonoBehaviour
{
	/// <summary>
	/// -初期化関数
	/// <para>インスタンス生成直後に行う処理</para>
	/// </summary>
	virtual protected void Awake() {}

	/// <summary>
	/// -Awake()関数カスタム用
	/// <para>Awake()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomAwake() {}

	/// <summary>
	/// -初期化関数
	/// <para>初回更新直前に行う処理</para>
	/// </summary>
	virtual protected void Start() {}

	/// <summary>
	/// -Start()関数カスタム用
	/// <para>Start()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomStart() {}

	/// <summary>
	/// -初期化関数
	/// <para>自身のオブジェクトが有効になった瞬間に行う処理</para>
	/// </summary>
	virtual protected void OnEnable() {}

	/// <summary>
	/// -OnEnable()関数カスタム用
	/// <para>OnEnable()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomOnEnable() {}

	/// <summary>
	/// -更新関数
	/// <para>一定時間ごとに行う更新処理</para>
	/// </summary>
	virtual protected void Update() {}

	/// <summary>
	/// -Update()関数カスタム用
	/// <para>Update()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomUpdate() {}

	/// <summary>
	/// -遅延性更新関数
	/// <para>各フレームにおいて通常のUpdate関数の後に行う更新処理</para>
	/// </summary>
	virtual protected void LateUpdate() {}

	/// <summary>
	/// -LateUpdate()関数カスタム用
	/// <para>LateUpdate()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomLateUpdate() {}

	/// <summary>
	/// -物理更新関数
	/// <para>一定時間ごとに行う物理的な更新処理</para>
	/// </summary>
	virtual protected void FixedUpdate() {}

	/// <summary>
	/// -FixedUpdate()関数カスタム用
	/// <para>FixedUpdate()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomFixedUpdate() {}

	/// <summary>
	/// -GUI更新関数
	/// <para>GUI描画用に高頻度で行われる更新処理</para>
	/// </summary>
	virtual protected void OnGUI() {}

	/// <summary>
	/// -OnGUI()関数カスタム用
	/// <para>OnGUI()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomOnGUI() {}

	/// <summary>
	/// -破棄関数カスタム用
	/// <para>インスタンス破棄時に行う処理</para>
	/// </summary>
	virtual protected void OnDestroy() {}

	/// <summary>
	/// -OnDestroy()関数カスタム用
	/// <para>OnDestroy()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomOnDestroy() {}

	/// <summary>
	/// -無効化関数
	/// <para>自身のオブジェクトが無効になった瞬間に行う処理</para>
	/// </summary>
	virtual protected void OnDisable() {}

	/// <summary>
	/// -OnDisable()関数カスタム用
	/// <para>OnDisable()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomOnDisable() {}

	/// <summary>
	/// -非描画時関数
	/// <para>任意のカメラに映り始めた瞬間に行う処理</para>
	/// </summary>
	virtual protected void OnBecameVisible() {}

	/// <summary>
	/// -OnBecameVisible()関数カスタム用
	/// <para>OnBecameVisible()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomOnBecameVisible() {}

	/// <summary>
	/// -シャイ時関数
	/// <para>任意のカメラに映らなくなった瞬間に行う処理</para>
	/// </summary>
	virtual protected void OnBecameInvisible() {}

	/// <summary>
	/// -OnBecameInvisible()関数カスタム用
	/// <para>OnBecameInvisible()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomOnBecameInvisible() {}

	/// <summary>
	/// -描画中関数
	/// <para>映っている間カメラごとに呼び出される処理(IsTrigger off時)</para>
	/// </summary>
	virtual protected void OnWillRenderObject() {}

	/// <summary>
	/// -OnWillRenderObject()関数カスタム用
	/// <para>OnWillRenderObject()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomOnWillRenderObject() {}

	/// <summary>
	/// -接触時関数3D
	/// <para>3D空間上で接触の当たり判定が取られた瞬間に行う処理(IsTrigger off時)</para>
	/// </summary>
	/// <param name="_Collision">接触相手</param>
	virtual protected void OnCollisionEnter(Collision _Collision) {}

	/// <summary>
	/// -OnCollisionEnter()関数カスタム用
	/// <para>OnCollisionEnter()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_Collision">接触相手</param>
	virtual protected void CustomOnCollisionEnter(Collision _Collision) {}

	/// <summary>
	/// -接触時関数2D
	/// <para>2D空間上で接触の当たり判定が取られた瞬間に行う処理(IsTrigger off時)</para>
	/// </summary>
	/// <param name="_Collision">接触相手</param>
	virtual protected void OnCollisionEnter2D(Collision2D _Collision) {}

	/// <summary>
	/// -OnCollisionEnter2D()関数カスタム用
	/// <para>OnCollisionEnter2D()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_Collision">接触相手</param>
	virtual protected void CustomOnCollisionEnter2D(Collision2D _Collision) {}

	/// <summary>
	/// -接触時関数3D
	/// <para>3D空間上で接触の当たり判定が取られた瞬間に行う処理(IsTrigger on時)</para>
	/// </summary>
	/// <param name="_Collider">接触相手</param>
	virtual protected void OnTriggerEnter(Collider _Collider) {}

	/// <summary>
	/// -OnTriggerEnter()関数カスタム用
	/// <para>OnTriggerEnter()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_Collider">接触相手</param>
	virtual protected void CustomOnTriggerEnter(Collider _Collider) {}

	/// <summary>
	/// -接触時関数2D
	/// <para>2D空間上で接触の当たり判定が取られた瞬間に行う処理(IsTrigger on時)</para>
	/// </summary>
	/// <param name="_Collider">接触相手</param>
	virtual protected void OnTriggerEnter2D(Collider2D _Collider) {}

	/// <summary>
	/// -OnTriggerEnter2D()関数カスタム用
	/// <para>OnTriggerEnter2D()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_Collider">接触相手</param>
	virtual protected void CustomOnTriggerEnter2D(Collider2D _Collider) {}

	/// <summary>
	/// -接触中関数3D
	/// <para>3D空間上で接触の当たり判定が取られている間行う処理(IsTrigger off時)</para>
	/// </summary>
	/// <param name="_Collision">接触相手</param>
	virtual protected void OnCollisionStay(Collision _Collision) {}

	/// <summary>
	/// -OnCollisionStay()関数カスタム用
	/// <para>OnCollisionStay()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_Collision">接触相手</param>
	virtual protected void CustomOnCollisionStay(Collision _Collision) {}

	/// <summary>
	/// -接触中関数2D
	/// <para>2D空間上で接触の当たり判定が取られている間行う処理(IsTrigger off時)</para>
	/// </summary>
	/// <param name="_Collision">接触相手</param>
	virtual protected void OnCollisionStay2D(Collision2D _Collision) {}

	/// <summary>
	/// -OnCollisionStay2D()関数カスタム用
	/// <para>OnCollisionStay2D()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_Collision">接触相手</param>
	virtual protected void CustomOnCollisionStay2D(Collision2D _Collision) {}

	/// <summary>
	/// -接触中関数3D
	/// <para>3D空間上で接触の当たり判定が取られている間行う処理(IsTrigger on時)</para>
	/// </summary>
	/// <param name="_Collider">接触相手</param>
	virtual protected void OnTriggerStay(Collider _Collider) {}

	/// <summary>
	/// -OnTriggerStay()関数カスタム用
	/// <para>OnTriggerStay()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_Collider">接触相手</param>
	virtual protected void CustomOnTriggerStay(Collider _Collider) {}

	/// <summary>
	/// -接触中関数2D
	/// <para>2D空間上で接触の当たり判定が取られている間行う処理(IsTrigger on時)</para>
	/// </summary>
	/// <param name="_Collider">接触相手</param>
	virtual protected void OnTriggerStay2D(Collider2D _Collider) {}

	/// <summary>
	/// -OnTriggerStay2D()関数カスタム用
	/// <para>OnTriggerStay2D()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_Collider">接触相手</param>
	virtual protected void CustomOnTriggerStay2D(Collider2D _Collider) {}

	/// <summary>
	/// -分離時関数3D
	/// <para>3D空間上で接触していた物体と離れた瞬間に行う処理(IsTrigger off時)</para>
	/// </summary>
	/// <param name="_Collision">接触相手</param>
	virtual protected void OnCollisionExit(Collision _Collision) {}

	/// <summary>
	/// -OnCollisionExit()関数カスタム用
	/// <para>OnCollisionExit()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_Collision">接触相手</param>
	virtual protected void CustomOnCollisionExit(Collision _Collision) {}

	/// <summary>
	/// -分離時関数2D
	/// <para>2D空間上で接触していた物体と離れた瞬間に行う処理(IsTrigger off時)</para>
	/// </summary>
	/// <param name="_Collision">接触相手</param>
	virtual protected void OnCollisionExit2D(Collision2D _Collision) {}

	/// <summary>
	/// -OnCollisionExit2D()関数カスタム用
	/// <para>OnCollisionExit2D()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_Collision">接触相手</param>
	virtual protected void CustomOnCollisionExit2D(Collision2D _Collision) {}

	/// <summary>
	/// -分離時関数3D
	/// <para>3D空間上で接触していた物体と離れた瞬間に行う処理(IsTrigger on時)</para>
	/// </summary>
	/// <param name="_Collider">接触相手</param>
	virtual protected void OnTriggerExit(Collider _Collider) {}

	/// <summary>
	/// -OnTriggerExit()関数カスタム用
	/// <para>OnTriggerExit()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_Collider">接触相手</param>
	virtual protected void CustomOnTriggerExit(Collider _Collider) {}

	/// <summary>
	/// -分離時関数2D
	/// <para>2D空間上で接触していた物体と離れた瞬間に行う処理(IsTrigger on時)</para>
	/// </summary>
	/// <param name="_Collider">接触相手</param>
	virtual protected void OnTriggerExit2D(Collider2D _Collider) {}

	/// <summary>
	/// -OnTriggerExit2D()関数カスタム用
	/// <para>OnTriggerExit2D()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_Collider">接触相手</param>
	virtual protected void CustomOnTriggerExit2D(Collider2D _Collider) {}

	/// <summary>
	/// -パーティクル接触中関数
	/// <para>パーティクルがコライダーと接触している間行う処理(SendCollisionMessage on時)</para>
	/// </summary>
	/// <param name="_GameObject">当たったオブジェクトの情報</param>
	virtual protected void OnParticleCollision(GameObject _GameObject) {}

	/// <summary>
	/// -OnParticleCollision()関数カスタム用
	/// <para>OnParticleCollision()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	/// <param name="_GameObject">当たったオブジェクトの情報</param>
	virtual protected void CustomOnParticleCollision(GameObject _GameObject) {}

	/// <summary>
	/// -パーティクルトリガー判定関数
	/// <para>ParticleSystemがTriggersモジュールを搭載している間呼び出される処理</para>
	/// </summary>
	virtual protected void OnParticleTrigger() {}

	/// <summary>
	/// -OnParticleTrigger()関数カスタム用
	/// <para>OnParticleTrigger()関数で任意に処理を追加しやすいように定義</para>
	/// </summary>
	virtual protected void CustomOnParticleTrigger() {}
}