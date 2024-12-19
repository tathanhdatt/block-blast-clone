using Dt.Attribute;
using UnityEngine;

public class BlockCell : Cell
{
    [Title("Block Cell")]
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField, ReadOnly]
    private Collider2D col;

    public BoardCell Hit()
    {
        Vector2 point = new Vector2(transform.position.x, transform.position.y);
        this.col = Physics2D.OverlapPoint(point, this.layerMask);
        return this.col?.GetComponent<BoardCell>();
    }
}