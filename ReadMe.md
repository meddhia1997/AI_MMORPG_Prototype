# ai_MMo_Rpg Setup Guide

Welcome to the **AI_MMO_Prototype** project! This guide will help you set up both the **Unity Game** and the **Backend Server** for the project. Follow the steps below to get everything up and running.

---

## Unity Game Setup

### 1. Unity Version
Ensure you're using **Unity 2022.3.50f1** or a later compatible version. If you don’t have Unity installed, download and install it via **Unity Hub**.

### 2. Setting Up LMNT from Asset Hub AI

**LMNT** is used in this project for dynamic voice-over integration. Follow these steps:

1. **Download LMNT**  
   Visit the [Unity Asset Hub](https://assetstore.unity.com/ai-hub) or the official LMNT source to download the package.

2. **Import LMNT into Unity**  
   - Open your Unity project.
   - Navigate to **Asset > Import Package > Custom Package**.
   - Select the downloaded **LMNT** package and click **Import** to add all necessary files.

3. **Setup LMNT**  
   - Create a new empty GameObject (e.g., **VoiceManager**).
   - Drag and drop the LMNT scripts or prefabs onto this GameObject.
   - Configure the dynamic voice-over system by linking gameplay events (such as quest completions or player actions) to trigger voice lines via LMNT.

### 3. Additional Unity Configurations

- **Scene Management & UI:**  
  Ensure that your scenes are added to Build Settings, and that you have set up a child-friendly UI (big buttons, simple text) as required by the project.

- **Backend Integration:**  
  The Unity game will communicate with the backend server for data persistence and authentication. Ensure your Firebase and Node.js configurations (see Backend Setup) are correctly set up.

---

## Backend Setup

### 1. Prerequisites

Before setting up the backend, ensure you have the following installed:
- **Node.js** (Recommended version: 18+)
- **MongoDB**  
  Use either a local MongoDB installation or MongoDB Atlas for a cloud-based solution.

### 2. Installing Dependencies

1. **Clone the Repository**  
   Download or clone the backend project files to your local machine.

2. **Navigate to the Backend Directory**  
   Open a terminal/command prompt and change to the directory containing the backend files:
   ```bash
   cd AI_MMO_Prototype_Backend
   ```

3. **Install Dependencies**  
   Run the following command to install the required dependencies:
   ```bash
   npm install
   ```
   This installs packages such as:
   - `bcryptjs`
   - `body-parser`
   - `cors`
   - `dotenv`
   - `express`
   - `jsonwebtoken`
   - `mongoose`

### 3. MongoDB Configuration

- **Local MongoDB:**  
  Ensure MongoDB is installed and running on your machine.

- **MongoDB Atlas:**  
  Create an account, set up a cluster, and obtain your connection string.

- **Configure Environment Variables:**  
  In the backend project directory, create a `.env` file (if it doesn’t exist) and add:
  ```bash
  MONGO_URI=your_mongo_database_connection_url_here
  ```

### 4. Running the Backend Server

Start the backend server by running:
```bash
npm start
```
The server will typically run on port **5000** by default 

### 5. Package Information

Your `package.json` should resemble the following:
```json
{
  "name": "ai_mmo_prototype_backend",
  "version": "1.0.0",
  "description": "Backend for ai_MMo_Rpg game.",
  "main": "index.js",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1"
  },
  "keywords": [],
  "author": "",
  "license": "ISC",
  "dependencies": {
    "ai_mmo_prototype_backend": "file:",
    "bcryptjs": "^3.0.2",
    "body-parser": "^1.20.3",
    "cors": "^2.8.5",
    "dotenv": "^16.4.7",
    "express": "^4.21.2",
    "jsonwebtoken": "^9.0.2",
    "mongoose": "^8.11.0"
  }
}
```

---

## Additional Notes & Troubleshooting

- **Testing:**  
  - **Unity:** Check the Unity Console for any errors related to LMNT or scene management.
  - **Backend:** Verify that MongoDB is running .Check terminal output for any errors when running `npm start`.

- **Support:**  
  If you encounter issues with Unity, verify that you are using the correct Unity version and that all imported assets are up to date. For backend issues, double-check your MongoDB URI and environment configuration.


