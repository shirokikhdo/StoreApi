# StoreApi

StoreApi � ��� RESTful API ��� ���������� ���������� � ��������. API ������������� ���������������� ��� ������ � ��������, �������� � ��������������.

## ����������

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) - ��������� ��� ����������
- [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) - ��������� ��� �������� ���-����������
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) - ORM ��� ������ � ������ ������
- [PostgreSQL](https://www.postgresql.org/) - ��������-����������� ������� ���������� ������ ������

## ����������

- ��� �������������� � �������������� JWT:
```
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```
- ��� ������������� Identity � Entity Framework Core:
```
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```
- ��� ������������ ���������� Entity Framework Core:
```
dotnet add package Microsoft.EntityFrameworkCore.Tools
```
- ��� ������� Entity Framework Core:
```
dotnet add package Microsoft.EntityFrameworkCore.Design
```
- ��� ������ � PostgreSQL ����� Entity Framework Core:
```
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```
- ��� ��������� OpenAPI (Swagger):
```
dotnet add package Microsoft.AspNetCore.OpenApi
```
- ��� ��������� �������� ������:
```
dotnet add package Bogus
```
- ��� ���������� ������ Swagger:
```
dotnet add package Swashbuckle.AspNetCore
```

## Docker

�������� ���������� � PostgreSQL
```
docker run --name store-api -e POSTGRES_PASSWORD=12345678 -p 5432:5432 -d postgres
```

## ���������

1. ���������� �����������:
```git clone https://github.com/shirokikhdo/StoreApi.git
cd StoreApi
```

2. ������������ �����������:

```
dotnet restore
```

3. �������� ���� ������ � ��������� ��������:

```
dotnet ef database update
```
   
4. ��������� ����������:

```
dotnet run
```

## API Endpoints

### AuthController
 - **POST /api/auth/register**
	- **��������:** ������������ ������ ������������ � �������.
	- **���� �������:** `RegisterRequestDto`
	- **������:**
		- 200 OK: ����������� ���������
		- 400 Bad Request: ������������ ������ ������� ��� ������������ ��� ����������
- **POST /api/auth/login**
	- **��������:** ��������� ���� ������������ � �������.
	- **���� �������:** `LoginRequestDto`
	- **������:**
		- 200 OK: �������� ����
		- 400 Bad Request: �������� email ��� ������
### AuthTestController
 - **GET /api/authtest/test1**
	- **��������:** �������� �����, ��������� ��� ���� �������������.
	- **������:**
		- 200 OK: "Test1: ��� ����"
 - **GET /api/authtest/test2**
	- **��������:** �������� �����, ��������� ������ ��� �������������� �������������.
	- **������:**
		- 200 OK: "Test2: ��� �������������� �������������"
		- 401 Unauthorized: ������������ �� �����������
 - **GET /api/authtest/test3**
	- **��������:** �������� ��� �������������� ������������� � ������� Consumer.
	- **������:**
		- 200 OK: "Test3: ��� �������������� ������������� � ������� Consumer"
		- 401 Unauthorized: ������������ �� �����������
		- 403 Forbidden: ���� ���� admin
 - **GET /api/authtest/test4**
	- **��������:** �������� ��� �������������� ������������� � ������� Admin.
	- **������:**
		- 200 OK: "Test4: ��� �������������� ������������� � ������� Admin"
		- 401 Unauthorized: ������������ �� �����������
		- 403 Forbidden: ���� ���� customer
### OrderController
 - **POST /api/order/createorder**
	- **��������:** ������� ����� �����.
	- **���� �������:** `OrderHeaderCreateDto`
	- **������:**
		- 201 Created: ����� ������� ������
		- 400 Bad Request: �������� ��������� ������ ������
 - **GET /api/order/getorder/{id}**
	- **��������:** �������� ����� �� ��������������.
	- **������:**
		- 200 OK: ����� ������
		- 400 Bad Request: �������� ������������� ������
		- 404 Not Found: ����� �� ������
### ProductController
 - **GET /api/product/getproducts**
	- **��������:** �������� ������ ���� ���������.
	- **������:**
		- 200 OK: ������ ���������
 - **GET /api/product/getproductbyid/{id}**
	- **��������:** �������� ������� �� ��������������.
	- **������:**
		- 200 OK: ������� ������
		- 400 Bad Request: �������� id
		- 404 Not Found: ������� �� ������
### ShoppingCartController
 - **GET /api/shoppingcart/appendorupdateitemincart**
	- **��������:** ��������� ��� ��������� ����� � ������� ������������.
	- **���������:** `userId`, `productId`, `updateQuantity`
	- **������:**
		- 200 OK: ����� �������� ��� ��������
		- 400 Bad Request: ����� �� ������

## �������������

������ ��� �������� ���� ������ � ��:
```
DO $$ DECLARE
    r RECORD;
BEGIN
    FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = 'public') LOOP
        EXECUTE 'DROP TABLE IF EXISTS ' || quote_ident(r.tablename) || ' CASCADE';
    END LOOP;
END $$;
```