using UnityEngine;

public class Book : MonoBehaviour
{
    public ObjectInteract objInt;
    public void OpenText()
    {
        objInt.ReadBook();
    }
}
