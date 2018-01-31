# Cấu trúc
Thư mục api chưa mã nguồn Golang. Thư mục vendor chưa code kết nối vào Postgresql.
Trong file docker_compose.yml tạo ra một Postgresql server có host name là go_db

# Chạy thử
Kéo source code về, build và run bằng docker-compose:
```
cd go/api_db
docker-compose up -d
curl http://localhost:8001/books
```

# Cần cải tiến thêm
- Hãy chia việc build container chứa REST server là 'api_book' thành 2 bước:
  - Bước 1: sử dụng container golang:alpine có đủ compiler để biên dịch
  - Bước 2: copy file binary biên dịch bước 1 vào container sử dụng image gọn hơn là alpine:latest
Tham khảo [Docker multi stage build](https://docs.docker.com/engine/userguide/eng-image/multistage-build/)
- Truyền tham số động vào container chứ không viết cứng nhắc trong file mã nguồn go.
Đoạn code trong file go/api_db/api/vendor/database/init.go rất khó bảo trì
```go
  connectionParams := "dbname=" + dbname + " user=docker password=docker sslmode=disable host=go_db"
```