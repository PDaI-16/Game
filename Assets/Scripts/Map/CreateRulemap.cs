using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CustomRuleTile", menuName = "Tiles/CustomRuleTile")]
public class CustomRuleTile : RuleTile
{
    public Vector2Int tileSize = new Vector2Int(2, 2); // Size of the tile in grid cells

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (go != null)
        {
            // Adjust the position of the GameObject to center it within the larger cell
            go.transform.position += new Vector3((tileSize.x - 1) * 0.5f, (tileSize.y - 1) * 0.5f, 0);
        }
        return base.StartUp(position, tilemap, go);
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        // Adjust the rule matching to account for the larger cell size
        if (tile is CustomRuleTile customTile)
        {
            return customTile.tileSize == tileSize;
        }
        return base.RuleMatch(neighbor, tile);
    }
}