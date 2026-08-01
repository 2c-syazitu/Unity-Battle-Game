using UnityEngine;

[CreateAssetMenu(fileName = "CreaterObj", menuName = "Scriptable Objects/CreaterObj")]
public class CreateObj : ScriptableObject
{
    public CreateCharacter createCharacter;

    private void Awake()
    {
        if (createCharacter == null)
        {
            createCharacter = new CreateCharacter();
        }
    }
}
