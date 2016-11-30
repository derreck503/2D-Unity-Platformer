using UnityEngine;
using System.Collections;

public class DestroyOnNonMobile : MonoBehaviour {

	#if !UNITY_ANDROID && !UNITY_IPHONE || UNITY_EDITOR
	void Start () 
	{
		Destroy (this.gameObject);
	}
	#endif
}
