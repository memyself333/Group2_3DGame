using UnityEngine;
[CreateAssetMenu(fileName = "PrisonerSO", menuName = "Prisoner/Prisoner")]

public class PrisonerSO : ScriptableObject
{
    public string prisonerName;
    public int prisonerPoints;
    public bool isAlive;
}
