using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTools : Singleton<CoroutineTools> {

    public IEnumerator WaitUntilOrTimeout(float maxTime, Func<bool> condition) {
        for(float i = 0; i <= maxTime; i += Time.deltaTime) {
            if(condition.Invoke())
                break;
            yield return new WaitForEndOfFrame();
        }
    }

}