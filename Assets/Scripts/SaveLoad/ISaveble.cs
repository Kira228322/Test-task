using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveble <T>
{
    public T SaveData();
    public void LoadData(T data);
}
