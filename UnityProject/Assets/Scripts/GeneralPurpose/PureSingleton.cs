/*=====
<Singleton.cs>
└作成者：takagi

＞内容
純粋なシングルトンの実装

＞更新履歴
__Y25
_M06
D
14:プログラム作成完了:takagi
=====*/

// クラス定義
public abstract class CPureSingleton<PureType> where PureType : CPureSingleton<PureType>, new()	// where文で継承ツリーを明示：PureType←CMonoSingleton<PureType>, またnew()制約を付与し純粋なクラスと定義
{
	// 変数宣言
	private static PureType m_Instance = new PureType();	// インスタンス格納用
	
	// プロパティ定義
	
	/// <summary>
	/// インスタンスプロパティ
	/// </summary>
	/// <value><see cref="m_Instance"/></value>
	public static PureType Instance	// 継承先オブジェクトのインスタンス
	{
		get
		{
			if (m_Instance == null)	// ヌルチェック
			{
				m_Instance = new PureType();	// 自身のコンポーネント登録
			}
			return m_Instance;	// インスタンス提供
		}
	}
	
	/// <summary>
	/// -コンストラクタ
	/// <para>生成処理のアクセス制限</para>
	/// </summary>
	protected CPureSingleton()
	{
	}
}