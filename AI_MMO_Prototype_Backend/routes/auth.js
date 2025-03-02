const express = require('express');
const bcrypt = require('bcryptjs');
const User = require('../models/User');
const jwt = require('jsonwebtoken');
const router = express.Router();

const JWT_SECRET = 'twj'; // Change this to an environment variable in production

// Sign Up Route (POST)
router.post('/signup', async (req, res) => {
  const { username, email, password, playerdata } = req.body;

  try {
    const existingUser = await User.findOne({ email });
    if (existingUser) {
      return res.status(400).send({ error: 'Email is already taken' });
    }

    const hashedPassword = await bcrypt.hash(password, 10);

    const newUser = new User({
      username,
      email,
      password: hashedPassword,
      playerdata: playerdata || {},
    });

    await newUser.save();

    // Create a token that includes playerdata
    const token = jwt.sign(
      {
        userId: newUser._id,
        username: newUser.username,
        email: newUser.email,
        playerdata: newUser.playerdata, // Embed playerdata in token
      },
      JWT_SECRET,
      { expiresIn: '1h' }
    );

    res.status(201).send({
      message: 'User created successfully',
      token, // Send token
    });
  } catch (error) {
    res.status(400).send({ error: error.message });
  }
});

// Login Route (POST)
router.post('/login', async (req, res) => {
  const { email, password } = req.body;

  try {
    const user = await User.findOne({ email });
    if (!user) {
      return res.status(400).send({ error: 'Invalid credentials' });
    }

    const isMatch = await bcrypt.compare(password, user.password);
    if (!isMatch) {
      return res.status(400).send({ error: 'Invalid credentials' });
    }

    // Create a token that includes playerdata
    const token = jwt.sign(
      {
        userId: user._id,
        username: user.username,
        email: user.email,
        playerdata: user.playerdata, // Embed playerdata in token
      },
      JWT_SECRET,
      { expiresIn: '1h' }
    );

    res.status(200).send({
      message: 'Login successful',
      token, // Send token with embedded playerdata
    });
  } catch (error) {
    res.status(400).send({ error: error.message });
  }
});

// Update Player Data Route (PUT)
router.put('/updatePlayerData', async (req, res) => {
  const { token, playerdata } = req.body;

  try {
    // Verify the JWT token and extract user info
    const decoded = jwt.verify(token, JWT_SECRET);
    const user = await User.findById(decoded.userId);
    
    if (!user) {
      return res.status(404).send({ error: 'User not found' });
    }

    // Handle playerdata based on its type (Map or Object)
    for (let key in playerdata) {
      if (user.playerdata instanceof Map) {
        // If the key already exists, update it
        if (user.playerdata.has(key)) {
          user.playerdata.set(key, playerdata[key]);
        } else {
          // If the key doesn't exist, add it
          user.playerdata.set(key, playerdata[key]);
        }
      } else {
        // If playerdata is an Object, merge it (replace the key-value pair)
        user.playerdata = { ...user.playerdata.toObject(), ...playerdata };
      }
    }

    // Save the updated user
    await user.save();

    // Generate a new token with updated playerdata
    const updatedToken = jwt.sign(
      {
        userId: user._id,
        username: user.username,
        email: user.email,
        playerdata: user.playerdata, // Updated playerdata
      },
      JWT_SECRET,
      { expiresIn: '1h' }
    );

    // Return the response with the updated token
    res.status(200).send({
      message: 'Player data updated successfully',
      token: updatedToken, // Return updated token
    });
  } catch (error) {
    res.status(400).send({ error: error.message });
  }
});
// Route to get player data (Token in request body)
router.post("/getPlayerData", async (req, res) => {
  try {
      const { token } = req.body;  // Get token from request body

      if (!token) {
          return res.status(400).json({ error: "Token must be provided" });
      }

      // Verify JWT Token
      let decoded;
      try {
          decoded = jwt.verify(token,JWT_SECRET);
      } catch (error) {
          return res.status(401).json({ error: "Invalid token" });
      }

      // Fetch user data using userId from the token
      const user = await User.findOne({ _id: decoded.userId });

      if (!user) {
          return res.status(404).json({ error: "User not found" });
      }

      res.json({ success: true, playerdata: user.playerdata });
  } catch (error) {
      console.error("Error fetching player data:", error);
      res.status(500).json({ error: "Internal Server Error" });
  }
});


module.exports = router;
