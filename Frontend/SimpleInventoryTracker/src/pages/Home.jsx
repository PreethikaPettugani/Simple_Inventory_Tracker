import React from 'react';
import { Link } from 'react-router-dom';
import './Home.css';

function Home() {
  return (
    <div className="home-container">
        <div className="home-split-layout">
           <div className="image-side">
            <img src="Background.png" alt="Inventory" />
            </div>
            <div className="text-side">
            <h1>Welcome to StockHub</h1>
            <p>Track, manage and update stock in one place.</p>
             <Link to="/dashboard" className="dashboard-button">Go to Dashboard</Link>
            </div>
        </div>
    </div>
  );
}

export default Home;
