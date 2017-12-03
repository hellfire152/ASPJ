//Object that contains ALL Tofu Universe variables and data
let _tofuUniverse = {};

//Loading the game
_tofuUniverse.player = {
    tps: 0,
    tCount: 0,
    "click": 1, //tofu earned per click
    "items": {}, //initially a clone of the ITEMS object
    "upgrades": [], //upgrades owned
    "upgradeEffects": {} //list of effects of said upgrades
};

//item data
_tofuUniverse.ITEMS = {
    0: {
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
    //for each effect
    $.each(effects, (index, effect) => {
        //I LOVE REGEX
        let reg = /(\d+)(?:(?:([\+\-\*\/=])(\d+(?:\.\d+)?))|(?:\.([a-zA-Z]+)(?:([\+\-\*\/=])(\d+(?:\.\d+)?))))/;

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
    //first reset item properties to their base
    item.cost = b.cost
    item.tps = b.tps
    item.description = b.description;
    item.icon = b.icon;

    let item = _tofuUniverse.player.items[benefactorId];
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

    $("#tps").text(tps);
}

//gets all matches for some regex
function regexMatch(regex, string) {
    let matchArr = [];
    let match;
    while (match = regex.exec(string)) {
        matchArr.push(match[index]);
    }
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
            applyUpgrade(_tofuUniverse.UPGRADES[purchaseId]);
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
    i.cost = _tofUniverse.ITEMS[itemId].cost * Math.pow(1.15, i.owned);
    applyEffects(itemId);
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

    //Updating of shop display
    //for items
    $.each(_tofuUniverse.player.items, () => {

    });

    //updating tofu count
    $("#tofu-count").text(Math.round(_tofuUniverse.player.tCount, 1));

    //continue game loop
    requestAnimationFrame(gameloop);
}

window.onload = () => {
    //dynamically generate all the item and upgrade displays
    //item shop
    $.each(_tofuUniverse.ITEMS, (index, item) => {
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
                purchase("upgrade", this.name);
            }
        });
        let description = $("<div>", {
            "id": "shop-upgrade-description-container-" + key,
            "class":"shop-upgrade-description-container"
        });
        let cost = $("<h6>");
        cost.append(upgrade.cost);
        let flair = $("<div>")
        flair.append(upgrade.description);
        let effectDescription = $("<div>");
        effectDescription.append(upgrade.effectDescription);

        let icon = $("<img>", {
            "src": "\\Contect\\images\\upgrades\\"
            + upgrade.icon,
            "class": "shop-upgrade-icon"
        });

        description.append(cost).append(flair).append(effectDescription);
        shopUpgrade.append(icon).append(description);
        $("#shop-upgrades").append(upgrade);
    });

    //load save (if any)

    //set tofu onclick
    $("#tofu").click(() => {
        _tofuUniverse.player.tCount += _tofuUniverse.player.click;

        //the +<tofu earned> thing
        let earning = $("<span>", {
            "class": "click-earn"
        });
        earning.text = _tofuUniverse.player.click;

        //animate the tofu

    });

    //start game loop
    window.requestAnimationFrame(gameloop);
};

//TEST FUNCTION
//gives the player more tofu without triggering anti-cheat
function giveMeMoreTofuPlease(tofu) {
    _tofuUniverse.player.tCount += tofu;
}
