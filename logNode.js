// save logs in a dedicated folder in CSV format

var http = require('http');
var fs = require('fs-extra');

http.createServer(function (request, response) {
    writeIP(request.socket.remoteAddress);
    response.writeHead(200, {'Content-Type': 'text/plain'});
    response.end('Hello World\n');
}).listen(8081);

function writeIP(requestIP) {
    var date = new Date();
    var day = date.getDate().toString(10).padStart(2, '0')
    var month = date.getMonth().toString(10).padStart(2, '0');
    var year = date.getFullYear().toString(10);
    var fullDate = day+month+year;

    // append to file
    if (fs.existsSync('logs/'+month+year+'.log')) {
        fs.appendFile("./logs/" + month+year + ".log",
            requestIP+', '+fullDate+ '\n',
            error => {
                if (error) console.log(error);
            });
    }

    // else create and write to file
    if (!fs.existsSync('logs/'+month+year+'.log')) {
        fs.writeFile("./logs/" + month+year + ".log",
            requestIP+', '+fullDate + '\n',
            error => {
                if (error) console.log(error);
            });
    }
}

server.listen(80);
