using Dt.Attribute;
using UnityEngine;

public class LevelAutoPlayer : MonoBehaviour
{
    [SerializeField, Required]
    private LevelInitializer level;

    private void Awake()
    {
        this.level.Initialize(null);
    }
}