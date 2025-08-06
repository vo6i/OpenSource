import hashlib
import os
import json
import datetime

def calculate_file_hash(filepath, hash_algorithm="sha256"):
    """
    Вычисляет хеш-сумму файла.
    Поддерживаемые алгоритмы: 'md5', 'sha1', 'sha256', 'sha512'.
    """
    hasher = hashlib.new(hash_algorithm)
    try:
        with open(filepath, 'rb') as f:
            while True:
                chunk = f.read(4096) # Читаем файл по частям
                if not chunk:
                    break
                hasher.update(chunk)
        return hasher.hexdigest()
    except FileNotFoundError:
        return None
    except Exception as e:
        print(f"Ошибка при чтении файла {filepath}: {e}")
        return None

def monitor_files_integrity(target_directory, hash_db_file="file_hashes.json"):
    """
    Мониторит целостность файлов в указанной директории.
    Создает или обновляет базу данных хешей.
    """
    print(f"Мониторинг целостности файлов в директории: {target_directory}\n")
    
    current_hashes = {}
    for root, _, files in os.walk(target_directory):
        for file in files:
            filepath = os.path.join(root, file)
            # Исключаем сам файл базы данных хешей
            if filepath == os.path.join(target_directory, hash_db_file):
                continue
            
            file_hash = calculate_file_hash(filepath)
            if file_hash:
                current_hashes[filepath] = file_hash

    # Загружаем предыдущие хеши, если они есть
    previous_hashes = {}
    if os.path.exists(hash_db_file):
        try:
            with open(hash_db_file, 'r', encoding='utf-8') as f:
                previous_hashes = json.load(f)
            print(f"Загружена предыдущая база данных хешей из '{hash_db_file}'.")
        except json.JSONDecodeError:
            print(f"Предупреждение: Файл '{hash_db_file}' поврежден или пуст. Создаем новую базу данных.")
        except Exception as e:
            print(f"Ошибка при загрузке '{hash_db_file}': {e}")

    # Сравниваем текущие хеши с предыдущими
    print("\nОтчет об изменениях:")
    changes_detected = False

    # Проверка измененных или новых файлов
    for filepath, current_hash in current_hashes.items():
        if filepath not in previous_hashes:
            print(f"  ➕ Новый файл обнаружен: {filepath}")
            changes_detected = True
        elif previous_hashes[filepath] != current_hash:
            print(f"  ⚠️ Файл изменен: {filepath}")
            print(f"     Старый хеш: {previous_hashes[filepath]}")
            print(f"     Новый хеш:  {current_hash}")
            changes_detected = True

    # Проверка удаленных файлов
    for filepath in previous_hashes:
        if filepath not in current_hashes:
            print(f"  ➖ Файл удален: {filepath}")
            changes_detected = True

    if not changes_detected:
        print("  ✅ Изменений не обнаружено. Целостность файлов сохранена.")
    else:
        print("\nОбнаружены изменения в файлах. Рекомендуется проверка.")

    # Сохраняем текущие хеши для следующей проверки
    try:
        with open(hash_db_file, 'w', encoding='utf-8') as f:
            json.dump(current_hashes, f, indent=4, ensure_ascii=False)
        print(f"\nТекущие хеши сохранены в '{hash_db_file}' для будущих проверок.")
    except Exception as e:
        print(f"Ошибка при сохранении '{hash_db_file}': {e}")

if __name__ == "__main__":
    print("--- Монитор Целостности Файлов ---")
    
    # Можно указать конкретную директорию или использовать текущую
    # target_dir = "C:/MyImportantProject" 
    target_dir = input("Введите путь к директории для мониторинга (оставьте пустым для текущей директории): ")
    if not target_dir:
        target_dir = os.getcwd() # Текущая рабочая директория
    
    if not os.path.isdir(target_dir):
        print(f"Ошибка: Директория '{target_dir}' не существует.")
        sys.exit(1)

    monitor_files_integrity(target_dir)

    print("\n\n---")
    print("Как использовать:")
    print("1. Запустите скрипт в первый раз, чтобы создать базу данных хешей.")
    print("2. Внесите изменения в файлы в целевой директории (добавьте, измените, удалите).")
    print("3. Запустите скрипт снова, чтобы увидеть отчет об изменениях.")
