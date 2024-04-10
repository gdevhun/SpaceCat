using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance=(T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    GameObject obj=new GameObject(typeof(T).Name,typeof(T));
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if(transform.parent != null && transform.root != null)
        {   //Managers 오브젝트안에 각각 싱글톤메니저들을 넣는 경우 예외처리를 위한 구문
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}