package main

import (
	"database"
	"encoding/json"
	"fmt"
	"github.com/julienschmidt/httprouter"
	"log"
	"net/http"
)


func showPostHandler(w http.ResponseWriter, r *http.Request, ps httprouter.Params) {
	setCors(w)
	var post database.Post
	database.DB.Where("ID = ?", ps.ByName("bookID")).First(&post)
	res, err := json.Marshal(post)
	if err != nil {
		http.Error(w, err.Error(), 500)
		return
	}

	w.Write(res)
}

func indexPostHandler(w http.ResponseWriter, r *http.Request, _ httprouter.Params) {
	setCors(w)
	var posts []database.Post
	database.DB.Find(&posts)
	res, err := json.Marshal(posts)
	if err != nil {
		http.Error(w, err.Error(), 500)
		return
	}

	w.Write(res)
}

func indexHandler(w http.ResponseWriter, r *http.Request, _ httprouter.Params) {
	setCors(w)
	fmt.Fprintf(w, "This is the api")
}

func setCors(w http.ResponseWriter) {
	w.Header().Set("Access-Control-Allow-Methods", "GET")
	w.Header().Set("Access-Control-Allow-Headers", "Content-Type")
}



func main() {
	defer database.DB.Close()

	// add router and routes
	router := httprouter.New()
	router.GET("/", indexHandler)
	router.GET("/books/:bookID", showPostHandler)
	router.GET("/books", indexPostHandler)

	// add database
	_, err := database.Init()
	if err != nil {
		log.Println("connection to DB failed, aborting...")
		log.Fatal(err)
	}

	log.Println("connected to DB")


	http.ListenAndServe(":8001", router)
}
