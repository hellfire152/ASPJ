//Object that contains ALL Tofu Universe variables and data
let _tofuUniverse = {};

//Loading the game
_tofuUniverse.player = {
    tps: 0,
    tCount: 0,
    "click": 1, //tofu earned per click
    "items": {  //items owned by id
        1: 0,
        2: 0
    },
    "itemTps": {},
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
        "effect": ["0+1,1.tps+0.1"]
    }
};

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
    jQuery.each(effects, (index, item) => {
        let re = /\d/
    });
}

//processes all purchases
function(purchaseType, purchaseId) {
    let _tofuUniverse.player = p;
    switch (purchaseType) {
        case "item":
            p.tps += p.itemTps[purchaseId];
            break;
        case "upgrade":
            
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
    for (let itemKey in _tofuUniverse.ITEMS) {
        if (_tofuUniverse.ITEMS.hasOwnProperty(itemKey)) {
            let shopItem = $("<div>", {
                "id": "shop-item-" + itemKey,
                "name" : itemKey,
                "class": "shop-item",
                "click": () => {
                    purchase("item", this.name);
                }
            });
            let icon = $("<img>", {
                "src": "~\\Content\\images\\items\\"
                + _tofuUniverse.ITEMS[itemKey].icon
            });
            shopItem.append(icon);
            $("shop-items").append(shopItem);
        }
    }
    //upgrade shop

    //load save (if any)

    //set tofu onclick
    $("#tofu-img").click(() => {
        _tofuUniverse.player.tCount += _tofuUniverse.player.click;
    });

    //start game loop
    window.requestAnimationFrame(gameloop);
});
