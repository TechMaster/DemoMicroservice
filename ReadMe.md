# Demo hệ thống Microservice

Hệ thống sẽ gồm có các container như sau:
1. Nginx đóng vai trò reverse proxy: thầy Cường
2. Node app cổng 8000 kết nối vào CSDL Mongo DB: cô Linh
3. Golang app cổng 8001 kết nối vào CSDL Postgresql: chú Long
4. ASP.net MVC Core cổng 8002 kết nối vào CSDL Microsoft SQL Server 2017: thầy Huy

```
                           +-----------------+           +-------------+
                 /customer/|                 |           |             |
                 +--------->  nodeapp:8000   +----------->  mongoDB    |
                 |         |                 |           |             |
                 |         +-----------------+           +-------------+
                 |
+-------------+  |         +-----------------+           +-------------+
|             |  |/book/   |                 |           |             |
| Nginx Proxy +------------>  goapp:8001     +----------->  PostgreSQL |
|             |  |         |                 |           |             |
+-------------+  |         +-----------------+           +-------------+
                 |
                 |         +-----------------+           +-------------+
                 |/blog/   |                 |           |             |
                 +--------->  asp.net:8002   +----------->  MS-SQL2017 |
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
   +---+nginx
   |
   +---+node
   |
   +---+go
   |
   +---+net
   |
   +---+documents
   |
   +---+docker-compose.yml
```

## Đọc kỹ hướng dẫn sử dụng trước khi dùng!

Đầu tiên bạn phải clone mã nguồn về đã. Nhớ là chúng tôi code trên Mac và Linux, nên không dám đảm bảo code, hướng dẫn chạy ổn trên Windows không. Nhưng mà chắc là chạy được đó!

```
git clone https://github.com/TechMaster/DemoMicroservice.git
```
Kiểm tra NodeApp container chạy ok không thì vào thư mục node, chạy file build.sh nếu có dữ
liệu trả về là ok
```
cd node\
.\build.sh
```