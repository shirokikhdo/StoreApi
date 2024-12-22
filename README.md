# StoreApi

StoreApi — это RESTful API для управления операциями в магазине. API предоставляет функциональность для работы с товарами, заказами и пользователями.

## Технологии

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) - платформа для разработки
- [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) - фреймворк для создания веб-приложений
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) - ORM для работы с базами данных
- [PostgreSQL](https://www.postgresql.org/) - объектно-реляционная система управления базами данных

## Библиотеки

- Для аутентификации с использованием JWT:
```
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```
- Для использования Identity с Entity Framework Core:
```
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```
- Для инструментов разработки Entity Framework Core:
```
dotnet add package Microsoft.EntityFrameworkCore.Tools
```
- Для дизайна Entity Framework Core:
```
dotnet add package Microsoft.EntityFrameworkCore.Design
```
- Для работы с PostgreSQL через Entity Framework Core:
```
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```
- Для поддержки OpenAPI (Swagger):
```
dotnet add package Microsoft.AspNetCore.OpenApi
```
- Для генерации фейковых данных:
```
dotnet add package Bogus
```
- Для нормальной работы Swagger:
```
dotnet add package Swashbuckle.AspNetCore
```

## Docker

Создание контейнера с PostgreSQL
```
docker run --name store-api -e POSTGRES_PASSWORD=12345678 -p 5432:5432 -d postgres
```

## Установка

1. Клонируйте репозиторий:
```git clone https://github.com/shirokikhdo/StoreApi.git
cd StoreApi
```

2. Восстановите зависимости:

```
dotnet restore
```

3. Создайте базу данных и примените миграции:

```
dotnet ef database update
```
   
4. Запустите приложение:

```
dotnet run
```

## API Endpoints

### AuthController
 - **POST /api/auth/register**
	- **Описание:** Регистрирует нового пользователя в системе.
	- **Тело запроса:** `RegisterRequestDto`
	- **Ответы:**
		- 200 OK: Регистрация завершена
		- 400 Bad Request: Некорректная модель запроса или пользователь уже существует
- **POST /api/auth/login**
	- **Описание:** Выполняет вход пользователя в систему.
	- **Тело запроса:** `LoginRequestDto`
	- **Ответы:**
		- 200 OK: Успешный вход
		- 400 Bad Request: Неверный email или пароль
### AuthTestController
 - **GET /api/authtest/test1**
	- **Описание:** Тестовый метод, доступный для всех пользователей.
	- **Ответы:**
		- 200 OK: "Test1: Для всех"
 - **GET /api/authtest/test2**
	- **Описание:** Тестовый метод, доступный только для авторизованных пользователей.
	- **Ответы:**
		- 200 OK: "Test2: Для авторизованных пользователей"
		- 401 Unauthorized: Пользователь не авторизован
 - **GET /api/authtest/test3**
	- **Описание:** Доступен для авторизованных пользователей с правами Consumer.
	- **Ответы:**
		- 200 OK: "Test3: Для авторизованных пользователей с правами Consumer"
		- 401 Unauthorized: Пользователь не авторизован
		- 403 Forbidden: если роль admin
 - **GET /api/authtest/test4**
	- **Описание:** Доступен для авторизованных пользователей с правами Admin.
	- **Ответы:**
		- 200 OK: "Test4: Для авторизованных пользователей с правами Admin"
		- 401 Unauthorized: Пользователь не авторизован
		- 403 Forbidden: если роль customer
### OrderController
 - **POST /api/order/createorder**
	- **Описание:** Создает новый заказ.
	- **Тело запроса:** `OrderHeaderCreateDto`
	- **Ответы:**
		- 201 Created: Заказ успешно создан
		- 400 Bad Request: Неверное состояние модели заказа
 - **GET /api/order/getorder/{id}**
	- **Описание:** Получает заказ по идентификатору.
	- **Ответы:**
		- 200 OK: Заказ найден
		- 400 Bad Request: Неверный идентификатор заказа
		- 404 Not Found: Заказ не найден
### ProductController
 - **GET /api/product/getproducts**
	- **Описание:** Получает список всех продуктов.
	- **Ответы:**
		- 200 OK: Список продуктов
 - **GET /api/product/getproductbyid/{id}**
	- **Описание:** Получает продукт по идентификатору.
	- **Ответы:**
		- 200 OK: Продукт найден
		- 400 Bad Request: Неверный id
		- 404 Not Found: Продукт не найден
### ShoppingCartController
 - **GET /api/shoppingcart/appendorupdateitemincart**
	- **Описание:** Добавляет или обновляет товар в корзине пользователя.
	- **Параметры:** `userId`, `productId`, `updateQuantity`
	- **Ответы:**
		- 200 OK: Товар добавлен или обновлен
		- 400 Bad Request: Товар не найден

## Дополнительно

Скрипт для удаления всех таблиц в БД:
```
DO $$ DECLARE
    r RECORD;
BEGIN
    FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = 'public') LOOP
        EXECUTE 'DROP TABLE IF EXISTS ' || quote_ident(r.tablename) || ' CASCADE';
    END LOOP;
END $$;
```