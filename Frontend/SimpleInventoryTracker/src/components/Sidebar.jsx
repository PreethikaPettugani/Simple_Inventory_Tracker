import React from 'react';
import './Sidebar.css';

function Sidebar({ onSelect, lowStockCount }) {
  return (
    <div className="sidebar">
      <h2>Dashboard Menu</h2>
      <nav>
        <button onClick={() => onSelect('items')}>View All Items</button>
        <button onClick={() => onSelect('categories')}>Category</button>
        <button onClick={() => onSelect('lowstock')}>
          View Low Stock
          {lowStockCount > 0 && (
            <span className="lowstock-badge">{lowStockCount}</span>
          )}
        </button>
        <button onClick={() => onSelect('highstock')}>View High Stock</button>
        <button onClick={() => onSelect('report')}>Reports</button>
      </nav>
    </div>
  );
}

export default Sidebar;


