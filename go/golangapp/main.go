package main

import (
    "encoding/json"
    "log"
    "net/http"
    "github.com/gorilla/mux"
    "io"
)

type Book struct {
    ID    string   `json:"id"`
    Title string   `json:"Title"`
    Author  string   `json:"Author"`
}

var book []Book

func sayHello(w http.ResponseWriter, r *http.Request) {
    io.WriteString(w, "Hello! Welcome to Golang Webbox!")
}

func GetBook(w http.ResponseWriter, req *http.Request) {
    params := mux.Vars(req)
    for _, item := range book {
        if item.ID == params["id"] {
            json.NewEncoder(w).Encode(item)
            return
        }
    }
    json.NewEncoder(w).Encode(&Book{})
}

func GetAllBook(w http.ResponseWriter, req *http.Request) {
    json.NewEncoder(w).Encode(book)
}

func main() {
    router := mux.NewRouter()
    book = append(book, Book{ID: "1", Title: "Hom nay, Toi That Tinh", Author: "Ha Vu"})
    book = append(book, Book{ID: "2", Title: "5 cm/s", Author: "Shinkai Makoto"})
    router.HandleFunc("/", sayHello)
    router.HandleFunc("/book", GetAllBook).Methods("GET")
    router.HandleFunc("/book/{id}", GetBook).Methods("GET")
    log.Fatal(http.ListenAndServe(":8001", router))
}
