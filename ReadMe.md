# Demo hệ thống Microservice

Hệ thống sẽ gồm có các container như sau:
1. Nginx đóng vai trò reverse proxy, host: api_gateway
2. Node.js REST, host api_customer, phục vụ ở cổng 8000, kết nối vào CSDL Mongo DB
3. Golang REST, host api_book, phục vụ ở cổng 8001, kết nối vào CSDL Postgresql host go_db
4. ASP.net MVC Core REST, host course,cổng 8002 kết nối vào CSDL Microsoft SQL Server 2017, host 

```
                           +-----------------+           +-------------+
                 /customer/|                 |           |             |
                 +--------->  Node.js:8000   +----------->  mongoDB    |
                 |         |                 |           |             |
                 |         +-----------------+           +-------------+
                 |
+-------------+  |         +-----------------+           +-------------+
|             |  |/book/   |                 |           |             |
| Nginx Proxy +------------>  Golang:8001    +----------->  PostgreSQL |
|             |  |         |                 |           |             |
+-------------+  |         +-----------------+           +-------------+
                 |
                 |         +-----------------+           +-------------+
                 |/blog/   |                 |           |             |
                 +--------->  Asp.net:8002   +----------->  MS-SQL2017 |
                           |                 |           |             |
                           +-----------------+           +-------------+
                                    X
 +------+proxy: network+-----------+X+----------+db: network+----------+
                                    X
                                    X

```

## Cấu trúc thư mục
```
Git Repo
   +
   |
   +---+gateway
   |
   +---+customer
   |
   +---+book
   |
   +---+course
   |
   +---+documents
   |
   +---+docker-compose.yml
```

## Đọc kỹ hướng dẫn sử dụng trước khi dùng!

Đầu tiên bạn phải clone mã nguồn về đã. Nhớ là chúng tôi code trên Mac và Linux, nên không dám đảm bảo code, hướng dẫn chạy ổn trên Windows không. Nhưng mà chắc là chạy được đó!

```
git clone https://github.com/TechMaster/DemoMicroservice.git
docker-compose up -d
```
Lệnh trên sẽ khởi động gateway và các REST service.

Kiểm tra NodeApp container chạy ok không thì vào thư mục node, chạy file build.sh nếu có dữ
liệu trả về là ok
```
cd node\
.\build.sh
```