import React, { useState, useEffect } from 'react';
import Sidebar from '../components/Sidebar';
import ViewAllItems from './ViewAllItems';
import Categories from './Categories';
import LowStockItems from './LowStockItems';
import HighStockItems from './HighStockItems';
import Reports from './Reports';
import { getLowStockItems } from '../Services/Api';
import './Dashboard.css';

function Dashboard() {
  const [activeSection, setActiveSection] = useState('items');
  const [reloadKey, setReloadKey] = useState(0);
  const [lowStockCount, setLowStockCount] = useState(0);

  const handleSelect = (section) => {
    setActiveSection(section);
    setReloadKey(prev => prev + 1);
  };

  useEffect(() => {
    const fetchLowStockCount = async () => {
      try {
        const items = await getLowStockItems();
        setLowStockCount(items.length);
      } catch (error) {
        console.error('Error fetching low stock count:', error);
      }
    };
    fetchLowStockCount();
  }, []);

  return (
    <div className="dashboard-container">
      <Sidebar onSelect={handleSelect} lowStockCount={lowStockCount} />
      <div className="dashboard-content">
        {activeSection === 'items' && <ViewAllItems key={`items-${reloadKey}`} />}
        {activeSection === 'categories' && <Categories key={`cat-${reloadKey}`} />}
        {activeSection === 'lowstock' && <LowStockItems key={`low-${reloadKey}`} />}
        {activeSection === 'highstock' && <HighStockItems key={`high-${reloadKey}`} />}
        {activeSection === 'report' && <Reports key={`report-${reloadKey}`} />}
      </div>
    </div>
  );
}

export default Dashboard;

