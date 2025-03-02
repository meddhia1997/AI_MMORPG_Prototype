const express = require("express");
const router = express.Router();
const playerController = require("../controllers/playerController");

router.get("/:username", playerController.getPlayerData);
router.put("/:username/update", playerController.updatePlayerData);

module.exports = router;
