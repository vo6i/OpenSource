using UnityEngine;

public class FlipbookAnimator : MonoBehaviour
{
    public Material targetMaterial; // Материал, который использует шейдер FlipbookVFXShader
    public float framesPerSecond = 10.0f; // Скорость воспроизведения анимации (кадров в секунду)

    private int _numRows;
    private int _numCols;
    private float _totalFrames;
    private float _currentFrame = 0f;

    void Start()
    {
        if (targetMaterial == null)
        {
            // Попытаться получить материал из MeshRenderer, если он не назначен
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                targetMaterial = renderer.material;
            }
            if (targetMaterial == null)
            {
                Debug.LogError("FlipbookAnimator: Материал не назначен и не найден на Renderer.", this);
                enabled = false;
                return;
            }
        }

        // Получаем количество строк и столбцов из материала
        _numRows = Mathf.RoundToInt(targetMaterial.GetFloat("_NumRows"));
        _numCols = Mathf.RoundToInt(targetMaterial.GetFloat("_NumCols"));
        _totalFrames = _numRows * _numCols;

        if (_totalFrames <= 0)
        {
            Debug.LogError("FlipbookAnimator: Неверное количество строк или столбцов в материале.", this);
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Увеличиваем текущий кадр со временем
        _currentFrame += framesPerSecond * Time.deltaTime;

        // Зацикливаем анимацию
        if (_currentFrame >= _totalFrames)
        {
            _currentFrame -= _totalFrames;
        }

        // Присваиваем текущий индекс кадра шейдеру
        targetMaterial.SetFloat("_FrameIndex", Mathf.Floor(_currentFrame));
    }

    void OnDisable()
    {
        // Опционально: сбросить кадр при отключении скрипта, чтобы избежать артефактов
        // targetMaterial.SetFloat("_FrameIndex", 0);
    }
}
