 var pagestyleCookieExpires = 365;  var pagestyleCookieDomain ="index.html";
  var pagestylePath = "sites/all/modules/pagestyle";
  var pagestyleCurrent = "standard";
 var text_resize_scope = "content-inner";
    var text_resize_minimum = "12";
    var text_resize_maximum = "25";
    var text_resize_line_height_allow = false;
    var text_resize_line_height_min = 16;
    var text_resize_line_height_max = 36;
$(document).ready(function(){
var schh = $.cookie("sche");
var chngtextsize= $.cookie("chngtext");
var chngtextspace= $.cookie("textspace"); 


  // Reset Font Size
  var originalFontSize = $('body').css('font-size');
  originalFontSize = '13px';
  $(".resetFont").click(function(){
  $.cookie("chngtext", "2");
  $('body').css('font-size', originalFontSize);
  //alert($('#wrapper').css('font-size'));
   //location.reload();
     
  });
  // Increase Font Size
  $(".increaseFont").click(function(){
    $.cookie("chngtext", "1");
  var currentFontSize = $('body').css('font-size');
 	var currentFontSizeNum = parseFloat(currentFontSize, 10);
  var newFontSize = currentFontSizeNum*1.2;
	if ( newFontSize < 16 ) {
	$('body').css('font-size', newFontSize+"px");
  	}
  // alert($('#wrapper').css('font-size')); 
  return false;
  });
  // Decrease Font Size
  $(".decreaseFont").click(function(){
  $.cookie("chngtext", "3");
  var currentFontSize = $('body').css('font-size');
 	var currentFontSizeNum = parseFloat(currentFontSize, 10);
	var newFontSize = currentFontSizeNum*0.8;
  if ( newFontSize > 8 ) {
	$('body').css('font-size', newFontSize);
  //alert($('#wrapper').css('font-size'));
 	}
   //alert($('#wrapper').css('font-size'));
	return false;
  });
 
   $(".yellowblue").click(function(){
    yellowblue();     
  });
  
    $(".yellowblack").click(function(){
  yellowblack();    
  });
  
     $(".fushblack").click(function(){
  fushblack();     
  });
  
   $(".standards").click(function(){
   $.cookie("sche", "0");
   location.reload();
   
       });
       
     $(".defaultspace").click(function(){
   $.cookie("textspace","0"); 
     $("body").css("letter-spacing", "0px");
       });
     
     $(".wider").click(function(){
   $.cookie("textspace","1"); 
   $("body").css("letter-spacing", "1px");
       });
       
       
     $(".widest").click(function(){
   $.cookie("textspace","2"); 
   $("body").css("letter-spacing", "2px");
       });        
       
       
 
//alert(schh);
if (schh == '')
{
if (i == 0) {location.reload(); i = 1;}
}

if (schh == 2){
yellowblue(); 
}
if (schh == 1){
yellowblack(); 
}

if (schh == 3)
{ 
fushblack();
}

if (chngtextsize == ''){ 
     if (j == 0) {location.reload(); j = 1;} 
    }
if (chngtextsize == 0){ 
     $("body").css("font-size", "17.2px"); 
    }
  if (chngtextsize == 1){ 
     $("body").css("font-size", "14.4px"); 
    }
  if (chngtextsize == 2){ 
     $("body").css("font-size", "12px"); 
    }
  if (chngtextsize == 3){ 
     $("body").css("font-size", "9.8px"); 
    }
  if (chngtextsize == 4){ 
     $("body").css("font-size", "7.7px"); 
    }
    
  if (chngtextspace == ''){ 
     if (k == 0) {location.reload(); k = 1;}
    } 
  if(chngtextspace == 0) {
     $("body").css("letter-spacing", "0px"); 
    }
  if(chngtextspace == 1) {
     $("body").css("letter-spacing", "1px"); 
    }
  if(chngtextspace == 2) {
     $("body").css("letter-spacing", "2px"); 
  }   

function yellowblue()
{
$.cookie("sche", "2");
$("body, *").css("background", "blue");
$("body, *").css("color", "yellow");
$(".bicons48 div.block-icon").css("height","0px");
}

function yellowblack()
{
$.cookie("sche", "1");
$("body, *").css("background", "black");
$("body, *").css("color", "yellow");
$(".bicons48 div.block-icon").css("height","0px");
}

function fushblack()
{
$.cookie("sche", "3");
$("body, *").css("background", "black");
$("body, *").css("color", "fuchsia");
$(".bicons48 div.block-icon").css("height","0px");
}
});
jQuery.extend(Drupal.settings, { "basePath": "/", "lightbox2": { "rtl": 0, "file_path": "/(\\w\\w/)sites/legalmetrology.kar.nic.in/files", "default_image": "./sites/all/modules/lightbox2/images/brokenimage.jpg", "border_size": 10, "font_color": "000", "box_color": "fff", "top_position": "", "overlay_opacity": "0.8", "overlay_color": "000", "disable_close_click": 1, "resize_sequence": 0, "resize_speed": 400, "fade_in_speed": 400, "slide_down_speed": 600, "use_alt_layout": 1, "disable_resize": 0, "disable_zoom": 0, "force_show_nav": 0, "show_caption": 1, "loop_items": 1, "node_link_text": "View Image Details", "node_link_target": 0, "image_count": "Image !current of !total", "video_count": "Video !current of !total", "page_count": "Page !current of !total", "lite_press_x_close": "press \x3ca href=\"#\" onclick=\"hideLightbox(); return FALSE;\"\x3e\x3ckbd\x3ex\x3c/kbd\x3e\x3c/a\x3e to close", "download_link_text": "", "enable_login": false, "enable_contact": false, "keys_close": "c x 27", "keys_previous": "p 37", "keys_next": "n 39", "keys_zoom": "z", "keys_play_pause": "32", "display_image_size": "original", "image_node_sizes": "()", "trigger_lightbox_classes": "", "trigger_lightbox_group_classes": "", "trigger_slideshow_classes": "", "trigger_lightframe_classes": "", "trigger_lightframe_group_classes": "", "custom_class_handler": 0, "custom_trigger_classes": "", "disable_for_gallery_lists": true, "disable_for_acidfree_gallery_lists": true, "enable_acidfree_videos": true, "slideshow_interval": 5000, "slideshow_automatic_start": 1, "slideshow_automatic_exit": 1, "show_play_pause": 1, "pause_on_next_click": 1, "pause_on_previous_click": 1, "loop_slides": 1, "iframe_width": 600, "iframe_height": 400, "iframe_border": 1, "enable_video": 0 }, "cron": { "basePath": "/poormanscron", "runNext": 1369305401 } });


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   