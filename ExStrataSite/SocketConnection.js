class SocketConnection {
    constructor(ip) {
        this.ip = ip;
        this.token = null;
        this.displayName = null;
        this.allApis = null;
        this.dateTimeIndex = null;
        this.dateTimeParams = null;
        this.showLoadedApis();
    }

    showLoadedApis() {
        let that = this;
        let style = this.token===null?'':'display:block';
        if (localStorage.getItem('apiKaarten') != null) {
            $('#APIList').html(localStorage.getItem('apiKaarten'));
        }
        this.request({
            action: 'getLoadedApis'
        }).then(function(apis) {
            console.log(apis);
            let html = '<h2>Klik op een kaart om het patroon te zien</h2>';
            for (let i = 0; i < apis.data.length; i++) {
                let streepje = apis.data[i].instanceInfo == '' ? '' : ' - ';
                html += `<div class='APICard' name='${apis.data[i].name}'>
                <div i='${apis.data[i].index}' style='${style}' class='deleteAPI'>✕</div><div class='info' title='Check interval: ${apis.data[i].displayDelay}'>?</div>
                <h2 class='title'>${apis.data[i].name + streepje + apis.data[i].instanceInfo}</h2>
                <p class='description'>${apis.data[i].description}</p>
                </div>`;
            }
            $('#APIList').html(html);
            $('.APICard').each(function(i) {
                $(this).click(function() {
                    that.getPattern(apis.data[i].name);
                });
            });
            $('.deleteAPI').each(function(i) {
                $(this).click(function(e) {
                    e.stopPropagation();
                    that.request({
                        action: 'removeAPI',
                        index: $(e.target).attr('i'),
                        token: that.token
                    }).then(function(data) {
                        that.showLoadedApis();
                    }, function(error) {
                        console.log(error);
                    });
                });
            });

            localStorage.setItem('apiKaarten', html);
        }, function(error) {
            console.error('There was an error connecting to ' + that.ip, error.error);
        });
    }
    getPattern(name) {
        let that = this;
        if (localStorage.getItem(name) == null) {
            this.request({
                action: 'getPattern',
                patternName: name
            }).then(function(data) {
                console.log(data);
                localStorage.setItem(name, data.data);
                model.fromPattern(data.data);
            }, function(error) {
                console.error('There was an error connecting to ' + that.ip, error.error);
            });
        } else {
            model.fromPattern(localStorage.getItem(name));
        }
    }
    request(data) {
        $('#loading').css('display', 'block');
        let that = this;
        let promise = new Promise(function(resolve, reject) {
            let socket = new WebSocket(that.ip);

            socket.onopen = function() {
                console.info(JSON.stringify(data));
                socket.send(JSON.stringify(data));
            }

            socket.onclose = function() {
                reject("Connection closed");
                $('#loading').css('display', 'none');
            }

            socket.onerror = function(e) {
                reject(e);
                $('#loading').css('display', 'none');
            }

            socket.onmessage = function(e) {
                socket.close();
                let message = JSON.parse(e.data);
                if (message.success) {
                    resolve(message);
                } else {
                    reject(message);
                }
                $('#loading').css('display', 'none');
            }
        });
        return promise;
    }
}
