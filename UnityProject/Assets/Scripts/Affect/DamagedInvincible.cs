/*=====
<DamagedInvincible.cs>
└作成者：takagi

＞内容
被ダメージ時の無敵付与処理を定義

＞注意事項
無敵時間は実装側が内部で定義する形となります。

＞更新履歴
__Y25
_M05
D
19:プログラム作成:takagi
=====*/

// インターフェース定義
public interface IDamagedInvincible
{
	// プロトタイプ宣言
	public void GrantInvincible();	// 被ダメージ時の無敵状態付与処理
}