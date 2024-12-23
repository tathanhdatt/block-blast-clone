using Dt.Attribute;
using UnityEngine;

public class LevelPlayer : MonoBehaviour
{
    [SerializeField, Required]
    private LevelEventHandler levelEventHandler;

    public void Play(BoardTemplate levelTemplate)
    {
        this.levelEventHandler.Initialize(levelTemplate);
    }

    public void Terminate()
    {
        this.levelEventHandler.Terminate();
    }
}
