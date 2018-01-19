//Object that contains ALL Tofu Universe variables and data
let _tofuUniverse = {};

//item data
_tofuUniverse.ITEMS = {
    0: {    //RESERVED ID: Click
        "name": "click",
        "tps": 1 //tofu per click
    },
    1: {
        "name": "Cursor",
        "cost": 10,
        "tps": 0.1,
        "description": "A cursor to help you click!",
        "icon": "cursor.png"
    },
    2: {
        "name": "Farm",
        "cost": 500,
        "tps": 10,
        "description": "Farm for more tofu!",
        "icon": "farm.png"
    },
    3: {
        "name": "test",
        "cost": 1,
        "tps": 1000,
        "description": "TESTING",
        "icon": "test1.jpg"
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
        "icon": "bettercursor.jpg"
    },
    1000: {
        "name": "Test",
        "description": "TESTING",
        "effectDescription": "+1000 tofu per click",
        "effect": "0+1000",
        "cost": 1,
        "icon": "test2.jpg"
    },
    1001: {
        "name": "Test2",
        "description": "testing other stuff",
        "effectDescription": "Testing other properties",
        "effect": "3.name=UPGRADED,1.tps+1000.0",
        "cost": 100,
        "icon": "test2.jpg"
    }
};