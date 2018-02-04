package database

import (
	"encoding/json"
	_ "io/ioutil"
	"os"
	"time"

	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/postgres"
)

var DB *gorm.DB
var err error

type Post struct {
	gorm.Model
	Author string
	Book   string
}

type Data struct {
    Database struct {
        User     string `json:"user"`
		Password string `json:"password"`
		Host string `json:"host"`
    } `json:"database"`
}

func LoadfileJsonDatabase(file string) Data {
	var data Data
	configFile, _ := os.Open(file)
	defer configFile.Close()

	jsonParser := json.NewDecoder(configFile)
	jsonParser.Decode(&data)
	return data
}

func Init() (*gorm.DB, error) {
	// read Json Database File
	database_json := LoadfileJsonDatabase("database.json")

	// set up DB connection and then attempt to connect 5 times over 25 seconds
	connectionParams := "user=" + database_json.Database.User + " password=" + database_json.Database.Password + " sslmode=disable host="+ database_json.Database.Host +""
	for i := 0; i < 5; i++ {
		DB, err = gorm.Open("postgres", connectionParams) // gorm checks Ping on Open
		if err == nil {
			break
		}
		time.Sleep(5 * time.Second)
	}

	if err != nil {
		return DB, err
	}

	// create table if it does not exist
	if !DB.HasTable(&Post{}) {
		DB.CreateTable(&Post{})
	}

	testPost := Post{Author: "Tô Hoài", Book: "Dế mèn Phiêu Lưu Ký"}
	DB.Create(&testPost)
	testPost = Post{Author: "Nguyễn Du", Book: "Truyện Kiều"}
	DB.Create(&testPost)
	testPost = Post{Author: "Hạ Vũ", Book: "Hôm nay tôi thất tình"}
	DB.Create(&testPost)
	testPost = Post{Author: "O.Henry", Book: "Chiếc lá cuối cùng"}
	DB.Create(&testPost)

	return DB, err
}
