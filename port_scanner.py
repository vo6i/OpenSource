import socket
import sys

def port_scan(target_host, start_port, end_port):
    """
    Сканирует указанный диапазон портов на целевом хосте.
    """
    print(f"Сканирование хоста: {target_host} на портах от {start_port} до {end_port}\n")

    try:
        # Получаем IP-адрес из имени хоста, если введено имя
        target_ip = socket.gethostbyname(target_host)
    except socket.gaierror:
        print(f"Ошибка: Не удалось разрешить имя хоста '{target_host}'. Проверьте правильность ввода.")
        return

    open_ports = []
    
    for port in range(start_port, end_port + 1):
        try:
            # Создаем сокет (AF_INET для IPv4, SOCK_STREAM для TCP)
            s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            s.settimeout(1) # Устанавливаем таймаут в 1 секунду

            # Пытаемся подключиться к порту
            result = s.connect_ex((target_ip, port))

            if result == 0:
                print(f"  ✅ Порт {port} открыт")
                open_ports.append(port)
            else:
                # print(f"  ❌ Порт {port} закрыт или фильтруется") # Можно раскомментировать для полного вывода
                pass
            s.close() # Закрываем сокет
        except socket.error as e:
            print(f"Ошибка сокета при сканировании порта {port}: {e}")
            break # Выходим из цикла при ошибке сокета
        except Exception as e:
            print(f"Непредвиденная ошибка при сканировании порта {port}: {e}")
            break

    if open_ports:
        print(f"\nОткрытые порты на {target_host}: {open_ports}")
    else:
        print(f"\nНа хосте {target_host} не найдено открытых портов в указанном диапазоне.")

if __name__ == "__main__":
    # Пример использования
    print("--- Простой Сканер Портов ---")
    print("Внимание: Сканируйте только те сети и хосты, на которые у вас есть разрешение (например, свою домашнюю сеть или виртуальную машину).")
    
    target = input("Введите целевой IP-адрес или домен (например, '127.0.0.1' или 'localhost'): ")
    
    try:
        port_start = int(input("Введите начальный порт (например, 1): "))
        port_end = int(input("Введите конечный порт (например, 1024): "))
    except ValueError:
        print("Ошибка: Порты должны быть числами.")
        sys.exit(1)

    if port_start < 1 or port_end > 65535 or port_start > port_end:
        print("Ошибка: Неверный диапазон портов. Порты должны быть от 1 до 65535, и начальный порт должен быть меньше или равен конечному.")
        sys.exit(1)

    port_scan(target, port_start, port_end)
