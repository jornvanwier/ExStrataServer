$(document).ready(function(){
	ids=[];
	gradient=[];
	grd=[];
	picker=[];
	pck=[];
	currentColors=[];
	previd='';
	colx=[];
	coly=[];
	checkers=[];
	sizes=[];
	pickerSize=30;
});
Picker={};
Picker.initialize = function(id) {
	ids.push(id);
	var index=ids.length-1;
	graddown=false;
	pickdown=false;
	if(typeof $(id).attr('colour')!='undefined'){
		currentColors.push($(id).attr('colour').toUpperCase());
	}else{
		currentColors.push('#FF0000');
		$(id).attr('colour','#FF0000');
	}
	$(ids[index]+' .currentcolor').css('background-color',currentColors[index]);
	$(ids[index]+' .newcolor').css('background-color',currentColors[index]);
	var rgb=Picker.hexToRgb(currentColors[index]);
	var hsb=Picker.rgbToHsb(rgb.r,rgb.g,rgb.b);
	if(hsb[2]>0.5&&hsb[1]<0.5){
		$(id+' .newcolor').css('color','black');
		$(id+' .currentcolor').css('color','black');
	}else{
		$(id+' .newcolor').css('color','white');
		$(id+' .currentcolor').css('color','white');
	}

	$(ids[index]+' .colorids label>input').eq(0).val(rgb.r);
	$(ids[index]+' .colorids label>input').eq(1).val(rgb.g);
	$(ids[index]+' .colorids label>input').eq(2).val(rgb.b);
	$(ids[index]+' .colorids>input').eq(0).val(currentColors[index]);

	colx[index]=$(ids[index]+' .gradient')[0].height;
	coly[index]=1;

	//init gradient
	gradient.push($(ids[index]+' .gradient')[0]);
	$(ids[index]+' .gradient').attr('i',ids[index]);
	grd.push(gradient[index].getContext('2d'));
	overloop = grd[index].createLinearGradient(0,0,0,gradient[index].height);
	overloop.addColorStop(0,		"rgb(255,0,0)");
	overloop.addColorStop(1/(7-1)*1,"rgb(255,0,255)");
	overloop.addColorStop(1/(7-1)*2,"rgb(0,0,255)");
	overloop.addColorStop(1/(7-1)*3,"rgb(0,255,255)");
	overloop.addColorStop(1/(7-1)*4,"rgb(0,255,0)");
	overloop.addColorStop(1/(7-1)*5,"rgb(255,255,0)");
	overloop.addColorStop(1,		"rgb(255,0,0)");
	grd[index].fillStyle = overloop;
	grd[index].fillRect(0,0,300,gradient[index].height);

	//init picker
	picker.push($(ids[index]+' .picker')[0]);
	$(ids[index]+' .picker').attr('i',ids[index]);
	pck.push(picker[index].getContext("2d"));
	pck[index].fillStyle=currentColors[index];
	pck[index].fillRect(0,0,picker[index].width,picker[index].height);


	$(ids[index]+' .gradient').mousedown(function(e){
		graddown=true;
		Picker.pickColor(e);
	});


	$(ids[index]+' .picker').mousedown(function(e){
		pickdown=true;
		Picker.pickColor(e);
	});

	$(document).mouseup(function(){
		pickdown=false;
		graddown=false;
	});
	$(document).mousemove(Picker.pickColor);
	$(ids[index]+' .colorids label input').keydown(function(e){
		Picker.updateRgb(e);
	});
	Picker.updateHex(ids[index]);

	$(id + ' .currentcolor').click();
}
Picker.pickColor = function(e) {
	var id,index;
	if(typeof $(e.originalEvent.target).attr('i')!='undefined'){
		id=$(e.originalEvent.target).attr('i');
		index=ids.indexOf(id);
	}else{
		id=previd;
		index=ids.indexOf(id);
	}
	if(graddown){
		y=e.pageY-$(id+' .gradient').offset().top;
		let d =grd[index].getImageData(0, y-1, 1, 1).data;
		var r=d[0];
		var g=d[1];
		var b=d[2];
		if(y<1){
			r=255;
			g=0;
			b=0;
			y=1;
		}
		if(y>gradient[index].height){
			y=gradient[index].height;
			r=255;
			g=0;
			b=0;
		}
		Picker.createGradients(index, 'rgb('+r+','+g+','+b+')');
		$(id+' .line').css('transform','translate('+(picker[index].width+20)+'px,'+(-(gradient[index].height+2)+y)+'px)');

		var r=pck[index].getImageData(colx[index]-1, coly[index]-1, 1, 1).data[0];
		var g=pck[index].getImageData(colx[index]-1, coly[index]-1, 1, 1).data[1];
		var b=pck[index].getImageData(colx[index]-1, coly[index]-1, 1, 1).data[2];

		if(colx[index]==1&&coly[index]==1)
			r=255,g=255,b=255;
		$(id+' .colorids label>input').eq(0).val(r);
		$(id+' .colorids label>input').eq(1).val(g);
		$(id+' .colorids label>input').eq(2).val(b);
		$(id+' .colorids>input').eq(0).val(Picker.rgbToHex(r,g,b));
		
		var hsb=Picker.rgbToHsb(r,g,b);
		if(hsb[2]>0.5&&hsb[1]<0.5){
			$(id+' .newcolor').css('color','black');
		}else{
			$(id+' .newcolor').css('color','white');
		}
		$(id+' .newcolor').css('background-color','rgb('+r+','+g+','+b+')');
	}
	if(pickdown){
		y=e.pageY-$(id+' .picker').offset().top;
		x=e.pageX-$(id+' .picker').offset().left;

		y1=false;
		x1=false;
		y2=false;
		x2=false;
		if(y<=0){
			y=1;
			y1=true;
		}else if(y>=picker[index].height){
			y=picker[index].height;
			y2=true;
		}
		if(x<=0){
			x=1;
			x1=true;
		}else if(x>picker[index].width){
			x=picker[index].width;
			x2=true;
		}
		coly[index]=y;
		colx[index]=x;
		var r=pck[index].getImageData(x-1, y-1, 1, 1).data[0];
		var g=pck[index].getImageData(x-1, y-1, 1, 1).data[1];
		var b=pck[index].getImageData(x-1, y-1, 1, 1).data[2];
		if(x1&&y1){
			r=255;
			g=255;
			b=255;
		}else if(x2&&y2){
			r=0;
			g=0;
			b=0;
		}
		$(id+' .colorids label>input').eq(0).val(r);
		$(id+' .colorids label>input').eq(1).val(g);
		$(id+' .colorids label>input').eq(2).val(b);
		$(id+' .colorids>input').eq(0).val(Picker.rgbToHex(r,g,b));
		$(id+' .dot').css('transform','translate('+(-10+x)+'px,'+(-(gradient[index].height+5)+y)+'px)');
		$(id+' .newcolor').css('background-color','rgb('+r+','+g+','+b+')');
		var hsb=Picker.rgbToHsb(r,g,b);
		if(hsb[2]>0.5&&hsb[1]<0.5){
			$(id+' .newcolor').css('color','black');
		}else{
			$(id+' .newcolor').css('color','white');
		}
	}
	previd=id;
}
Picker.updateRgb = function(e) {
	var id='#'+$(e.target).parent().parent().parent().attr('id');
	var index=ids.indexOf(id);
	
	var r=$(id+' .colorids label>input').eq(0).val();
	var g=$(id+' .colorids label>input').eq(1).val();
	var b=$(id+' .colorids label>input').eq(2).val();
	hsb=Picker.rgbToHsb(r,g,b);

	if(hsb[0]==hsb[1]==0){
		y=(1-hsb[0])*gradient[index].height+.1;
		if(y>gradient[index].height){
			y=gradient[index].height;
		}
		if(y<=1){
			y=1;
		}
		let d=grd[index].getImageData(0, y-1, 1, 1).data;
		var rg=d[0];
		var gg=d[1];
		var bg=d[2];

		Picker.createGradients(index, 'rgb('+rg+','+gg+','+bg+')');
		$(id+' .line').css('transform','translate('+(picker[index].width+20)+'px,'+(-gradient[index].height+y)+'px)');
	}

	x=hsb[1]*picker[index].width;
	y=(1-hsb[2])*picker[index].height;

	$(id+' .dot').css('transform','translate('+(-10+x)+'px,'+(-(gradient[index].height+5)+y)+'px)');
	if(y==0){
		y=1;
	}
	coly[index]=y;
	colx[index]=x;

	$(id+' .colorids>input').eq(0).val(Picker.rgbToHex(r,g,b));
	$(id+' .newcolor').css('background-color','rgb('+r+','+g+','+b+')');
	if(hsb[2]>0.5&&hsb[1]<0.5){
		$(id+' .newcolor').css('color','black');
	}else{
		$(id+' .newcolor').css('color','white');
	}
}
Picker.updateHex = function(id) {
	if(typeof id=='object')
		id='#'+$(id.target).parent().parent().attr('id');
	var index=ids.indexOf(id);

	var hex=$(id+' .colorids>input').eq(0).val().toUpperCase();
	if(hex.split('')[0]!=='#'){
		hex='#'+hex;
	}
	if(hex.length==4||hex.length==7){
		if(hex.length==4){
			hex='#'+hex.substr(1).split('')[0]+hex.substr(1).split('')[0]+hex.substr(1).split('')[1]+hex.substr(1).split('')[1]+hex.substr(1).split('')[2]+hex.substr(1).split('')[2];
		}
		var rgb=Picker.hexToRgb(hex);
		var r=rgb.r;
		var g=rgb.g;
		var b=rgb.b;


		hsb=Picker.rgbToHsb(r,g,b);

		y=(1-hsb[0])*gradient[index].height+.1;
		if(y>gradient[index].height){
			y=gradient[index].height;
		}
		if(y<1)
			y=1;
		let d=grd[index].getImageData(0, y-1, 1, 1).data;
		var rg=d[0];
		var gg=d[1];
		var bg=d[2];

		Picker.createGradients(index, 'rgb('+rg+','+gg+','+bg+')');

		$(id+' .line').css('transform','translate('+(picker[index].width+20)+'px,'+(-(gradient[index].height)+y)+'px)');

		x=hsb[1]*picker[index].width;
		y=(1-hsb[2])*picker[index].height;

		if(y<1)
			y=1;
		coly[index]=y;
		colx[index]=x;
		$(id+' .dot').css('transform','translate('+(-10+x)+'px,'+(-(gradient[index].height+5)+y)+'px)');

		$(id+' .colorids label>input').eq(0).val(r);
		$(id+' .colorids label>input').eq(1).val(g);
		$(id+' .colorids label>input').eq(2).val(b);

		$(id+' .newcolor').css('background-color','rgb('+r+','+g+','+b+')');
		if(hsb[2]>0.5&&hsb[1]<0.5){
			$(id+' .newcolor').css('color','black');
		}else{
			$(id+' .newcolor').css('color','white');
		}
	}
}
Picker.createGradients = function(index, color) {
	overloop1=pck[index].createLinearGradient(0,0,picker[index].width,0);
	overloop1.addColorStop(0,"white");
	overloop1.addColorStop(1,color);
	pck[index].fillStyle=overloop1;
	pck[index].fillRect(0,0,picker[index].width,picker[index].height);
	
	overloop1=pck[index].createLinearGradient(0,picker[index].height,0,0);
	overloop1.addColorStop(0,"black");
	overloop1.addColorStop(1,"transparent");
	pck[index].fillStyle=overloop1;
	pck[index].fillRect(0,0,picker[index].width,picker[index].height);
}
Picker.resetColor = function(e) {
	var id='#'+$(e.target).parent().parent().attr('id');
	var index=ids.indexOf(id);
	$(id+' .colorids>input').eq(0).val(currentColors[index]);
	Picker.updateHex(id);

	if($(e.target).text()==''){
		Picker.expand(e);
	}else{
		Picker.collapse(e);
	}
}
Picker.setNewColor = function(e) {
	var id='#'+$(e.target).parent().parent().attr('id');
	var index=ids.indexOf(id);
	currentColors[index]=$(id+' .colorids>input').eq(0).val();
	$(id+' .currentcolor').css('background-color',currentColors[index]);
	$(id).attr('colour',currentColors[index]);

	var rgb=Picker.hexToRgb(currentColors[index]);
	var hsb=Picker.rgbToHsb(rgb.r,rgb.g,rgb.b);
	if(hsb[2]>0.5&&hsb[1]<0.5){
		$(id+' .currentcolor').css('color','black');
	}else{
		$(id+' .currentcolor').css('color','white');
	}

	$(id+' .currentcolor').click();
}
Picker.setColor = function(id, r,g,b) {
	var color;
	if(typeof g=='undefined'){
		color=r;
	}else{
		color=Picker.rgbToHex(r,g,b);
	}
	color=color.toUpperCase();
	if(color.length==4)
		color='#'+color.substr(1).split('')[0]+color.substr(1).split('')[0]+color.substr(1).split('')[1]+color.substr(1).split('')[1]+color.substr(1).split('')[2]+color.substr(1).split('')[2];

	$(id).attr('colour',color);

	var rgb=Picker.hexToRgb(color);
	var hsb=Picker.rgbToHsb(rgb.r,rgb.g,rgb.b);
	if(hsb[2]>0.5&&hsb[1]<0.5){
		$(id+' .currentcolor').css('color','black');
		$(id+' .newcolor').css('color','black');
	}else{
		$(id+' .currentcolor').css('color','white');
		$(id+' .newcolor').css('color','white');
	}
	$(id+' .colorids>input').eq(0).val(color);
	$(id+' .currentcolor').css('background-color',color);
	Picker.updateHex(id);
}
Picker.colorPicker = function(id,w,h) {
	if(typeof w=='undefined')
		w=350;
	if(typeof h=='undefined')
		h=250;
	if(w<150)
		w=150
	if(h<150)
		h=150;


	$(id).css('width',w+'px');
	$(id).css('height',h+'px');
	$(id).attr('class','colorPicker');
	$(id).html("<canvas class='picker' width='"+(w-100)+"' height='"+h+"'></canvas><div class='dot'></div><canvas class='gradient' width='20' height='"+h+"'></canvas><div class='line'></div><div class='colorids'><label>R: <input onchange='Picker.updateRgb(event)' onkeyup='Picker.updateRgb(event)' type='number' min='0' max='255' value='255'></label><label>G: <input onchange='Picker.updateRgb(event)' onkeyup='Picker.updateRgb(event)' type='number' min='0' max='255' value='0'></label><label>B: <input onchange='Picker.updateRgb(event)' onkeyup='Picker.updateRgb(event)' type='number' min='0' max='255' value='0'></label><input type='text' onkeyup='Picker.updateHex(event)' placeholder='Hex' class='hex' value='#FF0000'><div title='New color' class='newcolor' onclick='Picker.setNewColor(event)'>OK</div><div onclick='Picker.resetColor(event)' title='Current color' class='currentcolor'>Cancel</div></div>");
	$(id+' .line').css('transform','translate('+(w-80)+'px,'+(-(0))+'px)');
	$(id+' .dot').css('transform','translate('+(w-110)+'px,'+(-(h+5))+'px)');
	$(id+' .colorids div').css('height',((h-130)/2)+'px');
	$(id+' .colorids div').css('line-height',((h-130)/2)+'px');

	sizes.push({
		width:w,
		height:h,
		divHeight:((h-130)/2)
	});

	Picker.initialize(id);
}
Picker.collapse = function(e){
	var id='#'+$(e.target).parent().parent().attr('id');
	var index=ids.indexOf(id);

	$(id + ' canvas').css('display','none');
	$(id + ' .line').css('display','none');
	$(id + ' .dot').css('display','none');
	$(id + ' input').css('display','none');
	$(id + ' label').css('display','none');
	$(id + ' .newcolor').css('display','none');
	$(id).css('margin-top','20px');
	$(id).css('margin-left','0px');

	$(e.target).text('');
	$(id).css('width',pickerSize+'px');
	$(id).css('height',pickerSize+'px');
	$(e.target).css('width',pickerSize+'px');
	$(e.target).css('height',pickerSize+'px');
	$(id).css('background-color','transparent');
	$('#timePicker').css('display','inline-block');
	$(e.target).css('border-radius','50%');
}
Picker.expand = function(e){
	var id='#'+$(e.target).parent().parent().attr('id');
	var index=ids.indexOf(id);

	$(id + ' canvas').css('display','inline');
	$(id + ' .line').css('display','block');
	$(id + ' .dot').css('display','block');
	$(id + ' input').css('display','inline-block');
	$(id + ' label').css('display','block');
	$(id + ' .newcolor').css('display','block');
	$('#timePicker').css('display','none');
	$(id).css('margin-top','-219px');
	$(id).css('margin-left','-24px');

	$(e.target).text('Cancel');
	$(id).css('width',sizes[index].width+'px');
	$(id).css('height',sizes[index].height+'px');
	$(e.target).css('width','70px');
	$(e.target).css('height',sizes[index].divHeight+'px');
	$(id).css('background-color','#282828');
	$(e.target).css('border-radius','0%');
}



































Picker.componentToHex = function(c) {
	var hex = c.toString(16);
	return hex.length == 1 ? "0" + hex : hex;
}
Picker.rgbToHex = function(r, g, b) {
	return ("#" + Picker.componentToHex(Number(r)) + Picker.componentToHex(Number(g)) + Picker.componentToHex(Number(b))).toUpperCase();
}
Picker.hexToRgb = function(hex) {
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
        r: parseInt(result[1], 16),
        g: parseInt(result[2], 16),
        b: parseInt(result[3], 16)
    } : null;
}

Picker.rgbToHsb = function(r,g,b) {
	var red = Number(r);
	var green = Number(g);
	var blue = Number(b);
	var min = Math.min(red, green);
	var max = Math.max(red, green);
	var delta = Number(0);
	var hue = Number(0);
	var sat = Number(0);
	var lumin = Number(0);
	min = Math.min(min, blue);
	max = Math.max(max, blue);
	delta = (max - min);
	if (max != 0) {
		sat = delta / max; 
		lumin = max / 255;
		delta = delta+0;
		red = red+0;
		green = green+0;
		blue = blue+0;
		if ( delta != 0 ) {
			if (red==max) {
				hue = (green - blue) / delta;
			} else if (green==max) {
				hue = 2 + ((blue - red) / delta);
			} else {
				hue = 4 + ((red - green) / delta);
			}
		}
		hue = hue * 60;
		if (hue < 0) hue = hue + 360;
	}
	return [hue/360,sat,lumin];
}