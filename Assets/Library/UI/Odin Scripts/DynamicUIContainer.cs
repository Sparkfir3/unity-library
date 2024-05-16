using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if !ODIN_INSPECTOR
using ShowInInspector = Sparkfire.Utility.BlankAttribute;
#else
using Sirenix.OdinInspector;
#endif

namespace Sparkfire.Utility
{
    public class DynamicUIContainer : UIBehaviour
    {

        [SerializeField, ShowInInspector]
        private SerializedDictionary<string, Object> references = new SerializedDictionary<string, Object>();
        public Dictionary<string, object> data = new Dictionary<string, object>();

        public T Get<T>(string key) where T : Object
        {
            return (T)references[key];
        }

        public T GetData<T>(string key)
        {
            return (T)data[key];
        }
    }
}