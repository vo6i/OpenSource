using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class ModLoader : MonoBehaviour
{
    // URL, по которому находится ваш AssetBundle.
    [Tooltip("URL к AssetBundle (например, \"http://example.com/player_mod\").")]
    public string assetBundleUrl;

    // Ссылка на компонент, который будет обрабатывать ассет.
    [Tooltip("Ссылка на PlayerModHandler в вашей сцене.")]
    public PlayerModHandler playerModHandler;

    /// <summary>
    /// Запускает процесс загрузки мода.
    /// </summary>
    public void LoadMod()
    {
        StartCoroutine(LoadModFromBundle());
    }

    private IEnumerator LoadModFromBundle()
    {
        // Создаем UnityWebRequest для загрузки AssetBundle.
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleUrl))
        {
            Debug.Log($"[ModLoader] Начинаю загрузку AssetBundle с URL: {assetBundleUrl}");
            yield return www.SendWebRequest();

            // Проверяем на ошибки.
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[ModLoader] Ошибка загрузки AssetBundle: {www.error}");
                yield break;
            }

            // Получаем AssetBundle из загруженных данных.
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            if (bundle == null)
            {
                Debug.LogError("[ModLoader] Не удалось получить AssetBundle из загруженных данных.");
                yield break;
            }

            Debug.Log("[ModLoader] AssetBundle успешно загружен.");

            // Загружаем манифест мода из AssetBundle.
            string manifestText = LoadManifestText(bundle);
            if (string.IsNullOrEmpty(manifestText))
            {
                Debug.LogError("[ModLoader] Не удалось загрузить манифест мода.");
                bundle.Unload(false);
                yield break;
            }

            // Десериализуем JSON манифеста.
            ModManifest modManifest = JsonUtility.FromJson<ModManifest>(manifestText);
            if (modManifest == null)
            {
                Debug.LogError("[ModLoader] Не удалось десериализовать манифест мода.");
                bundle.Unload(false);
                yield break;
            }

            Debug.Log($"[ModLoader] Манифест мода '{modManifest.modName}' загружен.");

            // Находим ассет типа "Player" в манифесте.
            ModAsset playerAsset = null;
            foreach (var asset in modManifest.assets)
            {
                if (asset.modType == "Player")
                {
                    playerAsset = asset;
                    break;
                }
            }

            if (playerAsset == null)
            {
                Debug.LogError("[ModLoader] В манифесте не найден ассет типа 'Player'.");
                bundle.Unload(false);
                yield break;
            }

            // Загружаем префаб из AssetBundle по имени.
            GameObject prefab = bundle.LoadAsset<GameObject>(playerAsset.assetName);
            if (prefab == null)
            {
                Debug.LogError($"[ModLoader] Не удалось найти префаб с именем '{playerAsset.assetName}' в AssetBundle.");
                bundle.Unload(false);
                yield break;
            }

            // Передаем префаб обработчику.
            playerModHandler.LoadPlayerModel(prefab);

            // Выгружаем AssetBundle. 'true' - выгружаем и сам ассет.
            // bundle.Unload(true); // Если хотите выгрузить ассет сразу после инстанцирования.
            bundle.Unload(false); // Если хотите сохранить ассет в памяти.
        }
    }

    private string LoadManifestText(AssetBundle bundle)
    {
        // Манифест должен быть текстовым файлом (например, с расширением .json)
        // и называться, например, "mod_manifest.json"
        TextAsset manifestAsset = bundle.LoadAsset<TextAsset>("mod_manifest.json");
        if (manifestAsset != null)
        {
            return manifestAsset.text;
        }

        return null;
    }
}
