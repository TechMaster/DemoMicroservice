# Hướng dẫn
----

## Cài đặt
- Download và cài đặt Docker: https://docs.docker.com/install/#supported-platforms.
- Kéo source code về, build và run bằng **docker-compose**:
```
  cd net
  docker-compose up -d
```
- Truy cập website ở cổng 8002.

## Cấu hình
- Project build qua Docker image *microsoft/aspnetcore-build:2.0*, run qua Docker image *microsoft/aspnetcore:2.0*, cấu hình ở **Dockerfile**.
- Cấu hình running port và cơ sở dữ liệu (*microsoft/mssql-server-linux:2017-latest*) trong **docker-compose.yml**.
- Cấu hình kết nối cơ sở dữ liệu trong file **appsettings.json**, dữ liệu mockup là file **seed/posts.json**.
