﻿//Object that contains ALL Tofu Universe variables and data
let _tofuUniverse = {};

//Loading the game
_tofuUniverse.player = {
    tps: 0,
    tCount: 0,
    "click": 1, //tofu earned per click
    "items": {}, //initially a clone of the ITEMS object
    "upgrades": [], //upgrades owned
    "upgradeEffects": {   //effects of said upgrades
        0: [],
        1: [],
        2: []
    }
};

//item data
_tofuUniverse.ITEMS = {
    1: {
        "name": "cursor",
        "baseCost": "10",
        "baseTps": 0.1,
        "tps": 0.1,
        "description": "A cursor to help you click!",
        "icon" : "cursor.png"
    },
    2: {
        "name": "farm",
        "baseCost": "500",
        "baseTps": 10,  //base tps
        "description": "Farm for more tofu!",
        "icon" : "farm.png"
    }
};

//upgrade data
_tofuUniverse.UPGRADES = {
    1: {
        "name": "Quality clicks",
        "description": "Quality > Quantity",
        "effectDescription": "+1 tofu per click",
        "effect": ["0+1,1.tps+0.1"],
        "cost": 100,
        "icon":"quality-clicks.png"
    }
};

//clone ITEMS to player
Object.assign(_tofuUniverse.player.items, _tofuUniverse.ITEMS);

//setting tps of items
for (let itemKey in _tofuUniverse.ITEMS) {
    if (_tofuUniverse.ITEMS.hasOwnProperty(itemKey)) {
        let i = _tofuUniverse.player.itemTps;
        i[itemKey] = _tofuUniverse.ITEMS[itemKey]; //set base tps
    }
}

//function for applying upgrades
function applyUpgrade(upgradeText) {
    //split multiple effects up
    let effects = upgradeText.split(",");
    //for each effect
    $.each(effects, (index, effect) => {
        //I LOVE REGEX
        let reg = /(\d+)(?:(?:([\+\-\*\/=])(\d+(?:\.\d+)?))|(?:\.([a-zA-Z]+)(?:([\+\-\*\/=])(\d+(?:\.\d+)?))))/;

        let matches = regexMatch(reg, effect);

        if (matches[0] === 0) {  //upgrade to the click

        } else {

        }
        //form of "<id><op><value>" <- implies tps/click property
        //or "<id><property><op><value>" <- set any other property
        let tpsAdd = matches.length <= 3 ? true : false;

        //get effect values
        let benefactorId = matches[0];
        let benefactorProperty = tpsAdd ? "tps" : matches[1];
        let operator = tpsAdd ? matches[1] : matches[2];
        let operand = tpsAdd ? matches[2] : matches[3];

        //adding upgrade to the upgrade list
        _tofuUniverse.player.upgradeEffects[benefactorId].push([operator, operand]);
        //^^^Why do I do this to myself^^^
        //update tps value after upgrade
        updateTps(benefactorId);
    });
}

//recalculates the tps of an item after upgrades
function updateTps(itemId) {
    let upgradeEffectsList = _tofuUniverse.player.upgradeEffects[itemId];

    //applying upgrades
    let multiplier = 1;
    let tps = _tofuUniverse.ITEMS[itemId].baseTps;
    //tps additions apply BEFORE multiplier
    $.each(upgradeEffectsList, (index, value) => {
        switch (value[0]) {
            case '*':
                multiplier * value[1]; //multiplier stacks multiplicatively
                break;
            case '/':
                multiplier / value[1];
                break;
            case '+':
                tps + value[1];
                break;
            case '-':
                tps - value[1];
                break;
        }
    });

    //calculate and update tps
    _tofuUniverse.player.items[itemId].tps = tps * multiplier;
}

//gets all matches for some regex
function regexMatch(regex, string) {
    let matchArr = [];
    let match;
    while (match = regex.exec(string)) {
        matcheArr.push(match[index]);
    }
    return matchArr;
}

//processes all purchases
function purchase(purchaseType, purchaseId) {
    //check costs

    //update player data if successful
    let p = _tofuUniverse.player;
    switch (purchaseType) {
        case "item":
            p.tps += p.itemTps[purchaseId];
            break;
        case "upgrade":
            p.upgrades.push(purchaseId);
            applyUpgrade(_tofuUniverse.UPGRADES[purchaseId]);
            break;
        case "beanUpgrade":
            break;
    }
}

//does all the ui updating
var t, dt;
function gameloop(time) {
    if (!t) t = time;
    dt = time - t;

    //calculate auto-generated tofu
    let s = dt / 1000; //convert ms to s
    _tofuUniverse.player.tCount += s * _tofuUniverse.player.tps;
    
}

window.onload(() => {
    //dynamically generate all the item and upgrade displays
    //item shop
    $("#shop-items").style.display = "block";    //show item shop first
    $.each(_tofuUniverse.ITEMS, (index, item) => {
        let shopItem = $("<div>", {
            "id": "shop-item-" + index,
            "name": index,
            "cost": item.baseCost,
            "class": "shop-item",
            "click": () => {
                purchase("item", index);
            }
        });
        let icon = $("<img>", {
            "src": "~\\Content\\images\\items\\"
            + item.icon
        });
        shopItem.append(icon);
        $("shop-items").append(shopItem);
    });
    //upgrade shop
    $.each(_tofuUniverse.UPGRADES, (index, upgrade) => {
        let shopUpgrade = $("<div>", {
            "id": "shop-upgrade-" + index,
            "name": index,
            "cost": upgrade.cost,
            "class": "shop-upgrade",
            "click": () => {
                purchase("upgrade", index);
            }
        });
        let icon = $("<img>", {
            "src": "~\\Contect\\images\\upgrades\\"
            +  upgrade.icon
        });
    });

    //load save (if any)

    //set tofu onclick
    $("#tofu-img").click(() => {
        _tofuUniverse.player.tCount += _tofuUniverse.player.click;
    });

    //start game loop
    window.requestAnimationFrame(gameloop);
});
