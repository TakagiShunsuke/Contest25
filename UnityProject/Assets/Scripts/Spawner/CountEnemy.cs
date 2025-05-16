/*=====
<CountEnemy.cs>
└作成者：Nishibu

＞内容
// 敵の総数をカウントするためのクラス

＞更新履歴
__Y25 
_M05
D
5:CountEnemyクラス生成:nishibu
6:修正:nishibu
7:修正、コメント:nishibu
=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CCountEnemy : MonoBehaviour
{
    // 現在ステージ上に存在している敵の数
    public static uint m_nValInstances { get; private set; } = 0;

    // ＞生成時場所
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：敵のインスタンス数を1つ増やす
    protected virtual void Start()
    {
        m_nValInstances++; // 敵の数を1増やす
    }

    // ＞死亡時場所
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：敵のインスタンス数を1つ減らす
    protected virtual void OnDestroy()
    {
        m_nValInstances--; // 敵の数を1減らす
    }
}
