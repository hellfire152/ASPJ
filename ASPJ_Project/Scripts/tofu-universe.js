//Object that contains ALL Tofu Universe variables and data
let _tofuUniverse = {};

//Loading the game
_tofuUniverse.player = {
    "tps": 0,
    "tCount": 0,
    "click": 1, //tofu earned per click
    "items": {}, //initially a clone of the ITEMS object
    "upgrades": [], //upgrades owned
    "upgradeEffects": {} //list of effects of said upgrades
};

//item data
_tofuUniverse.ITEMS = {
    0: {    //RESERVED ID: Click
        "name": "click",
        "tps": 1 //tofu per click
    },
    1: {
        "name": "cursor",
        "cost": 10,
        "tps": 0.1,
        "description": "A cursor to help you click!",
        "icon" : "cursor.png"
    },
    2: {
        "name": "farm",
        "cost": 500,
        "tps": 10,
        "description": "Farm for more tofu!",
        "icon" : "farm.png"
    }
};

//upgrade data
_tofuUniverse.UPGRADES = {
    100: {
        "name": "Quality clicks",
        "description": "Quality > Quantity",
        "effectDescription": "+1 tofu per click",
        "effect": "0+1,1.tps+0.1",
        "cost": 100,
        "icon":"quality-clicks.png"
    }
};

//settings object
_tofuUniverse.settings = {
    "showEarnings" : true
};

//clone ITEMS to player
$.extend(true, _tofuUniverse.player.items, _tofuUniverse.ITEMS);

//setting extra values for player
$.each(_tofuUniverse.player.items, (index, item) => {
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
        console.log("for each effect...");
        //I LOVE REGEX
        let reg = /(\d+)(?:(?:([\+\-\*\/=])(\d+(?:\.\d+)?))|(?:\.([a-zA-Z]+)(?:([\+\-\*\/=])(\d+(?:\.\d+)?))))/;

        console.log("attempting to match...");
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
    item.cost = b.cost;
    item.tps = b.tps
    item.description = b.description;
    item.icon = b.icon;

    //first sort the upgrade effects
    sortUpgradeEffects(benefactorId);
    //now apply the effects!
    $.each(_tofuUniverse.player.upgradeEffects, (index, eff) => {
        switch (eff[1]) {
            case '*':
                item[eff[0]] *= eff[2];
                break;
            case '+':
                item[eff[0]] += eff[2];
                break;
            case '-':
                item[eff[0]] -= eff[2];
                break;
            case '/':
                item[eff[0]] /= eff[2];
                break;
            case '=':
                item[eff[0]] = eff[2];
                break;
        }
    });
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
    console.log(matchArr);
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
                setCost(purchaseId);

                //update owned display
                let o = ++p.items[purchaseId].owned;
                $("#item-owned-" + purchaseId).text(o);
                break;
            } else {
                console.log("Not enough Tofu!");
                break;
            }
        case "upgrade":
            p.upgrades.push(purchaseId);
            applyUpgrade(_tofuUniverse.UPGRADES[purchaseId].effect);
            break;
        case "beanUpgrade":
            break;
    }
    recalculateTotalTps();
}

/*
    Handles the calculation regarding the increasing
    cost of items
*/
function setCost(itemId) {
    let i = _tofuUniverse.player.items[itemId];
    i.cost = _tofuUniverse.ITEMS[itemId].cost * Math.pow(1.15, i.owned);
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
    _tofuUniverse.player.tCount += s * _tofuUniverse.player.tps, 1;

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
        if (index == 0) return; //do not create shop item for click
        let shopItem = $("<div>", {
            "id": "shop-item-" + index,
            "name": index,
            "class": "shop-item",
            "click": () => {
                purchase("item", index);
            },
        });
        let description = $("<div>", {
            "id": "shop-item-description-" + index,
            "class": "shop-item-description"
        });
        let icon = $("<img>", {
            "src": "\\Content\\Images\\Items\\"
            + item.icon,
            "class": "shop-item-icon"
        });
        let cost = $("<span>", {
            "class": "shop-item-cost",
            "id": "item-cost-" + index
        });
        let owned = $("<span>", {
            "class": "shop-item-owned",
            "id": "item-owned-" + index
        });
        description.append(_tofuUniverse.ITEMS[index].description);
        owned.append("0");

        shopItem.append(icon).append(description).append(owned);
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
            "src": "\\Contect\\images\\upgrades\\"
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
        _tofuUniverse.player.tCount += _tofuUniverse.player.click;

        if (_tofuUniverse.settings.showEarnings) {
            //the +<tofu earned> thing
            let earning = $("<span>", {
                "class": "click-earn"
            });
            earning.css("style", "position:absolute;left:"+mouseX);
            earning.text(_tofuUniverse.player.click);
            $("temp").append(earning);
            
            //animate the tofu
            earning.css("opacity", 1);
            earning.t = setInterval(() => {
                if (earning.css("opacity") == 0) {
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
