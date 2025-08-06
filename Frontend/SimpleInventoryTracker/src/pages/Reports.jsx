import React from 'react';
import jsPDF from 'jspdf';
import 'jspdf-autotable';
import './Reports.css';

function Reports() {
  const fetchAndGenerate = async (type) => {
    let url = '';
    let title = '';

    if (type === 'all') {
      url = 'https://localhost:7045/api/Item'; 
      title = 'All Items';
    } else if (type === 'low') {
      url = 'https://localhost:7045/api/Item/lowstock';
      title = 'Low Stock Items';
    } else if (type === 'high') {
      url = 'https://localhost:7045/api/Item/highstock';
      title = 'High Stock Items';
    }

    try {
    const response = await fetch(url, { method: 'GET', mode: 'cors' });
    const data = await response.json();

    if (!Array.isArray(data) || data.length === 0) {
      alert('No data found to generate PDF.');
      return;
    }

      const doc = new jsPDF();
      doc.text(title, 14, 15);
      const rows = data.map(item => [
        item.itemId,
        item.name,
        item.description,
        item.quantity,
        item.category,
        item.minimumStockThreshold,
      ]);

      doc.autoTable({
        head: [['ID', 'Name', 'Description', 'Quantity', 'Category', 'Min Stock']],
        body: rows,
        startY: 20,
      });

      doc.save(`${title.replace(/\s/g, '_')}.pdf`);
    } catch (err) {
      console.error('Error generating report:', err);
    }
  };

  return (
    <div className="report-container">
      <h2 className="report-heading">Download Reports</h2>
      <div className="report-buttons">
        <button onClick={() => fetchAndGenerate('all')}>Download All Items</button>
        <button onClick={() => fetchAndGenerate('low')}>Download Low Stock</button>
        <button onClick={() => fetchAndGenerate('high')}>Download High Stock</button>
      </div>
    </div>
  );
}

export default Reports;
