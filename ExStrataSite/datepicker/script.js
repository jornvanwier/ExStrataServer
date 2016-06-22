const months = [{name: 'Januari', days: 31},{name: 'Februari', days: 28 },{name: 'Maart', days: 31 },{name: 'April', days: 30 },{name: 'Mei', days: 31 },{name: 'Juni', days: 30 },{name: 'Juli', days: 31 },{name: 'Augustus', days: 31 },{name: 'September', days: 30 },{name: 'Oktober', days: 31 },{name: 'November', days: 30 },{name: 'December', days: 31 }];
daypickerCallbacks=[];

class DayPicker{
	constructor(id, onSelected){
		this.now = new Date();
		this.leapyear=false;
		if(this.now.getFullYear()%4===0){
			months[1].days=29;
			this.leapyear=true;
		}

		this.element = $('#'+id);
		this.element.attr('onclick',"openWindow(event, '"+id+"')");

		let monthsHtml = "";
		for (let i = 0; i < months.length; i++) {
			monthsHtml+=`<div class='month' i="${i}" onclick="chooseMonth(event)">${months[i].name}</div>`;
		}

		$('body').append(`
		<div class='dayPicker' name='${id}'>
			<div class='months'>
				<p class='monthTitle'>Kies een maand</p>
				${monthsHtml}
			</div>
		</div>`);

		$("[name='"+id+"']").mousedown(function(e){
			e.stopPropagation();
		});

		daypickerCallbacks.push({
			name:id,
			call:onSelected
		});

	}
}
$(document).mousedown(function(e){
	$('.dayPicker').css('display','none');
});

function openWindow(e, id) {
	id='[name="'+id+'"]';
	$(id).css('left',e.pageX);
	$(id).css('top',e.pageY);
	$(id).css('display','inline-block');
}
function closeWindow(e, day, month) {
	$(e.target).parent().parent().css('display','none');
}

function chooseMonth(e) {
	let month=Number($(e.target).attr('i'));
	let id='[name="'+$(e.target).parent().parent().attr('name')+'"]';
	$(id).removeAttr('date-string');
	$(id).removeAttr('day');
	$(id+' .day').css('background-color','transparent');
	$(id+' .day').css('color','black');
	$(id).attr('month',month+1);
	
	let daysInMonth=months[month].days;
	let daysHtml = "";
	for (var i = 0; i < daysInMonth; i++) {
		daysHtml+=`<div onclick='chooseDay(event)' class='day'>${i+1}</div>`;
	}

	let x=e.pageX - $(id).offset().left,
		y=e.pageY - $(id).offset().top;

	x/=$(id).width();
	y/=$(id).height()+40;

	$(id).append(`
		<div class='days'>
			<input type='button' class='dateBack' onclick="backMonth(event, ${x}, ${y})" value='<'></input>
			<p class='dayTitle'>${$(e.target).text()}</p><br>
			${daysHtml}
		</div>`);

	zoomIn(id, x, y);
}

function backMonth(e, x, y) {
	let id='[name="'+$(e.target).parent().parent().attr('name')+'"]';

	zoomOut(id, x, y);
}

function zoomIn(id, x, y) {

	$(id+' .days').css('transform-origin', x*100+'% '+y*100+'%');
	$(id+' .months').css('transform-origin', x*100+'% '+y*100+'%');

	$(id+' .months').css('transition','transform 1s, opacity .5s');

	setTimeout(function() {
		$(id+' .months').css('transform','scale(4)');
		$(id+' .months').css('opacity','0');
		$(id+' .months').css('pointer-events','none');
	}, 10);
	
	setTimeout(function() {
		$(id+' .days').css('opacity', '1');
		$(id+' .days').css('transform', 'scale(1)');
		$(id+' .months').css('pointer-events','all');
	}, 10);

}
function zoomOut(id, x, y) {

	$(id+' .days').css('transform-origin', x*100+'% '+y*100+'%');
	$(id+' .months').css('transform-origin', x*100+'% '+y*100+'%');

	$(id+' .months').css('transition','transform .4s, opacity .5s');
	setTimeout(function() {
		$(id+' .months').css('transform','scale(1)');
		$(id+' .months').css('opacity','1');
		$(id+' .months').css('pointer-events','all');
	}, 10);


	
	setTimeout(function() {
		$(id+' .days').css('opacity', '0');
		$(id+' .days').css('transform', 'scale(0.1)');
		$(id+' .days').css('pointer-events','none');
		setTimeout(function() {
			$(id+' .days').eq(0).remove();
		}, 500);
	}, 10);

}

function chooseDay(e) {
	let day=$(e.target).text();

	let id='[name="'+$(e.target).parent().parent().attr('name')+'"]';
	$(id).attr('day',day);

	$(id+' .day').css('background-color','inherit');
	$(id+' .day').css('color','black');
	$(id+' .day').eq(day-1).css('background-color','#4b97d8');
	$(id+' .day').eq(day-1).css('color','white');

	let month=Number($(id).attr('month'));

	$(id).attr('date-string', day+' '+months[month-1].name);
	closeWindow(e, day, month);
	let name=$(e.target).parent().parent().attr('name');
	for (var i = 0; i < daypickerCallbacks.length; i++) {
		if(daypickerCallbacks[i].call!==undefined && daypickerCallbacks[i].name==name){
			daypickerCallbacks[i].call(day, month);
		}
	}
}