//This file contains the main game code of Tofu Universe
//tofu icon used pretty much everywhere
function tofuIcon() {
    return $("<img>", {
        "src": "\\Content\\Images\\tofu.png",
        "width": "15px",
        "height": "15px"
    });
}
//extending jquery for my disableSelection function
jQuery.fn.extend({
    disableSelection: function () {
        return this.each(function () {
            this.onselectstart = function () { return false; };
            this.unselectable = "on";
            jQuery(this).css('user-select', 'none');
            jQuery(this).css('-o-user-select', 'none');
            jQuery(this).css('-moz-user-select', 'none');
            jQuery(this).css('-khtml-user-select', 'none');
            jQuery(this).css('-webkit-user-select', 'none');
            jQuery(this).css('user-drag', 'none');
        });
    }
});

//SignalR stuff
_tofuUniverse.conn = $.connection.tofuUniverseHub;
_tofuUniverse.conn.client.pong = (message) => {
    console.log(message);
}

//Loading the game
_tofuUniverse.player = {
    "tps": 0,
    "tCount": 0,
    "click": 1, //tofu earned per click
    "items": {}, //initially a clone of the ITEMS object
    "upgrades": [], //upgrades owned
    "upgradeEffects": {}, //list of effects of said upgrades
    //tracking information
    "tofuClicks": 0,
    "purchases" : []
};

//settings object
_tofuUniverse.settings = {
    "showEarnings": true,
    "autosave": false
};

//clone ITEMS to player
$.extend(true, _tofuUniverse.player.items, _tofuUniverse.ITEMS);

//setting extra values for player
$.each(_tofuUniverse.player.items, (index, item) => {
    item.baseCost = item.cost;
    item.owned = 0;
    _tofuUniverse.player.upgradeEffects[index] = [];
});

//function for applying upgrades
function applyUpgrade(upgradeText) {
    let p = _tofuUniverse.player;

    //split multiple effects up
    let effects = upgradeText.split(",");
    //for each effect
    $.each(effects, (index, effect) => {
        //I LOVE REGEX
        let reg = /(\d+)(?:(?:([\+\-\*\/=])((?:\d+(?:\.\d+)?)|\w+))|(?:\.([a-zA-Z]+)(?:([\+\-\*\/=])((?:\d+(?:\.\d+)?)|\w+))))/;
        let matches = regexMatch(reg, effect);

        //form of "<id><op><value>" <- implies tps/click property
        //or "<id><property><op><value>" <- set any other property
        let tpsAdd = matches.length <= 3 ? true : false;

        //get effect values
        let benefactorId = matches[0];
        let benefactorProperty = tpsAdd ? "tps" : matches[1];
        let operator = tpsAdd ? matches[1] : matches[2];
        let operand = tpsAdd ? matches[2] : matches[3];

        //adding upgrade to the upgrade list
        let eff = [benefactorProperty, operator, operand];
        p.upgradeEffects[benefactorId].push(eff);
        applyEffects(benefactorId);
    });
}

function applyEffects(benefactorId) {
    let b = _tofuUniverse.ITEMS[benefactorId];
    let item = _tofuUniverse.player.items[benefactorId];
    //first reset item properties to their base
    item.baseCost = b.cost;
    item.cost = b.cost;
    item.tps = b.tps
    item.description = b.description;
    item.icon = b.icon;

    //first sort the upgrade effects
    sortUpgradeEffects(benefactorId);
    //now apply the effects!
    $.each(_tofuUniverse.player.upgradeEffects[benefactorId], (index, eff) => {
        switch (eff[1]) {
            case '*':
                item[eff[0]] *= Number(eff[2]);
                break;
            case '+':
                item[eff[0]] += Number(eff[2]);
                break;
            case '-':
                item[eff[0]] -= Number(eff[2]);
                break;
            case '/':
                item[eff[0]] /= Number(eff[2]);
                break;
            case '=':
                if (isNaN(eff[2]))
                    item[eff[0]] = eff[2];
                else item[eff[0]] = Number(eff[2]);
                break;
        }
    });
    setText(benefactorId); 
}

//object containing the operator hierarchy, used in sortUpgradeEffects
//sorts the upgrade effects list, so that applying upgrades is easier
function sortUpgradeEffects(benefactorId) {
    let operatorRank = {
        '+': 0,
        '-': 1,
        '*': 2,
        '/': 3,
        '=': 4
    };
    let upgradeEffects = _tofuUniverse.player.upgradeEffects[benefactorId];

    upgradeEffects.sort((a, b) => {
        return operatorRank[a] - operatorRank[b];
    });
}
 
//recalculates the total tps across all upgrades and items, and updates the display
function recalculateTotalTps() {
    let tps = 0;
    $.each(_tofuUniverse.player.items, (index, item) => {
        tps += item.tps * item.owned;
    });

    $("#tps").text(formatTofuCount(tps));
    _tofuUniverse.player.tps = tps;
}

//gets all matches for some regex
function regexMatch(regex, string) {
    let matchArr = [];
    let match = regex.exec(string);
    $.each(match, (index, m) => {
        if(m !== string && m !== undefined)
             matchArr.push(m);
    });
    return matchArr;
}

//processes all purchases
function purchase(purchaseType, purchaseId, fromSave) {
    //update player data if successful
    let p = _tofuUniverse.player;
    //log regular purchases
    if (!fromSave) {
        let p = _tofuUniverse.player;
        p["purchases"].push([Date.now(), purchaseType, purchaseId]);
    }
    switch (purchaseType) {
        case "item":
            //check cost
            if (p.tCount >= p.items[purchaseId].cost || fromSave) {
                //pay cost
                if(!fromSave) p.tCount -= p.items[purchaseId].cost;
                //add to tps
                p.tps += p.items[purchaseId].tps;
                //increase cost of item
                applyEffects(purchaseId);

                //update owned display
                p.items[purchaseId].owned++;
                setText(purchaseId);
                $("#item-owned-" + purchaseId).text(p.items[purchaseId].owned);
                break;
            } else {
                showNotification("Not enough Tofu!", "Go and earn more tofu!");
                break;
            }
        case "upgrade":
            //check cost and if purchased before
            if (p.upgrades.indexOf(purchaseId) < 0) {
                if (p.tCount >= _tofuUniverse.UPGRADES[purchaseId].cost || fromSave) {
                    //pay cost
                    if(!fromSave) p.tCount -= _tofuUniverse.UPGRADES[purchaseId].cost;
                    //applying upgrade
                    p.upgrades.push(purchaseId);
                    applyUpgrade(_tofuUniverse.UPGRADES[purchaseId].effect);
                    //removing the upgrade purchase icon
                    $("#shop-upgrade-" + purchaseId).remove();
                }
            } else {
                showNotification("Not enough Tofu!", "Go and earn more tofu!");
            }
            break;
    }
    recalculateTotalTps();
}

/*
    Handles the calculation regarding the increasing
    cost of items, and updates the display to reflect all changes
*/
function setText(itemId) {
    let i = _tofuUniverse.player.items[itemId];
    i.cost = Math.round(i.baseCost * Math.pow(1.15, i.owned));
    //update display
    $("#shop-item-cost-" + itemId).text(formatTofuCount(i.cost));
    $("#shop-item-cost-" + itemId).append(tofuIcon());
    $("#shop-item-name-" + itemId).text(i.name);
    $("#shop-item-description" + itemId).text(i.description);
}

//does all the ui updating
var t, dt, lt = 0;
function gameloop(time) {
    //settling delta time
    if (!t) {
        t = time;
        dt = time;
    }
    dt = time - t;
    t = time;
    let s = dt / 1000; //delta time in seconds
    lt += s;

    if (lt >= 30 && _tofuUniverse.settings.autosave) {
        saveProgress();
        lt = 0;
    }

    //calculate auto-generated tofu
    _tofuUniverse.player.tCount += s * _tofuUniverse.player.tps;

    //updating tofu count
    $("#tofu-count").text(formatTofuCount(_tofuUniverse.player.tCount));

    //continue game loop
    requestAnimationFrame(gameloop);
}

//takes in ugly long numbers and writes them in terms of the highest fitting 'illion'
function formatTofuCount(tCount, short) {
    tCount = round(tCount, 0);
    let tString = "" + tCount;
    let significantsCount = tString.length % 3;
    if (tString.length <= 3) {
        return tString;
    } else if (tString.length <= 6) {
        if (significantsCount == 0) significantsCount = 3;
        let sigs = tString.substr(0, significantsCount);
        return sigs + ',' + tString.substr(significantsCount);
    } else {
        let sig = tString.substr(0, significantsCount + 2) + ' ';
        switch (Math.floor(tString.length / 3)) {
            case 2: {
                sig += (short == true) ? "m" : "million";
                break;
            }
            case 3: {
                sig += (short == true) ? "b" : "billion";
                break;
            }
            case 4: {
                sig += (short == true) ? "t" : "trillion";
                break;
            }
            case 5: {
                sig += (short == true) ? "q" : "quadrillion";
                break;
            }
            default: {
                sig = "Just collapse into a singularity already..."
                break;
            }
        }
        return sig;
    }
}

//function to help me with rounding decimals
function round(value, decimals) {
    return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
}

//sends progress data to the server
function saveProgress(callback) {
    //get item data
    let owned = {};
    $.each(_tofuUniverse.player.items, (key, item) => {
        owned[key] = item.owned;
    });

    //send data to server
    _tofuUniverse.conn.server.saveProgress({
        "tCount": _tofuUniverse.player.tCount,
        "items": owned,
        "upgrades": _tofuUniverse.player.upgrades,
        "tofuClicks": _tofuUniverse.player.tofuClicks,
        "purchases": _tofuUniverse.player.purchases
    }).done((resultId) => {
        console.log(resultId);
        switch (resultId) {
            case 1:
                showNotification("Save success!", "");
                break;
            case 0:
                showNotification("You're saving too often!", "Just wait a little while...");
                break;
            case -1:
                showAlert();
                break;
            case -2:
                showNotification("Error saving progress", "");
                break;
            default:
                console.log("Result ID not recognized!");
                break;
        }
        if (callback) callback(resultId);
    });

    //reset tracking variables
    _tofuUniverse.player.tofuClicks = 0;
    _tofuUniverse.player.purchases = [];
};

//tracking variables for mouse cursor
var mouseX, mouseY;
window.onload = () => {
    //dynamically generate all the item and upgrade displays
    //item shop
    $.each(_tofuUniverse.ITEMS, (index, item) => {
        if (index < 1) return; //do not create shop item for reserved ids
        let shopItem = $("<div>", {
            "id": "shop-item-" + index,
            "name": index,
            "class": "shop-item",
            "click": () => {
                purchase("item", index);
            },
        });
        let name = $("<div>", {
            "id": "shop-item-name-" + index,
            "class": "shop-item-name"
        });
        name.text(item.name);
        let description = $("<div>", {
            "id": "shop-item-description-" + index,
            "class": "shop-item-description"
        });
        description.text(item.description);
        let icon = $("<img>", {
            "src": "\\Content\\Images\\Items\\"
            + item.icon,
            "class": "shop-item-icon"
        });
        let cost = $("<div>", {
            "class": "shop-item-cost",
            "id": "shop-item-cost-" + index
        });
        cost.text(formatTofuCount(item.cost));
        cost.append(tofuIcon());
        let owned = $("<span>", {
            "class": "shop-item-owned",
            "id": "item-owned-" + index
        });
        owned.text(0);

        shopItem.append(icon).append(name).append(owned)
            .append(cost).append(description);
        $("#shop-items").append(shopItem);
    });
    //upgrade shop
    $.each(_tofuUniverse.UPGRADES, (key, upgrade) => {
        let shopUpgrade = $("<div>", {
            "id": "shop-upgrade-" + key,
            "name": key,
            "class": "shop-upgrade",
            "click": () => {
                purchase("upgrade", key);
            }
        });
        let description = $("<div>", {
            "id": "upgrade-description-container-" + key,
            "class":"upgrade-description-container"
        });
        let cost = $("<h6>", {
            "id":"upgrade-description-cost-" + key,
            "class":"upgrade-description-cost"
        });
        cost.text(formatTofuCount(upgrade.cost));
        cost.append(tofuIcon());
        let flair = $("<div>", {
            "id": "upgrade-description-flair-" + key,
            "class": "upgrade-description-flair"
        });
        flair.text(upgrade.description);
        let effectDescription = $("<div>", {
            "id": "upgrade-description-effect-" + key,
            "class":"upgrade-description-effect"
        });
        effectDescription.text(upgrade.effectDescription);

        let icon = $("<img>", {
            "src": "\\Content\\Images\\Upgrades\\"
            + upgrade.icon,
            "class": "shop-upgrade-icon"
        });

        description.append(cost).append(flair).append(effectDescription);
        shopUpgrade.append(icon).append(description);
        $("#shop-upgrades").append(shopUpgrade);
    });

    //load save (if any)
    $.connection.hub.start()
        .done(() => {
            _tofuUniverse.conn.server.requestSave(_tofuUniverse.code)
                .done((rawSave) => {
                    if (rawSave !== null) {
                        console.log("SAVE: " + rawSave);
                        let save = JSON.parse(rawSave);
                        console.log(save);

                        //use purchase method to redo progress
                        let p = _tofuUniverse.player;
                        p.tCount = save.tCount;
                        $.each(save.items, (key, owned) => {
                            for (let i = 0; i < owned; i++) {
                                purchase("item", key, true);
                            }
                        });
                        $.each(save.upgrades, (key, upgradeId) => {
                            purchase("upgrade", upgradeId, true);
                        });
                    }
                    else {
                        console.log("No save found!");
                    }
                });
        });

    //set tofu onclick
    $("#tofu").click((e) => {
        _tofuUniverse.player.tofuClicks++;
        _tofuUniverse.player.tCount += _tofuUniverse.player.items[0].tps;

        if (_tofuUniverse.settings.showEarnings) {
            //the +<tofu earned> thing
            let earning = $("<span>");
            earning.addClass("click-earn");
            earning.disableSelection();
            earning.css({
                "position": "fixed",
                "left": e.clientX,
                "top": e.clientY,
                "font-size": "1.5em",
                "-ms-transform": "scale(1.5)",
                "-webkit-transform": "scale(1.5)",
                "transform": "scale(1.5)",
            });
            earning.text('+' + _tofuUniverse.player.items[0].tps);
            earning.append(tofuIcon());
            $("#click-earnings").append(earning);
            
            //animate the tofu
            earning.animate({
                top: "-=100",
                opacity: 0
            }, 3000, () => {
                earning.remove();
            });
        }
    });

    //disabling selection so stuff isn't highlighted constantly
    $(".shop-item").disableSelection();
    $(".shop-upgrade").disableSelection();
    $("#tofu").disableSelection();
    $("#tofu-count-container").disableSelection();
    $("#tps-container").disableSelection();
    $("#tofu-area").disableSelection();

    /*---Cheat Alert code---*/
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

    //start game loop
    window.requestAnimationFrame(gameloop);
};

//Attempts to end the game
_tofuUniverse.collapse = function() {
    saveProgress(() => {
        _tofuUniverse.conn.server.collapse()
            .done((result) => {
                if (result == 1) {
                    $("body").empty();
                    let endVideoElement = $("<video autoplay id='ending-video'></video>");
                    let sourceElement = $("<source>", {
                        "src": "tofu-collapse.mp4",
                        "type": "video/mp4"
                    });
                    endVideoElement.append(sourceElement);

                    endVideoElement.on('ended', () => {
                        console.log("You have reached the end!");
                    });
                    $('body').append(endVideoElement);
                } else {
                    showNotification("Not enough tofu!");
                }
            });
    });
}

//TEST FUNCTIONS
//gives the player more tofu without triggering anti-cheat
function giveMeMoreTofuPlease(tofu) {
    _tofuUniverse.player.tCount += tofu;
}
//applies a setting
function setting(property, value) {
    _tofuUniverse.settings[property] = value;
}

