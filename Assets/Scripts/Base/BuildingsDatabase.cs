using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingsDatabase", menuName = "Database/Buildings database")]
public class BuildingsDatabase: ScriptableObject
{
    public List<Building> Buildings = new ();

    public Building GetBuildingByName(string name)
    {
        foreach (var building in Buildings)
        {
            if (building.Name == name)
                return building;
        }
        
        Debug.LogWarning($"Здание {name} не найдено");
        return null;
    }
}
