import requests
from urllib.parse import urlparse

def analyze_security_headers(url):
    """
    Анализирует HTTP-заголовки безопасности для заданного URL.
    """
    print(f"Анализируем: {url}\n")

    # Список заголовков безопасности, которые мы хотим проверить
    security_headers = {
        "Strict-Transport-Security": False,
        "Content-Security-Policy": False,
        "X-Content-Type-Options": False,
        "X-Frame-Options": False,
        "X-XSS-Protection": False,
        "Referrer-Policy": False,
        "Permissions-Policy": False,
    }

    try:
        # Отправляем GET-запрос к URL
        # allow_redirects=True, чтобы следовать за редиректами (например, с HTTP на HTTPS)
        response = requests.get(url, allow_redirects=True, timeout=10)
        response.raise_for_status() # Вызывает исключение для ошибок HTTP (4xx или 5xx)

        headers = response.headers

        print("Найденные заголовки безопасности:")
        for header_name in security_headers:
            if header_name in headers:
                security_headers[header_name] = True
                print(f"  ✅ {header_name}: {headers[header_name]}")
            else:
                print(f"  ❌ {header_name}: Отсутствует")

        print("\nОбщий отчет:")
        for header, present in security_headers.items():
            if present:
                print(f"  [✓] {header} - Присутствует")
            else:
                print(f"  [✗] {header} - Отсутствует")

        # Дополнительные проверки для некоторых заголовков
        print("\nДополнительные рекомендации:")
        if security_headers["Strict-Transport-Security"] and "max-age" in headers["Strict-Transport-Security"]:
            if int(headers["Strict-Transport-Security"].split("max-age=")[1].split(";")[0]) < 31536000: # 1 год в секундах
                print("  ⚠️ Strict-Transport-Security: max-age слишком короткий. Рекомендуется не менее 1 года.")
        
        if security_headers["X-Content-Type-Options"] and headers["X-Content-Type-Options"].lower() != "nosniff":
            print("  ⚠️ X-Content-Type-Options: Рекомендуется значение 'nosniff'.")

        if security_headers["X-Frame-Options"] and headers["X-Frame-Options"].lower() not in ["deny", "sameorigin"]:
            print("  ⚠️ X-Frame-Options: Рекомендуется 'DENY' или 'SAMEORIGIN'.")

    except requests.exceptions.RequestException as e:
        print(f"Произошла ошибка при запросе к {url}: {e}")
    except Exception as e:
        print(f"Произошла непредвиденная ошибка: {e}")

if __name__ == "__main__":
    # Пример использования
    target_url = input("Введите URL веб-сайта для анализа (например, https://www.google.com): ")
    if not target_url.startswith("http://") and not target_url.startswith("https://"):
        target_url = "https://" + target_url # Добавляем HTTPS по умолчанию

    analyze_security_headers(target_url)

    print("\n\n---")
    print("Как использовать Beautiful Soup для расширенного анализа:")
    print("Beautiful Soup используется для парсинга HTML-контента страницы,")
    print("чтобы найти уязвимости, такие как устаревшие библиотеки JavaScript,")
    print("инлайновые скрипты, или неправильно настроенные мета-теги.")
    print("Пример: from bs4 import BeautifulSoup; soup = BeautifulSoup(response.text, 'html.parser')")
    print("Вы могли бы использовать ее для поиска <script> тегов, атрибутов 'onload', 'onerror' и т.д.")
