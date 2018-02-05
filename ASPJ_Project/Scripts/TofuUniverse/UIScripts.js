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
    $("#toggle").click(() => {
        $("#contact").slideToggle();
        return false;
    });
});

function clickEarningsDisplay(tofuEarned) {
    if (tofuUniverse.settings.showEarnings) {
        let earning = $("<span>", {
            "class": "click-earn"
        });
        earning.css("style", "position:" + mouseX + "px;top:" + mouseY + "px");
        earning.text(_tofuUniverse.player.items[0].tps);
        $("#temp").append(earning);
    } else return null;
}
var chat;
function initChat() {

    var text_max = 200;
    $('#numCount').html(text_max + ' characters remaining');

    $('#message').keyup(function () {
        var text_length = $('#message').val().length;
        var text_remaining = text_max - text_length;
        $('#numCount').html(text_remaining + ' characters remaining');
    });
    $('#message').keypress(function () {
        var textWarn = "";
        if ($('#message').val().includes("<script>")) {
            textWarn = '<b><p style="color:red;font-family:Arial;font-size:15px;">Using the script tag is at your own risk!!!</p></b>';
            $('#warning').html(textWarn);
        }
        else {
            textWarn = "";
            $('#warning').html(textWarn);
        }
    });
    // Reference the auto-generated proxy for the hub.
    chat = $.connection.chatHub;
    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message, time) {
        // Add the message to the page.
        $('#discussion').append('<li><strong>' + htmlEncode(name)
            + '</strong>: ' + htmlEncode(message) + '<small class="pull-right text-muted">' + htmlEncode(time) + '</small>' + '</li>');
    };

    $("#s").animate({ "right": "-=300" });
    let isShown = false;
    $("#toggle").click((e) => {
        e.preventDefault();
        if (isShown) {
            $("#s").animate({ "right": "-=300" });
            isShown = false;
        } else {
            $("#s").animate({ "right": "+=300" });
            isShown = true;
        }
        return false;
    });
}
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}