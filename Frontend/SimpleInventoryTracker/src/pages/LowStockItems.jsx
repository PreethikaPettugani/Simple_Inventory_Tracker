import React, { useEffect, useState } from 'react';
import { getLowStockItems, updateItemQuantity } from '../Services/Api';
import './LowStockItems.css';
import { DefaultButton, PrimaryButton } from '@fluentui/react';

function LowStockItems() {
  const [items, setItems] = useState([]);
  const [editItem, setEditItem] = useState(null);
  const [newQuantity, setNewQuantity] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 8;

  useEffect(() => {
    fetchLowStock();
  }, []);

  const fetchLowStock = async () => {
    try {
      const data = await getLowStockItems();
      setItems(data);
    } catch (error) {
      console.error('Error fetching low stock items:', error);
    }
  };

  const openEdit = (item) => {
    setEditItem(item);
    setNewQuantity('');
  };

  const closeEdit = () => {
    setEditItem(null);
    setNewQuantity('');
  };

  const handleSaveQuantity = async () => {
    try {
      const updated = await updateItemQuantity(editItem.itemId, parseInt(newQuantity, 10));
      console.log('Updated:', updated);
      await fetchLowStock();
      closeEdit();
    } catch (err) {
      console.error('Error updating quantity:', err);
    }
  };

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = items.slice(indexOfFirstItem, indexOfLastItem);
  const totalPages = Math.ceil(items.length / itemsPerPage);

  const paginate = (pageNumber) => setCurrentPage(pageNumber);

  return (
    <div className="lowstock-container">
      <h2 className="lowstock-heading">Low Stock Items</h2>
      <div className="lowstock-card-container">
        {currentItems.length === 0 ? (
          <p>No low stock items found.</p>
        ) : (
          currentItems.map((item) => (
            <div key={item.itemId} className="lowstock-card">
              <p><strong>Name:</strong> {item.name}</p>
              <p><strong>Description:</strong> {item.description}</p>
              <p><strong>Quantity:</strong> {item.quantity}</p>
              <p><strong>Category:</strong> {item.category}</p>
              <p><strong>Min Stock Threshold:</strong> {item.minimumStockThreshold}</p>

              <div className="lowstock-action-buttons">
                <button id ="edit-button"className="lowstock-edit-button" onClick={() => openEdit(item)}>Edit</button>
              </div>
            </div>
          ))
        )}
      </div>

      {totalPages > 1 && (
        <div className="lowstock-pagination-controls">
          <DefaultButton
            text="Prev"
            onClick={() => setCurrentPage((prev) => Math.max(prev - 1, 1))}
            disabled={currentPage === 1}
          />
          <span style={{ margin: '0 10px' }}>
            Page {currentPage} of {totalPages}
          </span>
          <DefaultButton
            text="Next"
            onClick={() => setCurrentPage((prev) => Math.min(prev + 1, totalPages))}
            disabled={currentPage === totalPages}
          />
        </div>
      )}
      {editItem && (
        <div className="edit-overlay">
          <div className="edit-modal">
            <h3>Edit Quantity</h3>
            <p><strong>ID:</strong> {editItem.itemId}</p>
            <p><strong>Name:</strong> {editItem.name}</p>
            <label htmlFor="newQuantityInput">New Quantity:</label>
            <input
              id="newQuantityInput"
              type="number"
              value={newQuantity}
              onChange={(e) => setNewQuantity(e.target.value)}
              className="modal-quantity-input"
            />
            <div className="modal-buttons">
              <PrimaryButton text="Update" onClick={handleSaveQuantity} id="edit-update-button" />
              <DefaultButton text="Cancel" onClick={closeEdit} />
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default LowStockItems;

