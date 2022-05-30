using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	public static T Instance { get; private set; }

	protected virtual void Awake() {
		if(Instance != null && Instance != this as T) {
			Destroy(gameObject);
			return;
		}
		Instance = this as T;
	}

	protected virtual void Start() {
		if(!GetComponentInParent<Singleton<T>>())
			 gameObject.transform.parent = null;
		DontDestroyOnLoad(gameObject);
	}

}