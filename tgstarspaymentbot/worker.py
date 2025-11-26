import json
import requests
from js import Response

def get_cors_headers(request):
    """
    Возвращает заголовки CORS для ответа.
    """
    return {
        'Access-Control-Allow-Origin': request.headers.get("Origin", "*"),
        'Access-Control-Allow-Methods': 'POST, OPTIONS',
        'Access-Control-Allow-Headers': 'Content-Type',
        'Access-Control-Max-Age': '86400',
    }

class WorkerEntrypoint:
    async def fetch(self, request, env):
        cors_headers = get_cors_headers(request)

        # Обработка OPTIONS-запроса (Preflight)
        if request.method == "OPTIONS":
            return Response.new(None, status=204, headers=cors_headers)

        # Обработка POST-запросов
        if request.method == "POST":
            try:
                data = await request.json()
                
                # --- Логика обработки запроса ---
                
                # Обработка успешного платежа (от Telegram)
                if 'successful_payment' in data.get('message', {}):
                    message = data['message']
                    payload = message['successful_payment']['invoice_payload']
                    user_id = message['from']['id']
                    item_id = payload.split('_')[1]

                    # 1. Логика бэкенда (например, сохранение в KV)
                    # await env.KV_STORAGE.put(f"user_{user_id}_item_{item_id}", "unlocked")

                    # 2. Уведомление администратора
                    url = f'api.telegram.org{env.TELEGRAM_BOT_TOKEN}/sendMessage'
                    requests.post(url, json={
                        'chat_id': env.ADMIN_CHAT_ID,
                        'text': f"✅ УСПЕШНЫЙ ПЛАТЕЖ: {item_id} от {user_id}"
                    })
                    
                    # 3. Уведомление пользователя
                    requests.post(url, json={
                        'chat_id': user_id,
                        'text': f"Покупка {item_id} успешно завершена! Теперь вернитесь в игру, чтобы увидеть разблокировку."
                    })
                    
                    response_body = {"status": "ok", "message": "payment processed"}
                
                # Обработка запроса из Mini App (для создания счёта)
                elif 'web_app_data' in data.get('message', {}):
                    message = data['message']
                    web_app_data = json.loads(message['web_app_data']['data'])
                    user_id = message['from']['id']
                    
                    if web_app_data.get('action') == 'create_invoice':
                        item_id = web_app_data.get('item_id', 'item_default')
                        price_stars = int(web_app_data.get('price', 1))
                        
                        # Создание Счета (Invoice)
                        url = f'api.telegram.org{env.TELEGRAM_BOT_TOKEN}/sendInvoice'
                        requests.post(url, json={
                            'chat_id': user_id,
                            'title': f'Разблокировка: {item_id}',
                            'description': f'Покупка за {price_stars} Звёзд.',
                            'payload': f'unlock_{item_id}_{user_id}',
                            'currency': 'XTR',
                            'prices': [{'label': f'{item_id} (Stars)', 'amount': price_stars * 100}] # Telegram требует цену в копейках/центах
                        })
                        response_body = {"status": "ok", "message": "invoice created"}
                    else:
                        response_body = {"status": "error", "message": "unknown action"}
                
                else:
                    # Запрос пришел не от Mini App и не от Telegram (может быть, это вебхук)
                    response_body = {"status": "ok", "message": "unknown request type"}

                # Создание ответа с CORS-заголовками
                response = Response.new(json.dumps(response_body), headers={
                    **cors_headers,
                    'content-type': 'application/json;charset=UTF-8'
                })
                return response

            except Exception as e:
                # Обработка ошибок
                error_response = Response.new(json.dumps({"status": "error", "message": str(e)}), headers={
                    **cors_headers,
                    'content-type': 'application/json;charset=UTF-8'
                })
                return error_response
        
        # Ответ на другие типы запросов
        return Response.new(json.dumps({"status": "error", "message": "Method not allowed"}), status=405, headers=cors_headers)
