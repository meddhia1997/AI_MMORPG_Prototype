const express = require('express');
const bodyParser = require('body-parser');
const authRoutes = require('./routes/auth');
const mongoose = require('./config/mongo');  // Import MongoDB connection
const app = express();

// Middleware to parse incoming requests
app.use(bodyParser.json());

// Use the authentication routes
app.use('/auth', authRoutes);

// Start the server
const port = 5000;
app.listen(port, () => {
  console.log(`Server running on port ${port}`);
});
