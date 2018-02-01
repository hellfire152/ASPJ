function closeMessage(el) {
    e1.remove();
}

$('.js-messageClose').on('click', function (e) {
    closeMessage($(this).closest('.Message'));
});

function showNotification(mainMessage, subMessage) {
    let notificationHtml = "<div class='Message' id='js-timer'><div class='Message-icon'><i class='fa fa-bell-o'></i></div><div class='Message-body'><p>" + mainMessage + "</p><p class='u-italic'>" + subMessage + "</p></div><button class='Message-close js-messageClose'><i class='fa fa-times'></i></button></div >";
    document.getElementById("notifications").innerHTML += (notificationHtml);
    setTimeout(() => {
        $("#js-timer").remove();
    }, 3000);
}

function showAlert() {
    $(".cd-popup").addClass('is-visible');
}

function toggleAutosave() {
    _tofuUniverse.settings.autosave = document.getElementById("autosave-toggle").checked;
}

$.ready(function ($) {
    //close popup
    $('.cd-popup').on('click', function (event) {
        if ($(event.target).is('.cd-popup-close') || $(event.target).is('.cd-popup')) {
            event.preventDefault();
            $(this).removeClass('is-visible');
        }
    });
    //close popup when clicking the esc keyboard button
    $(document).keyup(function (event) {
        if (event.which == '27') {
            $('.cd-popup').removeClass('is-visible');
        }
    });
});