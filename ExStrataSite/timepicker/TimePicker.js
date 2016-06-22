class TimePicker {
    constructor(selector, callback, radius = 100) {
        let that = this;

        this.selectedHour = 12;
        this.selectedMinute = 0;

        this.callback = callback;
        this.element = document.querySelector(selector);

        this.infoElement = document.createElement('span');
        this.infoElement.textContent = this.element.innerHTML;

        this.element.innerHTML = '';
        this.element.appendChild(this.infoElement);
        this.shadowElement = document.createElement('div');
        this.element.appendChild(this.shadowElement);
        this.shadowRoot = this.shadowElement.createShadowRoot();

        this.shadowRoot.innerHTML += `
            <style>
            </style>
            <content>
                <div id='picker'>
                </div>
            </content>
        `;

        this.shadow = {
            content: this.shadowRoot.getElementById('picker'),
            style: this.shadowRoot.querySelector('style')
        };

        this.shadow.style.innerHTML = `
            #picker{
                font-size:medium !important;
                position: fixed;
                width:${radius * 2}px;
                height:${radius * 2}px;
                overflow: hidden;
                background-color:white;
                border-radius:50%;
                box-shadow:0px 2px 3px 0px rgba(0,0,0,0.2);
                padding:20px;
                transform:scale(0.1);
                opacity:0;
                pointer-events:none;
                transition: opacity .2s, transform .2s;
            }
            #picker>div, #hours>div {
                position: absolute;
                top: 0;
                left: 0;
                transform-origin:${radius + 20}px ${radius + 20}px;
            }

            .clockNum {
                display: inline-block;
                position: absolute;
                top: ${radius + 10}px;
                width: 20px;
                height: 20px;
                left: ${radius + 10}px;
                text-align: center;
                border-radius:50%;
                cursor:pointer;
                transition:.05s;
            }
            .clockNum:hover{
                background-color: #4b97d8;
                box-shadow: 0px 0px 0px 10px rgba(75, 151, 216, 1);
                color: white !important;
            }

            #smallNums .clockNum{
                color: #aaaaaa;
            }
            #minutes, #hours{
                transition: transform 0.3s;
            }
            #minutes{
                pointer-events: none;
                transform: scale(.5);
                opacity: 0;
            }
            #minutes .clockNum{
                transition: background-color 0.05s, color 0.05s, box-shadow 0.05s;
            }
            #minutes .clockNum:hover{
                opacity:1 !important;
            }
        `;


        this.shadow.content.innerHTML = `
            <div id='hours'>
                <div style='transform: scale(0.95)' id='bigNums'></div>
                <div style='transform: scale(0.7)' id='smallNums'></div>
            </div>
            <div id='minutes'>
            </div>`;


        this.hoursElement = this.shadowRoot.querySelector('#hours');
        this.minutesElement = this.shadowRoot.querySelector('#minutes');

        let bigHTML = '',
            smallHTML = '',
            minutesHTML = '';

        for (let num = 1; num < 13; num++) {
            bigHTML += `<div style="transform: translate(${ radius * Math.sin(Math.PI * ((12 - num) / 6) + Math.PI) }px, ${ radius * Math.cos(Math.PI * ((12 - num) / 6) + Math.PI) }px) scale(${radius * 0.015})" class='clockNum'>${num}</div>`;
            smallHTML += `<div style="transform: translate(${ radius * Math.sin(Math.PI * ((12 - num) / 6) + Math.PI) }px, ${ radius * Math.cos(Math.PI * ((12 - num) / 6) + Math.PI) }px) scale(${radius * 0.016})" class='clockNum'>${(num+12)%24}</div>`;
        }

        for (let num = 0; num < 60; num++) {
            minutesHTML += `<div style="opacity: ${num%5===0?1:0}; transform: translate(${ radius * Math.sin(Math.PI * ((60 - num) / 30) + Math.PI) }px, ${ radius * Math.cos(Math.PI * ((60 - num) / 30) + Math.PI) }px) scale(${radius * 0.01})" class='clockNum'>${num}</div>`;
        }

        this.shadowRoot.querySelector('#bigNums').innerHTML = bigHTML;
        this.shadowRoot.querySelector('#smallNums').innerHTML = smallHTML;
        this.minutesElement.innerHTML = minutesHTML;

        let elems = this.shadowRoot.querySelectorAll('#bigNums>div, #smallNums>div');
        for (let i = 0; i < elems.length; i++) {
            elems[i].addEventListener('click', function(e) {
                that.selectedHour = e.target.innerText;
                that.showMinutes();
            });
        }

        elems = this.shadowRoot.querySelectorAll('#minutes>div');
        for (let i = 0; i < elems.length; i++) {
            elems[i].addEventListener('click', function(e) {
                that.selectedMinute = e.target.innerText;
                callback(that.selectedHour, that.selectedMinute);
                that.resetHours();
                that.closePicker();
            });
        }



        document.addEventListener('click', function(e) {
            that.closePicker();
        });

        this.element.addEventListener('mousedown', function(e) {
            that.openPicker();
            that.shadow.content.style.top = e.pageY - (radius + 20);
            that.shadow.content.style.left = e.pageX - (radius + 20);
        });

        this.shadow.content.addEventListener('mousedown', function(e) {
            e.stopPropagation();
        });
        this.element.addEventListener('click', function(e) {
            e.stopPropagation();
        });
    }

    showMinutes() {
        this.hoursElement.style.pointerEvents = 'none';
        this.hoursElement.style.opacity = '0';
        this.hoursElement.style.transform = 'scale(3)';

        this.minutesElement.style.pointerEvents = 'all';
        this.minutesElement.style.opacity = '1';
        this.minutesElement.style.transform = 'scale(1)';
    }
    resetHours() {
        this.minutesElement.style.pointerEvents = 'none';
        this.minutesElement.style.opacity = '0';
        this.minutesElement.style.transform = 'scale(0.5)';

        this.hoursElement.style.pointerEvents = 'all';
        this.hoursElement.style.opacity = '1';
        this.hoursElement.style.transform = 'scale(1)';
    }

    closePicker() {
        this.shadow.content.style.pointerEvents = 'none';
        this.shadow.content.style.opacity = '0';
        this.shadow.content.style.transform = 'scale(0.1)';
    }

    openPicker() {
        this.shadow.content.style.pointerEvents = 'all';
        this.shadow.content.style.opacity = '1';
        this.shadow.content.style.transform = 'scale(1)';
    }

    pickHour() {
        alert(this);
    }
}
