using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [Header("Persistent Objects")]
    public GameObject[] persistentObjects;
    public enum StateType
    {
        DEFAULT,      //Fall-back state, should never happen
        MAIN_MENU,    //Main menu state
        IN_GAME,      //In-game state
        PAUSED,       //Paused state
        ENDING,
        // Add more states as needed
    }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager instance is null. Make sure a GameManager script is attached to a GameObject in the scene.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        MarkPersistentObjects();
    }

    private void MarkPersistentObjects()
    {
        foreach (GameObject obj in persistentObjects)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }

    }
}
