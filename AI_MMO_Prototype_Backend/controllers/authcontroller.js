const { auth } = require("../config/mongo");
const User = require("../models/User");

exports.signup = async (req, res) => {
  const { username, email, password } = req.body;
  if (!username || !email || !password) {
    return res.status(400).json({ error: "Missing fields" });
  }

  try {
    const userRecord = await auth.createUser({
      email,
      password,
      displayName: username,
    });

    await User.createUser(userRecord.uid, username, email);

    res.status(201).json({ message: "User created", uid: userRecord.uid });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
};

exports.login = async (req, res) => {
  const { username } = req.body;
  if (!username) {
    return res.status(400).json({ error: "Username is required" });
  }

  try {
    const userData = await User.getUserByUsername(username);
    if (!userData) {
      return res.status(404).json({ error: "User not found" });
    }

    res.status(200).json({ message: "Login successful", userData });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
};
