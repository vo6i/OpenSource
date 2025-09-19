using UnityEngine;

/// <summary>
/// Обработчик для модов, меняющих модель игрока.
/// </summary>
public class PlayerModHandler : MonoBehaviour
{
    [Tooltip("Трансформ, к которому прикреплена модель игрока.")]
    public Transform playerModelHolder;

    private GameObject currentModel;

    /// <summary>
    /// Заменяет текущую модель игрока на новую.
    /// </summary>
    public void LoadPlayerModel(GameObject newModelPrefab)
    {
        // Удаляем старую модель, если она есть.
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        // Создаем экземпляр новой модели и привязываем её к трансформу игрока.
        currentModel = Instantiate(newModelPrefab, playerModelHolder.position, playerModelHolder.rotation, playerModelHolder);
        Debug.Log($"[PlayerModHandler] Модель игрока заменена на '{newModelPrefab.name}'.");
    }
}
