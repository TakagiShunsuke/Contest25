/*=====
<PanelBillboard.cs>
└作成者：tei

＞内容
パネルのビルボード設定

＞注意事項
　・今のカメラと合わせて作りましたが、現在のカメラは要修正と思いますので
　　カメラが変更したら、ビルボードの設定も要修正。

＞更新履歴
__Y25
_M04
D
26：プログラム作成：tei

_M05
D
01：スクリプト名、変数名修正：tei
04：コーディングルールの沿ってコード修正：tei
23：ビルボード処理修正：tei

=====*/

// 名前空間宣言
using UnityEngine;

// クラス定義
public class CPanelBillboard : MonoBehaviour
{
    // ＞更新関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：更新処理
    void Update()
    {
        if (Camera.main == null) return;

        // カメラの向いている方向（Y軸だけ取得）
        Vector3 camEuler = Camera.main.transform.rotation.eulerAngles;

        // XZ平面に張られた板として、-90度で正しく表示しつつY軸だけ追従
        transform.rotation = Quaternion.Euler(-90f, camEuler.y, 0f);
    }
}
