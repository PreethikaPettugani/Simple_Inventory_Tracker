import React from 'react';
import { Link, Routes, Route, Navigate } from 'react-router-dom';
import './Dashboard.css';

function AllItems() {
  return <h2>All Items Section</h2>;
}

function Categories() {
  return <h2>Categories Section</h2>;
}

function LowStock() {
  return <h2>Low Stock Section</h2>;
}

function Dashboard() {
  return (
    <div className="dashboard-container">
      <div className="sidebar">
        <h3>Dashboard</h3>
        <ul className="sidebar-links">
          <li><Link to="all-items">All Items</Link></li>
          <li><Link to="categories">Categories</Link></li>
          <li><Link to="low-stock">Low Stock</Link></li>
        </ul>
      </div>

      <div className="dashboard-content">
        <Routes>
          <Route path="/" element={<Navigate to="all-items" />} />
          <Route path="all-items" element={<AllItems />} />
          <Route path="categories" element={<Categories />} />
          <Route path="low-stock" element={<LowStock />} />
        </Routes>
      </div>
    </div>
  );
}

export default Dashboard;


// import React from 'react';
// import './Dashboard.css';

// function Dashboard() {
//   return (
//     <div className="dashboard-container">
//       <aside className="sidebar">
//         <h3>Dashboard</h3>
//         <ul className="sidebar-links">
//           <li><a href="#">All Items</a></li>
//           <li><a href="#">Categories</a></li>
//           <li><a href="#">Low Stock</a></li>
//         </ul>
//       </aside>

//       <main className="dashboard-content">
//         <h2>Welcome to Dashboard</h2>
//         <p>Select an option from the sidebar to get started.</p>
//       </main>
//     </div>
//   );
// }

// export default Dashboard;
