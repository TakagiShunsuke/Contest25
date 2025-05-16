


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public abstract class CAffect : MonoBehaviour
{
	//virtual public void Affect<Meta>(GameObject _Oneself, GameObject _Opponent, Meta _MetaData) where Meta : struct
	virtual public void Affect(GameObject _Oneself, GameObject _Opponent)
	{
	}
}
