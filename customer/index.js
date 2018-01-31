const express = require('express')
const http = require('http')
// Constants
const PORT = 8000
const HOST = '0.0.0.0'

// App
const app = express()

app.use(express.static('public'))
app.get('/', (req, res) => {
  res.send('<h1>Customer REST API by Node.js</h1>')
})

app.get('/list', (req, res) => {
  res.json([{
    "first_name": "Giana",
    "last_name": "Stovine",
    "email": "gstovine0@over-blog.com"
  }, {
    "first_name": "Corrianne",
    "last_name": "Pickin",
    "email": "cpickin1@jigsy.com"
  }, {
    "first_name": "Alano",
    "last_name": "Pettiford",
    "email": "apettiford2@icq.com"
  }, {
    "first_name": "Barton",
    "last_name": "Cotmore",
    "email": "bcotmore3@github.com"
  }, {
    "first_name": "Elijah",
    "last_name": "Ivamy",
    "email": "eivamy4@unc.edu"
  }])
})
const server = http.createServer(app)
server.listen(PORT, HOST, ()=> {
 
})
console.log(`Running on http://${HOST}:${PORT}`)