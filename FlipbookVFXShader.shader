Shader "Custom/FlipbookVFXShader"
{
    Properties
    {
        _MainTex ("Texture Atlas (RGBA)", 2D) = "white" {} // Основная текстура-атлас
        _Color ("Tint Color", Color) = (1,1,1,1) // Цветной оттенок
        _NumRows ("Number of Rows", Float) = 4 // Количество строк в атласе
        _NumCols ("Number of Columns", Float) = 4 // Количество столбцов в атласе
        _FrameIndex ("Current Frame Index (0-based)", Float) = 0 // Текущий индекс кадра (начиная с 0)
    }
    SubShader
    {
        // Теги для рендеринга прозрачных объектов
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        // Включаем смешивание для прозрачности (стандартное альфа-смешивание)
        Blend SrcAlpha OneMinusSrcAlpha
        // Отключаем отсечение задних граней, полезно для частиц
        Cull Off

        CGPROGRAM
        // Используем модель Surface Shader для простоты
        // 'Standard' для стандартного освещения, 'alpha:fade' для прозрачности
        #pragma surface surf Standard alpha:fade noambient noforwardadd

        // Объявляем свойства, которые мы определили выше
        sampler2D _MainTex;
        fixed4 _Color;
        float _NumRows;
        float _NumCols;
        float _FrameIndex;

        // Структура Input определяет, какие данные передаются в Surface Shader
        struct Input
        {
            float2 uv_MainTex; // UV-координаты текстуры
        };

        // Функция surf - это ядро Surface Shader, где происходит расчет цвета
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Вычисляем ширину и высоту одного кадра в атласе (в диапазоне 0-1)
            float frameWidth = 1.0 / _NumCols;
            float frameHeight = 1.0 / _NumRows;

            // Вычисляем индекс столбца (u) и строки (v) для текущего кадра
            // fmod(_FrameIndex, _NumCols) дает остаток от деления, т.е. номер столбца
            float frameX = fmod(_FrameIndex, _NumCols);
            // floor(_FrameIndex / _NumCols) дает целое от деления, т.е. номер строки
            float frameY = floor(_FrameIndex / _NumCols);

            // Вычисляем UV-координаты для выборки из атласа
            // IN.uv_MainTex - это UV-координаты внутри текущего полигона (0-1)
            // Мы масштабируем их до размера одного кадра и смещаем к нужному кадру в атласе.

            // Для X-координаты: (UV-координата внутри кадра * ширина кадра) + (индекс столбца * ширина кадра)
            float2 finalUV;
            finalUV.x = IN.uv_MainTex.x * frameWidth + frameX * frameWidth;

            // Для Y-координаты:
            // Большинство флипбуков начинаются с верхнего левого угла.
            // Unity UV (0,0) - это нижний левый угол текстуры.
            // Поэтому, чтобы правильно выбрать строку, мы инвертируем Y-координату строки.
            // (_NumRows - 1 - frameY) дает инвертированный индекс строки (0-based)
            finalUV.y = IN.uv_MainTex.y * frameHeight + (_NumRows - 1 - frameY) * frameHeight;

            // Выбираем цвет из основной текстуры по вычисленным UV-координатам
            // и умножаем его на цветной оттенок
            fixed4 c = tex2D (_MainTex, finalUV) * _Color;

            // Присваиваем полученный цвет альбедо (основной цвет поверхности)
            o.Albedo = c.rgb;
            // Присваиваем альфа-канал для прозрачности
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Standard" // Если этот шейдер не поддерживается, используем стандартный
}
