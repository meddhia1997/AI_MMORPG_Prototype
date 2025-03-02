const User = require("../models/User");

exports.getPlayerData = async (req, res) => {
  const { username } = req.params;

  try {
    const userData = await User.getUserByUsername(username);
    if (!userData) {
      return res.status(404).json({ error: "Player not found" });
    }

    res.status(200).json(userData.playerData);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
};

exports.updatePlayerData = async (req, res) => {
  const { username } = req.params;
  const { playerData } = req.body;

  try {
    const userData = await User.getUserByUsername(username);
    if (!userData) {
      return res.status(404).json({ error: "Player not found" });
    }

    await User.updatePlayerData(username, playerData);

    res.status(200).json({ message: "Player data updated" });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
};
