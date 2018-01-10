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
$.connection.hub.start();

//Loading the game
_tofuUniverse.player = {
    "tps": 0,
    "tCount": 0,
    "click": 1, //tofu earned per click
    "items": {}, //initially a clone of the ITEMS object
    "upgrades": [], //upgrades owned
    "upgradeEffects": {} //list of effects of said upgrades
};

//settings object
_tofuUniverse.settings = {
    "showEarnings" : true
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
    console.log(effects);
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
        console.log("Applying effect:");
        console.log(eff);
        console.log("BEFORE: " + item[eff[0]]);
        console.log(item[eff[0]]);
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
        console.log("AFTER: " + item[eff[0]]);
    });
    setText(benefactorId); 
}

//object containing the operator hierarchy, used in sortUpgradeEffects
let operatorRank = {
    '+': 0,
    '-': 1,
    '*': 2,
    '/': 3,
    '=': 4
};
//sorts the upgrade effects list, so that applying upgrades is easier
function sortUpgradeEffects(benefactorId) {
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

    $("#tps").text(round(tps, 1));
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
function purchase(purchaseType, purchaseId) {
    //update player data if successful
    let p = _tofuUniverse.player;
    switch (purchaseType) {
        case "item":
            //check cost
            if (p.tCount >= p.items[purchaseId].cost) {
                //pay cost
                p.tCount -= p.items[purchaseId].cost;
                //add to tps
                p.tps += p.items[purchaseId].tps;
                //increase cost of item
                applyEffects(purchaseId);

                //update owned display
                let o = ++p.items[purchaseId].owned;
                setText(purchaseId);
                $("#item-owned-" + purchaseId).text(o);
                break;
            } else {
                console.log("Not enough Tofu!");
                break;
            }
        case "upgrade":
            //check cost and if purchased before
            if (p.tCount >= _tofuUniverse.UPGRADES[purchaseId].cost
                && p.upgrades.indexOf(purchaseId) < 0) {
                //pay cost
                p.tCount -= _tofuUniverse.UPGRADES[purchaseId].cost;
                //applying upgrade
                p.upgrades.push(purchaseId);
                applyUpgrade(_tofuUniverse.UPGRADES[purchaseId].effect);
                //removing the upgrade purchase icon
                $("#shop-upgrade-" + purchaseId).remove();
            } else {
                console.log("Not enough tofu (upgrade)!");
            }
            break;
        case "beanUpgrade":
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
    $("#shop-item-cost-" + itemId).text(round(i.cost, 0) + " Tofu");
    $("#shop-item-name-" + itemId).text(i.name);
    $("#shop-item-description" + itemId).text(i.description);
}

//does all the ui updating
var t, dt;
function gameloop(time) {
    //settling delta time
    if (!t) {
        t = time;
        dt = time;
    }
    dt = time - t;
    t = time;
    let s = dt / 1000; //delta time in seconds

    //calculate auto-generated tofu
    _tofuUniverse.player.tCount += s * _tofuUniverse.player.tps;

    //updating tofu count
    $("#tofu-count").text(round(_tofuUniverse.player.tCount, 0));

    //continue game loop
    requestAnimationFrame(gameloop);
}

//function to help me with rounding decimals
function round(value, decimals) {
    return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
}

//tracking variables for mouse cursor
var mouseX, mouseY;
window.onload = () => {
    //always track position of the mouse in tofu area
    $("#tofu-area").mousemove((e) => {
        mouseX = e.pageX;
        mouseY = e.pageY;
    });

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
        name.append(item.name);
        let description = $("<div>", {
            "id": "shop-item-description-" + index,
            "class": "shop-item-description"
        });
        description.append(item.description);
        let icon = $("<img>", {
            "src": "\\Content\\Images\\Items\\"
            + item.icon,
            "class": "shop-item-icon"
        });
        let cost = $("<div>", {
            "class": "shop-item-cost",
            "id": "shop-item-cost-" + index
        });
        cost.append(item.cost + ' Tofu')
        let owned = $("<span>", {
            "class": "shop-item-owned",
            "id": "item-owned-" + index
        });
        owned.append(0);

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
        cost.append(upgrade.cost);
        let flair = $("<div>", {
            "id": "upgrade-description-flair-" + key,
            "class": "upgrade-description-flair"
        });
        flair.append(upgrade.description);
        let effectDescription = $("<div>", {
            "id": "upgrade-description-effect-" + key,
            "class":"upgrade-description-effect"
        });
        effectDescription.append(upgrade.effectDescription);

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

    //set tofu onclick
    $("#tofu").click(() => {
        _tofuUniverse.player.tCount += _tofuUniverse.player.items[0].tps;

        if (_tofuUniverse.settings.showEarnings) {
            //the +<tofu earned> thing
            let earning = $("<span>", {
                "class": "click-earn"
            });
            earning.css("style", "position:absolute;left:"+mouseX +"px;top:" +mouseY +"px");
            earning.text(_tofuUniverse.player.items[0].tps);
            $("#temp").append(earning);
            
            //animate the tofu
            earning.css("opacity", 1);
            earning.t = setInterval(() => {
                if (earning.css("opacity") === 0) {
                    earning.remove();
                    clearInterval(earning.t);
                }

                //set opacity
                earning.css("opacity", parseInt(earning.css("opacity") - 0.05));

                //float up
                earning.css("top", parseInt(mouseY) + 'px');
            }, 50);
        }
       
    });

    //disabling selection so stuff isn't highlighted constantly
    $(".shop-item").disableSelection();
    $(".shop-upgrade").disableSelection();
    $("#tofu").disableSelection();
    $("#tofu-count-container").disableSelection();
    $("#tps-container").disableSelection();
    
    //start game loop
    window.requestAnimationFrame(gameloop);
};

//TEST FUNCTIONS
//gives the player more tofu without triggering anti-cheat
function giveMeMoreTofuPlease(tofu) {
    _tofuUniverse.player.tCount += tofu;
}
//applies a setting
function setting(property, value) {
    _tofuUniverse.settings[property] = value;
}
