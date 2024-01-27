using UnityEngine;

public class Singleton <T> : MonoBehaviour where T: Component
{
    static T _instance;

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject(typeof(T).Name + "_Singelton");
                go.hideFlags = HideFlags.HideAndDontSave;
                _instance = go.AddComponent<T>();
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance != this)
            return;

        _instance = null;
    }
}
