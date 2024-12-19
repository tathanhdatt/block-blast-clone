using Dt.Attribute;
using UnityEngine;

public class BlockCell : Cell
{
    [Title("Block Cell")]
    [SerializeField]
    private LayerMask layerMask;

    [Title("Read Only Attributes")]
    [SerializeField, ReadOnly]
    private Collider2D col;

    [SerializeField, ReadOnly]
    private int x;

    [SerializeField, ReadOnly]
    private int y;

    public int X => this.x;
    public int Y => this.y;

    public void SetXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public BoardCell Hit()
    {
        Vector2 point = new Vector2(transform.position.x, transform.position.y);
        this.col = Physics2D.OverlapPoint(point, this.layerMask);
        return this.col?.GetComponent<BoardCell>();
    }
}