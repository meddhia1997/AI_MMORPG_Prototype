const mongoose = require('mongoose');

// Define the user schema with the playerdata field as a JSON object
const userSchema = new mongoose.Schema({
  username: { type: String, required: true },
  email: { type: String, required: true, unique: true },
  password: { type: String, required: true },
  playerdata: { 
    type: Map, 
    of: mongoose.Schema.Types.Mixed,  // Allows storing arbitrary JSON data
    default: {} 
  }
}, { timestamps: true });  // Timestamps to store creation/updated times

// Create the User model from the schema
const User = mongoose.model('User', userSchema);

module.exports = User;
