using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Database / Brush")]
public class DB_Brush : ScriptableObject
{
    public List<BrushDeform> brushDeformList = new();

    public BrushDeform FindDeformBrushInDatabase(int id)
    {
        foreach (BrushDeform brush in brushDeformList)
        {
            if (brush.id == id)
            {
                return brush;
            }
        }
        return null;
    }
}

[System.Serializable]
public class BrushDeform
{
    public int id;
    public Texture2D maskTexture;
}
