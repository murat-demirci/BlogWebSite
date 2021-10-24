var btn = $("#back-top");

$(window).scroll(function () {
  $(this).scrollTop() > 100 ? btn.fadeIn() : btn.fadeOut();
});

btn.click(function () {
  $("body,html").animate({
      scrollTop: 0
    }, 1000);
  $(".rocket").addClass("fly");

  setTimeout(function () {
    $(".rocket").removeClass("fly");
  }, 1000);

  return false;
});




