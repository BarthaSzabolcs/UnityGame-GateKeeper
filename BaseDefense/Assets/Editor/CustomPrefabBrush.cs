using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditor
{
[CustomGridBrush(true, false, false, "Prefab Brush")]
public class CustomPrefabBrush : UnityEditor.Tilemaps.GridBrush
{
    #region ShowInEditor
    [SerializeField] PrefabPalette [] prefabPalette;
    [SerializeField] string current;
    #endregion
    #region HideInEditor
    private int palleteIndex = 0;
    private int palletteItemIndex = 0;
    public int PalletteIndex
    {
        get
        {
            return palleteIndex;
        }
        set
        {
            if(value < prefabPalette.Length && value >= 0)
            {
                palleteIndex = value;
            }
            else if(value >= prefabPalette.Length)
            {
                palleteIndex = 0;
            }
            else
            {
                palleteIndex = prefabPalette.Length - 1;
            }
            current = prefabPalette[palleteIndex].name + " | " + prefabPalette[palleteIndex].palette[PalletteItemIndex].name;
            }
    }
    public int PalletteItemIndex
    {
        get
        {
            return palletteItemIndex;
        }
        set
        {
            if(value < prefabPalette[palleteIndex].palette.Length && value >= 0)
            {
                palletteItemIndex = value;
            }
            else if(value >= prefabPalette[palleteIndex].palette.Length)
            {
                palletteItemIndex = 0;
            }
            else
            {
                palletteItemIndex = prefabPalette[palleteIndex].palette.Length - 1;
            }
            current = prefabPalette[palleteIndex].name + " | " + prefabPalette[palleteIndex].palette[PalletteItemIndex].name;
        }
    }
    #endregion

    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        if(GetObjectInCell(grid, brushTarget.transform, position) == null) { 
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabPalette[PalletteIndex].palette[PalletteItemIndex]);
        Undo.RegisterCreatedObjectUndo(instance, "Paint Prefabs");
        if (instance != null)
        {
            instance.transform.SetParent(brushTarget.transform);
            instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, 0) + new Vector3(.5f, .5f, .5f)));
            }
        }
    }
    public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        // Do not allow editing palettes
        if (brushTarget.layer == 31)
            return;

        Transform erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, position.z));
        if (erased != null)
            Undo.DestroyObjectImmediate(erased.gameObject);
    }
    private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
    {
        int childCount = parent.childCount;
        Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
        Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
        Bounds bounds = new Bounds((max + min) * .5f, max - min);

        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (bounds.Contains(child.position))
                return child;
        }
        return null;
    }

        [CustomEditor(typeof(CustomPrefabBrush))]
        public class CustomPrefabBrushEditor : UnityEditor.Tilemaps.GridBrushEditor
        {
            private CustomPrefabBrush customBrush { get { return target as CustomPrefabBrush; } }

            //GameObject holder;

            public override void OnMouseLeave()
            {
                base.OnMouseLeave();

                //if (holder)
                //    DestroyImmediate(holder);
            }
            public override void OnPaintSceneGUI(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Tool tool, bool executing)
            {
                base.OnPaintSceneGUI(gridLayout, null, position, tool, executing);

                Event e = Event.current;

                if (e.type == EventType.KeyDown)
                {
                    if (e.keyCode == KeyCode.KeypadPlus)
                    {
                        customBrush.PalletteItemIndex++;

                        //DestroyImmediate(holder);
                        //holder = Instantiate(customBrush.prefabPalette[customBrush.PalletteIndex].palette[customBrush.PalletteItemIndex]);
                    }
                    else if (e.keyCode == KeyCode.KeypadMinus)
                    {
                        customBrush.PalletteItemIndex--;

                        //DestroyImmediate(holder);
                        //holder = Instantiate(customBrush.prefabPalette[customBrush.PalletteIndex].palette[customBrush.PalletteItemIndex]);
                    }
                    if (e.keyCode == KeyCode.KeypadDivide)
                    {
                        customBrush.PalletteIndex++;
                        customBrush.PalletteItemIndex = 0;

                        //DestroyImmediate(holder);
                        //holder = Instantiate(customBrush.prefabPalette[customBrush.PalletteIndex].palette[customBrush.PalletteItemIndex]);
                    }
                    else if (e.keyCode == KeyCode.KeypadMultiply)
                    {
                        customBrush.PalletteIndex--;
                        customBrush.PalletteItemIndex = 0;

                        //DestroyImmediate(holder);
                        //holder = Instantiate(customBrush.prefabPalette[customBrush.PalletteIndex].palette[customBrush.PalletteItemIndex]);
                    }
                }
                //if (!holder)
                //{
                //    holder = Instantiate(customBrush.prefabPalette[customBrush.PalletteIndex].palette[customBrush.PalletteItemIndex]);
                //}

                //holder.transform.position = position.position + new Vector3(.5f, 0.5f, 0.0f);
            }
        }
    }
}
