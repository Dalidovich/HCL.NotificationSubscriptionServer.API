# HCL.NotificationSubscriptionServer.API

Сервис подписки на уведомления для системы статей HCL. Позволяет пользователям подписываться на обновления статей и получать уведомления через Kafka.

## 📌 Основные функции

- Управление отношениями между пользователями (подписки)
- Создание и управление уведомлениями
- Интеграция с Kafka для обработки событий новых статей
- OData API для гибких запросов
- JWT аутентификация
- Логирование в Elasticsearch

## 🛠 Технологический стек

- .NET 7
- PostgreSQL
- Kafka
- Docker
- OData
- JWT
- Elasticsearch + Serilog
- xUnit + FluentAssertions (тестирование)

## 🏗️ Архитектура

Проект следует многослойной архитектуре:

1. **API** - Контроллеры и middleware
2. **BLL** - Бизнес-логика и сервисы
3. **DAL** - Работа с базой данных (Entity Framework Core)
4. **Domain** - Сущности, DTO, enums

## 🚀 Запуск проекта

### Требования
- Docker
- Docker Compose
- .NET 7 SDK (для разработки)

### Запуск через Docker Compose

1. Склонируйте репозиторий
2. Перейдите в директорию проекта
3. Выполните команду:
```bash
docker-compose up -d
```
### Переменные окружения
Основные переменные окружения (устанавливаются в docker-compose.yml):  
- **`ConnectionStrings__NpgConnectionString`** - строка подключения к PostgreSQL  
- **`KafkaSettings__BootstrapServers`** - адрес Kafka сервера  
- **`KafkaSettings__Topic`** - топик Kafka для новых статей  
- **`ElasticConfiguration__Uri`** - адрес Elasticsearch для логов  
- **`JWTSettings:SecretKey`** - ключ для JWT  
- **`JWTSettings:Issuer`** - издатель JWT  
- **`JWTSettings:Audience`** - аудитория JWT  

## 📚 API Endpoints
### Управление отношениями (Relationship)  
- **`POST /api/Relationship/v1/relationship`** - Создать новое отношение
- **`DELETE /api/Relationship/v1/relationship/account`** - Удалить отношение (для владельца)
- **`DELETE /api/Relationship/v1/relationship/admin`** - Удалить отношение (для админа)
- **`GET /api/RelationshipOData/odata/v1/Relationship`** - OData запрос отношений

### Управление уведомлениями (Notification)
- **`DELETE /api/Notification/v1/notification/account`** - Удалить уведомление (для владельца)
- **`DELETE /api/Notification/v1/notification/admin`** - Удалить уведомление (для админа)
- **`GET /api/NotificationOData/odata/v1/Notification`** - OData запрос уведомлений

## 🔧 Тестирование
Проект включает unit и интеграционные тесты:
- Unit тесты для сервисов (NotificationService, RelationshipService)
- Интеграционные тесты для контроллеров
- Тесты для KafkaConsumerService

### Для запуска тестов:
```bash
dotnet test
```
## 📊 База данных
Используется PostgreSQL с двумя основными таблицами:  
- **relationships** - Хранит отношения между пользователями
  - account_master_id - ID ведущего аккаунта
  - account_slave_id - ID ведомого аккаунта
  - relationship_status - статус отношения (Normal/Subscription)
- **notifications** - Хранит уведомления
  - article_id - ID статьи
  - relationship_id - связь с таблицей relationships  
Миграции применяются автоматически при старте через CheckDBHostedService.

## 🎯 Особенности реализации
- Фоновые сервисы для Kafka consumer и проверки БД
- Гибкая система логгирования с Elasticsearch
- Полноценная система авторизации (JWT)
- Поддержка OData для сложных запросов
- Полное покрытие тестами
- Контейнеризация через Docker
