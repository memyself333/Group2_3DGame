using UnityEngine;
[CreateAssetMenu(fileName = "PrisonerSO", menuName = "Prisoner/Prisoner")]

public class PrisonerSO : ScriptableObject
{
    public string prisonerName;
    public int prisonerPoints = 0;
    public bool isAlive = true;

    public void Start()
    {
        prisonerPoints = 0;
        isAlive = true;
    }
}
