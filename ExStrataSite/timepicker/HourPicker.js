class HourPicker {
    constructor(selector, callback, radius = 100) {
        let that = this;
        this.callback = callback;
        this.element = document.querySelector(selector);

        this.infoElement = document.createElement('span');
        this.infoElement.textContent = this.element.innerHTML;
        this.infoElement.style.all = 'inherit';

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
                position: absolute;
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
            #picker>div, #picker>div>div {
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

            #minutes{
                pointer-events: none;
                transform: scale(.5);
                opacity: 0;
            }
        `;


        this.shadow.content.innerHTML = `
            <div id='hours'>
                <div style='transform: scale(0.95)' id='bigNums'></div>
                <div style='transform: scale(0.7)' id='smallNums'></div>
            </div>
            <div id='minutes'>
            </div>`;

        let bigHTML = '',
            smallHTML = '';

        for (let num = 1; num < 13; num++) {
            bigHTML += `<div style="transform: translate(${ radius * Math.sin(Math.PI * ((12 - num) / 6) + Math.PI) }px, ${ radius * Math.cos(Math.PI * ((12 - num) / 6) + Math.PI) }px) scale(${radius * 0.015})" class='clockNum'>${num}</div>`;
            smallHTML += `<div style="transform: translate(${ radius * Math.sin(Math.PI * ((12 - num) / 6) + Math.PI) }px, ${ radius * Math.cos(Math.PI * ((12 - num) / 6) + Math.PI) }px) scale(${radius * 0.016})" class='clockNum'>${(num+12)%24}</div>`;
        }

        this.shadowRoot.querySelector('#bigNums').innerHTML = bigHTML;
        this.shadowRoot.querySelector('#smallNums').innerHTML = smallHTML;

        for(let elem of this.shadowRoot.querySelectorAll('#bigNums>div, #smallNums>div')){
            elem.addEventListener('click', function(e) {
                that.callback(e.target.innerText);
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
