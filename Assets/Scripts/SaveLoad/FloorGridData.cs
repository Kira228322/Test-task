using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class FloorGridData 
{
    public List<SavedBuilding> Buildings;
    
    [System.Serializable]
    public class SavedBuilding
    {
        public string name;
        public int[] coords = new int[2];

        public SavedBuilding(Building building, int x, int y)
        {
            name = building.Name;
            coords[0] = x;
            coords[1] = y;
        }
    }

    public FloorGridData(Building[,] _buildingsOnGrid)
    {
        Buildings = new ();
        for (int j = 0; j < _buildingsOnGrid.GetLength(1); j++)
        {
            for (int i = 0; i < _buildingsOnGrid.GetLength(0) ; i++)
            {
                if (_buildingsOnGrid[i,j] != null)
                {
                    Building currentBuilding = _buildingsOnGrid[i, j];
                    Buildings.Add(new SavedBuilding(currentBuilding, i ,j));
                    
                    // ��� ��� ������ �� ����� ����� ����� � ������ ����� �����, �� �� ���������� �����������
                    // � ��������� ������ �� ���������� �����. ���� ������ ����� ��� 1�1, �� ��������� ����������
                    // �� ������ ������ ��������������.
                    for (int k = 0; k < currentBuilding.SizeOnGrid.x; k++)
                    {
                        for (int l = 0; l < currentBuilding.SizeOnGrid.y; l++)
                        {
                            _buildingsOnGrid[i + k, j + l] = null;
                        }
                    }
                }
            }
        }
    }
}
