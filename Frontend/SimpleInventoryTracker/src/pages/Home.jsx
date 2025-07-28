import React from 'react';
import './Home.css';


function Home() {
  return (
    <div className="home-split-layout">
      <div className="image-side">
        <img src="Inventory.png" alt="Inventory Illustration" />
      </div>
      <div className="text-side">
        <h1>Welcome to Simple Inventory Tracker</h1>
        <p>Manage and track your inventory with ease.</p>
      </div>
    </div>
  );
}

export default Home;
