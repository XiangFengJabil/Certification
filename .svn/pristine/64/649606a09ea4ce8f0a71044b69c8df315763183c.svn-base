document.writeln('<iframe id="endDateLayer" name="endDateLayer" frameborder="0" scrolling="no" style="position: absolute;  z-index: 9998; display: none;width:162px;height=212px"></iframe>');
var outObject=null;
var outButton=null;
var outDate= null;
var bflagTime =true;
var odatelayer=null;
var bUseTime=false;
function getFrame()
{
     var  strFrame='<style>';
        strFrame+="body{margin: 0px;padding: 0px;font: normal 12px/20px Arial, Verdana, Lucida, Helvetica, simsun, sans-serif;}"
        strFrame+='INPUT.button{BORDER-RIGHT: #63A3E9 1px solid;BORDER-TOP: #63A3E9 1px solid;BORDER-LEFT: #63A3E9 1px solid;';
        strFrame+='BORDER-BOTTOM: #63A3E9 1px solid;BACKGROUND-COLOR: #63A3E9;}';
        strFrame+='TD{FONT-SIZE: 9pt;}';
        strFrame+='</style>';
        strFrame+='<div style="z-index:9999;position: absolute; left:0px; top:0px;text-align:center" onselectstart="return false">';
    var n=0;
    if(bflagTime)
    {
        strFrame+='<span id=tmpSelectYearLayer  style="z-index: 9999;position: absolute;top:3px; left:19px:display: none"></span>';
        strFrame+='<span id=tmpSelectMonthLayer  style="z-index: 9999;position: absolute;top:3px; left:78px:display: none"></span>';
        strFrame+='<span id=tmpSelectHourLayer  style="z-index: 9999;position: absolute;top:180px: left:56px:display: none"></span>';
        strFrame+='<span id=tmpSelectMinuteLayer style="z-index:9999;position:absolute;top:180px: left:107px:display: none"></span>';
        strFrame+='<table border="1px" cellspacing=0 cellpadding=0  height="200px" width="100%" bordercolor=#63A3E9 bgcolor=#63A3E9 >';
        strFrame+='    <tr><td>';
        strFrame+='        <table border="0" cellspacing=1 cellpadding=0  height=23 width="100%"  bgcolor=#FFFFFF>';
        strFrame+='            <tr align=center >';
        strFrame+='                <td width=16 align=center bgcolor=#63A3E9 style="cursor: pointer;color: #ffffff" ';
        strFrame+='					   onclick="parent.meizzPrevM()" title="向前翻 1 月" ><b >&lt;</b></td>';
        strFrame+='				   <td width=70 align="center" bgcolor="#63A3E9"  style="cursor:pointer;" ';
        strFrame+='				       onmouseover="style.backgroundColor=\'#aaccf3\'"';
        strFrame+='					   onmouseout="style.backgroundColor=\'#63A3E9\'" ';
        strFrame+='					   onclick="parent.tmpSelectYearInnerHTML(this.innerHTML)" ';
        strFrame+='					   title="点击这里选择年份"><span  id=meizzYearHead></span></td>';
        strFrame+='				   <td width=50 align="center" style="color: #ffffff;cursor:pointer;" ';
        strFrame+='					   bgcolor="#63A3E9" onmouseover="style.backgroundColor=\'#aaccf3\'" ';
        strFrame+='					   onmouseout="style.backgroundColor=\'#63A3E9\'" ';
        strFrame+='					   onclick="parent.tmpSelectMonthInnerHTML(this.innerHTML)"';
        strFrame+='					   title="点击这里选择月份"><span id=meizzMonthHead ></span></td>';
        strFrame+='				   <td width=16 bgcolor=#63A3E9 align=center style="cursor: pointer;color: #ffffff" ';
        strFrame+='					   onclick="parent.meizzNextM()" title="向后翻 1 月" ><b >&gt;</b></td>';
        strFrame+='			   </tr>';
        strFrame+='		   </table></td></tr>';
        strFrame+='	   <tr><td >';
        strFrame+='		   <table border="0" cellspacing=0 cellspacing=1 bgcolor=#63A3E9 ';
        strFrame+='				BORDERCOLORLIGHT=#63A3E9 BORDERCOLORDARK=#FFFFFF width="100%">';
        strFrame+='	   <tr align=center><td style="color:#afafaf;text-align:center;" height="18px">日</td>';
        strFrame+='<td style="color:#FFFFFF;text-align:center;">一</td><td style="color:#FFFFFF;text-align:center;">二</td>';
        strFrame+='<td style="color:#FFFFFF;text-align:center;">三</td><td style="color:#FFFFFF;text-align:center;">四</td>';
        strFrame+='<td style="color:#FFFFFF;text-align:center;">五</td><td style="color:#afafaf;text-align:center;">六</td></tr>';
        strFrame+='</table></td></tr>';
        strFrame+='  <tr ><td>';
        strFrame+='    <table border="1px" cellspacing=1 cellspacing=1 BORDERCOLORLIGHT=#63A3E9 BORDERCOLORDARK=#FFFFFF bgcolor=#fff8ec width="100%" height=120 >';
        n=0; for (j=0;j<5;j++){ strFrame+= ' <tr align=center >'; for (i=0;i<7;i++){
        strFrame+='<td width="18px" height="18px" id=meizzDay'+n+' onclick=parent.meizzDayClick(this.innerHTML,0)></td>';n++;}
        strFrame+='</tr>';}
        strFrame+='      <tr align=center >';
        for (i=35;i<37;i++)strFrame+='<td width="18px" height="18px" id=meizzDay'+i+'  onclick="parent.meizzDayClick(this.innerHTML,0)"></td>';
        strFrame+='        <td colspan=5 align=right style="color:#1478eb"><span onclick="parent.setNull()" style="cursor:pointer;"';
        strFrame+='         onmouseover="style.color=\'#ff0000\'" onmouseout="style.color=\'#1478eb\'" title="将日期置空">置空</span>&nbsp;&nbsp;<span onclick="parent.meizzToday()" style="cursor:pointer;"';
        strFrame+='         onmouseover="style.color=\'#ff0000\'" onmouseout="style.color=\'#1478eb\'" title="当前日期时间">当前</span>&nbsp;&nbsp;<span style="cursor:pointer;" id=evaAllOK onmouseover="style.color=\'#ff0000\'" onmouseout="style.color=\'#1478eb\'"  onclick="parent.closeLayer()" title="关闭日历">关闭&nbsp;</span></td></tr>';
        strFrame+='    </table></td></tr><tr ><td width="100%">';
        strFrame+='        <table border="0" cellspacing=1 cellpadding=0  bgcolor=#FFFFFF height=22 >';
        strFrame+='          <tr bgcolor="#63A3E9"><td id=bUseTimeLayer width=55  style="cursor:pointer;" title="点击这里启用/禁用时间"';
        strFrame+='				onmouseover="style.backgroundColor=\'#aaccf3\'" align=center onmouseout="style.backgroundColor=\'#63A3E9\'"';
        strFrame+='				 onclick="parent.UseTime(this)">';
        strFrame+=' <span></span></td>';
        strFrame+='             <td style="cursor:pointer;" onclick="parent.tmpSelectHourInnerHTML(this.innerHTML)"';
        strFrame+=' onmouseover="style.backgroundColor=\'#aaccf3\'" onmouseout="style.backgroundColor=\'#63A3E9\'"';
        strFrame+=' title="点击这里选择时间" align=center width=50>' ;
        strFrame+='					<span id=meizzHourHead></span></td>';
        strFrame+='             <td style="cursor:pointer;" onclick="parent.tmpSelectMinuteInnerHTML(this.innerHTML)"';
        strFrame+=' onmouseover="style.backgroundColor=\'#aaccf3\'" onmouseout="style.backgroundColor=\'#63A3E9\'"';
        strFrame+=' title="点击这里选择时间" align=center width=50>' ;
        strFrame+='					<span id=meizzMinuteHead></span></td>';
        strFrame+='			 </tr></table></td></tr></table>';
    }
    else
    {
        strFrame+='<span id=tmpSelectYearLayer  style="z-index: 9999;position: absolute;top:3px; left:19px:display: none"></span>';
        strFrame+='<span id=tmpSelectMonthLayer  style="z-index: 9999;position: absolute;top:3px; left:78px:display: none"></span>';
        strFrame+='<table border="1px" cellspacing=0 cellpadding=0 height="176px" width="100%" bordercolor=#63A3E9 bgcolor=#63A3E9 >';
        strFrame+='    <tr><td>';
        strFrame+='        <table border="0" cellspacing=1 cellpadding=0  width="100%" height=23  bgcolor=#FFFFFF>';
        strFrame+='            <tr align=center >';
        strFrame+='                <td width=16 align=center bgcolor=#63A3E9 style="cursor: pointer;color: #ffffff" ';
        strFrame+='					   onclick="parent.meizzPrevM()" title="向前翻 1 月" ><b >&lt;</b></td>';
        strFrame+='				   <td width=70 align="center" bgcolor="#63A3E9"  style="cursor:pointer;" ';
        strFrame+='				       onmouseover="style.backgroundColor=\'#aaccf3\'"';
        strFrame+='					   onmouseout="style.backgroundColor=\'#63A3E9\'" ';
        strFrame+='					   onclick="parent.tmpSelectYearInnerHTML(this.innerHTML)" ';
        strFrame+='					   title="点击这里选择年份"><span  id=meizzYearHead></span></td>';
        strFrame+='				   <td width=50 align="center" style="color: #ffffff;cursor:pointer;" ';
        strFrame+='					   bgcolor="#63A3E9" onmouseover="style.backgroundColor=\'#aaccf3\'" ';
        strFrame+='					   onmouseout="style.backgroundColor=\'#63A3E9\'" ';
        strFrame+='					   onclick="parent.tmpSelectMonthInnerHTML(this.innerHTML)"';
        strFrame+='					   title="点击这里选择月份"><span id=meizzMonthHead ></span></td>';
        strFrame+='				   <td width=16 bgcolor=#63A3E9 align=center style="cursor: pointer;color: #ffffff" ';
        strFrame+='					   onclick="parent.meizzNextM()" title="向后翻 1 月" ><b >&gt;</b></td>';
        strFrame+='			   </tr>';
        strFrame+='		   </table></td></tr>';
        strFrame+='	   <tr><td >';
        strFrame+='		   <table border="0" cellspacing=0 cellspacing=1 bgcolor=#63A3E9 ';
        strFrame+='				BORDERCOLORLIGHT=#63A3E9 BORDERCOLORDARK=#FFFFFF width="100%">';
        strFrame+='	   <tr align=center><td style="color:#afafaf;text-align:center;" height="18px">日</td>';
        strFrame+='<td style="color:#FFFFFF;text-align:center;">一</td><td style="color:#FFFFFF;text-align:center;">二</td>';
        strFrame+='<td style="color:#FFFFFF;text-align:center;">三</td><td style="color:#FFFFFF;text-align:center;">四</td>';
        strFrame+='<td style="color:#FFFFFF;text-align:center;">五</td><td style="color:#afafaf;text-align:center;">六</td></tr>';
        strFrame+='</table></td></tr>';
        strFrame+='  <tr ><td >';
        strFrame+='    <table border="1px" cellspacing=1 cellspacing=1 BORDERCOLORLIGHT=#63A3E9 BORDERCOLORDARK=#FFFFFF bgcolor=#fff8ec width="100%" height=120 >';
        n=0; for (j=0;j<5;j++){ strFrame+= ' <tr align=center >'; for (i=0;i<7;i++){
        strFrame+='<td width="18px" height="18px" id=meizzDay'+n+' onclick=parent.meizzDayClick(this.innerHTML,0)></td>';n++;}
        strFrame+='</tr>';}
        strFrame+='      <tr align=center >';
        for (i=35;i<37;i++)strFrame+='<td width="18px" height="18px" id=meizzDay'+i+'  onclick="parent.meizzDayClick(this.innerHTML,0)"></td>';
        strFrame+='        <td colspan=5 align=right style="color:#1478eb"><span onclick="parent.setNull()" style="cursor:pointer;"';
        strFrame+='         onmouseover="style.color=\'#ff0000\'" onmouseout="style.color=\'#1478eb\'" title="将日期置空">置空</span>&nbsp;&nbsp;<span onclick="parent.meizzToday()" style="cursor:pointer;"';
        strFrame+='         onmouseover="style.color=\'#ff0000\'" onmouseout="style.color=\'#1478eb\'" title="当前日期时间">当前</span>&nbsp;&nbsp;<span style="cursor:pointer;" id=evaAllOK onmouseover="style.color=\'#ff0000\'" onmouseout="style.color=\'#1478eb\'"  onclick="parent.closeLayer()" title="关闭日历">关闭&nbsp;</span></td></tr>';
        strFrame+='    </table></td></tr></table>';
    }
    strFrame+='</div>';
    return strFrame;
}
function setday(tt,bflag,obj)
{
  try{
	if (arguments.length == 0){alert("对不起！您没有传回本控件任何参数！");return;}
    outObject = (arguments.length == 3 ? obj:tt);
	outButton = (arguments.length == 3 ? tt: null);
    var s = "";
    var i;
    bflagTime = bflag;
    var strFrame = getFrame();
    odatelayer=window.frames.endDateLayer;
    odatelayer.document.writeln(strFrame);
    odatelayer.document.close();
	var dads = document.getElementById("endDateLayer").style;
    if(bflagTime)
    {
        dads.height="212px";
        odatelayer.document.getElementById("bUseTimeLayer").innerHTML=bImgSwitch();
    }
    else
    {
        dads.height="188px";
    }
	var ttop = tt.offsetTop;
	var thei = tt.clientHeight;	
	var tleft = tt.offsetLeft;
	var ttyp = tt.type;
	tt = tt.offsetParent;
	while (tt){ttop+=tt.offsetTop; tleft+=tt.offsetLeft;tt = tt.offsetParent;}
	dads.top = (ttyp=="image") ? ttop+thei : ttop+thei+"px";
	dads.left = tleft+"px";
	var reg = /^(\d+)-(\d{1,2})-(\d{1,2})/;
	var r = outObject.value.match(reg);
	if(r!=null){
		r[2]=r[2]-1;
		var d=new Date(r[1],r[2],r[3]);
		if(d.getFullYear()==r[1] && d.getMonth()==r[2] && d.getDate()==r[3])
		{
			outDate=d;
			parent.meizzTheYear = r[1];
			parent.meizzTheMonth = r[2];
			parent.meizzTheDate = r[3];
		}
		else
		{
			outDate=null;
		}
		meizzSetDay(r[1],r[2]+1);
	}
	else
	{
		outDate=null;
		meizzSetDay(new Date().getFullYear(), new Date().getMonth() + 1);
	}
	if ((outObject.value.length>10)||(outObject.value.length == 0))
	{
		bUseTime=true&bflag;
		if(bflagTime){
		    odatelayer.document.getElementById("bUseTimeLayer").innerHTML=bImgSwitch();
		}
		meizzWriteHead(meizzTheYear,meizzTheMonth);
	}
	else
	{
		bUseTime=false&bflag;
		if(bflagTime)
		{
		    odatelayer.document.getElementById("bUseTimeLayer").innerHTML=bImgSwitch();
		    meizzWriteHead(meizzTheYear,meizzTheMonth);
		}
	}
	dads.display = '';
    if (outButton){
      return false;
    }
 } catch (e) {return false;}
}
var MonHead = new Array(12);
MonHead[0] = 31; MonHead[1] = 28; MonHead[2] = 31; MonHead[3] = 30; MonHead[4]  = 31; MonHead[5]  = 30;
MonHead[6] = 31; MonHead[7] = 31; MonHead[8] = 30; MonHead[9] = 31; MonHead[10] = 30; MonHead[11] = 31;
var meizzTheYear=new Date().getFullYear(); 
var meizzTheMonth=new Date().getMonth()+1;
var meizzTheDate=new Date().getDate();	
var meizzTheHour=new Date().getHours();
var meizzTheMinute=new Date().getMinutes();
var meizzWDay=new Array(37);
  document.onclick=function(event) { 
    var evt = null;
    if (arguments.length==1){
        evt=arguments[0];
    }else{
	    evt = (window.event ? window.event : null);
    }	
	if(evt)
	{
	    var el=evt.srcElement || evt.target;
		if ( el != outObject  && el != outButton){
		    closeLayer();
		}
	}
  }
  document.onkeyup=function(event) {
    var evt = null;
    if (arguments.length==1){
        evt=arguments[0];
    }else{
	    evt = (window.event ? window.event : null);
    }	
    if (evt && evt.keyCode==27){
		if(outObject)outObject.blur();
		closeLayer();
	}
	else if(document.activeElement)
	{
		if(document.activeElement != outObject && document.activeElement != outButton)
		{
			closeLayer();
		}
	}
 }
 
function meizzWriteHead(yy,mm)
{
	odatelayer.document.getElementById("meizzYearHead").innerHTML	= yy + " 年";
	odatelayer.document.getElementById("meizzMonthHead").innerHTML	= format(mm) + " 月";
	if(bflagTime)
	{
	    odatelayer.document.getElementById("meizzHourHead").innerHTML=bUseTime?(meizzTheHour+" 时"):""; 
	    odatelayer.document.getElementById("meizzMinuteHead").innerHTML=bUseTime?(meizzTheMinute+" 分"):"";
	}
}
function tmpSelectYearInnerHTML(strYear)
{
	//if (strYear.match(/\D/)!=null){alert("年份输入参数不是数字！");return;}
	strYear = getNum(strYear);
	var m = (strYear) ? strYear : new Date().getFullYear();
	if (m < 1000 || m > 9999) {alert("年份值不在 1000 到 9999 之间！");return;}
	var n = m - 50;
	if (n < 1000) n = 1000;
	if (n + 101 > 9999) n = 9974;
	var s = "&nbsp;<select name=tmpSelectYear "
	s += "onblur='document.getElementById(\"tmpSelectYearLayer\").style.display=\"none\"' "
	s += "onchange='document.getElementById(\"tmpSelectYearLayer\").style.display=\"none\";"
	s += "parent.meizzTheYear = this.value; parent.meizzSetDay(parent.meizzTheYear,parent.meizzTheMonth)'>\r\n";
	var selectInnerHTML = s;
	for (var i = n; i < n + 101; i++)
	{
		if (i == m) { selectInnerHTML += "<option value='" + i + "' selected>" + i + "年" + "</option>\r\n"; }
		else { selectInnerHTML += "<option value='" + i + "'>" + i + "年" + "</option>\r\n"; }
	}
	selectInnerHTML += "</select>";
	odatelayer.document.getElementById("tmpSelectYearLayer").style.display="";
	odatelayer.document.getElementById("tmpSelectYearLayer").innerHTML = selectInnerHTML;
	//odatelayer.document.getElementById("tmpSelectYear").focus();
}
function tmpSelectMonthInnerHTML(strMonth)
{
	//if (strMonth.match(/\D/)!=null){alert("月份输入参数不是数字！");return;}
	strMonth = getNum(strMonth);
	var m = (strMonth) ? strMonth : new Date().getMonth() + 1;
	var s = "&nbsp;&nbsp;<select name=tmpSelectMonth "
	s += "onblur='document.getElementById(\"tmpSelectMonthLayer\").style.display=\"none\"' "
	s += "onchange='document.getElementById(\"tmpSelectMonthLayer\").style.display=\"none\";"
	s += "parent.meizzTheMonth = this.value; parent.meizzSetDay(parent.meizzTheYear,parent.meizzTheMonth)'>\r\n";
	var selectInnerHTML = s;
	for (var i = 1; i < 13; i++)
	{
		if (i == m) { selectInnerHTML += "<option value='"+i+"' selected>"+i+"月"+"</option>\r\n"; }
		else { selectInnerHTML += "<option value='"+i+"'>"+i+"月"+"</option>\r\n"; }
	}
	selectInnerHTML += "</select>";
	odatelayer.document.getElementById("tmpSelectMonthLayer").style.display="";
	odatelayer.document.getElementById("tmpSelectMonthLayer").innerHTML = selectInnerHTML;
	//odatelayer.document.getElementById("tmpSelectMonth").focus();
}
function tmpSelectHourInnerHTML(strHour)
{
	if (!bUseTime){return;}
	//if (strHour.match(/\D/)!=null){alert("小时输入参数不是数字！");return;}
	strHour = getNum(strHour);
	var m = (strHour) ? strHour : new Date().getHours();
	var s = "&nbsp;<select name=tmpSelectHour "
	s += "onblur='document.getElementById(\"tmpSelectHourLayer\").style.display=\"none\"' "
	s += "onchange='document.getElementById(\"tmpSelectHourLayer\").style.display=\"none\";"
	s += "parent.meizzTheHour = this.value; parent.evaSetTime(parent.meizzTheHour,parent.meizzTheMinute);'>\r\n";
	var selectInnerHTML = s;
	for (var i = 0; i < 24; i++)
	{
		if (i == m) { selectInnerHTML += "<option value='"+i+"' selected>&nbsp;"+i+"&nbsp;</option>\r\n"; }
		else { selectInnerHTML += "<option value='"+i+"'>&nbsp;"+i+"&nbsp;</option>\r\n"; }
	}
	selectInnerHTML += "</select>";
	odatelayer.document.getElementById("tmpSelectHourLayer").style.display="";
	odatelayer.document.getElementById("tmpSelectHourLayer").innerHTML = selectInnerHTML;
	//odatelayer.document.getElementById("tmpSelectHour").focus();
}
function tmpSelectMinuteInnerHTML(strMinute)
{
	if (!bUseTime){return;}
	//if (strMinute.match(/\D/)!=null){alert("分钟输入参数不是数字！");return;}
	strMinute = getNum(strMinute);
	var m = (strMinute) ? strMinute : new Date().getMinutes();
	var s = "&nbsp;<select name=tmpSelectMinute "
	s += "onblur='document.getElementById(\"tmpSelectMinuteLayer\").style.display=\"none\"' "
	s += "onchange='document.getElementById(\"tmpSelectMinuteLayer\").style.display=\"none\";"
	s += "parent.meizzTheMinute = this.value; parent.evaSetTime(parent.meizzTheHour,parent.meizzTheMinute);'>\r\n";
	var selectInnerHTML = s;
	for (var i = 0; i < 60; i++)
	{
		if (i == m) { selectInnerHTML += "<option value='"+i+"' selected>&nbsp;"+i+"&nbsp;</option>\r\n"; }
		else { selectInnerHTML += "<option value='"+i+"'>&nbsp;"+i+"&nbsp;</option>\r\n"; }
	}
	selectInnerHTML += "</select>";
	odatelayer.document.getElementById("tmpSelectMinuteLayer").style.display="";
	odatelayer.document.getElementById("tmpSelectMinuteLayer").innerHTML = selectInnerHTML;
	//odatelayer.document.getElementById("tmpSelectMinute").focus();
}
function closeLayer()
{
    var o=document.getElementById("endDateLayer");
    if (o){
        o.style.display="none";
    }
}
function showLayer()
{
    var o=document.getElementById("endDateLayer");
    if (o){
	    o.style.display="";
	}
}
function IsPinYear(year)
{
	if (0==year%4&&((year%100!=0)||(year%400==0))) return true;else return false;
}
function GetMonthCount(year,month)
{
	var c=MonHead[month-1];if((month==2)&&IsPinYear(year)) c++;return c;
}
function GetDOW(day,month,year)
{
	var dt=new Date(year,month-1,day).getDay()/7; return dt;
}
function meizzPrevY()
{
	if(meizzTheYear > 999 && meizzTheYear <10000){meizzTheYear--;}
	else{alert("年份超出范围（1000-9999）！");}
	meizzSetDay(meizzTheYear,meizzTheMonth);
}
function meizzNextY()
{
	if(meizzTheYear > 999 && meizzTheYear <10000){meizzTheYear++;}
	else{alert("年份超出范围（1000-9999）！");}
	meizzSetDay(meizzTheYear,meizzTheMonth);
}
function setNull()
{
	outObject.value = '';
	closeLayer();
}
function meizzToday()
{
	parent.meizzTheYear		= new Date().getFullYear();
	parent.meizzTheMonth	= new Date().getMonth()+1;
	parent.meizzTheDate		= new Date().getDate();
	parent.meizzTheHour		= new Date().getHours();
	parent.meizzTheMinute	= new Date().getMinutes();
	var meizzTheSecond		= new Date().getSeconds();
	if (meizzTheMonth<10 && meizzTheMonth.length<2)
	{
		parent.meizzTheMonth="0"+parent.meizzTheMonth;
	}
	if (parent.meizzTheDate<10 && parent.meizzTheDate.length<2)
	{
		parent.meizzTheDate="0"+parent.meizzTheDate;
	}
	//meizzSetDay(meizzTheYear,meizzTheMonth);
	if(outObject)
	{
		if (bUseTime)
		{
			outObject.value= parent.meizzTheYear + "-" + format( parent.meizzTheMonth) + "-" + 
							format(parent.meizzTheDate) + " " + format(parent.meizzTheHour) + ":" + 
							format(parent.meizzTheMinute) + ":" + format(meizzTheSecond); 
		}
		else
		{
			outObject.value= parent.meizzTheYear + "-" + format( parent.meizzTheMonth) + "-" + 
							format(parent.meizzTheDate);
		}
	}
	closeLayer();
}
function meizzPrevM()
{
	if(meizzTheMonth>1){meizzTheMonth--}else{meizzTheYear--;meizzTheMonth=12;}
	meizzSetDay(meizzTheYear,meizzTheMonth);
}
function meizzNextM()
{
	if(meizzTheMonth==12){meizzTheYear++;meizzTheMonth=1}else{meizzTheMonth++}
	meizzSetDay(meizzTheYear,meizzTheMonth);
}
function meizzSetDay(yy,mm)
{
	meizzWriteHead(yy,mm);
	meizzTheYear=yy;
	meizzTheMonth=mm;
	for (var i = 0; i < 37; i++){meizzWDay[i]=""};
	var day1 = 1,day2=1,firstday = new Date(yy,mm-1,1).getDay();
	for (i=0;i<firstday;i++){meizzWDay[i]=GetMonthCount(mm==1?yy-1:yy,mm==1?12:mm-1)-firstday+i+1;}
	for (i = firstday; day1 < GetMonthCount(yy,mm)+1; i++) { meizzWDay[i]=day1;day1++; }
	for (i=firstday+GetMonthCount(yy,mm);i<37;i++) { meizzWDay[i]=day2;day2++; }
	for (i = 0; i < 37; i++)
	{
	  var da = odatelayer.document.getElementById("meizzDay"+i);
	  if (meizzWDay[i]!="")
	 {
		da.borderColorLight="#63A3E9";
		da.borderColorDark="#63A3E9";
		da.style.color="#1478eb";
		if(i<firstday)
		{
			da.innerHTML="<b><font color=#BCBABC>" + meizzWDay[i] + "</font></b>";
			da.title=(mm==1?12:mm-1) +"月" + meizzWDay[i] + "日";
			da.onclick=Function("meizzDayClick(this.innerHTML,-1)");
			if(!outDate)
				da.style.backgroundColor = ((mm==1?yy-1:yy) == new Date().getFullYear() && 
					(mm==1?12:mm-1) == new Date().getMonth()+1 && meizzWDay[i] == new Date().getDate()) ?
					 "#5CEFA0":"#f5f5f5";
			else
			{
				da.style.backgroundColor =((mm==1?yy-1:yy)==outDate.getFullYear() && (mm==1?12:mm-1)== outDate.getMonth() + 1 && 
				meizzWDay[i]==outDate.getDate())? "#84C1FF" :
				(((mm==1?yy-1:yy) == new Date().getFullYear() && (mm==1?12:mm-1) == new Date().getMonth()+1 && 
				meizzWDay[i] == new Date().getDate()) ? "#5CEFA0":"#f5f5f5");
				if((mm==1?yy-1:yy)==outDate.getFullYear() && (mm==1?12:mm-1)== outDate.getMonth() + 1 && 
				meizzWDay[i]==outDate.getDate())
				{
					da.borderColorLight="#FFFFFF";
					da.borderColorDark="#63A3E9";
				}
			}
		}
		else if (i>=firstday+GetMonthCount(yy,mm))
		{
			da.innerHTML="<b><font color=#BCBABC>" + meizzWDay[i] + "</font></b>";
			da.title=(mm==12?1:mm+1) +"月" + meizzWDay[i] + "日";
			da.onclick=Function("meizzDayClick(this.innerHTML,1)");
			if(!outDate)
				da.style.backgroundColor = ((mm==12?yy+1:yy) == new Date().getFullYear() && 
					(mm==12?1:mm+1) == new Date().getMonth()+1 && meizzWDay[i] == new Date().getDate()) ?
					 "#5CEFA0":"#f5f5f5";
			else
			{
				da.style.backgroundColor =((mm==12?yy+1:yy)==outDate.getFullYear() && (mm==12?1:mm+1)== outDate.getMonth() + 1 && 
				meizzWDay[i]==outDate.getDate())? "#84C1FF" :
				(((mm==12?yy+1:yy) == new Date().getFullYear() && (mm==12?1:mm+1) == new Date().getMonth()+1 && 
				meizzWDay[i] == new Date().getDate()) ? "#5CEFA0":"#f5f5f5");
				if((mm==12?yy+1:yy)==outDate.getFullYear() && (mm==12?1:mm+1)== outDate.getMonth() + 1 && 
				meizzWDay[i]==outDate.getDate())
				{
					da.borderColorLight="#FFFFFF";
					da.borderColorDark="#63A3E9";
				}
			}
		}
		else
		{
			da.innerHTML="<b>" + meizzWDay[i] + "</b>";
			da.title=mm +"月" + meizzWDay[i] + "日";
			da.onclick=Function("meizzDayClick(this.innerHTML,0)");
			if(!outDate)
				da.style.backgroundColor = (yy == new Date().getFullYear() && mm == new Date().getMonth()+1 && meizzWDay[i] == new Date().getDate())?
					"#5CEFA0":"#f5f5f5";
			else
			{
				da.style.backgroundColor =(yy==outDate.getFullYear() && mm== outDate.getMonth() + 1 && meizzWDay[i]==outDate.getDate())?
					"#84C1FF":((yy == new Date().getFullYear() && mm == new Date().getMonth()+1 && meizzWDay[i] == new Date().getDate())?
					"#5CEFA0":"#f5f5f5");
				if(yy==outDate.getFullYear() && mm== outDate.getMonth() + 1 && meizzWDay[i]==outDate.getDate())
				{
					da.borderColorLight="#FFFFFF";
					da.borderColorDark="#63A3E9";
				}
			}
		}
			da.style.cursor="pointer"
			}
		else { 
		    da.innerHTML="";
		    da.style.backgroundColor="";
		    da.style.cursor="default"; 
		}
	 }
}
function meizzDayClick(n,ex)
{
	n = getNum(n);
	parent.meizzTheDate=n;
	var yy=meizzTheYear;
	var mm = parseInt(meizzTheMonth)+ex;
	var hh=meizzTheHour;
	var mi=meizzTheMinute;
	if(mm<1){
		yy--;
		mm=12+mm;
	}
	else if(mm>12){
		yy++;
		mm=mm-12;
	}
	if (mm < 10)	{mm = "0" + mm;}
	if (hh<10)		{hh="0" + hh;}
	if (mi<10)		{mi="0" + mi;}
	if (outObject)
	{
		if (!n) {return;}
		if ( n < 10){n = "0" + n;}
		WriteDateTo(yy,mm,n,hh,mi);
		closeLayer(); 
	}
	else {closeLayer(); alert("您所要输出的控件对象并不存在！");}
}
function format(n)
{
	var m=new String();
	var tmp=new String(n);
	if (n<10 && tmp.length<2)
	{
		m="0"+n;
	}
	else
	{
		m=n;
	}
	return m;
}
function evaSetTime()
{
	odatelayer.document.getElementById("meizzHourHead").innerHTML=meizzTheHour+" 时";
	odatelayer.document.getElementById("meizzMinuteHead").innerHTML=meizzTheMinute+" 分";
	WriteDateTo(meizzTheYear,meizzTheMonth,meizzTheDate,meizzTheHour,meizzTheMinute)
}
function evaSetTimeNothing()
{
	odatelayer.document.getElementById("meizzHourHead").innerHTML="";
	odatelayer.document.getElementById("meizzMinuteHead").innerHTML="";
	WriteDateTo(meizzTheYear,meizzTheMonth,meizzTheDate,meizzTheHour,meizzTheMinute)
}
function evaSetTimeNow()
{
	odatelayer.document.getElementById("meizzHourHead").innerHTML=new Date().getHours()+" 时";
	odatelayer.document.getElementById("meizzMinuteHead").innerHTML=new Date().getMinutes()+" 分";
	meizzTheHour = new Date().getHours();
	meizzTheMinute = new Date().getMinutes();
	WriteDateTo(meizzTheYear,meizzTheMonth,meizzTheDate,meizzTheHour,meizzTheMinute)
}
function UseTime(ctl)
{
	bUseTime=!bUseTime;
	if (bUseTime)
	{
		ctl.innerHTML=bImgSwitch();
		evaSetTime();
	}
	else
	{
		ctl.innerHTML=bImgSwitch();
		evaSetTimeNothing();
	}
}
function WriteDateTo(yy,mm,n,hh,mi)
{
	if (bUseTime)
	{
		outObject.value= yy + "-" + format(mm) + "-" + format(n) + " " + format(hh) + ":" + format(mi) + ":" + "00";
	}
	else
	{
		outObject.value= yy + "-" + format(mm) + "-" + format(n);
	}
}
function bImgSwitch()
{
	if (bUseTime)
	{
		return "时间开启";
	}
	else
	{
		return "时间关闭";
	}
}

function getNum(v)
{
    if (v)
    {
        var reg = /(\d+)/;
	    var r = v.match(reg);
        if(r!=null){
            return r[0];
        }
    }
    return "";
}


