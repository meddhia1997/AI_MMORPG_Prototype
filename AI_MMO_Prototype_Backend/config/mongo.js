const mongoose = require('mongoose');

// MongoDB connection URI (replace with your MongoDB connection string)
const mongoURI = 'mongodb://localhost:27017/ai_mmo_prototype'; // Replace with your MongoDB URI if needed

mongoose.connect(mongoURI, {
  useNewUrlParser: true,
  useUnifiedTopology: true,
}).then(() => {
  console.log('MongoDB connected');
}).catch((err) => {
  console.error('MongoDB connection error:', err);
});

module.exports = mongoose;
