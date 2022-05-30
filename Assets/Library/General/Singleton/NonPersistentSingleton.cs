using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A singleton that doesn't persist between scenes
//  To be used as a quick static reference
[DefaultExecutionOrder(-50)]
public abstract class NonPersistentSingleton<T> : Singleton<T> where T : MonoBehaviour {

    protected override void Start() { }

}