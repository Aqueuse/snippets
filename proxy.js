var bouncy = require('bouncy');

var server = bouncy(function (req, res, bounce) {
    if (req.headers.host === 'mehari-consulting.com') {
        bounce(3001);
    }
    if (req.headers.host === 'ours-agile.com') {
        bounce(3000);
    }
});

server.listen(80);