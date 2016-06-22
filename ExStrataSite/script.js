$(document).ready(initialize);

function initialize() {
    //Verander de ip hieronder naar de ip van de ExStrataServer
    connection = new SocketConnection('ws://192.168.178.14:912');

    dayPicker = new DayPicker('datePicker', function(day, month) {
        $('#dateTimePrompt').css('display', 'block');
        connection.dateTimeParams[1].Value = Number(month);
        connection.dateTimeParams[2].Value = Number(day);
    });
    timePicker = new TimePicker('#timePicker', function(hour, minute) {
        $('#dateTimePrompt').css('display', 'block');
        connection.dateTimeParams[3].Value = Number(hour);
        connection.dateTimeParams[4].Value = Number(minute);
    });

    model = new ExStrataModel();

    Picker.colorPicker('#colorPicker');
    $(document).click(closePrompt);
    $('.prompt, #colorful a').click(function(e) {
        e.stopPropagation();
    });
    $('.prompt').on('keyup', function(e) {
        if (e.which == 13)
            login();
    });
    msgopen = false;
}

function addDateTimeApi() {
    let colour = Picker.hexToRgb($('#colorPicker').attr('colour'));
    connection.dateTimeParams[5].Value = `${colour.r},${colour.g},${colour.b}`;

    connection.request({
        action: 'addApi',
        index: connection.dateTimeIndex,
        parameters: connection.dateTimeParams,
        token: connection.token
    }).then(function(apis) {
        $('#dateTimePrompt').css('display', 'none');
        connection.showLoadedApis();
    }, function(data) {
        console.error('There was an error connecting to ' + connection.ip, data.error);
        console.log('Parameters: ', connection.dateTimeParams);
        msg("Error: " + data.error);
    });
}

function loginPrompt() {
    $('#controlPanel, #renderView').css('-webkit-filter', 'blur(5px)');
    $('.prompt').css('opacity', '1');
    $('.prompt').css('transform', 'scale(1)');
    $('.prompt').css('pointer-events', 'all');
    $('.prompt input').eq(0).select();
}

function closePrompt() {
    $('#controlPanel, #renderView').css('-webkit-filter', 'blur(0px)');
    $('.prompt').css('opacity', '0');
    $('.prompt').css('transform', 'scale(.6)');
    $('.prompt').css('pointer-events', 'none');
}

function login() {
    let username = $('#username').val();
    let password = $('#password').val();
    console.log(username, password);

    if (username != '' && password != '') {
        console.log('requesting login');
        connection.request({
            action: 'login',
            user: username,
            pass: password
        }).then(function(info) {
            console.log(info);
            closePrompt();
            connection.token = info.token;
            connection.displayName = info.displayName;
            showControls(info);
        }, function(error) {
            console.error(error.error);
            msg('Inloggen mislukt');
        });
    } else {
        msg("Je hebt een veld niet ingevuld");
        closePrompt();
        showControls();
    }
}

function showControls(info) {
    $('[onclick="loginPrompt()"]').css('display', 'none');
    $('#controlPanel').append(`<h2>Server controls</h2>`);
    $('#addApiCard').remove();
    $('#controlPanel').append(`<div id='addApiCard' class='newCard'></div>`);
    $('#instantControl').remove();
    $('#controlPanel').append(`<div id='instantControl' class='newCard'></div>`);

    connection.request({
        action: 'getAllApis',
        token: connection.token
    }).then(function(apis) {
        console.log(apis);
        connection.allApis = apis;
        let html = `<h2 class='title'>Voeg een API toe</h2>`;
        let html2 = `<h2 class='title'>Direct control</h2>`;
        apis.data.forEach(function(elem) {
            html += `<div onclick='addApi(${elem.index}, event)' i='${elem.index}' class="newAPI">${elem.name}</div>`;
            html2 += `<div onclick="playApi('${elem.name}')" i='${elem.index}' class="newAPI">${elem.name}</div>`;
        });
        $('.deleteAPI').css('display', 'block');
        $('#addApiCard').html(html);
        $('#instantControl').html(html2);
    }, function(data) {
        console.error('There was an error connecting to ' + connection.ip, data.error);
    });

    $('#logList').remove();
    $('#controlPanel').append(`<div id='logList' class='newCard'></div>`);

    connection.request({
        action: 'listLogs',
        token: connection.token
    }).then(function(logs) {
        console.log(logs);
        let html = `<h2 class='title'>Server logs</h2>`;
        logs.logs.forEach(function(log) {
            html += `
            <div class='newAPI' onclick="openLog('${log}')">${log.split('\\')[log.split('\\').length-1]}</div>
        `;
        });
        $('#logList').html(html);

    }, function(data) {
        console.error('There was an error connecting to ' + connection.ip, data.error);
    });
}

function playApi(pattern) {
    console.log(pattern);
    connection.request({
        pattern: pattern,
        token: connection.token,
        action: 'directControl'
    }).then(function(succes) {
        msg('Pattern is succesvol afgespeeld');
        console.log(succes);
    }, function(error) {
        msg('Pattern kon niet afgespeeld worden');
        console.log(error);
    })
}

function openLog(file) {
    connection.request({
        action: 'readlog',
        token: connection.token,
        filename: file
    }).then(function(log) {
        console.log(log);
        let blob = new Blob([log.log], { type: 'text/plain' });
        window.open(URL.createObjectURL(blob));
    }, function(data) {
        console.error('There was an error connecting to ' + connection.ip, data.error);
    });
}

function addApi(index, e) {
    let apiName = e.target.innerText;
    let params = connection.allApis.data[index].parameters;
    let cont = true;

    if (apiName != 'DateTime') {
        params.forEach(function(elem, index) {
            if (cont)
                params[index].Value = prompt('Vul een ' + params[index].Name + ' in.');
            if (!params[index].Value)
                cont = false;
        });
    } else {
        cont = false;
        params.map((p) => p.Value = -1);
        params[0].Value = 60;
        connection.dateTimeParams = params;
        connection.dateTimeIndex = index;
        $('#dateTimePrompt').css('display', 'block');
    }

    if (cont) {
        connection.request({
            action: 'addApi',
            index: index,
            parameters: params,
            token: connection.token
        }).then(function(apis) {
            console.info("added api", apis);
            connection.showLoadedApis();
        }, function(data) {
            console.error('There was an error connecting to ' + connection.ip, data.error);
        });
    }
}

function msg(str) {
    if (!msgopen) {
        msgopen = true;
        $('#message').html(str);
        $('#message').css('bottom', '0%');
        t = setTimeout(function() {
            $('#message').css('bottom', '-100%');
            msgopen = false;
        }, str.length * 50 + 2000);
    } else {
        clearTimeout(t);
        msgopen = false;
        msg(str);
    }
}
