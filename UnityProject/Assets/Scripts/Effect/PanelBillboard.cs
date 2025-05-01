/*=====
<CPanelBillboard.cs>
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

=====*/

using UnityEngine;

public class CPanelBillboard : MonoBehaviour
{
    // 変数宣言
    [SerializeField,Tooltip("カメラとの固定距離")] private float m_fDistanceFromCamera = 5f;

    // ＞更新関数
    // 引数：なし
    // ｘ
    // 戻値：なし
    // ｘ
    // 概要：更新処理
    void Update()
    {
        if (Camera.main == null) return;

        // カメラの位置と向きを取得
        Vector3 CameraPosition = Camera.main.transform.position;
        Vector3 CameraForward = Camera.main.transform.forward;

        // カメラの前方distanceだけ離れた場所にパネルを移動
        transform.position = CameraPosition + CameraForward * m_fDistanceFromCamera;

        // カメラのY軸回転だけ取る
        Vector3 Euler = Camera.main.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(-90f, Euler.y, 0f);
    }
}
