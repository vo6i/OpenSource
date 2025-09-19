using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, описывающий один ассет внутри мода.
/// </summary>
[System.Serializable]
public class ModAsset
{
    // Тип мода: "Player", "House", "HealingPotion" и т.д.
    public string modType;
    // Название ассета в AssetBundle.
    public string assetName;
}

/// <summary>
/// Класс-манифест, описывающий все ассеты в моде.
/// </summary>
[System.Serializable]
public class ModManifest
{
    // Имя мода.
    public string modName;
    // Описание.
    public string description;
    // Список всех ассетов в моде.
    public List<ModAsset> assets;
}
