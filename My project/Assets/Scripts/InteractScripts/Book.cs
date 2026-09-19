using UnityEngine;

public class Book : MonoBehaviour
{

    //Literally just to allow the book animation event to reach the InteractingManager
    public ObjectInteract objInt;
    public void OpenText()
    {
        objInt.ReadBook();
    }
}
